using Dapper;
using FastFood.Dto;
using FastFood.Models;
using FastFood.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;

namespace FastFood.Service
{
    public class CustomerService : ICustomerService
    {
        private IWebHostEnvironment _environment;
        private readonly IConfiguration _config;
        private readonly ILogger<CustomerService> _logger;
        private IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAESService _AESService;

        public CustomerService(ILogger<CustomerService> logger, IWebHostEnvironment environment,
            IConfiguration config, IEmailService emailService, IHttpContextAccessor httpContextAccessor,
            IAESService AESService)
        {
            _environment = environment;
            _config = config;
            _logger = logger;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _AESService = AESService;
        }
        public async Task<IEnumerable<CartList>?> GetCartList(int CustId)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    return await db.QueryAsync<CartList>("getcart", new { CustId = CustId });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting customer Cart List ID: {CustId}");
                return null;
            }
        }
        public async Task<bool> RemoveCart(int CustId,int FoodID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    return await db.QueryFirstOrDefaultAsync<int>("removecart", new { CustId,FoodID })==1;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while Removing customer Cart.  ID: {CustId}");
                return false;
            }
        }
        public async Task<IEnumerable<CartList>?> GetOrderList(int CustId)
        {
            try { 
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                return await db.QueryAsync<CartList>("getOrder", new { CustId = CustId });
            }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting customer Order List ID: {CustId}");
                return null;
            }
        }
        public async Task<CustProfileName?> GetCustomer(int CustId)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    return await db.QueryFirstOrDefaultAsync<CustProfileName>("GetCustProfileName", new { CustId = CustId },
                        commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting customer by ID: {CustId}");
                return null;
            }
        }
        public async Task<CustomerUpdate?> GetCustomer2(int CustId)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    return await db.QueryFirstOrDefaultAsync<CustomerUpdate>("GetCustomerAccount2", new { CustId = CustId },
                        commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting customer by ID: {CustId}");
                return null;
            }
        }
        public async Task<int> DoesCustEmailExist(string email)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    return await db.ExecuteScalarAsync<int>("DoesCustEmailExist", new { email = email });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while checking the customer email: {email}");
                return -1;
            }
        }

        public async Task<int> SignUp(CustomerSignUp CustModel)
        {
            int CustId = -1;

            try
            {
                if (CustModel != null)
                {
                    using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                    {

                        var result = await db.QueryFirstOrDefaultAsync<SignUpResponse>("signup", new
                        {
                            CustName = CustModel?.CustName?.Trim(),
                            CustAddress = CustModel?.CustAddress?.Trim(),
                            CustPhone = CustModel?.CustPhone?.Trim(),
                            CustEmail = CustModel?.CustEmail?.Trim(),
                            CustModel?.CustPassword
                        });
                        if (result != null)
                        {
                            if (result.CustId != null) CustId = int.Parse(result.CustId);
                            if (CustId > 0)
                            {
                                if (CustModel?.ImageFile != null)
                                {

                                    bool response = await UpdateProfileImage(CustModel.ImageFile, CustId);
                                    if (response == false)
                                    {
                                        _logger.LogError($"An error occurred while Uploading Image. Customer ID: {CustId}");
                                    }

                                }

                                if (result.ActivatedPin != null)
                                {
                                    var CustEmail = CustModel?.CustEmail;
                                    if (CustEmail != null)
                                    {
                                        await _emailService.SendMail("FastFood New Account", "Your Account has just been created.<br/>" +
                                               "<a href='https://localhost:7210/Account/ConfirmCustEmail?CustId=" + $"{CustId}&&ActivatedPin={result.ActivatedPin}'> " +
                                               "CLICK HERE TO CONFIRM YOUR ACCOUNT</a>", CustEmail);
                                    }

                                }
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Adding customer details");

            }

            return CustId;
        }

        public async Task<int> ConfirmAccount(int CustId, string ActivatedPin)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    return await db.ExecuteScalarAsync<int>("ConfirmAccount",
                        new { CustId = CustId, ActivatedPin = ActivatedPin });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while confirming the customer account with ID: {CustId}");
                return -1;
            }
        }

        public async Task<int> Login(LoginCredential LoginModel)
        {
            int response = -3;
            try
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    var result = await db.QueryFirstOrDefaultAsync<LoginResponse>("CustomerLogin",
                        new { CustEmail = LoginModel.Email?.Trim() });
                    if (result != null)
                    {
                        if (result.activated == 1)
                        {
                            if (result.CustPassword != null)
                            {
                                if (result.CustPassword == LoginModel.Password)
                                {
                                    if (result.CustId != null)
                                    {
                                        response = int.Parse(result.CustId);
                                    }
                                }
                                else response = 0;
                            }
                            else response = -1;
                        }
                        else
                        {
                            if (result.ActivatedPin != null)
                            {
                                response = -2;
                                if ((LoginModel.Email != null) && (LoginModel.Password != null) &&
                                    result.CustId != null)
                                {
                                    await _emailService.SendMail("FastFood Account Confirmation", "Please confirm your account using the link below.<br/>" +
                                                  "<br/><br/> <a href='https://localhost:7210/Account/ConfirmCustEmail?CustId=" + $"{result.CustId}&&ActivatedPin={result.ActivatedPin}'> " +
                                                  "CLICK HERE TO CONFIRM YOUR ACCOUNT</a>", LoginModel.Email);
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while Login the customer account with email: {LoginModel.Email}");
                response = -3;
            }
            return response;
        }

        public async Task<int> RequestPassword(string CustEmail)
        {
            int response = -1;
            try
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    string? password = await db.QueryFirstOrDefaultAsync<string>("getPassword",
                          new { CustEmail = CustEmail?.Trim() });

                    response = 0;
                    if (password != null)
                    {
                        string? email = CustEmail?.Trim();
                        if (email != null)
                        {
                            await _emailService.SendMail("FastFood Account Details", $"Email:{email}  Password: {password}"
                                , email);
                            response = 1;
                        }
                    }
                    
                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the customer password. Email: {CustEmail}");
                response= -1;
            }
            return response;
        }
        public async Task<int> GetCustIdFromHttpContext()
        {
            var CustIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("CustId");
            if (CustIdClaim != null)
            {
                string DecryptedCustId = await _AESService.Decrypt(CustIdClaim.Value);
                if (DecryptedCustId == null || !int.TryParse(DecryptedCustId, out int CustId))
                {
                    return -1;
                }
                else
                {

                    return CustId;
                }
            }

            return -1;
        }
        public async Task<CustomerDTO?> GetCustomerAccount(int CustId)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    using (var multi = await db.QueryMultipleAsync(
                        "GetCustomerAccount",
                        new { CustId = CustId },
                        commandType: CommandType.StoredProcedure))
                    {
                        var customer = await multi.ReadFirstOrDefaultAsync<CustomerDTO>();
                        if (customer != null)
                        {
                            customer.CustOrder = await multi.ReadAsync<Order>();
                        }
                        return customer;
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while Getting the customer account with ID: {CustId}");

            }
            return null;
        }

        public async Task<bool> SetLoginCookie(int CustId)
        {
            bool done = false;
            try
            {
                string? Authentication = _config["CookieAuth:Name"];
                if (Authentication != null)
                {
                    string EncryptedId = await _AESService.Encrypt(CustId.ToString());
                    var claims = new List<Claim>()
                    {
                        new Claim("CustId", EncryptedId),
                        new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddDays(10).ToString()) // Set expiration for 10 days from now
                    };
                    var identity = new ClaimsIdentity(claims, Authentication);
                    ClaimsPrincipal ClaimsPrincipal = new ClaimsPrincipal(identity);
                    if (_httpContextAccessor.HttpContext != null)
                    {
                        await _httpContextAccessor.HttpContext.SignInAsync(Authentication, ClaimsPrincipal);
                        done = true;
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while setting Login Cookie ID: {CustId}");

            }

            return done;
        }

        public async Task<int> UpdateProfile(CustomerUpdate model)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    var response = await db.QuerySingleAsync<int>("UpdateProfile", new
                    {
                        CustName = model?.CustName?.Trim(),
                        CustAddress = model?.CustAddress?.Trim(),
                        CustPhone = model?.CustPhone?.Trim(),
                        CustEmail = model?.CustEmail?.Trim(),
                        model?.CustPassword,
                        model?.CustId
                    });
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while Updating the customer account with ID: {model?.CustId}");

            }
            return -1;
        }

        public async Task<bool> UpdateProfileImage(IFormFile ImageFile, int CustId)
        {
            try
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "CustomerImage");
                string filePath = Path.Combine(uploadsFolder, $"{CustId}.jpg");
                if (File.Exists(filePath))
                {

                    File.Delete(filePath);

                }
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }
                return true;
            }
            catch (Exception ex) { _logger.LogError(ex, $"Unable to upload image of the customer with ID {CustId}"); }

            return false;
        }
        public async Task<bool> AddToCart(int CustId, CartItemViewModel model)
        {
            
            try
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                   return  await db.QueryFirstOrDefaultAsync<int>("AddToCart", new
                    {
                       CustId,
                       model.FoodId,
                       model.Quantity,
                       model.Price
                    }) == 1 ;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while Adding to Cart with customer account ID: {CustId},{model}");

            }
            return false;
        }

        public async Task<CustomerCart?> CheckOut(int CustId)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    using (var multi = await db.QueryMultipleAsync("CheckOut",
                        new { CustId = CustId }, commandType: CommandType.StoredProcedure))
                    {
                        var customer = await multi.ReadFirstOrDefaultAsync<CustomerCart>();
                        if (customer != null)
                        {
                            customer.CustCart = await multi.ReadAsync<Cart>();
                        }
                        return customer;
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while checking out.  ID: {CustId}");

            }
            return null;
        }
        public async Task<int> PlaceOrder(int CustId)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    var response = await db.QueryFirstOrDefaultAsync<int>("PlaceOrder", new
                    {
                      CustId
                    });
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while Placing Order with ID: {CustId}");

            }
            return -1;
        }
    }
}

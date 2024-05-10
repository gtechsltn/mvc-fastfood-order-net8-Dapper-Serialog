using FastFood.Dto;
using FastFood.Models;
using FastFood.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Controllers
{
    // Controller handling account-related actions such as sign up, login, forgot password, and logout.
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ICustomerService _customerService;
        private IConfiguration _configuration;

        public AccountController(ILogger<HomeController> logger, ICustomerService customerService, IConfiguration configuration)
        {
            _logger = logger;
            _customerService = customerService;
            _configuration = configuration;
        }

        // Action method for displaying sign up form.
        public IActionResult SignUp()
        {
            return View();
        }

        // Action method for displaying login form.
        public IActionResult Login()
        {
            return View();
        }

        // Action method for displaying forgot password form.
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // Action method for processing forgot password form submission.
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(EmailDTO model)
        {
            // Validate the model.
            if (ModelState.IsValid)
            {
                if (model.CustEmail != null)
                {
                    // Request password reset.
                    int response = await _customerService.RequestPassword(model.CustEmail.Trim());
                    if (response == -1) TempData["error"] = "An error occurred while confirming the email. If persist, contact the admin";
                    else if (response == 0) TempData["error"] = "Email Address not exist";
                    else if (response == 1) TempData["success"] = "Your Account Details has been sent to your email address";
                }
            }
            else TempData["error"] = "The Model is Invalid. Try Again";

            return View(model);
        }

        // Action method for processing sign up form submission.
        [HttpPost]
        public async Task<IActionResult> SignUp(CustomerSignUp model)
        {
            // Validate the model.
            if (ModelState.IsValid)
            {
                if (model.CustEmail != null)
                {
                    // Check if email already exists.
                    int ConfirmEmail = await _customerService.DoesCustEmailExist(model.CustEmail.Trim());

                    if (ConfirmEmail == -1) TempData["error"] = "An error occurred while confirming the email. If persist, contact the admin";
                    else if (ConfirmEmail == 0)
                    {
                        // Sign up new customer.
                        int CustID = await _customerService.SignUp(model);
                        if (CustID == -1) TempData["error"] = "Unable to save your data. Try Again";
                        else if (CustID > 0) TempData["success"] = $"Saved Successfully. Check {model.CustEmail} inbox to confirm your account";
                    }
                    else if (ConfirmEmail > 0) TempData["error"] = $"Email Already Exist. Login to your account if you have registered with {model.CustEmail} before";
                }
            }
            else TempData["error"] = "The Model is Invalid. Try Again";

            return View();
        }

        // Action method for confirming customer email.
        public async Task<IActionResult> ConfirmCustEmail(string CustID, string ActivatedPin)
        {
            int customerId = -1;
            if (!int.TryParse(CustID, out customerId)) TempData["error"] = "Invalid Customer ID";
            else if (ActivatedPin.Length > 15) TempData["error"] = "Invalid Activated Pin";
            else
            {
                // Confirm customer account.
                int ConfirmAccount = await _customerService.ConfirmAccount(customerId, ActivatedPin);
                if (ConfirmAccount == -1) TempData["error"] = "An error occurred while confirming the email. If persist, contact the admin";
                else if (ConfirmAccount == 0) TempData["error"] = "Invalid Account. If persist, contact the admin";
                else if (ConfirmAccount > 0) TempData["success"] = "FastFood Account Confirmed Successfully. Login to your account";
            }
            return View();
        }

        // Action method for processing login form submission.
        [HttpPost]
        public async Task<IActionResult> Login(LoginCredential model)
        {
            if (ModelState.IsValid)
            {
                // Login customer.
                int response = await _customerService.Login(model);
                if (response == -3) TempData["error"] = "An error occurred while login. If persist, contact the admin";
                else if (response == -2) TempData["error"] = "Please check your mail inbox to confirm your account";
                else if (response == -1) TempData["error"] = "Email Address not exist";
                else if (response == 0) TempData["error"] = "Invalid Password";
                else if (response > 0)
                {
                    // Set authentication cookie.
                    int id = response;
                    bool setcookie = await _customerService.SetLoginCookie(id);
                    if (setcookie)
                    {
                        string? returnUrl = HttpContext.Request.Query["ReturnUrl"];
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                        else return RedirectToAction("MyAccount", "Customer");
                    }
                    else TempData["error"] = "Unable to Set Login. Try Again";
                }
            }
            return View();
        }

        // Action method for logging out.
        public async Task<IActionResult> Logout()
        {
            string? Authentication = _configuration["CookieAuth:Name"];
            if (Authentication != null)
            {
                await HttpContext.SignOutAsync(Authentication);
                TempData["success"] = "Log Out Sussessfully";
                return RedirectToAction("Index","Home");
            }
            else TempData["error"] = "Authentication Fail. Try Again";

            return RedirectToAction("Login");
        }
    }
}

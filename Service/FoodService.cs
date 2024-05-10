using Dapper;
using FastFood.Dto;
using FastFood.Service.Interface;
using Microsoft.Data.SqlClient;
using System.Data;
namespace FastFood.Service
{
    public class FoodService:IFoodService
    {
        private IWebHostEnvironment Environment;
        private readonly IConfiguration _config;
        private readonly ILogger<FoodService> _logger;

        public FoodService(IWebHostEnvironment env, IConfiguration config, ILogger<FoodService> logger)
        {
            Environment = env;
            _config = config;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryList>?> GetCategoryList()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    return await db.QueryAsync<CategoryList>("getcategories");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting Category List");
                return null;
            }
        }
       
        public async Task<IEnumerable<RecentItem>?> GetRecentItem()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    return await db.QueryAsync<RecentItem>("getrecentitem");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting Recent Item");
                return null;
            }

        }
        public async Task<CartList?> GetProductDetails(int Id)
        {
            try { 
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
              
                return await db.QueryFirstOrDefaultAsync<CartList>("getProductDetail", new { id = Id},
                        commandType: CommandType.StoredProcedure);
            }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting Product Details with ID: {Id}");
                return null;
            }
        }
        public async Task<IEnumerable<Shop>?> GetShopList(string SearchString)
        {
            try
            {  
                if (SearchString== null) SearchString = "";
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {

                    return await db.QueryAsync<Shop>("shop", new { SearchString=SearchString },
                            commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting Shop List");
                return null;
            }
        }
    }
}

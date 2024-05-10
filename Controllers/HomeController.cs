using FastFood.Models;
using FastFood.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FastFood.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IFoodService _foodService;
         public HomeController(ILogger<HomeController> logger, IFoodService foodService) 
        {
            _logger = logger;
            _foodService = foodService;               
        }

        public async Task<IActionResult> Index()
        {
          
            var model= await _foodService.GetRecentItem();
            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        public async Task<IActionResult> ProductDetails(int id)
        {
            var model = await _foodService.GetProductDetails(id);
            return View(model);
        }

        

        public async Task<IActionResult> Shop(string SearchString)
        {
            var model = await _foodService.GetShopList(SearchString);       
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

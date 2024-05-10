
using FastFood.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Models.Components
{

    public class CategoryViewComponent : ViewComponent
    {


        private readonly IFoodService _foodservice;

        public CategoryViewComponent(IFoodService foodService)
        {
            _foodservice = foodService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _foodservice.GetCategoryList();
            return View("Category", model);

        }

    }
}

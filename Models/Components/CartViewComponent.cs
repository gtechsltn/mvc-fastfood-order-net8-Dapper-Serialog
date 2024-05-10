using FastFood.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Models.Components
{
    
    public class CartViewComponent : ViewComponent
    {

        
        private readonly ICustomerService _customerservice;

        public CartViewComponent(ICustomerService customerservice)
        {           
           _customerservice = customerservice;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int CustId = await _customerservice.GetCustIDFromHttpContext();
            if (CustId > 0)
            {
                var model = await _customerservice.GetCartList(CustId);
                return View("Cart", model);
            }
            else
            {
                return View("Cart", (object?)null);
            }

            
        }

    }


}

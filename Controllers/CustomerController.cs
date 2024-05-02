using FastFood.Dto;
using FastFood.Models;
using FastFood.Service;
using FastFood.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Reflection;
using System.Security.Claims;

namespace FastFood.Controllers
{
    // Controller handling customer-related actions such as viewing account details, updating profile, managing cart, and placing orders.
    // Requires authentication with "Customer" policy.
    // Utilizes services for accessing customer data and performing actions.
    // Utilizes Serilog for logging.
    [Authorize(Policy = "Customer")]
    public class CustomerController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IFoodService _foodService;
        private ICustomerService _customerService;

        public CustomerController(ILogger<HomeController> logger, IFoodService foodService, ICustomerService customerService)
        {
            _logger = logger;
            _foodService = foodService;
            _customerService = customerService;
        }

        // Action method for displaying customer account details.
        public async Task<IActionResult> MyAccount()
        {
            // Retrieve customer ID from HTTP context.
            int CustID = await _customerService.GetCustIdFromHttpContext();
            if (CustID > 0)
            {
                // Retrieve and display customer account details.
                var model = await _customerService.GetCustomerAccount(CustID);
                return View(model);
            }
            else
            {
                TempData["error"] = "Unable to retrieve the customer details. Try Login Again";
                return RedirectToAction(nameof(AccountController.Login), nameof(AccountController));
            }
        }

        // Action method for updating customer profile.
        public async Task<IActionResult> UpdateProfile()
        {
            int CustID = await _customerService.GetCustIdFromHttpContext();
            if (CustID > 0)
            {
                // Retrieve customer profile for updating.
                var model = await _customerService.GetCustomer2(CustID);
                return View(model);
            }
            else
            {
                TempData["error"] = "Unable to retrieve the customer details. Try Login Again";
                return RedirectToAction(nameof(AccountController.Login), nameof(AccountController));
            }
        }

        // Action method for processing updated customer profile.
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(CustomerUpdate model)
        {
            // Validate the model.
            if (ModelState.IsValid)
            {
                if (model.CustId > 0)
                {
                    // Update customer profile.
                    int response = await _customerService.UpdateProfile(model);
                    if (response == -1) TempData["error"] = "Unable to update your data. Try Again";
                    else if (response == 0) TempData["error"] = $"This email {model.CustEmail} has been used by another customer ";
                    else if (response == 1) TempData["success"] = $"Updated Successfully.";
                }
                else TempData["error"] = "Invalid Customer Account ID";
            }
            else TempData["error"] = "The Model is Invalid. Try Again";

            return View(model);
        }

        // Action method for uploading customer profile image.
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                int CustID = await _customerService.GetCustIdFromHttpContext();
                if (CustID > 0)
                {
                    // Update customer profile image.
                    bool response = await _customerService.UpdateProfileImage(imageFile, CustID);
                    if (response) TempData["success"] = "Profile Image Updated Successfully";
                    else TempData["error"] = "Unable to update profile image. Try Again";
                }
                else
                {
                    TempData["error"] = "Unable to retrieve the customer details. Try Login Again";
                    return RedirectToAction("Login", "Account");
                }
            }
            else TempData["error"] = "The Model is Invalid. Try Again";

            return RedirectToAction(nameof(MyAccount));
        }

        // Action method for adding an item to the customer's cart.
        public IActionResult AddToCart(int id)
        {
            return RedirectToAction("ProductDetails", "Home", new { id = id });
        }

        // Action method for processing adding an item to the customer's cart.
        [HttpPost]
        public async Task<IActionResult> AddToCart(CartItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                int CustID = await _customerService.GetCustIdFromHttpContext();
                if (CustID > 0)
                {
                    // Add item to the cart.
                    bool response = await _customerService.AddToCart(CustID, model);
                    if (response)
                    {
                        TempData["success"] = "Item Added to the Cart Successfully";
                        return RedirectToAction("ViewCart");
                    }
                    else TempData["error"] = "Unable to add item to Cart";
                }
                else TempData["error"] = "Unable to retrieve the customer details. Try Login Again";
            }
            else TempData["error"] = "The Model is Invalid. Try Again";

            return RedirectToAction("ProductDetails", "Home", new { id = model.FoodId });
        }

        // Action method for viewing the customer's cart.
        public async Task<IActionResult> ViewCart()
        {
            int CustId = await _customerService.GetCustIdFromHttpContext();
            if (CustId > 0)
            {
                // Retrieve and display the customer's cart.
                var model = await _customerService.GetCartList(CustId);
                return View(model);
            }
            else TempData["error"] = "Unable to retrieve the customer details. Try Login Again";

            return View();
        }

        // Action method for removing an item from the customer's cart.
        [HttpPost]
        public async Task<IActionResult> RemoveCart(int id)
        {
            int CustId = await _customerService.GetCustIdFromHttpContext();
            if (CustId > 0)
            {
                // Remove item from the cart.
                var response = await _customerService.RemoveCart(CustId, id);
                if (response) TempData["success"] = "Cart Item Removed Successfully";
                else TempData["error"] = "Unable To Remove Cart Item Removed";
            }
            else TempData["error"] = "Unable to retrieve the customer details. Try Login Again";

            return RedirectToAction("ViewCart");
        }

        // Action method for checking out the items in the customer's cart.
        public async Task<IActionResult> CheckOut()
        {
            int CustId = await _customerService.GetCustIdFromHttpContext();
            if (CustId > 0)
            {
                // Proceed to checkout.
                var model = await _customerService.CheckOut(CustId);
                return View(model);
            }
            else TempData["error"] = "Unable to retrieve the customer details. Try Login Again";

            return View();
        }

        // Action method for placing an order.
        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            int CustId = await _customerService.GetCustIdFromHttpContext();
            if (CustId > 0)
            {
                // Place the order.
                var response = await _customerService.PlaceOrder(CustId);
                if (response == -1) TempData["error"] = "Unable to Place Order. Try Login Again";
                else if (response == 0) TempData["error"] = "Low Ewallet Balance";
                else if (response == 1) return RedirectToAction("MyAccount");
            }
            else TempData["error"] = "Unable to retrieve the customer details. Try Login Again";

            return RedirectToAction("CheckOut");
        }
    }
}

using ComicShopPOS.Models.DTOs;
using ComicShopPOS.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComicShopPOS.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET: /Cart
        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            return View(cart);
        }

        // POST: /Cart/AddToCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(AddToCartDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var firstError = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();
                TempData["ErrorMessage"] = firstError ?? "Please enter a valid quantity.";
                return RedirectToAction("Index", "Product");
            }

            var result = _cartService.AddToCart(dto);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
            }
            else
            {
                TempData["SuccessMessage"] = "Item added to cart.";
            }

            return RedirectToAction("Index", "Product");
        }

        // POST: /Cart/UpdateQuantity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateQuantity(UpdateCartDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var firstError = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();
                TempData["ErrorMessage"] = firstError ?? "Please enter a valid quantity.";
                return RedirectToAction("Index");
            }

            var result = _cartService.UpdateQuantity(dto);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] =
                result.Success ? "Quantity updated." : result.ErrorMessage;

            return RedirectToAction("Index");
        }

        // POST: /Cart/RemoveItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveItem(int productId)
        {
            _cartService.RemoveItem(productId);
            TempData["SuccessMessage"] = "Item removed from cart.";
            return RedirectToAction("Index");
        }
    }
}

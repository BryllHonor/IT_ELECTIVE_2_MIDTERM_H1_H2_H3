using ComicShopPOS.Models.DTOs;
using ComicShopPOS.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComicShopPOS.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ICheckoutService _checkoutService;
        private readonly ITransactionService _transactionService;

        public CheckoutController(
            ICartService cartService,
            ICheckoutService checkoutService,
            ITransactionService transactionService)
        {
            _cartService = cartService;
            _checkoutService = checkoutService;
            _transactionService = transactionService;
        }

        // GET: /Checkout
        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            if (cart.Items.Count == 0)
            {
                TempData["ErrorMessage"] = "Your cart is empty. Add items before checking out.";
                return RedirectToAction("Index", "Cart");
            }

            ViewBag.Cart = cart;
            return View(new CheckoutFormDTO());
        }

        // POST: /Checkout/ProcessCheckout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessCheckout(CheckoutFormDTO dto)
        {
            var cart = _cartService.GetCart();

            if (cart.Items.Count == 0)
            {
                TempData["ErrorMessage"] = "Your cart is empty. Add items before checking out.";
                return RedirectToAction("Index", "Cart");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Cart = cart;
                return View("Index", dto);
            }

            var result = _checkoutService.ProcessCheckout(dto);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Checkout could not be completed.");
                ViewBag.Cart = _cartService.GetCart();
                return View("Index", dto);
            }

            return RedirectToAction("Confirmation", new { id = result.Transaction!.TransactionId });
        }

        // GET: /Checkout/Confirmation/5
        public IActionResult Confirmation(int id)
        {
            var transaction = _transactionService.GetTransactionById(id);
            if (transaction is null)
            {
                return NotFound();
            }

            return View(transaction);
        }
    }
}

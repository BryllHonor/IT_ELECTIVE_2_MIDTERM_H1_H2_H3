using ComicShopPOS.Models.DTOs;
using ComicShopPOS.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComicShopPOS.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: /Product
        public IActionResult Index()
        {
            var products = _productService.GetAllProducts();
            return View(products);
        }
    }
}

using ComicShopPOS.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComicShopPOS.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // GET: /Transaction
        public IActionResult Index()
        {
            var transactions = _transactionService.GetAllTransactions();
            return View(transactions);
        }

        // GET: /Transaction/Details/5
        public IActionResult Details(int id)
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

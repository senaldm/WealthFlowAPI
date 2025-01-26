using Microsoft.AspNetCore.Mvc;

namespace WealthFlow.API.Controllers.Transactions
{
    public class TransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

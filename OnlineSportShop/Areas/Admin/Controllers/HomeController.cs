using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSportShop.Data;
using OnlineSportShop.Models;
using OnlineSportShop.ViewModel;
using Stripe;

namespace OnlineSportShop.Areas.Admin.Controllers
{
    [Area("Admin")]
       public class HomeController : Controller
    {
        private readonly OnlineSportShopContext _context;

        public HomeController(OnlineSportShopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SaveMessage()
        {
            var message = _context.Contacts.ToList();
            return View(message);
        }

        public IActionResult StripeDashboard()
        {
            var response = new StripeDashboardVM();

            var balanceService = new BalanceService();
            var balanceResult = balanceService.Get();
            response.Balance = balanceResult;

            var transactionService = new BalanceTransactionService();
            var transactionResult = transactionService.List().ToList();
            response.Transactions = transactionResult;

            var customerService = new CustomerService();
            var customerResult = customerService.List().ToList();
            response.Customers = customerResult;

            var chargeService = new ChargeService();
            var chargeResult = chargeService.List().ToList();
            response.Charges = chargeResult;


            var disputeService = new DisputeService();
            var disputeResult = disputeService.List().ToList();
            response.Disputes = disputeResult;

            var refundService = new RefundService();
            var refundResult = refundService.List().ToList();
            response.Refunds = refundResult;

            return View(response);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineSportShop.Data;
using OnlineSportShop.Models;
using OnlineSportShop.ViewModel;
using System.Diagnostics;

namespace OnlineSportShop.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OnlineSportShopContext _context;

        public HomeController(ILogger<HomeController> logger, OnlineSportShopContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        public IActionResult Index()
        {
            var model = new IndexVM
            {
                Categories = _context.Categories.ToList(),
                Products = _context.Products.Take(12).ToList()
            };

            return View(model);
        }

        public IActionResult Product(int pg=1)
        {
            var products = _context.Products.ToList();
            const int pageSize = 3;
            if (pg < 1)
                pg = 1;

            int recsCount = products.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = products.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;

            return View(data);
            //return View(products);
        }

        public IActionResult SearchProduct(string productName)
        {
            var products = _context.Products
                .Where(p => p.ProName == productName)
                .ToList();
            return View(products);
        }
        public IActionResult ProductCategory(int? id, int pg=1)
        {
            var products = _context.Products
                .Where(c => c.CatId == id)
                .ToList();
            const int pageSize = 3;
            if (pg < 1)
                pg = 1;

            int recsCount = products.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = products.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;

            return View(data);
            //return View(products);
        }
        public IActionResult ProductDetails(int? id)
        {
            var product = _context.Products.Include(x => x.Category)
                .FirstOrDefault(p => p.ProId == id);
            return View(product);
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(Contact model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
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
                .Where(p => p.ProName.Contains(productName) )
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

        public IActionResult WishDetails(int? id)
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
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com");
                    client.Authenticate("jibrilomar04@gmail.com", "ustgkusaghrgukuv");
                    var bodyBuilder = new BodyBuilder()
                    {
                        HtmlBody = $"<p>{model.Name}</p> <p>{model.Email}</p> <p>{model.Subject}</p> <p>{model.Message}</p>",
                        TextBody = "{model.Name} \r\n {model.Email} \r\n {model.Subject} \r\n {model.Message}"
                    };

                    var message = new MimeMessage
                    {
                        Body = bodyBuilder.ToMessageBody(),
                    };
                    message.From.Add(new MailboxAddress("Do Not Reply", model.Email));
                    message.To.Add(new MailboxAddress("Testing", "jibrilomar04@gmail.com"));
                    message.Subject = model.Subject;
                    client.Send(message);

                    client.Disconnect(true);
                }
                TempData["Message"] = "Thank you for your quary, We will contact you shortly";
                return RedirectToAction(nameof(Contact));
                //_context.Add(model);
                //_context.SaveChanges();
                //return RedirectToAction("Index");
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
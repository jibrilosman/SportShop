using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineSportShop.Data;
using OnlineSportShop.Models;
using OnlineSportShop.ViewModel;

namespace OnlineSportShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly OnlineSportShopContext _context;

        public ProductsController(OnlineSportShopContext context)
        {
            _context = context;
        }
        public IActionResult Index(int pg=1)
        {
            var products = _context.Products.Include(c => c.Category).ToList();
            const int pageSize = 4;
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
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CatId", "CatName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product model, IFormFile File)
        {
            if(File != null)
            {
                string imageName = Guid.NewGuid().ToString() + ".jpg";
                string filePathImage = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/productImages", imageName);
                using(var stream = System.IO.File.Create(filePathImage))
                {
                    await File.CopyToAsync(stream);
                }
                model.ProImage = imageName;
            }
            _context.Add(model);
            await _context.SaveChangesAsync();
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CatId", "CatName");
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var product = _context.Products.Find(id);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CatId", "CatName");
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, Product model, IFormFile File)
        {
            if (File != null)
            {
                string imageName = Guid.NewGuid().ToString() + ".jpg";
                string filePathImage = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/productImages", imageName);
                using (var stream = System.IO.File.Create(filePathImage))
                {
                    await File.CopyToAsync(stream);
                }
                model.ProImage = imageName;
            }
            else
            {
                model.ProImage = model.ProImage;
            }
            _context.Update(model);
            await _context.SaveChangesAsync();
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CatId", "CatName");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            var product = _context.Products.Find(id);
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

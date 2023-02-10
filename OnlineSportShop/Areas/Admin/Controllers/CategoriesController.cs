using Microsoft.AspNetCore.Mvc;
using OnlineSportShop.Data;
using OnlineSportShop.Models;

namespace OnlineSportShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly OnlineSportShopContext _context;

        public CategoriesController(OnlineSportShopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }
      
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category model, IFormFile File)
        {
            if(File != null) 
            {
                string imageName = Guid.NewGuid().ToString() + ".jpg";
                string filePathImage = Path.Combine(Directory.GetCurrentDirectory(),@"wwwroot\categoryImages", imageName);
                using(var stream = System.IO.File.Create(filePathImage))
                {
                    await File.CopyToAsync(stream);
                }
                model.CatPhoto = imageName;
            }
            _context.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int? id)
        {
            var category = _context.Categories.Find(id);
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category model, IFormFile File)
        {
            if (File != null)
            {
                string imageName = Guid.NewGuid().ToString() + ".jpg";
                string filePathImage = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\categoryImages", imageName);
                using (var stream = System.IO.File.Create(filePathImage))
                {
                  File.CopyTo(stream);
                }
                model.CatPhoto = model.CatPhoto;
            }
            _context.Update(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if(id != null)
            {
                var cat = _context.Categories.Find(id);
                _context.Categories.Remove(cat);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}

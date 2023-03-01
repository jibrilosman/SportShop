using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSportShop.Repositories.Abstract;
using OnlineSportShop.ViewModel;

namespace OnlineSportShop.Controllers
{

    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _service;

        public UserAuthenticationController(IUserAuthenticationService service)
        {
            this._service = service;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationVM model)
        {
            if (!ModelState.IsValid)
                return View(model);
            model.Role = "user";
            var result = await _service.RegistrationAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Registration));
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.LoginAsync(model);
                if (result.StatusCode == 1)
                {
                    TempData["msg"] = result.Message;
                    return RedirectToAction("Index", "Home");
                }
                TempData["msg"] = result.Message;
                RedirectToAction(nameof(Login));
            }
            return View(model);
        }
    

        [Authorize]
        public async Task<IActionResult> Logout() 
        {
            await _service.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }


        //public async Task<IActionResult> Reg()
        //{
        //    var model = new RegistrationVM
        //    {
        //        UserName = "admin1",
        //        Name = "Omar",
        //        Email = "jibrilosman@gmail.com",
        //        Password = "Admin@12345#"
        //    };
        //    model.Role = "admin";
        //    var result = await _service.RegistrationAsync(model);
        //    return Ok(result);
        //}

    }
}

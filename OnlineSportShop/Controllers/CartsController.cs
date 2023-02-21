using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OnlineSportShop.Data;
using OnlineSportShop.Models;
using OnlineSportShop.ViewModel;
using Stripe;

namespace OnlineSportShop.Controllers
{
    public class CartsController : Controller
    {
        private readonly OnlineSportShopContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CartsController(OnlineSportShopContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [TempData]
        public string? TotalAmount { get; set; }
        public async Task<IActionResult> Cart()
        {
            var user = await _userManager.GetUserAsync(User);
            var result = _context.ShoppingCarts
                .Include(p=> p.Product)
                .Where(u => u.UserId == user.Id)
                .ToList();
            return View(result);
        }

        public async Task<IActionResult> IncreaseCart(ShoppingCart model, int qty)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProId == model.ProId);
            var user = await _userManager.GetUserAsync(User);
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                UserId = user.Id,
                ProId = product.ProId,
                Qty = qty
            };

            List<ShoppingCart> shoppingCarts= new List<ShoppingCart>();
            shoppingCarts.Add(shoppingCart);

            return View(shoppingCarts);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(ShoppingCart model, int qty)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProId == model.ProId);
            var user = await _userManager.GetUserAsync(User);
            var cart = new ShoppingCart
            {
                UserId = user.Id,
                ProId = product.ProId,
                Qty = qty
            };
            var shopcart = _context.ShoppingCarts.FirstOrDefault(u => u.UserId == user.Id && u.ProId == model.ProId);
            if(qty <= 0)
            {
               qty = 1;
            }
            if (shopcart == null)
            {
                _context.ShoppingCarts.Add(cart);
            }
            else
            {
                shopcart.Qty += model.Qty;
            }
            
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");

            
        }

        public async Task<IActionResult> Plus(int id)
        {
            var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.CartId == id);
            cart.Qty += 1;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Cart));
        }
        public async Task<IActionResult> Minus(int id)
        {
            var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.CartId == id);
            if(cart.Qty == 1)
            {
                _context.ShoppingCarts.Remove(cart);
                _context.SaveChanges();
            }
            else
            {
                cart.Qty -= 1;
                await _context.SaveChangesAsync();
            } 
            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int id )
        {
            var user = await _userManager.GetUserAsync(User);
            
            var shopcart = _context.ShoppingCarts.FirstOrDefault(u => u.UserId == user.Id && u.CartId == id);
            if(shopcart != null)
            {
                _context.ShoppingCarts.Remove(shopcart);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Cart));
        }

        // checkout and payment

        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            var result = _context.ShoppingCarts
                .Include(p => p.Product)
                .Where(u => u.UserId == user.Id)
                .ToList();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Processing(string stripeToken, string stripeEmail)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = _context.ShoppingCarts.Include(p => p.Product)
                 .Where(u => u.UserId == user.Id).ToList();

            
            var shippingCharge = Convert.ToInt64(10);
            
            ViewBag.result = result;
            ViewBag.DollarAmount = result.Sum(item => item.Product.Price * item.Qty);
            ViewBag.total = Math.Round(ViewBag.DollarAmount, 2) * 100;
            //ViewBag.total = Convert.ToInt64(ViewBag.total);
            //long total = ViewBag.total;
            /*TotalAmount = total.ToString()*/;

            var hstCount = Math.Round(ViewBag.total * Convert.ToDecimal((0.13)));
            var total = shippingCharge + hstCount + ViewBag.total;

            var customerOptions = new CustomerCreateOptions
            {
                Email = stripeEmail,
                Name = user.Name,

            };
            var customers = new CustomerService();
            Customer customer = customers.Create(customerOptions);

            var chargeOptions = new ChargeCreateOptions
            {
                Amount = Convert.ToInt64(total),
                Currency = "Usd",
                Source = stripeToken,
                ReceiptEmail= stripeEmail,
                

                Metadata = new Dictionary<string, string>()
                {
                    {"User id", user.Id },
                    {"User Name", user.UserName },
                     {"Name", user.Name },
                    {"User Email", user.Email },
                   
                }

            };

            var service = new ChargeService();
            Charge charge = service.Create(chargeOptions);

            if (charge.Status == "succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;
                //ViewBag.DollarAmount = Convert.ToDecimal(charge.Amount) % 100 / 100 + (charge.Amount) / 100;
                //ViewBag.BalanceTxId = BalanceTransactionId;
                return RedirectToAction(nameof(Success));
            }
            return View("Failure");
        }

        public async Task<IActionResult> Success()
        {
            var user = await _userManager.GetUserAsync(User);
            var result = _context.ShoppingCarts
                .Include(p => p.Product)
                .Where(u => u.UserId == user.Id)
                .ToList();
            return View(result);
        }


        // WishList 
        public async Task<IActionResult> Wish()
        {
            var user = await _userManager.GetUserAsync(User);
            var result = _context.WishLists
                .Include(p => p.Product)
                .Where(u => u.UserId == user.Id)
                .ToList();
            return View(result);
        }


        public async Task<IActionResult> AddToWishList(WishList model)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProId == model.ProId);
            var user = await _userManager.GetUserAsync(User);
            var cart = new WishList
            {
                UserId = user.Id,
                ProId = product.ProId
            };
            var shopcart = _context.WishLists.FirstOrDefault(u => u.UserId == user.Id && u.ProId == model.ProId);

            if (shopcart == null)
            {
                _context.WishLists.Add(cart);
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveWish(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var shopcart = _context.WishLists.FirstOrDefault(u => u.UserId == user.Id && u.ProId == id);
            if (shopcart != null)
            {
                _context.WishLists.Remove(shopcart);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Wish));
        }



    }
}

using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        public OrderController(IOrderRepository repoService, Cart cartService, UserManager<AppUser> userMgr)
        {
            repository = repoService;
            cart = cartService;
            userManager = userMgr;
        }

        [Authorize]
        public ViewResult List() =>
            View(repository.Orders.Where(o => !o.Shipped));

        public ViewResult UserOrders() =>
            View(repository.Orders.Where(o => o.UserID == CurrentUser.Result.Id));

        [HttpPost]
        [Authorize]
        public IActionResult MarkShipped(int orderID)
        {
            Order order = repository.Orders
            .FirstOrDefault(o => o.OrderID == orderID);
            if (order != null)
            {
                order.Shipped = true;
                repository.SaveOrder(order);
            }
            return RedirectToAction(nameof(List));
        }

        public ViewResult Checkout()
        {
            //ViewBag.Test = "testa";
            ViewBag.UserID = CurrentUser.Result.Id;
            ViewBag.Name = CurrentUser.Result.UserName;
            ViewBag.Line1 = CurrentUser.Result.Line1;
            ViewBag.Line2 = CurrentUser.Result.Line2;
            ViewBag.City = CurrentUser.Result.City;
            ViewBag.Zip = CurrentUser.Result.Zip;

            return View(new Order());
        }


        private UserManager<AppUser> userManager;

        private Task<AppUser> CurrentUser =>
            userManager.FindByNameAsync(HttpContext.User.Identity.Name);

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                order.UserID = CurrentUser.Result.Id;
                order.Line1 = CurrentUser.Result.Line1; 
                order.Line2 = CurrentUser.Result.Line2;
                order.City = CurrentUser.Result.City;
                order.Zip = CurrentUser.Result.Zip;

                order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }

        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }
    }
}
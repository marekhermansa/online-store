﻿using Microsoft.AspNetCore.Mvc;
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

        public async Task<ViewResult> UserOrders()
        {
            ViewBag.currentuserid = CurrentUser.Result.Id;

            AppUser user = await userManager.FindByEmailAsync(CurrentUser.Result.Email);
            if (await userManager.IsInRoleAsync(user, "Admins"))
            {
                ViewBag.isAdmin = 1;
            }
            else
            {
                ViewBag.isAdmin = 0;
            }
            return View(repository.Orders.Where(o => (o.UserID == CurrentUser.Result.Id)));
            //return View(repository.Orders.Where(o => !o.Shipped));
        }

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
            //user
            ViewBag.UserID = CurrentUser.Result.Id;
            ViewBag.Name = CurrentUser.Result.UserName;
            //address
            ViewBag.Line1 = CurrentUser.Result.Line1;
            ViewBag.Line2 = CurrentUser.Result.Line2;
            ViewBag.City = CurrentUser.Result.City;
            ViewBag.Zip = CurrentUser.Result.Zip;
            //creditcard
            ViewBag.CreditCardOwner = CurrentUser.Result.CreditCardOwner;
            ViewBag.CreditCardNumber = CurrentUser.Result.CreditCardNumber;
            ViewBag.ExpirationDate = CurrentUser.Result.ExpirationDate;

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
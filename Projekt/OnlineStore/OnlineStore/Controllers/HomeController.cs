using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OnlineStore.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System;

namespace OnlineStore.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<AppUser> userManager;

        public HomeController(UserManager<AppUser> userMgr)
        {
            userManager = userMgr;
        }

        [Authorize]
        public IActionResult Index() => View(GetData(nameof(Index)));

        //[Authorize]
        //public async Task<IActionResult> Index(string id)
        //{
        //    AppUser user = await userManager.FindByIdAsync(id);
        //    if (user != null)
        //    {
        //        return View(GetCustomData(nameof(Index)));
        //    }
        //    else
        //    {
        //        return RedirectToAction("Users", "Admin");
        //    }
        //}

        //[Authorize(Roles = "Users")]
        [Authorize]
        public IActionResult OtherAction() => View("Index",
        GetData(nameof(OtherAction)));

        [Authorize]
        public async Task<IActionResult> OtherActionCustom(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View("Index", GetCustomData(user, id, nameof(OtherActionCustom)));
            }
            else
            {
                return RedirectToAction("Users", "Admin");
            }
        }

        private Dictionary<string, object> GetCustomData(AppUser user, string id, string actionName) =>
            new Dictionary<string, object>
            {
                ["Action"] = actionName,
                ["Id"] = user.Id,
                ["User"] = user.UserName,
                ["Email"] = user.Email,

                ["Line1"] = user.Line1,
                ["Line2"] = user.Line2,
                ["City"] = user.City,
                ["Zip"] = user.Zip,

                ["CreditCardOwner"] = user.CreditCardOwner,
                ["CreditCardNumber"] = user.CreditCardNumber,
                ["ExpirationDate"] = user.ExpirationDate

                //["City"] = CurrentUser.Result.City,
                //["Qualification"] = CurrentUser.Result.Qualifications,
            };

        private Dictionary<string, object> GetData(string actionName) =>
            new Dictionary<string, object>
            {

                ["Action"] = actionName,
                ["Id"] = CurrentUser.Result.Id,
                ["Authenticated"] = HttpContext.User.Identity.IsAuthenticated,
                ["Auth Type"] = HttpContext.User.Identity.AuthenticationType,
                ["In Users Role"] = HttpContext.User.IsInRole("Users"),
                ["User"] = HttpContext.User.Identity.Name,
                ["Email"] = CurrentUser.Result.Email,

                ["Line1"] = CurrentUser.Result.Line1,
                ["Line2"] = CurrentUser.Result.Line2,
                ["City"] = CurrentUser.Result.City,
                ["Zip"] = CurrentUser.Result.Zip,

                ["CreditCardOwner"] = CurrentUser.Result.CreditCardOwner,
                ["CreditCardNumber"] = CurrentUser.Result.CreditCardNumber,
                ["ExpirationDate"] = CurrentUser.Result.ExpirationDate

                //["City"] = CurrentUser.Result.City,
                //["Qualification"] = CurrentUser.Result.Qualifications,
            };

        [Authorize]
        public async Task<IActionResult> UserProps()
        {
            return View(await CurrentUser);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UserProps(
            [Required]String city, 
            [Required]String line1,
            [Required]String line2, 
            [Required]String zip,
            [Required]String creditCardOwner,
            [Required]String creditCardNumber,
            [Required]String expirationDate)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await CurrentUser;
                user.City = city;
                user.Line1 = line1;
                user.Line2 = line2;
                user.Zip = zip;
                user.CreditCardOwner = creditCardOwner;
                user.CreditCardNumber = creditCardNumber;
                user.ExpirationDate = expirationDate;
                await userManager.UpdateAsync(user);


                return RedirectToAction("Index");
            }
            return View(await CurrentUser);
        }

        //[Authorize]
        //[HttpPost]
        //public async Task<IActionResult> UserProps(
        //    [Required]Cities city,
        //    [Required]QualificationLevels qualifications)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        AppUser user = await CurrentUser;
        //        user.City = city;
        //        user.Qualifications = qualifications;
        //        await userManager.UpdateAsync(user);
        //        return RedirectToAction("Index");
        //    }
        //    return View(await CurrentUser);
        //}

        private Task<AppUser> CurrentUser =>
            userManager.FindByNameAsync(HttpContext.User.Identity.Name);


        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Name,
                    Email = model.Email
                };
                IdentityResult result
                    = await userManager.CreateAsync(user, model.Password);
                await userManager.AddToRoleAsync(user, "Users"); //changed
                if (result.Succeeded)
                {
                    return RedirectToAction("OtherAction", "Home");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}
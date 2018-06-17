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

        //[Authorize(Roles = "Users")]
        [Authorize]
        public IActionResult OtherAction() => View("Index",
        GetData(nameof(OtherAction)));

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
    }
}
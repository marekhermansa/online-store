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

        [Authorize(Roles = "Users")]
        public IActionResult OtherAction() => View("Index",
        GetData(nameof(OtherAction)));

        private Dictionary<string, object> GetData(string actionName) =>
            new Dictionary<string, object>
            {
                ["Action"] = actionName,
                ["User"] = HttpContext.User.Identity.Name,
                ["Authenticated"] = HttpContext.User.Identity.IsAuthenticated,
                ["Auth Type"] = HttpContext.User.Identity.AuthenticationType,
                ["In Users Role"] = HttpContext.User.IsInRole("Users"),
                //["City"] = CurrentUser.Result.City,
                //["Qualification"] = CurrentUser.Result.Qualifications,
                ["Email"] = CurrentUser.Result.Email,
                ["Id"] = CurrentUser.Result.Id,
                ["Line1"] = CurrentUser.Result.Line1,
                ["Line2"] = CurrentUser.Result.Line2,
                ["City"] = CurrentUser.Result.City,
                ["Zip"] = CurrentUser.Result.Zip
            };

        [Authorize]
        public async Task<IActionResult> UserProps()
        {
            return View(await CurrentUser);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UserProps(
            [Required]String city, [Required]String line1,
            [Required]String line2, [Required]String zip)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await CurrentUser;
                user.City = city;
                user.Line1 = line1;
                user.Line2 = line2;
                user.Zip = zip;
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
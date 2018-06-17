using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using System.Threading.Tasks;

namespace OnlineStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<AppUser> userMgr,
        SignInManager<AppUser> signinMgr,
        RoleManager<IdentityRole> roleMgr)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            roleManager = roleMgr;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result =
                        await signInManager.PasswordSignInAsync(
                    user, details.Password, false, false);
                    if (result.Succeeded)
                    {
                        if (await userManager.IsInRoleAsync(user, "Admins"))
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else //if(role == null)
                        {
                            return Redirect(returnUrl ?? "/");
                        }
                    }
                }

                ModelState.AddModelError(nameof(LoginModel.Email),
                "Invalid user or password");
            }
            return View(details);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("List", "Product");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        //private UserManager<IdentityUser> userManager;
        //private SignInManager<IdentityUser> signInManager;
        //public AccountController(UserManager<IdentityUser> userMgr,

        //    SignInManager<IdentityUser> signInMgr)
        //{
        //    userManager = userMgr;
        //    signInManager = signInMgr;
        //}

        //public ViewResult Index() => View(userManager.Users); //new

        //[AllowAnonymous]
        //public ViewResult Login(string returnUrl)
        //{
        //    return View(new LoginModel
        //    {
        //        ReturnUrl = returnUrl
        //    });
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginModel loginModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        IdentityUser user =
        //        await userManager.FindByNameAsync(loginModel.Name);
        //        if (user != null)
        //        {
        //            await signInManager.SignOutAsync();
        //            if ((await signInManager.PasswordSignInAsync(user,
        //            loginModel.Password, false, false)).Succeeded)
        //            {
        //                return Redirect(loginModel?.ReturnUrl ?? "/Admin/Index");
        //            }
        //        }
        //    }
        //ModelState.AddModelError("", "Invalid name or password");
        //    return View(loginModel);
        //}

        //public async Task<RedirectResult> Logout(string returnUrl = "/")
        //{
        //    await signInManager.SignOutAsync();
        //    return Redirect(returnUrl);
        //}
    }
}
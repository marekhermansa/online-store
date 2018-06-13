using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using System.Threading.Tasks;

namespace OnlineStore.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;
        private IUserValidator<AppUser> userValidator;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;

        public AdminController(UserManager<AppUser> usrMgr,
            IUserValidator<AppUser> userValid,
            IPasswordValidator<AppUser> passValid,
            IPasswordHasher<AppUser> passwordHash)
        {
            userManager = usrMgr;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passwordHash;
        }

        //public AdminController(UserManager<AppUser> usrMgr)
        //{
        //    userManager = usrMgr;
        //}

        public ViewResult Users() => View("Users", userManager.Users); //delete if in fact redundant

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
                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
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

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("Users", userManager.Users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View("EditUser", user);
            }
            else
            {
                return RedirectToAction("Users");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email, string password)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail
                = await userValidator.ValidateAsync(userManager, user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager,
                    user, password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user,
                        password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }
                if ((validEmail.Succeeded && validPass == null)
                || (validEmail.Succeeded
                && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Users");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("EditUser", user);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }

    //[Authorize]
    //public class AdminController : Controller
    //{
    //    private IProductRepository repository;

    //    public AdminController(IProductRepository repo)
    //    {
    //        repository = repo;
    //    }

    //    public ViewResult Index() => View(repository.Products);

    //    public ViewResult Edit(int productId) =>
    //        View(repository.Products
    //            .FirstOrDefault(p => p.ProductID == productId));

    //    [HttpPost]
    //    public IActionResult Edit(Product product)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            repository.SaveProduct(product);
    //            TempData["message"] = $"{product.Name} has been saved";
    //            return RedirectToAction("Index");
    //        }
    //        else
    //        {
    //            return View(product);
    //        }
    //    }

    //    public ViewResult Create() => View("Edit", new Product());
    
    //    [HttpPost]
    //    public IActionResult Delete(int productId)
    //    {
    //        Product deletedProduct = repository.DeleteProduct(productId);
    //        if (deletedProduct != null)
    //        {
    //            TempData["message"] = $"{deletedProduct.Name} was deleted";
    //        }
    //        return RedirectToAction("Index");
    //    }
    //}
}
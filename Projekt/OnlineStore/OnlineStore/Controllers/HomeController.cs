using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace OnlineStore.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ViewResult Index() =>
            View("Index", new Dictionary<string, object> { ["Placeholder"] = "Placeholder" });
    }
}
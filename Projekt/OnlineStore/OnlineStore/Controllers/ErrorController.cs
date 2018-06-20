using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Controllers
{
    public class ErrorController : Controller
    {
        public ViewResult Error() => View();
    }
}

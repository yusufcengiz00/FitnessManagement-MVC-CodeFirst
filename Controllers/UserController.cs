using Microsoft.AspNetCore.Mvc;

namespace Project1.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

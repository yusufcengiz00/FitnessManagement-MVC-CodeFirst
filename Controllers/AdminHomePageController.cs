using Microsoft.AspNetCore.Mvc;

namespace Project1.Controllers
{
    public class AdminHomePageController : Controller
    {
        public IActionResult HomePage()
        {
            return View();
        }


    }
}

using Microsoft.AspNetCore.Mvc;

namespace Project1.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminIndex()
        {
            return RedirectToAction("HomePage" , "AdminHomePage");
        }
    }
}

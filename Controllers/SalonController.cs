using Microsoft.AspNetCore.Mvc;
using Project1.Models;

namespace Project1.Controllers
{
    public class SalonController : Controller
    {

        private readonly ApplicationDbContext _context;

        public SalonController(ApplicationDbContext dbContext)

        { _context = dbContext; }

        public IActionResult Index(string searchString)
        {
            var salonlar = from u in _context.salons
                         select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                salonlar = salonlar.Where(s => s.SalonAdi.Contains(searchString));
            }

            ViewBag.CurrentFilter = searchString;

            return View(salonlar.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(Salon salon)
        {
            _context.salons.Add(salon);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var salon = _context.salons.Find(id);
            if (salon == null)
            {
                return NotFound();
            }
            return View(salon);
        }

        [HttpPost]
        public IActionResult Edit(Salon salon)
        {
            _context.salons.Update(salon);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var salon = _context.salons.Find(id);

            if (salon == null)
            {
                return NotFound();
            }
            return View(salon);
        }

        [HttpPost]
        public IActionResult Delete(Salon salon)
        {
            _context.Remove(salon);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project1.Models;

namespace Project1.Controllers
{
    public class SupplementController : Controller
    {

        private readonly ApplicationDbContext _context;

        public SupplementController(ApplicationDbContext dbContext)

        { _context = dbContext; }

        public IActionResult Index(string searchString)
        {
          
            var supplement = from u in _context.supplements
                         select u;

            if (!String.IsNullOrEmpty(searchString))
            {

                supplement = supplement.Where(s => s.SupplementAdi.Contains(searchString));
            }

            ViewBag.CurrentFilter = searchString;
            supplement = _context.supplements.Include(a => a.salonlar).AsQueryable();

            return View(supplement.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            var salonListesi = _context.salons.ToList();

            // View tarafında kullanabilmek için ViewBag içine atıyoruz
            ViewBag.Salonlar = salonListesi;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Supplement supplement)
        {
            _context.supplements.Add(supplement);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var supplement = _context.supplements.Find(id);
            if (supplement == null)
            {
                return NotFound();
            }
            return View(supplement);
        }

        [HttpPost]
        public IActionResult Edit(Supplement supplement)
        {
            _context.supplements.Update(supplement);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var supplement = _context.supplements.Find(id);

            if (supplement == null)
            {
                return NotFound();
            }
            return View(supplement);
        }

        [HttpPost]
        public IActionResult Delete(Supplement supplement)
        {
            _context.Remove(supplement);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}

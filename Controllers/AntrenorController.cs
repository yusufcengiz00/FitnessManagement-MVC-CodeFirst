using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project1.Models;

namespace Project1.Controllers
{
    public class AntrenorController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AntrenorController(ApplicationDbContext dbContext)

        { _context = dbContext; }

        public IActionResult Index(string searchString)
        {
            var antrenorler = from u in _context.antrenors
                         select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                antrenorler = antrenorler.Where(s => s.userName.Contains(searchString));
            }

            ViewBag.CurrentFilter = searchString;
             antrenorler = _context.antrenors.Include(a => a.uyeler).AsQueryable();

            return View(antrenorler.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            var uyeListesi = _context.uyes.ToList();

            ViewBag.uyes = uyeListesi;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Antrenor antrenor)
        {
            _context.antrenors.Add(antrenor);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var antrenor = _context.antrenors.Find(id);
            if (antrenor == null)
            {
                return NotFound();
            }
            return View(antrenor);
        }

        [HttpPost]
        public IActionResult Edit(Antrenor antrenor)
        {
            _context.antrenors.Update(antrenor);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var antrenorler = _context.antrenors.Find(id);

            if (antrenorler == null)
            {
                return NotFound();
            }
            return View(antrenorler);
        }

        [HttpPost]
        public IActionResult Delete(Antrenor antrenor)
        {
            _context.Remove(antrenor);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}

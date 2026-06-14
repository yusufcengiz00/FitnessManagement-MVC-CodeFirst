using Microsoft.AspNetCore.Mvc;
using Project1.Models;

namespace Project1.Controllers
{
    public class UyeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public UyeController(ApplicationDbContext dbContext)
        
        { _context = dbContext; }

        public IActionResult Index(string searchString)
        {
            // Veritabanındaki üyeleri sorgu olarak hazırlıyoruz (hemen ToList yapmıyoruz)
            var uyeler = from u in _context.uyes
                         select u;

            // Eğer arama kutusuna bir şey yazılmışsa filtreleme yapıyoruz
            if (!String.IsNullOrEmpty(searchString))
            {
                // Üye adında veya soyadında bu kelime geçenleri filtrele
                uyeler = uyeler.Where(s => s.UyeAdi.Contains(searchString) || s.UyeSoyadi.Contains(searchString));
            }

            // Arama kelimesini kutunun içinde çakılı kalsın diye ViewBag ile sayfaya geri gönderiyoruz
            ViewBag.CurrentFilter = searchString;

            // Filtrelenmiş listeyi ToList() diyerek sayfaya gönderiyoruz
            return View(uyeler.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            // Veritabanındaki tüm salonları çekiyoruz
            var salonListesi = _context.salons.ToList();

            // View tarafında kullanabilmek için ViewBag içine atıyoruz
            ViewBag.Salonlar = salonListesi;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Uye uye)
        {
            _context.uyes.Add(uye);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var uye = _context.uyes.Find(id);
            if (uye == null)
            {
                return NotFound(); // Üye bulunamadıysa 404 hatası ver
            }
            return View(uye);
        }

        [HttpPost]
        public IActionResult Edit(Uye uye)
        {
            _context.uyes.Update(uye);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var uye = _context.uyes.Find(id);

            if (uye == null)
            {
                return NotFound();
            }
            return View(uye);
        }

        [HttpPost]
        public IActionResult Delete(Uye uye)
        {
            _context.Remove(uye);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}

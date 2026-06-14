using Microsoft.AspNetCore.Mvc;
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
            // Veritabanındaki Antrenörleri sorgu olarak hazırlıyoruz (hemen ToList yapmıyoruz)
            var antrenorler = from u in _context.antrenors
                         select u;

            // Eğer arama kutusuna bir şey yazılmışsa filtreleme yapıyoruz
            if (!String.IsNullOrEmpty(searchString))
            {
                // Antrenör Kullanıcı adında bu kelime geçenleri filtrele
                antrenorler = antrenorler.Where(s => s.userName.Contains(searchString));
            }

            // Arama kelimesini kutunun içinde çakılı kalsın diye ViewBag ile sayfaya geri gönderiyoruz
            ViewBag.CurrentFilter = searchString;

            // Filtrelenmiş listeyi ToList() diyerek sayfaya gönderiyoruz
            return View(antrenorler.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            var uyeListesi = _context.uyes.ToList();

            // View tarafında kullanabilmek için ViewBag içine atıyoruz
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

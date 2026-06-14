using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project1.Models;

namespace Project1.Controllers
{
    public class RaporController : Controller
    {

        private readonly ApplicationDbContext _context;

        public RaporController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public IActionResult Index()
        {
            // --- ÜYE İSTATİSTİKLERİ ---
            ViewBag.YasOrtalamasi = _context.uyes.Any() ? Math.Round(_context.uyes.Average(u => u.Yas), 1) : 0;
            ViewBag.EnYasliUye = _context.uyes.Any() ? _context.uyes.Max(u => u.Yas) : 0;
            ViewBag.EnGencUye = _context.uyes.Any() ? _context.uyes.Min(u => u.Yas) : 0;

            // --- ANTRENÖR İSTATİSTİKLERİ ---
            ViewBag.ToplamAntrenor = _context.antrenors.Count();
            ViewBag.OrtalamaTecrube = _context.antrenors.Any() ? Math.Round(_context.antrenors.Average(a => a.TecrübeYili), 1) : 0;

            // --- SUPPLEMENT / MAĞAZA İSTATİSTİKLERİ ---
            if (_context.supplements.Any())
            {
                var enPahaliUrun = _context.supplements.OrderByDescending(s => s.SupplementFiyati).FirstOrDefault();
                ViewBag.EnPahaliUrunAdi = enPahaliUrun.SupplementAdi;
                ViewBag.EnPahaliUrunFiyat = enPahaliUrun.SupplementFiyati;
                ViewBag.ToplamUrunCesidi = _context.supplements.Count();
            }
            else
            {
                ViewBag.EnPahaliUrunAdi = "Ürün Yok";
                ViewBag.EnPahaliUrunFiyat = 0;
                ViewBag.ToplamUrunCesidi = 0;
            }

            // --- SALON İSTATİSTİKLERİ ---
            ViewBag.ToplamSalon = _context.salons.Count();

            return View();
        }

       
    }
}

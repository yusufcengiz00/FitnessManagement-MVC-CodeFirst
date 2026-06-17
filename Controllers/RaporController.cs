using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project1.Models;
using System;
using System.Linq;

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

            //1: Üye yaş ortalaması ve yaş uç değerleri
            ViewBag.YasOrtalamasi = _context.uyes.Any() ? Math.Round(_context.uyes.Average(u => u.Yas), 1) : 0;
            ViewBag.EnYasliUye = _context.uyes.Any() ? _context.uyes.Max(u => u.Yas) : 0;
            ViewBag.EnGencUye = _context.uyes.Any() ? _context.uyes.Min(u => u.Yas) : 0;

            //2: Genel supplement / ürün çeşitliliği istatistikleri
            if (_context.supplements.Any())
            {
                var enPahaliUrun = _context.supplements.OrderByDescending(s => s.SupplementFiyati).FirstOrDefault();
                ViewBag.EnPahaliUrunAdi = enPahaliUrun?.SupplementAdi ?? "Ürün Yok";
                ViewBag.EnPahaliUrunFiyat = enPahaliUrun?.SupplementFiyati ?? 0;
                ViewBag.ToplamUrunCesidi = _context.supplements.Count();
            }
            else
            {
                ViewBag.EnPahaliUrunAdi = "Ürün Yok";
                ViewBag.EnPahaliUrunFiyat = 0;
                ViewBag.ToplamUrunCesidi = 0;
            }

            // JOINLI SORGULAR

            //1: Antrenörler ve Sorumlu Oldukları Üyeler (Tablona göre Antrenör -> Üye'ye bağlı)
            var antrenorUyeListesi = _context.antrenors
                .Join(_context.uyes,
                    ant => ant.UyeID,    // antrenors tablosundaki FK
                    uye => uye.UyeID,    // uyes tablosundaki PK
                    (ant, uye) => new { AntrenorAdi = ant.userName, UyeAdi = uye.UyeAdi + " " + uye.UyeSoyadi })
                .ToList();
            ViewBag.AntrenorUyeListesi = antrenorUyeListesi;

            //2: Üyeler ve Kayıtlı Oldukları Salonlar
            var uyeSalonListesi = _context.uyes
                .Join(_context.salons,
                    uye => uye.SalonID,   // uyes tablosundaki FK
                    salon => salon.SalonID, // salons tablosundaki PK
                    (uye, salon) => new { UyeAdi = uye.UyeAdi + " " + uye.UyeSoyadi, SalonAdi = salon.SalonAdi })
                .ToList();
            ViewBag.UyeSalonListesi = uyeSalonListesi;

            //3: Supplementler ve Satıldıkları Salonlar
            var supplementSalonListesi = _context.supplements
                .Join(_context.salons,
                    sup => sup.SalonID,     // supplements tablosundaki FK
                    salon => salon.SalonID, // salons tablosundaki PK
                    (sup, salon) => new { SupplementAdi = sup.SupplementAdi, Fiyat = sup.SupplementFiyati, SalonAdi = salon.SalonAdi })
                .ToList();
            ViewBag.SupplementSalonListesi = supplementSalonListesi;

            // 3. JOIN + GROUP BY SORGU

            // Hangi salonda kaç adet üye var? (Salonlar ile Üyeleri joinleyip SalonAdi'na göre gruplar)
            var salonUyeSayilari = _context.salons
                .Join(_context.uyes,
                    salon => salon.SalonID,
                    uye => uye.SalonID,
                    (salon, uye) => new { SalonAdi = salon.SalonAdi, UyeID = uye.UyeID })
                .GroupBy(x => x.SalonAdi)
                .Select(g => new { SalonAdi = g.Key, UyeSayisi = g.Count() })
                .ToList();
            ViewBag.SalonUyeSayilari = salonUyeSayilari;

            return View();
        }
    }
}
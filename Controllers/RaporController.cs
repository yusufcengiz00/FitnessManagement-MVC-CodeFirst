using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Project1.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
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

        [HttpGet]
        public IActionResult ExportToPdf()
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            // --- Verilerin Hazırlanması (Hata almamak için RAM'e ToList ile çekiyoruz) ---
            var yasOrtalamasi = _context.uyes.Any() ? Math.Round(_context.uyes.Average(u => u.Yas), 1) : 0;
            var enYasliUye = _context.uyes.Any() ? _context.uyes.Max(u => u.Yas) : 0;
            var enGencUye = _context.uyes.Any() ? _context.uyes.Min(u => u.Yas) : 0;

            string enPahaliUrunAdi = "Ürün Yok";
            double enPahaliUrunFiyat = 0;
            int toplamUrunCesidi = 0;

            if (_context.supplements.Any())
            {
                var enPahaliUrun = _context.supplements.OrderByDescending(s => s.SupplementFiyati).FirstOrDefault();
                enPahaliUrunAdi = enPahaliUrun?.SupplementAdi ?? "Ürün Yok";
                enPahaliUrunFiyat = enPahaliUrun?.SupplementFiyati ?? 0;
                toplamUrunCesidi = _context.supplements.Count();
            }

            var antrenorUyeListesi = _context.antrenors
                .Join(_context.uyes, ant => ant.UyeID, uye => uye.UyeID,
                    (ant, uye) => new { AntrenorAdi = ant.userName, UyeAdi = uye.UyeAdi + " " + uye.UyeSoyadi }).ToList();

            var uyeSalonListesi = _context.uyes
                .Join(_context.salons, uye => uye.SalonID, salon => salon.SalonID,
                    (uye, salon) => new { UyeAdi = uye.UyeAdi + " " + uye.UyeSoyadi, SalonAdi = salon.SalonAdi }).ToList();

            var supplementSalonListesi = _context.supplements
                .Join(_context.salons, sup => sup.SalonID, salon => salon.SalonID,
                    (sup, salon) => new { SupplementAdi = sup.SupplementAdi, Fiyat = sup.SupplementFiyati, SalonAdi = salon.SalonAdi }).ToList();

            var salonUyeSayilari = _context.salons
                .Join(_context.uyes, salon => salon.SalonID, uye => uye.SalonID, (salon, uye) => new { salon.SalonAdi })
                .GroupBy(x => x.SalonAdi)
                .Select(g => new { SalonAdi = g.Key, UyeSayisi = g.Count() }).ToList();

            // --- QuestPDF Tasarımı ---
            var pdfDocument = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                    page.Header()
                        .Text("Genel Sistem ve İstatistik Raporu")
                        .SemiBold().FontSize(18).FontColor(Colors.Blue.Medium);

                    page.Content().Column(column =>
                    {
                        // 1. Özet İstatistikler Tablosu
                        column.Item().PaddingTop(0.5f, Unit.Centimetre).Text("1. Genel İstatistikler").Bold().FontSize(12);
                        column.Item().PaddingTop(0.2f, Unit.Centimetre).Table(table =>
                        {
                            table.ColumnsDefinition(columns => { columns.RelativeColumn(); columns.RelativeColumn(); });
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Metrik").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Değer").Bold();
                            });
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text("Üye Yaş Ortalaması");
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(yasOrtalamasi.ToString());
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text("En Yaşlı Üye Yaşı");
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(enYasliUye.ToString());
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text("En Genç Üye Yaşı");
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(enGencUye.ToString());
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text("Toplam Ürün Çeşitliliği");
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(toplamUrunCesidi.ToString());
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text("En Pahalı Ürün");
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text($"{enPahaliUrunAdi} ({enPahaliUrunFiyat} TL)");
                        });

                        // 2. Salon Üye Sayıları Tablosu
                        column.Item().PaddingTop(0.5f, Unit.Centimetre).Text("2. Salon Üye Dağılımı").Bold().FontSize(12);
                        column.Item().PaddingTop(0.2f, Unit.Centimetre).Table(table =>
                        {
                            table.ColumnsDefinition(columns => { columns.RelativeColumn(); columns.ConstantColumn(100); });
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Salon Adı").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Üye Sayısı").Bold();
                            });
                            foreach (var item in salonUyeSayilari)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.SalonAdi ?? "-");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.UyeSayisi.ToString());
                            }
                        });

                        // 3. Antrenör ve Üye Eşleşme Tablosu
                        column.Item().PaddingTop(0.5f, Unit.Centimetre).Text("3. Antrenör ve Sorumlu Olunan Üyeler").Bold().FontSize(12);
                        column.Item().PaddingTop(0.2f, Unit.Centimetre).Table(table =>
                        {
                            table.ColumnsDefinition(columns => { columns.RelativeColumn(); columns.RelativeColumn(); });
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Antrenör Kullanıcı Adı").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Sorumlu Olduğu Üye").Bold();
                            });
                            foreach (var item in antrenorUyeListesi)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.AntrenorAdi ?? "-");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.UyeAdi ?? "-");
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(x => { x.Span("Sayfa "); x.CurrentPageNumber(); });
                });
            });

            var pdfBytes = pdfDocument.GeneratePdf();
            return File(pdfBytes, "application/pdf", $"Genel_Rapor_{DateTime.Now:yyyyMMdd}.pdf");
        }

        [HttpGet]
        public IActionResult ExportToExcel()
        {
            ExcelPackage.License.SetNonCommercialPersonal("Backend softito");

            // --- Verilerin Çekilmesi ---
            var yasOrtalamasi = _context.uyes.Any() ? Math.Round(_context.uyes.Average(u => u.Yas), 1) : 0;
            var enYasliUye = _context.uyes.Any() ? _context.uyes.Max(u => u.Yas) : 0;
            var enGencUye = _context.uyes.Any() ? _context.uyes.Min(u => u.Yas) : 0;

            string enPahaliUrunAdi = "Ürün Yok";
            double enPahaliUrunFiyat = 0;
            int toplamUrunCesidi = 0;

            if (_context.supplements.Any())
            {
                var enPahaliUrun = _context.supplements.OrderByDescending(s => s.SupplementFiyati).FirstOrDefault();
                enPahaliUrunAdi = enPahaliUrun?.SupplementAdi ?? "Ürün Yok";
                enPahaliUrunFiyat = enPahaliUrun?.SupplementFiyati ?? 0;
                toplamUrunCesidi = _context.supplements.Count();
            }

            var antrenorUyeListesi = _context.antrenors
                .Join(_context.uyes, ant => ant.UyeID, uye => uye.UyeID,
                    (ant, uye) => new { AntrenorAdi = ant.userName, UyeAdi = uye.UyeAdi + " " + uye.UyeSoyadi }).ToList();

            var uyeSalonListesi = _context.uyes
                .Join(_context.salons, uye => uye.SalonID, salon => salon.SalonID,
                    (uye, salon) => new { UyeAdi = uye.UyeAdi + " " + uye.UyeSoyadi, SalonAdi = salon.SalonAdi }).ToList();

            var salonUyeSayilari = _context.salons
                .Join(_context.uyes, salon => salon.SalonID, uye => uye.SalonID, (salon, uye) => new { salon.SalonAdi })
                .GroupBy(x => x.SalonAdi)
                .Select(g => new { SalonAdi = g.Key, UyeSayisi = g.Count() }).ToList();

            using (var package = new ExcelPackage())
            {
                // SAYFA 1: Genel Özet ve İstatistikler
                var ws1 = package.Workbook.Worksheets.Add("Genel İstatistikler");
                ws1.Cells[1, 1].Value = "İstatistik Başlığı";
                ws1.Cells[1, 2].Value = "Değer";

                using (var r = ws1.Cells[1, 1, 1, 2])
                {
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(41, 128, 185));
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                }

                ws1.Cells[2, 1].Value = "Üye Yaş Ortalaması"; ws1.Cells[2, 2].Value = yasOrtalamasi;
                ws1.Cells[3, 1].Value = "En Yaşlı Üye"; ws1.Cells[3, 2].Value = enYasliUye;
                ws1.Cells[4, 1].Value = "En Genç Üye"; ws1.Cells[4, 2].Value = enGencUye;
                ws1.Cells[5, 1].Value = "Toplam Ürün Çeşidi"; ws1.Cells[5, 2].Value = toplamUrunCesidi;
                ws1.Cells[6, 1].Value = "En Pahalı Ürün"; ws1.Cells[6, 2].Value = $"{enPahaliUrunAdi} ({enPahaliUrunFiyat} TL)";
                ws1.Cells[ws1.Dimension.Address].AutoFitColumns();

                // SAYFA 2: Salon Üye Dağılımları
                var ws2 = package.Workbook.Worksheets.Add("Salon Dağılımları");
                ws2.Cells[1, 1].Value = "Salon Adı";
                ws2.Cells[1, 2].Value = "Üye Sayısı";
                using (var r = ws2.Cells[1, 1, 1, 2])
                {
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(41, 128, 185));
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                }
                int row = 2;
                foreach (var item in salonUyeSayilari)
                {
                    ws2.Cells[row, 1].Value = item.SalonAdi;
                    ws2.Cells[row, 2].Value = item.UyeSayisi;
                    row++;
                }
                ws2.Cells[ws2.Dimension.Address].AutoFitColumns();

                // SAYFA 3: İlişkili Listeler (Antrenör - Üye)
                var ws3 = package.Workbook.Worksheets.Add("Antrenör-Üye İlişkisi");
                ws3.Cells[1, 1].Value = "Antrenör Adı";
                ws3.Cells[1, 2].Value = "Sorumlu Olduğu Üye";
                using (var r = ws3.Cells[1, 1, 1, 2])
                {
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(41, 128, 185));
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                }
                row = 2;
                foreach (var item in antrenorUyeListesi)
                {
                    ws3.Cells[row, 1].Value = item.AntrenorAdi;
                    ws3.Cells[row, 2].Value = item.UyeAdi;
                    row++;
                }
                ws3.Cells[ws3.Dimension.Address].AutoFitColumns();

                var fileBytes = package.GetAsByteArray();
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Genel_Sistem_Raporu_{DateTime.Now:yyyyMMdd}.xlsx");
            }
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Project1.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Project1.Controllers
{
    public class UyeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public UyeController(ApplicationDbContext dbContext)
        
        { _context = dbContext; }

        [HttpGet]
        public IActionResult ExportToPdf()
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var products = _context.uyes
                .Select(u => new
                {
                    u.UyeID,
                    AdSoyad = (u.UyeAdi ?? "") + " " + (u.UyeSoyadi ?? ""),
                    SalonAdi = u.Salonlar != null ? u.Salonlar.SalonAdi : "-"
                })
                .ToList();

            var pdfDocument = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    page.Header()
                        .Text("Üye Listesi Raporu")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingTop(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                                columns.ConstantColumn(120);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("ID").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Üye Adı Soyadı").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Kayıtlı Salon").Bold();
                            });

                            foreach (var item in products)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.UyeID.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.AdSoyad);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.SalonAdi);
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Sayfa ");
                            x.CurrentPageNumber();
                        });
                });
            });

            var pdfBytes = pdfDocument.GeneratePdf();
            return File(pdfBytes, "application/pdf", $"Uye_Listesi_{DateTime.Now:yyyyMMdd}.pdf");
        }

        [HttpGet]
        public IActionResult ExportToExcel()
        {
            ExcelPackage.License.SetNonCommercialPersonal("Backend softito");

            var products = _context.uyes
                .Select(u => new
                {
                    u.UyeID,
                    AdSoyad = (u.UyeAdi ?? "") + " " + (u.UyeSoyadi ?? ""),
                    SalonAdi = u.Salonlar != null ? u.Salonlar.SalonAdi : "-"
                })
                .ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Üye Listesi");

                worksheet.Cells[1, 1].Value = "Üye ID";
                worksheet.Cells[1, 2].Value = "Üye Adı Soyadı";
                worksheet.Cells[1, 3].Value = "Kayıtlı Salon";

                using (var range = worksheet.Cells[1, 1, 1, 3])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(41, 128, 185));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                int rowNumber = 2;
                foreach (var item in products)
                {
                    worksheet.Cells[rowNumber, 1].Value = item.UyeID;
                    worksheet.Cells[rowNumber, 2].Value = item.AdSoyad;
                    worksheet.Cells[rowNumber, 3].Value = item.SalonAdi;

                    rowNumber++;
                }

                if (worksheet.Dimension != null)
                {
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                }

                var fileBytes = package.GetAsByteArray();
                string fileName = $"Uye_Listesi_{DateTime.Now:yyyyMMdd}.xlsx";

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
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

            uyeler = _context.uyes.Include(a => a.Salonlar).AsQueryable();
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

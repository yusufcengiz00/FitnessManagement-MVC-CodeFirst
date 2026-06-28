using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Project1.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Project1.Controllers
{
    public class AntrenorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AntrenorController(ApplicationDbContext dbContext)

        { _context = dbContext; }

        [HttpGet]
        public IActionResult ExportToPdf()
        {
            var products = _context.antrenors
                .Select(a => new
                {
                    a.AntrenorID,
                    a.userName,
                    UyeSayisi = a.uyeler != null ? 1 : 0
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

                    // Üst Bilgi (Header)
                    page.Header()
                        .Text("Antrenör Listesi Raporu")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    // İçerik (Tablo Oluşturma)
                    page.Content()
                        .PaddingTop(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            // Sütun genişliklerini tanımlayın
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);  // ID sütunu genişliği
                                columns.RelativeColumn();    // Antrenör adı sütunu (esnek)
                                columns.ConstantColumn(100); // Kayıtlı Üye Sayısı sütunu genişliği
                            });

                            // Tablo Başlıkları (Header Row)
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("ID").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Antrenör Kullanıcı Adı").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Toplam Üye").Bold();
                            });

                            // Veri Satırları (Döngü ile verileri basıyoruz)
                            foreach (var item in products)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.AntrenorID.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.userName ?? "-");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.UyeSayisi.ToString());
                            }
                        });

                    // Alt Bilgi (Footer)
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Sayfa ");
                            x.CurrentPageNumber();
                        });
                });
            });

            // 3. PDF'i byte dizisine çevirip tarayıcıya indirtme
            var pdfBytes = pdfDocument.GeneratePdf();
            return File(pdfBytes, "application/pdf", $"Antrenor_Listesi_{DateTime.Now:yyyyMMdd}.pdf");
        }

        [HttpGet]
        public IActionResult ExportToExcel()
        {
            ExcelPackage.License.SetNonCommercialPersonal("Backend softito");

            // 2. Veri tabanından güncel listenizi çekin
            var products = _context.antrenors
                .Select(a => new
                {
                    a.AntrenorID,
                    a.userName,
                    // Koleksiyon olmadığı için null değilse 1, null ise 0 kabul ediyoruz
                    UyeSayisi = a.uyeler != null ? 1 : 0
                })
                .ToList();

            // 3. Bellekte (Memory) boş bir Excel dosyası oluşturun
            using (var package = new ExcelPackage())
            {
                // Excel içinde "Antrenör Listesi" adında bir sayfa aç
                var worksheet = package.Workbook.Worksheets.Add("Antrenör Listesi");

                // 4. Tablo Başlıklarını Yazın (1. Satır)
                worksheet.Cells[1, 1].Value = "Antrenor ID";
                worksheet.Cells[1, 2].Value = "Antrenör Kullanıcı Adı";
                worksheet.Cells[1, 3].Value = "Kayıtlı Üye Sayısı";

                // 5. Başlık Satırını Şıklaştırın (Arka plan rengi, kalın yazı vb.)
                using (var range = worksheet.Cells[1, 1, 1, 3]) // 1. satır, 1'den 3. sütuna kadar seç
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(41, 128, 185));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                // 6. Verileri Döngü ile Excel Satırlarına Basın
                int rowNumber = 2; // Veriler 2. satırdan başlayacak
                foreach (var item in products)
                {
                    worksheet.Cells[rowNumber, 1].Value = item.AntrenorID;
                    worksheet.Cells[rowNumber, 2].Value = item.userName;
                    worksheet.Cells[rowNumber, 3].Value = item.UyeSayisi;

                    rowNumber++;
                }

                // 7. Sütun genişliklerini içeriğe göre otomatik ayarla
                if (worksheet.Dimension != null)
                {
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                }

                // 8. Excel dosyasını byte dizisine çevirip tarayıcıya fırlat
                var fileBytes = package.GetAsByteArray();
                string fileName = $"Antrenor_Listesi_{DateTime.Now:yyyyMMdd}.xlsx";

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        public IActionResult Index(string searchString)
        {
            var antrenorler = _context.antrenors.Include(a => a.uyeler).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                antrenorler = antrenorler.Where(s => s.userName.Contains(searchString));
            }

            ViewBag.CurrentFilter = searchString;

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

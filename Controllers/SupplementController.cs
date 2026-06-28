using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Project1.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Project1.Controllers
{
    public class SupplementController : Controller
    {

        private readonly ApplicationDbContext _context;

        public SupplementController(ApplicationDbContext dbContext)

        { _context = dbContext; }

        [HttpGet]
        public IActionResult ExportToPdf()
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var products = _context.supplements
                .Select(s => new
                {
                    s.SupplementID,
                    s.SupplementAdi,
                    SalonAdi = s.salonlar != null ? s.salonlar.SalonAdi : "-"
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
                        .Text("Supplement Listesi Raporu")
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
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Supplement Adı").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Bulunduğu Salon").Bold();
                            });

                            foreach (var item in products)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.SupplementID.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.SupplementAdi ?? "-");
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
            return File(pdfBytes, "application/pdf", $"Supplement_Listesi_{DateTime.Now:yyyyMMdd}.pdf");
        }

        [HttpGet]
        public IActionResult ExportToExcel()
        {
            ExcelPackage.License.SetNonCommercialPersonal("Backend softito");

            var products = _context.supplements
                .Select(s => new
                {
                    s.SupplementID,
                    s.SupplementAdi,
                    SalonAdi = s.salonlar != null ? s.salonlar.SalonAdi : "-"
                })
                .ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Supplement Listesi");

                worksheet.Cells[1, 1].Value = "Supplement ID";
                worksheet.Cells[1, 2].Value = "Supplement Adı";
                worksheet.Cells[1, 3].Value = "Bulunduğu Salon";

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
                    worksheet.Cells[rowNumber, 1].Value = item.SupplementID;
                    worksheet.Cells[rowNumber, 2].Value = item.SupplementAdi;
                    worksheet.Cells[rowNumber, 3].Value = item.SalonAdi;

                    rowNumber++;
                }

                if (worksheet.Dimension != null)
                {
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                }

                var fileBytes = package.GetAsByteArray();
                string fileName = $"Supplement_Listesi_{DateTime.Now:yyyyMMdd}.xlsx";

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        public IActionResult Index(string searchString)
        {
            var supplement = _context.supplements.Include(a => a.salonlar).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                supplement = supplement.Where(s => s.SupplementAdi.Contains(searchString));
            }

            ViewBag.CurrentFilter = searchString;

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

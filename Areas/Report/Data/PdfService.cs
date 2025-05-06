//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.html.simpleparser;
using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using System.IO;

namespace Standus_5_0.Areas.Report.Data
{
    public class PdfService
    {
        private readonly IWebHostEnvironment _env;

        public PdfService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string GeneratePDF(string report_topdf, string chartBase64, string filename)
        {
                        
            // Generate PDF output path
            string timestamp = DateTime.Now.ToString("HHmmssfff");
            string outputFileName = $"{System.IO.Path.GetFileNameWithoutExtension(filename)}_{timestamp}.pdf";
            string outputPath = System.IO.Path.Combine(_env.WebRootPath, "temp_pdf_files", outputFileName);

            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath)!);

            // Prepare and add HTML table
            string htmlContent = $"<html><body>";
            htmlContent = htmlContent + "<img src=" + chartBase64 + " />";
            htmlContent = htmlContent + "<table border='1'>";
            htmlContent = htmlContent + report_topdf.Replace(Environment.NewLine, "").Replace("\"", "'");
            htmlContent = htmlContent +"</table></body></html>";
           

            using (var htmlStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(htmlContent)))
            using (var outputStream = new FileStream(outputPath, FileMode.Create))
            {
                var writer = new PdfWriter(outputStream);
                var pdfDoc = new PdfDocument(writer);

                // potrait pdf

                var pagesize = new PageSize(
                    new Rectangle(
                        PageSize.A4.GetX(),
                        PageSize.A4.GetY(),
                        PageSize.A4.GetWidth(), 
                        PageSize.A4.GetHeight())
                    );

                //pdfDoc.SetDefaultPageSize(PageSize.A4);
                pdfDoc.SetDefaultPageSize(pagesize);
                //TODO : remove comments for landscape pdf
                //pdfDoc.SetDefaultPageSize(PageSize.A4.Rotate()); // Landscape

                

                HtmlConverter.ConvertToPdf(htmlStream, pdfDoc); // ✅ expects Stream + PdfDocument

                pdfDoc.Close();
            }


            //document.Close();


            return outputPath;
        }
    }

}

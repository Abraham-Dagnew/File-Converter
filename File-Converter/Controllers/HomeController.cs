using File_Converter.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace File_Converter.Controllers
{
    public class HomeController : Controller
    {
        private readonly Converter _converter;

        public HomeController(Converter converter)
        {
            _converter = converter;
        }

        [HttpPost]
        public async Task<IActionResult> ConvertFile(IFormFile file, string conversionType)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Please select a file to upload.";
                return View("Index");
            }

            try
            {
                string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadDir);
                string filePath = Path.Combine(uploadDir, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "converted");

                string convertedFilePath = string.Empty;

                if (conversionType == "PDFToWord")
                {
                    convertedFilePath = await _converter.ConvertPdfToWord(filePath, outputDir);
                }
                else if (conversionType == "WordToPDF")
                {
                    convertedFilePath = await _converter.ConvertWordToPdf(filePath, outputDir);
                }

                if (string.IsNullOrEmpty(convertedFilePath))
                {
                    ViewBag.Message = "Conversion failed.";
                    return View("Index");
                }

                var fileBytes = System.IO.File.ReadAllBytes(convertedFilePath);
                var fileDownloadName = Path.GetFileName(convertedFilePath);
                return File(fileBytes, "application/octet-stream", fileDownloadName);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "An error occurred while converting the file.";
                return View("Index");
            }
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
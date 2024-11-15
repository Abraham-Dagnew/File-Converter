using Aspose.Words;
using System.IO;
using System.Threading.Tasks;

namespace File_Converter.Services
{
    public class Converter
    {
        public async Task<string> ConvertAsync(IFormFile file, string conversionType)
        {
            string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadDir);

            string filePath = Path.Combine(uploadDir, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string convertedFilePath = Path.ChangeExtension(filePath, conversionType == "WordToPDF" ? ".pdf" : ".docx");

            if (conversionType == "WordToPDF")
            {
                var doc = new Document(filePath);
                doc.Save(convertedFilePath, SaveFormat.Pdf);
            }
            else if (conversionType == "PDFToWord")
            {
                var doc = new Document(filePath);
                doc.Save(convertedFilePath, SaveFormat.Docx);
            }
            else
            {
                return null; 
            }

            return convertedFilePath;
        }
    }
}
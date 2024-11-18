using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Freeware;  

namespace File_Converter.Services
{
    public class Converter
    {
        public async Task<string> ConvertPdfToWord(string filePath, string outputDir)
        {
            Directory.CreateDirectory(outputDir);

            try
            {
                byte[] pdfBytes = File.ReadAllBytes(filePath);

                using (MemoryStream pdfStream = new MemoryStream(pdfBytes))
                {
                    byte[] docxBytes = Pdf2Docx.Convert(pdfStream);  

                    string outputFilePath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(filePath) + ".docx");
                    await File.WriteAllBytesAsync(outputFilePath, docxBytes);

                    return outputFilePath;  
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while converting PDF to DOCX: " + ex.Message);
            }
        }

        public async Task<string> ConvertWordToPdf(string filePath, string outputDir)
        {
            string libreOfficePath = @"C:\Program Files\LibreOffice\program\soffice.exe";

            Directory.CreateDirectory(outputDir);

            string arguments = $"--headless --convert-to pdf --outdir \"{outputDir}\" \"{filePath}\"";

            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = libreOfficePath,
                    Arguments = arguments,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (var process = Process.Start(processStartInfo))
                {
                    await process.WaitForExitAsync();
                }

                string convertedFilePath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(filePath) + ".pdf");
                return convertedFilePath;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while converting Word to PDF: " + ex.Message);
            }
        }
    }
}

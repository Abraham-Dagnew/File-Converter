using Microsoft.AspNetCore.Http;

namespace File_Converter.Models
{
    public class ConvertFileViewModel
    {
        public IFormFile File { get; set; }
        public string ConversionType { get; set; }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Helpers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TextRecognitionController : ControllerBase
    {
        [HttpPost]
        public async Task<List<OcrResults>> PostDocument([FromBody] TextRecognitionSettings inputSettings)
        {
            var documentPath = FileHelper.GetProcessingDocumentPath(inputSettings.FileName);
            var language = LanguageHelper.GetLanguageByString(inputSettings.Language);
            var results = new List<OcrResults>();
            if (FileHelper.IsPdf(documentPath))
            {
                var pdf = new ProcessingPdf(documentPath, language);
                pdf.GeneratePagesForPdfDoc();
                results = pdf.Process(inputSettings);
            }
            else
            {
                var img = new ProcessingImage(documentPath, language);
                results = img.Process(inputSettings);
            }

            FileHelper.CleanUpFilesByName(documentPath);
            return results;
        }
    }
}
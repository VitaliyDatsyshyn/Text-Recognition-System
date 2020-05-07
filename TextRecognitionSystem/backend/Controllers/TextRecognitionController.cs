using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TextRecognitionController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostImage([FromBody] object filePath)
        {
            ProcessingImage img = new ProcessingImage(filePath.ToString().Replace(@"\\", @"\"), Enums.Languages.English);
            img.Binarize();
            img.RemoveNoise();
            img.AdjustContrast();
            var angle = img.PredictTurningAngle();
            img.Rotate(angle);
            img.OcrImage(OCR.OcrEngines.Tesseract);
            if (!img.IsOCRedTextValid())
            {
                img.Rotate(180);
                img.OcrImage(OCR.OcrEngines.Tesseract);
            }

            img.CorrectOCRedText(@"Resources\EnglishDictionary.json");

            return Ok(img.OCRedText);
        }
    }
}
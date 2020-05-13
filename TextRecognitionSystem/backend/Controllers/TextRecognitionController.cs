using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using backend.Helpers;
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
        public async Task<string> PostImage([FromBody] TextRecognitionSettings inputSettings)
        {
            var documentPath = FileHelper.GetProcessingDocumentPath(inputSettings.FileName);
            var language = LanguageHelper.GetLanguageByString(inputSettings.Language);
            ProcessingImage img = new ProcessingImage(documentPath, language);
            //img.Binarize();
            //img.RemoveNoise();
            //img.AdjustContrast();
            //var angle = img.PredictTurningAngle();
            //img.Rotate(angle);
            //img.OcrImage(OCR.OcrEngines.Tesseract);
            //if (!img.IsOCRedTextValid())
            //{
            //    img.Rotate(180);
            // img.OcrImage(OCR.OcrEngines.Tesseract);
            //}

            //img.CorrectOCRedText(@"Resources\EnglishDictionary.json");
            return "";
        }
    }
}
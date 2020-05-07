using backend.Enums;
using backend.OCR.Concrete;
using System;
using System.Collections.Generic;

namespace backend.OCR
{
    public class OcrEnginesFactory
    {
        private static Languages _language = Languages.English;

        private IDictionary<OcrEngines, Func<IOcrEngine>> Creators =
            new Dictionary<OcrEngines, Func<IOcrEngine>>()
            {
                { OcrEngines.Tesseract, () => new TesseractOcrEngine(_language)},
                { OcrEngines.None, () => throw new ArgumentException("Ocr Engine is not specified.")}
            };

        public OcrEnginesFactory(Languages language)
        {
            _language = language;
        }

        public IOcrEngine CreateInstance(OcrEngines enumModuleName)
        {
            return Creators[enumModuleName]();
        }
    }
}

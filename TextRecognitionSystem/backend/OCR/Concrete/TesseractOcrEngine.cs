using backend.Enums;
using backend.Helpers;
using System;
using Tesseract;

namespace backend.OCR.Concrete
{
    public class TesseractOcrEngine : IOcrEngine
    {
        private TesseractEngine _tesseractEngine = null;

        public TesseractOcrEngine(Languages language)
        {
            var datapath = @"Resources\";
            _tesseractEngine = new TesseractEngine(datapath, LanguageHelper.GetLanguageString(language));
        }

        public string GetText(string imageFilePath)
        {
            if (_tesseractEngine != null)
            {
                using (var img = Pix.LoadFromFile(imageFilePath))
                {
                    using (var page = _tesseractEngine.Process(img))
                    {
                        return page.GetText();
                    }
                }
            }
            else
            {
                throw new ArgumentException("Ocr engine not initialized");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //remove managed resources
            }

            if (_tesseractEngine != null)
            {
                _tesseractEngine.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TesseractOcrEngine()
        {
            Dispose(false);
        }
    }
}

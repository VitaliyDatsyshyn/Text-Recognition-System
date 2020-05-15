using backend.Enums;
using backend.Helpers;
using backend.ImagePreprocessing;
using backend.OCR;
using backend.TextPostprocessing;
using System.Collections.Generic;
using System.IO;

namespace backend.Models
{
    public class ProcessingImage
    {
        private string _currentImagePath;
        private Languages _language;
        private string _ocredText;
        private string _inputFileName;

        public ProcessingImage(string imagePath, Languages language)
        {
            _currentImagePath = imagePath;
            _language = language;
            _inputFileName = new FileInfo(_currentImagePath).Name;
        }

        public List<OcrResults> Process(TextRecognitionSettings settings)
        {
            var results = new List<OcrResults>();
            if (settings.IsBinarizationEnable)
                Binarize();

            if (settings.IsNoiseRemovalEnable)
                RemoveNoise();

            if (settings.IsContrastAdjusmentEnable)
                AdjustContrast();

            if (settings.IsRotationEnable)
            {
                var angle = PredictTurningAngle();
                if (angle != 0)
                    Rotate(angle);
            }

            OcrImage(OcrEngines.Tesseract);

            if (settings.IsRotationEnable && !IsOCRedTextValid())
            {
                Rotate(180);
                OcrImage(OcrEngines.Tesseract);
            }

            var paragraphs = TextHelper.SplitTextIntoParagraphs(_ocredText);
            for(var i = 0; i < paragraphs.Length; i++)
            {
                if (settings.IsWordsCorrectionEnable)
                    paragraphs[i] = CorrectOCRedText(@"Resources\EnglishDictionary.json", paragraphs[i]);

                string[] typeKeyWords = new string[0];
                if (settings.IsTypeRecognitionEnable)
                {
                    var type = PredictType();
                    typeKeyWords = GetSpecialTypeKeyWords(type);
                }
                var ocrResult = new OcrResults { FileName = _inputFileName };
                if (settings.KeyWords.Length > 0)
                    ocrResult.KeyWords = WordsFinder.GetCountedKeyWordsFromText(paragraphs[i], settings.KeyWords);

                if (typeKeyWords.Length > 0)
                {
                    var foundTypeKeyWords = WordsFinder.GetCountedKeyWordsFromText(paragraphs[i], typeKeyWords);
                    if (string.IsNullOrEmpty(ocrResult.KeyWords))
                        ocrResult.KeyWords = foundTypeKeyWords;
                    else
                        ocrResult.KeyWords += foundTypeKeyWords;
                }

                if (settings.KeyWords.Length > 0 && string.IsNullOrEmpty(ocrResult.KeyWords))
                    continue;
                else
                    ocrResult.OcredText = paragraphs[i];

                results.Add(ocrResult);
            }

            return results;
        }

        private void Binarize()
        {
            FileHelper.CheckFilePathExisting(_currentImagePath);
            var binarizedImagePath = FileHelper.InsertMarkIntoFileName(_currentImagePath, "binarized");
            var binarizator = new Binarizator(_currentImagePath, binarizedImagePath);
            if (binarizator.Binarize())
                _currentImagePath = binarizedImagePath;
        }

        private void RemoveNoise()
        {
            FileHelper.CheckFilePathExisting(_currentImagePath);
            var unnoisedImagePath = FileHelper.InsertMarkIntoFileName(_currentImagePath, "unnoised");
            var noiseRemover = new NoiseRemover(_currentImagePath, unnoisedImagePath);
            if (noiseRemover.RemoveNoise())
                _currentImagePath = unnoisedImagePath;
        }

        private void AdjustContrast()
        {
            FileHelper.CheckFilePathExisting(_currentImagePath);
            var contrastedImagePath = FileHelper.InsertMarkIntoFileName(_currentImagePath, "contrasted");
            var contrastAdjuster = new ContrastAdjuster(_currentImagePath, contrastedImagePath, 180);
            if (contrastAdjuster.AdjustContrast())
                _currentImagePath = contrastedImagePath;
        }

        private void Rotate(int angle)
        {
            FileHelper.CheckFilePathExisting(_currentImagePath);
            var rotatedImagePath = FileHelper.InsertMarkIntoFileName(_currentImagePath, "rotated on " + angle.ToString());
            var rotator = new Rotator(_currentImagePath, rotatedImagePath, angle);
            if (rotator.Rotate())
                _currentImagePath = rotatedImagePath;
        }

        private int PredictTurningAngle(string pathToMLModel = "")
        {
            if (string.IsNullOrEmpty(pathToMLModel))
                pathToMLModel = @"Resources\TurningAngleModel.zip";

            var predictor = new TurningAnglePredictor(_currentImagePath, pathToMLModel);
            return predictor.Predict();
        }

        private void OcrImage(OcrEngines ocrEngine)
        {
            _ocredText = new OcrEnginesFactory(_language).CreateInstance(ocrEngine).GetText(_currentImagePath);
        }

        private string CorrectOCRedText(string pathToDictionary, string text)
        {
            FileHelper.CheckFilePathExisting(pathToDictionary);
            var dictionary = JsonHelper.LoadListOfString(pathToDictionary);
            var wordsCorrector = new WordsCorrector(dictionary, text);
            return wordsCorrector.CorrectText();
        }

        private bool IsOCRedTextValid()
        {
            return TextChecker.IsTextValid(_language, _ocredText);
        }

        private string PredictType(string pathToMLModel = "")
        {
            if (string.IsNullOrEmpty(pathToMLModel))
                pathToMLModel = @"Resources\TypePredictionModel.zip";

            var typePredictor = new DocTypePredictor(_ocredText, pathToMLModel);
            return typePredictor.Predict();
        }

        private string[] GetSpecialTypeKeyWords(string type)
        {
            if (type == "Наказ")
                return new[] { "наказ", "наказую", "призупинити" };
            else if (type == "Договір")
                return new[] { "договір", "предмет договору", "обов'язок", "умови" };
            else
                return new string[0];
        }
    }
}

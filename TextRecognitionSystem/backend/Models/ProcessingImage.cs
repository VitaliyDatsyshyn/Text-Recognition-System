using backend.Enums;
using backend.Helpers;
using backend.ImagePreprocessing;
using backend.OCR;
using backend.TextPostprocessing;

namespace backend.Models
{
    public class ProcessingImage
    {
        private string _currentImagePath;
        private Languages _language;
        public string OCRedText { get; private set; }

        public ProcessingImage(string imagePath, Languages language)
        {
            _currentImagePath = imagePath;
            _language = language;
        }

        public void Binarize()
        {
            FileHelper.CheckFilePathExisting(_currentImagePath);
            var binarizedImagePath = FileHelper.InsertMarkIntoFileName(_currentImagePath, "binarized");
            var binarizator = new Binarizator(_currentImagePath, binarizedImagePath);
            if (binarizator.Binarize())
                _currentImagePath = binarizedImagePath;
        }

        public void RemoveNoise()
        {
            FileHelper.CheckFilePathExisting(_currentImagePath);
            var unnoisedImagePath = FileHelper.InsertMarkIntoFileName(_currentImagePath, "unnoised");
            var noiseRemover = new NoiseRemover(_currentImagePath, unnoisedImagePath);
            if (noiseRemover.RemoveNoise())
                _currentImagePath = unnoisedImagePath;
        }

        public void AdjustContrast()
        {
            FileHelper.CheckFilePathExisting(_currentImagePath);
            var contrastedImagePath = FileHelper.InsertMarkIntoFileName(_currentImagePath, "contrasted");
            var contrastAdjuster = new ContrastAdjuster(_currentImagePath, contrastedImagePath, 180);
            if (contrastAdjuster.AdjustContrast())
                _currentImagePath = contrastedImagePath;
        }

        public void Rotate(int angle)
        {
            FileHelper.CheckFilePathExisting(_currentImagePath);
            var rotatedImagePath = FileHelper.InsertMarkIntoFileName(_currentImagePath, "rotated on " + angle.ToString());
            var rotator = new Rotator(_currentImagePath, rotatedImagePath, angle);
            if (rotator.Rotate())
                _currentImagePath = rotatedImagePath;
        }

        public int PredictTurningAngle(string pathToMLModel = "")
        {
            if (string.IsNullOrEmpty(pathToMLModel))
                pathToMLModel = @"Resources\TurningAngleModel.zip";

            var predictor = new TurningAnglePredictor(_currentImagePath, pathToMLModel);
            return predictor.Predict();
        }

        public void OcrImage(OcrEngines ocrEngine)
        {
            OCRedText = new OcrEnginesFactory(_language).CreateInstance(ocrEngine).GetText(_currentImagePath);
        }

        public void CorrectOCRedText(string pathToDictionary)
        {
            FileHelper.CheckFilePathExisting(pathToDictionary);
            var dictionary = JsonHelper.LoadListOfString(pathToDictionary);
            var wordsCorrector = new WordsCorrector(dictionary, OCRedText);
            OCRedText = wordsCorrector.CorrectText();
        }

        public bool IsOCRedTextValid()
        {
            return TextChecker.IsTextValid(_language, OCRedText);
        }
    }
}

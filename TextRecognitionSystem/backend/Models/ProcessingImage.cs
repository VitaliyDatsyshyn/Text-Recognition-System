using backend.Helpers;
using backend.ImagePreprocessing;

namespace backend.Models
{
    public class ProcessingImage
    {
        private string _currentImagePath;

        public ProcessingImage(string imagePath)
        {
            _currentImagePath = imagePath;
        }

        public void Binarize()
        {
            FileHelper.CheckFilePathExisting(_currentImagePath);
            var binarizedImagePath = FileHelper.InsertMarkIntoFileName(_currentImagePath, "binarized");
            var binarizator = new Binarizator(_currentImagePath, binarizedImagePath);
            if (binarizator.Binarize())
                _currentImagePath = binarizedImagePath;
        }

        public void RomoveNoise()
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

        public int PredictTurningAngle(string pathToMLModel)
        {
            if (string.IsNullOrEmpty(pathToMLModel))
                pathToMLModel = @"Resources\TurningAngleModel.zip";

            var predictor = new TurningAnglePredictor(_currentImagePath, pathToMLModel);
            return predictor.Predict();
        }
    }
}

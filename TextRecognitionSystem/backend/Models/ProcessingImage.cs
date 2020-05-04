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
            FileHelper.CheckImagePathExisting(_currentImagePath);
            var binarizedImagePath = FileHelper.InsertMarkIntoFileName(_currentImagePath, "binarized");
            var binarizator = new Binarizator(_currentImagePath, binarizedImagePath);
            if (binarizator.Binarize())
                _currentImagePath = binarizedImagePath;
        }

        public void RomoveNoise()
        {
            FileHelper.CheckImagePathExisting(_currentImagePath);
            var unnoisedImagePath = FileHelper.InsertMarkIntoFileName(_currentImagePath, "unnoised");
            var noiseRemover = new NoiseRemover(_currentImagePath, unnoisedImagePath);
            if (noiseRemover.RemoveNoise())
                _currentImagePath = unnoisedImagePath;
        }

        public void AdjustContrast()
        {
            FileHelper.CheckImagePathExisting(_currentImagePath);
            var contrastedImagePath = FileHelper.InsertMarkIntoFileName(_currentImagePath, "contrasted");
            var contrastAdjuster = new ContrastAdjuster(_currentImagePath, contrastedImagePath, 180);
            if (contrastAdjuster.AdjustContrast())
                _currentImagePath = contrastedImagePath;
        }

        public void Rotate()
        {
            FileHelper.CheckImagePathExisting(_currentImagePath);
            var rotatedImagePath = FileHelper.InsertMarkIntoFileName(_currentImagePath, "rotated");
            var turningAngle = 50; //PREDICT TURNING ANGLE HERE
            var rotator = new Rotator(_currentImagePath, rotatedImagePath, turningAngle);
            if (rotator.Rotate())
                _currentImagePath = rotatedImagePath;
        }
    }
}

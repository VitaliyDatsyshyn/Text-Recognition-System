using System;
using System.Collections.Generic;
using System.Drawing;

namespace backend.ImagePreprocessing
{
    public class NoiseRemover
    {
        private Bitmap _inputImage;
        private string _outputImagePath;

        public NoiseRemover(string inputImagePath, string outputImagePath)
        {
            _inputImage = new Bitmap(inputImagePath);
            _outputImagePath = outputImagePath;
        }

        public bool RemoveNoise()
        {
            try
            {
                for (int x = 2; x < _inputImage.Width - 1; x++)
                {
                    for (int y = 2; y < _inputImage.Height - 1; y++)
                    {
                        var neighboringPixels = ExtractNeighboringPixels(x, y);
                        double averageNeighbourPixelsValue = CalculateAveragePixelsValue(neighboringPixels);
                        var centerPixel = _inputImage.GetPixel(x, y);
                        double centerPixelValue = (0.3 * (centerPixel.R) + 0.59 * (centerPixel.G) + 0.11 * (centerPixel.B)) + 1;
                        if (CheckRemovalCondition(centerPixelValue, neighboringPixels))
                        {
                            int pixelR = ValidatePixelValue((int)(centerPixel.R * (averageNeighbourPixelsValue / centerPixelValue)));
                            int pixelG = ValidatePixelValue((int)(centerPixel.G * (averageNeighbourPixelsValue / centerPixelValue)));
                            int pixelB = ValidatePixelValue((int)(centerPixel.B * (averageNeighbourPixelsValue / centerPixelValue)));
                            _inputImage.SetPixel(x, y, Color.FromArgb(pixelR, pixelG, pixelB));
                        }
                    }
                }

                _inputImage.Save(_outputImagePath);

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private List<double> ExtractNeighboringPixels(int x, int y)
        {
            var neighboringPixels = new List<double>();
            for (int i = -1; i < 2; i++)
                for (int j = -1; j < 2; j++)
                    if (i != 0 && j != 0)
                        neighboringPixels.Add(CalculatePixelValue(x + i, y + j));

            return neighboringPixels;
        }

        private double CalculatePixelValue(int x, int y)
        {
            var pixel = _inputImage.GetPixel(x, y);
            return (0.3 * pixel.R + 0.59 * pixel.G + 0.11 * pixel.B);
        }

        private double CalculateAveragePixelsValue(List<double> pixels)
        {
            double pixelAvg = 0;
            foreach (var pixel in pixels)
                pixelAvg += pixel;

            return pixelAvg / pixels.Count;
        }

        private bool CheckRemovalCondition(double centeredPixelValue, List<double> neighboringPixels)
        {
            int successCounter = 0;
            foreach (var neighboringPixel in neighboringPixels)
                if (Math.Abs(centeredPixelValue - neighboringPixel) > 210)
                    successCounter++;

            return successCounter >= neighboringPixels.Count - 2;
        }

        private int ValidatePixelValue(int pixelValue)
        {
            if (pixelValue > 255)
                return 255;
            else if (pixelValue < 0)
                return 0;
            else
                return pixelValue;
        }
    }
}

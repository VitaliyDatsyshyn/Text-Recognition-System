using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace backend.ImagePreprocessing
{
    public class Binarizator
    {
        private Bitmap _inputImage;
        private string _outputImagePath;

        public Binarizator(string inputImagePath, string outputImagePath)
        {
            _inputImage = new Bitmap(inputImagePath);
            _outputImagePath = outputImagePath;
        }

        public bool Binarize()
        {
            try
            {
                Bitmap binarizedImage = new Bitmap(_inputImage.Width, _inputImage.Height);
                Rectangle rect = new Rectangle(0, 0, _inputImage.Width, _inputImage.Height);

                BitmapData imageData = _inputImage.LockBits(rect, ImageLockMode.ReadWrite, _inputImage.PixelFormat);
                BitmapData binarizedImageData = binarizedImage.LockBits(rect, ImageLockMode.ReadWrite, _inputImage.PixelFormat);

                IntPtr ptr = imageData.Scan0;
                IntPtr binarizedPtr = binarizedImageData.Scan0;

                int bytes = Math.Abs(imageData.Stride) * _inputImage.Height;
                byte[] rgbValues = new byte[bytes];

                int binarizedBytes = Math.Abs(binarizedImageData.Stride) * binarizedImage.Height;
                byte[] binarizedRgbValues = new byte[binarizedBytes];

                Marshal.Copy(ptr, rgbValues, 0, bytes);
                Marshal.Copy(binarizedPtr, binarizedRgbValues, 0, binarizedBytes);

                binarizedRgbValues = BinarizeRgbValues(rgbValues);

                Marshal.Copy(rgbValues, 0, ptr, bytes);
                Marshal.Copy(binarizedRgbValues, 0, binarizedPtr, binarizedBytes);

                _inputImage.UnlockBits(imageData);
                binarizedImage.UnlockBits(binarizedImageData);
                binarizedImage.Save(_outputImagePath);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private byte[] BinarizeRgbValues(byte[] rgbValues)
        {
            byte binarizationThreshold = 160;
            byte whiteRgbValue = 255;
            var binarizedgbValues = new byte[rgbValues.Length];
            for (int i = 1; i < rgbValues.Length; i++)
            {
                if (rgbValues[i] < binarizationThreshold)
                    binarizedgbValues[i] = rgbValues[i];
                else
                    binarizedgbValues[i] = whiteRgbValue;
            }

            return binarizedgbValues;
        }
    }
}

using System;
using System.Drawing;

namespace backend.ImagePreprocessing
{
    public class Rotator
    {
        private Bitmap _inputImage;
        private string _outputImagePath;
        private int _angle;

        public Rotator(string inputImagePath, string outputImagePath, int angle)
        {
            _inputImage = new Bitmap(inputImagePath);
            _outputImagePath = outputImagePath;
            _angle = angle;
        }

        public bool Rotate()
        {
            try
            {
                Bitmap rotatedDoc = new Bitmap(_inputImage.Width, _inputImage.Height);
                using (Graphics g = Graphics.FromImage(rotatedDoc))
                {
                    g.TranslateTransform(_inputImage.Width / 2, _inputImage.Height / 2);
                    g.RotateTransform(360 - _angle);
                    g.TranslateTransform(-_inputImage.Width / 2, -_inputImage.Height / 2);
                    g.DrawImage(_inputImage, new Point(0, 0));
                }

                rotatedDoc.Save(_outputImagePath);

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}

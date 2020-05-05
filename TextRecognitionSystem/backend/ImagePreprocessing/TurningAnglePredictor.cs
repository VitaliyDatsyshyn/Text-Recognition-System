using backend.Helpers;
using Microsoft.ML;
using System;
using backend.Models.ML;

namespace backend.ImagePreprocessing
{
    public class TurningAnglePredictor
    {
        private string _pathToDocument;
        private string _pathToModel;

        public TurningAnglePredictor(string pathToDoc, string pathToModel)
        {
            _pathToDocument = pathToDoc;
            _pathToModel = pathToModel;
        }

        public int Predict()
        {
            try
            {
                FileHelper.CheckFilePathExisting(_pathToDocument);
                FileHelper.CheckFilePathExisting(_pathToModel);
                MLContext mlContext = new MLContext(seed: 1);
                ITransformer loadedModel = mlContext.Model.Load(_pathToModel, out var columns);
                var predictor = mlContext.Model.CreatePredictionEngine<TurningAngleInput, TurningAnglePrediction>(loadedModel);
                var predictionResult = predictor.Predict(new TurningAngleInput() { PathToDoc = _pathToDocument });
                return Convert.ToInt32(predictionResult.PredictedTurningAngle);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
    }
}

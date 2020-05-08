using backend.Helpers;
using backend.Models.ML;
using Microsoft.ML;
using System;

namespace backend.TextPostprocessing
{
    public class DocTypePredictor
    {
        private string _docText;
        private string _pathToModel;

        public DocTypePredictor(string docText, string pathToModel)
        {
            _docText = docText;
            _pathToModel = pathToModel;
        }

        public string Predict()
        {
            try
            {
                FileHelper.CheckFilePathExisting(_pathToModel);
                MLContext mlContext = new MLContext(seed: 1);
                ITransformer loadedModel = mlContext.Model.Load(_pathToModel, out var columns);
                var predictor = mlContext.Model.CreatePredictionEngine<DocTypeInput, DocsTypePrediction>(loadedModel);
                var predictionResult = predictor.Predict(new DocTypeInput() { DocText = _docText });
                return predictionResult.DocType;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return String.Empty;
            }
        }
    }
}

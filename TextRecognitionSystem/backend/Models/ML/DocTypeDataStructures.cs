using Microsoft.ML.Data;

namespace backend.Models.ML
{
    public class DocTypeInput
    {
        [LoadColumn(0)]
        public string DocType;

        [LoadColumn(1)]
        public string DocText;
    }

    public class DocsTypePrediction
    {
        [ColumnName("PredictedLabel")]
        public string DocType;

        [ColumnName("Score")]
        public float[] Score;
    }
}

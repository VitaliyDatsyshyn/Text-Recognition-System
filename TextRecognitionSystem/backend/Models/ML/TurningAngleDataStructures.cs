using Microsoft.ML.Data;

namespace backend.Models.ML
{
    public class TurningAngleInput
    {
        [LoadColumn(0)]
        public string PathToDoc { get; set; }

        [LoadColumn(1)]
        public string TurningAngle { get; set; }
    }

    public class TurningAnglePrediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedTurningAngle { get; set; }
    }
}

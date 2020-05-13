using System.Collections.Generic;

namespace backend.Models
{
    public class TextRecognitionSettings
    {
        public string FileName { get; set; }
        public string Language { get; set; }
        public bool IsBinarizationEnable { get; set; }
        public bool IsNoiseRemovalEnable { get; set; }
        public bool IsContrastAdjusmentEnable { get; set; }
        public bool IsRotationEnable { get; set; }
        public bool IsTypeRecognitionEnable { get; set; }
        public bool IsWordsCorrectionEnable { get; set; }
        public string[] KeyWords { get; set; }
    }
}

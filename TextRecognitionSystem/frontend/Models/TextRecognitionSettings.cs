using frontend.Enums;
using System.Collections.Generic;

namespace frontend.Models
{
    public class TextRecognitionSettings
    {
        public string FileName { get; set; }
        public string Language { get; private set; }
        public bool IsBinarizationEnable { get; set; }
        public bool IsNoiseRemovalEnable { get; set; }
        public bool IsContrastAdjusmentEnable { get; set; }
        public bool IsRotationEnable { get; set; }
        public bool IsTypeRecognitionEnable { get; set; }
        public bool IsWordsCorrectionEnable { get; set; }
        public List<string> KeyWords { get; set; }

        public TextRecognitionSettings()
        {
            FileName = string.Empty;
            Language = "eng";
            IsBinarizationEnable = true;
            IsNoiseRemovalEnable = true;
            IsContrastAdjusmentEnable = true;
            IsRotationEnable = true;
            IsTypeRecognitionEnable = true;
            IsWordsCorrectionEnable = true;
            KeyWords = new List<string>();
        }

        public void SetLanguage(Languages language)
        {
            if (language == Languages.English)
                Language = "eng";
            else if (language == Languages.Ukrainian)
                Language = "ukr";
        }
    }
}

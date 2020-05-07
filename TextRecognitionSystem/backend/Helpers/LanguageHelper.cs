using backend.Enums;
using System;

namespace backend.Helpers
{
    public class LanguageHelper
    {
        public static string GetLanguageString(Languages language)
        {
            if (language == Languages.English)
                return "eng";
            else if (language == Languages.Ukrainian)
                return "ukr";
            else
                throw new NotImplementedException("Not supported language");
        }
    }
}

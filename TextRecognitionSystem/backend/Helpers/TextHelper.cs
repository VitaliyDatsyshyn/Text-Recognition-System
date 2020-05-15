using System.Linq;
using System.Text.RegularExpressions;

namespace backend.Helpers
{
    public class TextHelper
    {
        public static string[] SplitTextIntoParagraphs(string text)
        {
            return Regex.Split(text, "((\r\n){2}|\r{2}|\n{2})").Where(parag => !string.IsNullOrEmpty(parag.Trim())).ToArray();
        }
    }
}

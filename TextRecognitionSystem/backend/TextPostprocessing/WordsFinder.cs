namespace backend.TextPostprocessing
{
    public class WordsFinder
    {
        public static string GetCountedKeyWordsFromText(string text, string[] keyWords)
        {
            var foundKeyWords = string.Empty;
            var words = text.ToLower().Replace("\n", " ").Split(' ');
            foreach(var keyWord in keyWords) {
                var keyWordCount = 0;
                foreach (var word in words)
                {
                    if (word.Trim() == keyWord.ToLower().Trim())
                        keyWordCount++;
                }

                if (keyWordCount > 0)
                    foundKeyWords += $"{keyWord}({keyWordCount}); ";
            }

            return foundKeyWords;
        }
    }
}

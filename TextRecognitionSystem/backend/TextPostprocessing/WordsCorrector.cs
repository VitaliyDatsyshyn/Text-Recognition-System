using Fastenshtein;
using System;
using System.Collections.Generic;
using System.Linq;

namespace backend.TextPostprocessing
{
    public class WordsCorrector
    {
        private List<string> _dictionaryWords;

        private string _text;

        public WordsCorrector(List<string> dictionary, string text)
        {
            _dictionaryWords = dictionary;
            _text = text;
        }

        public string CorrectText()
        {
            var wordsFromText = _text.Trim().Replace("\n", " ").Split(' ').ToList();
            var minWordLength = 2;
            for (int i = 0; i < wordsFromText.Count; i++)
            {
                if (wordsFromText[i].Length > minWordLength)
                    wordsFromText[i] = CorrectWord(wordsFromText[i]);
            }

            return String.Join(" ", wordsFromText);
        }
        private string CorrectWord(string word)
        {
            var puctuationMark = String.Empty;
            if (IsTheLastSymbolPunctuationMark(word))
            {
                puctuationMark = word.Substring(word.Length - 1, 1);
                word = word.Substring(0, word.Length - 1);
            }

            return CompareWordWithDictionaryByLevenshtein(word) + puctuationMark;

        }

        private bool IsTheLastSymbolPunctuationMark(string word)
        {
            var punctuationMarks = new[] { ',', '.', ';', ':', '?', '!'};
            int lastIndex = word.Length - 1;
            return punctuationMarks.Contains(word[lastIndex]);

        }

        private string CompareWordWithDictionaryByLevenshtein(string word)
        {
            int minLevenshteinDistance = word.Length;
            string matchedWord = String.Empty;
            Levenshtein levenshtein = new Levenshtein(word.ToLower());
            foreach (var dictionaryWord in _dictionaryWords)
            {
                int distance = levenshtein.DistanceFrom(dictionaryWord);
                if (distance < minLevenshteinDistance)
                {
                    minLevenshteinDistance = distance;
                    matchedWord = dictionaryWord;
                }
            }

            return minLevenshteinDistance > 0 && minLevenshteinDistance <= word.Length / 3 ? matchedWord : word;
        }
    }
}

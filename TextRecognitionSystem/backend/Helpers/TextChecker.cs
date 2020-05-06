using backend.Enums;
using Fastenshtein;
using System;
using System.Collections.Generic;

namespace backend.Helpers
{
    public class TextChecker
    {
        private static List<string> _dictionary;
        public static bool IsTextValid(Languages language, string text)
        {
            InitDictionary(language);
            var wordsFromText = text.Trim().ToLower().Replace("\n", " ").Split(' ');
            int differenceBetweenWordsAndDictionary = 0;
            foreach (var word in wordsFromText)
                differenceBetweenWordsAndDictionary += LevenshteinDistanceBetweenWordAndDictionary(word);

            return differenceBetweenWordsAndDictionary <= text.Length / 3;
        }

        private static void InitDictionary(Languages language)
        {
            string pathToDictionary = String.Empty;
            if (language == Languages.English)
                pathToDictionary = @"Resources\EnglishDictionary.json";
            else if (language == Languages.Ukrainian)
                pathToDictionary = @"Resources\UkrainianDictionary.json"; 

            _dictionary = JsonHelper.LoadListOfString(pathToDictionary);
        }

        private static int LevenshteinDistanceBetweenWordAndDictionary(string word)
        {
            int minLevenshteinDistance = word.Length;
            Levenshtein levenshtein = new Levenshtein(word.ToLower());
            foreach (var dictionaryWord in _dictionary)
            {
                int distance = levenshtein.DistanceFrom(dictionaryWord);
                if (distance < minLevenshteinDistance)
                    minLevenshteinDistance = distance;
            }

            return minLevenshteinDistance;
        }
    }
}

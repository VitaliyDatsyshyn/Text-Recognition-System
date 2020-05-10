using frontend.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace frontend.Helpers
{
    public class FileHelper
    {
        public static List<string> PickFiles(string filter)
        {
            var picker = new OpenFileDialog
            {
                Filter = filter,
                Multiselect = true
            };
            bool? result = picker.ShowDialog();
            if (result.HasValue)
                return picker.FileNames.ToList();
            else
                return new List<string>();
        }
        public static string PickFile(string filter)
        {
            var picker = new OpenFileDialog
            {
                Filter = filter
            };
            bool? result = picker.ShowDialog();
            if (result.HasValue)
                return picker.FileName;
            else
                return string.Empty;
        }

        public static List<string> GetWordsFromFile(string file)
        {
            if (File.Exists(file))
            {
                var text = File.ReadAllText(file);
                return text.Replace("\n", " ").Split(' ').ToList();
            }
            else
            {
                return new List<string>();
            }
        }

        public static string SaveResultsAsTxt(List<OcrResults> results)
        {
            var fileName = "Ocr Results " + DateTime.Now.ToString("dd.MM.yyyy") + ".txt";
            var headers = "File Name\tRecognized Text\tKey Words\n";
            File.WriteAllText(fileName, headers);
            foreach(var result in results)
            {
                File.AppendAllText(fileName, result.FileName + "\t" + result.OcredText.Replace('\n', ' ') + "\t" + result.KeyWords + "\n");
            }

            return fileName;
        }

        public static string SaveResultsAsCsv(List<OcrResults> results)
        {
            var fileName = "Ocr Results " + DateTime.Now.ToString("dd.MM.yyyy") + ".csv";
            var headers = "File Name,Recognized Text,Key Words\n";
            File.WriteAllText(fileName, headers);
            foreach (var result in results)
            {
                File.AppendAllText(fileName, result.FileName + "," + result.OcredText.Replace('\n', ' ') + "," + result.KeyWords + "\n");
            }

            return fileName;
        }
    }
}

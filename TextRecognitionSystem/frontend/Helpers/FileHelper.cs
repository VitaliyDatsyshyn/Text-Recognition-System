using Microsoft.Win32;
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
    }
}

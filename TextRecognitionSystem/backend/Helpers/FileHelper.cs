using System;
using System.IO;

namespace backend.Helpers
{
    public class FileHelper
    {
        public static void CheckFilePathExisting(string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
                throw new NullReferenceException("File path is empty");
            else if (!File.Exists(filePath))
                throw new InvalidDataException("Invalid file path! File doesn`t exist!");
        }

        public static string InsertMarkIntoFileName(string filePath, string mark)
        {
            var file = new FileInfo(filePath);
            return Path.Combine(file.DirectoryName, Path.GetFileNameWithoutExtension(file.FullName) + " " + mark + file.Extension);
        }

        public static void CleanUpByFileName(string filePath)
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), Path.GetFileNameWithoutExtension(filePath) + "*");
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }
    }
}

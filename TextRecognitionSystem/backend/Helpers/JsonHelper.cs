using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace backend.Helpers
{
    public class JsonHelper
    {
        public static List<string> LoadListOfString(string path)
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<List<string>>(json);
            }
            else
            {
                return new List<string>();
            }
        }
    }
}

using frontend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace frontend.Helpers
{
    public class ServerHelper
    {
        public static bool UploadFileToServer(string filePath)
        {
            IFormFile formFile;
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                formFile = new FormFile(fs, fs.Position, new FileInfo(filePath).Length, "ProcessingDocument", filePath);
                using (var client = new HttpClient())
                {
                    var fileName = formFile.FileName;
                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(new StreamContent(formFile.OpenReadStream())
                        {
                            Headers =
                            {
                                ContentLength = formFile.Length,
                                ContentType = new MediaTypeHeaderValue("multipart/form-data")
                            }
                        }, "File", fileName);

                        var response = client.PostAsync("https://localhost:44345/api/uploadfile", content).Result;
                        return response.IsSuccessStatusCode;
                    }
                }
            }
        }

        public static IEnumerable<OcrResults> GetOcrResults(TextRecognitionSettings settings)
        {
            var jsonSettings = JsonConvert.SerializeObject(settings);
            var stringContent = new StringContent(jsonSettings, Encoding.UTF8, "application/json");
            string response = string.Empty;
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(20);
                response = client.PostAsync("https://localhost:44345/api/textrecognition", stringContent).Result.Content.ReadAsStringAsync().Result;
            }

            return JsonConvert.DeserializeObject<IEnumerable<OcrResults>>(response);
        }
    }
}

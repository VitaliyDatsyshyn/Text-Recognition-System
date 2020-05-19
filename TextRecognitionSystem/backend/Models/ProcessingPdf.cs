using backend.Enums;
using backend.Helpers;
using GhostscriptSharp;
using PdfSharp.Pdf.IO;
using System.Collections.Generic;
using System.IO;

namespace backend.Models
{
    public class ProcessingPdf
    {
        private string _pathToPdf;
        private List<ProcessingImage> _pdfPages;
        private Languages _language;

        public ProcessingPdf(string pathToPdf, Languages language)
        {
            _pathToPdf = pathToPdf;
            _language = language;
            _pdfPages = new List<ProcessingImage>();
        }

        public void GeneratePagesForPdfDoc()
        {
            var pdfPagesPath = ParsePdfDocumentToImages();
            foreach (var pagePath in pdfPagesPath)
            {
                _pdfPages.Add(new ProcessingImage(pagePath, _language));
            }
        }

        public List<OcrResults> Process(TextRecognitionSettings settings)
        {
            var results = new List<OcrResults>();
            foreach(var page in _pdfPages)
            {
                results.AddRange(page.Process(settings));
            }

            return results;
        }

        private IEnumerable<string> ParsePdfDocumentToImages()
        {
            FileHelper.CheckFilePathExisting(_pathToPdf);
            int pdfPageCount = GetPdfPageCount();
            string pdfName = Path.GetFileNameWithoutExtension(_pathToPdf);
            string imagePathTemplate = new FileInfo(_pathToPdf).DirectoryName + @"\" + pdfName + "_0%d.tiff";
            GhostscriptWrapper.GeneratePageThumbs(_pathToPdf, imagePathTemplate, 1, pdfPageCount, 200, 200);
            return Directory.GetFiles(new FileInfo(_pathToPdf).DirectoryName, pdfName + "_*");
        }

        private int GetPdfPageCount()
        {
            var pdfDocument = PdfReader.Open(_pathToPdf);
            int pdfPageCount = pdfDocument.PageCount;
            pdfDocument.Close();
            return pdfPageCount;
        }
    }
}

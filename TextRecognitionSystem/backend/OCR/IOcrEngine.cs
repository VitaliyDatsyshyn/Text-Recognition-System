using System;

namespace backend.OCR
{
    public interface IOcrEngine : IDisposable
    {
        string GetText(string filePath);
    }
}

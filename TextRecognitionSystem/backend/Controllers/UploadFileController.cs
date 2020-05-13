using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var filePath = Path.Combine("ProcessingDocuments", new FileInfo(file.FileName).Name);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Ok();
        }
    }
}
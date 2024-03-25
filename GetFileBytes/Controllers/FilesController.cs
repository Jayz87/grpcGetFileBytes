using FileBytes.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FileBytes.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> logger;
        private readonly IGetFileClientService getFileClientService;

        public FilesController(ILogger<FilesController> logger, IGetFileClientService getFileClientService)
        {
            this.logger = logger;
            this.getFileClientService = getFileClientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFileByName([Required] string fileName)
        {
            try
            {
                var result = await getFileClientService.GetFileAsync(fileName);
                if (result == null)
                    return NotFound();
                return File(result.ToArray(), "application/octet-stream", "filename.extension");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, nameof(GetFileByName));
                return null;
            }

        }
    }
}

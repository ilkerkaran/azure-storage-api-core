using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using azure_storage_api_core.Models;
using azure_storage_api_core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace azure_storage_api_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private IStorageService storageService;
        public UploadController(IStorageService storageService)
        {
            this.storageService = storageService;

        }


        [HttpPost()]
        public async Task<IActionResult> Upload([FromForm, Required] FormInfoModel formInfo, [FromForm] IFormFile file)
        {
            var serviceResult = string.Empty;
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileData = ms.ToArray();
                    serviceResult = await storageService.Upload(fileData,formInfo.SecureStore);
                    // act on the Base64 data
                }
            }
            if (!string.IsNullOrEmpty(serviceResult))
                return Ok(serviceResult);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
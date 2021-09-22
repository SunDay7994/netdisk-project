using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using sunday_netdisk_backend.Models;
using sunday_netdisk_backend.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace sunday_netdisk_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        BasicOperationService basicOperationService = new BasicOperationService();

        AuthorityService authorityService = new AuthorityService();

        [HttpPost]
        public async Task<IActionResult> PostUpload(FileDto fileDto)
        {
            List<IFormFile> files = fileDto.files;

            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.Combine(fileDto.filePath, Path.GetTempFileName());

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return Ok(new { count = files.Count, size });
        }

        [HttpGet]
        [Route("list")]
        public List<FileListVo> GetFileList(string filePath)
        {
            return basicOperationService.GetFileList(authorityService.ProcessPath(filePath));
        }

    }
}

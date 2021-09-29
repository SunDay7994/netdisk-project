using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sunday_netdisk_backend.Models;
using sunday_netdisk_backend.Services;
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

        [HttpPost("upload")]
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
        public List<FileListVo> GetFileList(string filePath)
        {
            return basicOperationService.GetFileList(basicOperationService.ProcessPathDto(filePath));
        }

        [HttpPost("create")]
        public async Task<IActionResult> PostCreateDir(string filePath, string fileName)
        {
            await basicOperationService.CreateDir(basicOperationService.ProcessPathDto(filePath), fileName);
            return Ok();
        }

        [HttpPost("rename")]
        public ActionResult PostRenameFile(string filePath, string newName)
        {
            basicOperationService.RenameFile(basicOperationService.ProcessPathDto(filePath), newName);
            return Ok();
        }

        [HttpPost("copy")]
        public ActionResult PostCopyFile(string[] soursePaths, string destinationPath)
        {
            basicOperationService.CopyFile(soursePaths, destinationPath);
            return Ok();
        }

        [HttpPost("cut")]
        public ActionResult PostMoveFile(string[] soursePaths, string destinationPath)
        {
            basicOperationService.MoveFile(soursePaths, destinationPath);
            return Ok();
        }

        [HttpPost("delete")]
        public ActionResult DeleteFile([FromBody] string[] filePaths)
        {
            basicOperationService.DeleteFile(filePaths);
            return Ok();
        }

    }
}

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
    public class UploadController
    {
        [HttpGet]
        public void Get(FileChunkDto chunk)
        {
            FileTransferService service = new FileTransferService();
            if (service.testFile(chunk))
                return;
        }
    }
}

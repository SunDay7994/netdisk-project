using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sunday_netdisk_backend.Models
{
    public class FileDto
    {
        public List<IFormFile> files;
        public string filePath;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sunday_netdisk_backend.Models
{
    public class FileListVo
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public DateTime LastModified { get; set; }
        public bool IsDirectory { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sunday_netdisk_backend.Models
{
    public class FileChunkDto
    {
        public int FlowChunkNumber { get; set; }
        public int FlowTotalChunks { get; set; }
        public int FlowChunkSize { get; set; }
        public int FlowTotalSize { get; set; }
        public string FlowIdentifier { get; set; }
        public string FlowFilename { get; set; }
        public string FlowRelativePath { get; set; }
        public string destinationPath { get; set; }
    }
}

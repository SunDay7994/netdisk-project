using sunday_netdisk_backend.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace sunday_netdisk_backend.Services
{
    public class FileTransferService
    {
        const int maxFileSize = -1;
        const string root = "D:\\Test";

        public string processTempPath(string identifier, int chunkNumber, bool isTemp = false)
        {
            var chunkString = chunkNumber.ToString().PadLeft(5, '0');
            string relativePath = "";
            if (isTemp)
            {
                relativePath = identifier + "/chunk-" + chunkString + ".downloading";
            }
            else
            {
                relativePath = identifier + "/chunk-" + chunkString + ".chunk";
            }
            var tempPath = Path.Combine(root, relativePath);
            File.Create(tempPath);
            return tempPath;
        }

        public string cleanIdentifier(string identifier)
        {
            var re = new Regex("[^0-9A-Za-z_-]/g");
            return re.Replace(identifier, "");            
        }

        public void validateRequest(FileChunkDto chunk)
        {
            var flowIdentifier = cleanIdentifier(chunk.FlowIdentifier);

            if (chunk.FlowChunkNumber == 0 ||
            chunk.FlowChunkSize == 0 ||
            chunk.FlowTotalSize == 0 ||
            flowIdentifier.Length == 0 ||
            chunk.FlowFilename.Length == 0 ||
            chunk.FlowChunkNumber > chunk.FlowTotalChunks ||
            (maxFileSize > 0 && chunk.FlowTotalSize > maxFileSize))
                throw new FileException();
                   
        }

        public bool testFile(FileChunkDto chunk)
        {
            validateRequest(chunk);
            var filePathChunk = processTempPath(chunk.FlowIdentifier, chunk.FlowChunkNumber);
            return File.Exists(filePathChunk);

        }
    }
}

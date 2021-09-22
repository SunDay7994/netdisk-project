using sunday_netdisk_backend.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace sunday_netdisk_backend.Services
{
    public class BasicOperationService
    {
        public List<FileListVo> GetFileList(String filePath)
        {
            List<FileListVo> result = new List<FileListVo>();

            String[] files = Directory.GetFiles(filePath);

            String[] directories = Directory.GetDirectories(filePath);

            foreach (String file in files)
            {
                FileListVo f = new FileListVo();

                f.FileName = Path.GetFileName(file);

                f.FilePath = file;

                FileInfo fi = new FileInfo(file);
                f.FileSize = fi.Length;

                f.LastModified = File.GetLastWriteTime(file);

                f.IsDirectory = false;

                result.Add(f);
            }

            foreach (String directory in directories)
            {
                FileListVo f = new FileListVo();

                f.FileName = Path.GetFileName(directory);

                f.FilePath = directory;

                f.FileSize = 0;

                f.LastModified = File.GetLastWriteTime(directory);

                f.IsDirectory = true;

                result.Add(f);
            }

            return result;
        }

        public void createDir(string filePath)
        {
            if(!File.Exists(filePath) && !Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            else
            {
                int i = 1;
                string newPath = "";
                while (File.Exists(filePath)|| Directory.Exists(filePath))
                {
                    newPath = filePath + "(" + i++ + ")";
                }
                Directory.CreateDirectory(newPath);
            }
        }

        public void deleteFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
            if (Directory.Exists(filePath))
                Directory.Delete(filePath, true);
        }

        public void renameFile()
    }
}

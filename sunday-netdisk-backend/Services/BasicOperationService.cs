using sunday_netdisk_backend.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace sunday_netdisk_backend.Services
{
    public class BasicOperationService
    {
        const string root = "D:\\Test";
        public string ProcessPathDto(string input)
        {
            if (input == "/" || input == "" || input == null) 
                return root;

            input = input.Replace("/", "\\");

            if (input.StartsWith("root"))
            {
                input = input.Replace("root", root);
                return input;
            }
            else if (input.StartsWith("/root"))
            {
                input = input.Replace("/root", root);
                return input;
            }
            else if (input.StartsWith(root)) return input;
            else return root;
        }

        public string ProcessPathVo(string input)
        {
            input = input.Replace(root, "/root");
            input = input.Replace("\\", "/");
            return input;
        }

        public List<FileListVo> GetFileList(String filePath)
        {
            List<FileListVo> result = new List<FileListVo>();

            String[] files = Directory.GetFiles(filePath);

            String[] directories = Directory.GetDirectories(filePath);

            foreach (String file in files)
            {
                FileListVo f = new FileListVo();

                f.FileName = Path.GetFileName(file);

                f.FilePath = ProcessPathVo(file);

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

                f.FilePath = ProcessPathVo(directory);

                f.FileSize = 0;

                f.LastModified = File.GetLastWriteTime(directory);

                f.IsDirectory = true;

                result.Add(f);
            }

            return result;
        }

        public async Task CreateDir(string filePath, string fileName)
        {
            string newFile = Path.Combine(filePath, fileName);
            if (!File.Exists(newFile) && !Directory.Exists(newFile))
            {
                Directory.CreateDirectory(newFile);
            }
            else
            {
                int i = 1;
                while (File.Exists(newFile) || Directory.Exists(newFile))
                {
                    newFile += "(" + i++ + ")";
                    await Task.Yield();
                }
                Directory.CreateDirectory(newFile);
            }
        }

        public void DeleteFile(string[] filePaths)
        {
            foreach(string f in filePaths)
            {
                string filePath = ProcessPathDto(f);
                if (File.Exists(filePath))
                    File.Delete(filePath);
                else if (Directory.Exists(filePath))
                    Directory.Delete(filePath, true);
            }
        }

        public void RenameFile(string fileName, string newName)
        {
            string filePath = Path.GetDirectoryName(fileName);
            string destinationPath = Path.Combine(filePath, newName);
            FileInfo tempFileInfo;
            DirectoryInfo tempDirectoryInfo;
            if (File.Exists(filePath))
            {
                tempFileInfo = new FileInfo(filePath);
                tempFileInfo.MoveTo(destinationPath);
            }
            else if (Directory.Exists(filePath))
            {
                tempDirectoryInfo = new DirectoryInfo(filePath);
                tempDirectoryInfo.MoveTo(destinationPath);
            }
        }

        public void CopyFile(string[] soursePaths, string destinationPath)
        {
            {
                foreach(string s in soursePaths)
                {
                    string soursePath = ProcessPathDto(s);
                    string[] files = Directory.GetFiles(soursePath);
                    string fileName;
                    string destinationFile;
                    if (!Directory.Exists(destinationPath))
                    {
                        Directory.CreateDirectory(destinationPath);
                    }
                    foreach (string f in files)
                    {
                        fileName = Path.GetFileName(f);
                        destinationFile = Path.Combine(destinationPath, fileName);
                        File.Copy(f, destinationFile, true);
                    }

                    string[] filefolders = Directory.GetFiles(soursePath);
                    DirectoryInfo dirinfo = new DirectoryInfo(soursePath);
                    DirectoryInfo[] subFileFolder = dirinfo.GetDirectories();
                    for (int j = 0; j < subFileFolder.Length; j++)
                    {
                        string[] subSourcePath = { soursePath + "\\" + subFileFolder[j].ToString() };
                        string subDestinationPath = destinationPath + "\\" + subFileFolder[j].ToString();
                        CopyFile(subSourcePath, subDestinationPath);
                    }
                }
                
            }
        }

        public void MoveFile(string[] soursePath, string destinationPath)
        {
            CopyFile(soursePath, destinationPath);
            DeleteFile(soursePath);
        }
    }
}
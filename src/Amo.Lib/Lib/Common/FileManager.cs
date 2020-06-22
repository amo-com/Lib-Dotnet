using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Amo.Lib
{
    public static class FileManager
    {
        /// <summary>
        /// Copy文件夹和文件夹中的内容
        /// </summary>
        /// <param name="sourceDirectory">源文件夹</param>
        /// <param name="targetDirectory">目标文件夹</param>
        /// <param name="isPenetration">是否穿透文件夹,继续内容遍历文件夹</param>
        public static void DirectoryCopy(string sourceDirectory, string targetDirectory, bool isPenetration)
        {
            if (Directory.Exists(sourceDirectory))
            {
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                DirectoryInfo sourceInfo = new DirectoryInfo(sourceDirectory);
                FileInfo[] fileInfo = sourceInfo.GetFiles();
                foreach (FileInfo fiTemp in fileInfo)
                {
                    File.Copy(sourceDirectory + "\\" + fiTemp.Name, targetDirectory + "\\" + fiTemp.Name, true);
                }

                if (isPenetration)
                {
                    DirectoryInfo[] diInfo = sourceInfo.GetDirectories();
                    foreach (DirectoryInfo diTemp in diInfo)
                    {
                        string sourcePath = diTemp.FullName;
                        string targetPath = diTemp.FullName.Replace(sourceDirectory, targetDirectory);
                        Directory.CreateDirectory(targetPath);
                        DirectoryCopy(sourcePath, targetPath, isPenetration);
                    }
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Copy文件
        /// </summary>
        /// <param name="sourceFile">源文件夹</param>
        /// <param name="targetFile">目标文件夹</param>
        /// <param name="overWrite">是否覆盖</param>
        public static void FileCopy(string sourceFile, string targetFile, bool overWrite)
        {
            File.Copy(sourceFile, targetFile, overWrite);
        }

        public static void FileMove(string sourceFile, string targetFile)
        {
            File.Move(sourceFile, targetFile);
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path">FilePath</param>
        public static void DirectoryCreate(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path">FilePath</param>
        public static void FileCreate(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
        }

        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public static void DirectoryDelete(string path, bool recursive)
        {
            if (Directory.Exists(path))
            {
                DirectoryRemoveReadOnly(path);
                Directory.Delete(path, recursive);
            }
        }

        public static void FileDelete(string path)
        {
            if (File.Exists(path))
            {
                FileRemoveReadOnly(path);
                File.Delete(path);
            }
        }

        /// <summary>
        /// 文件夹只读
        /// </summary>
        /// <param name="path">FilePath</param>
        /// <returns>success</returns>
        public static bool DirectoryReadOnly(string path)
        {
            if (Directory.Exists(path))
            {
                System.IO.DirectoryInfo dirInfo = new DirectoryInfo(path);
                dirInfo.Attributes = FileAttributes.ReadOnly & FileAttributes.Directory;
            }

            return true;
        }

        /// <summary>
        /// 文件夹去除只读
        /// </summary>
        /// <param name="path">FilePath</param>
        /// <returns>Success</returns>
        public static bool DirectoryRemoveReadOnly(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    System.IO.DirectoryInfo dirInfo = new DirectoryInfo(path);
                    dirInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 文件去除只读
        /// </summary>
        /// <param name="path">FilePath</param>
        /// <returns>success</returns>
        public static bool FileRemoveReadOnly(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    System.IO.File.SetAttributes(path, System.IO.FileAttributes.Normal);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 读取文件夹下文件夹
        /// </summary>
        /// <param name="path">FilePath</param>
        /// <returns>文件夹列表</returns>
        public static List<string> GetDirectory(string path)
        {
            List<string> filePathList = new List<string>();

            if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                foreach (DirectoryInfo d in directoryInfo.GetDirectories())
                {
                    filePathList.Add(d.FullName);
                }
            }

            return filePathList;
        }

        /// <summary>
        /// 读取文件夹下文件
        /// </summary>
        /// <param name="path">FilePath</param>
        /// <param name="searchPattern">Pattern</param>
        /// <param name="throughFolder">穿透</param>
        /// <returns>文件列表</returns>
        public static List<string> GetFile(string path, string searchPattern, bool throughFolder = false)
        {
            List<string> filePathList = new List<string>();

            if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                if (!string.IsNullOrEmpty(searchPattern))
                {
                    foreach (FileInfo f in directoryInfo.GetFiles(searchPattern))
                    {
                        filePathList.Add(f.FullName);
                    }
                }
                else
                {
                    foreach (FileInfo f in directoryInfo.GetFiles())
                    {
                        filePathList.Add(f.FullName);
                    }
                }

                if (throughFolder)
                {
                    foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
                    {
                        filePathList.AddRange(GetFile(dir.FullName, searchPattern, throughFolder));
                    }
                }
            }

            return filePathList;
        }

        public static string GetDirectoryName(string path)
        {
            string name = Path.GetDirectoryName(path);
            return name;
        }

        public static string GetFileName(string path)
        {
            string name = Path.GetFileName(path);
            return name;
        }

        public static string GetFileNameWithoutExtension(string path)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            return name;
        }

        public static DateTime GetDirectoryLastWriteTime(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            DateTime date = directoryInfo.LastWriteTime;

            return date;
        }

        public static string GetExtension(string path)
        {
            string name = Path.GetExtension(path);

            return name;
        }

        /// <summary>
        /// 获取路径下的所有文件名,并按最后一次修改时间排序
        /// </summary>
        /// <param name="path">FilePath</param>
        /// <param name="searchPattern">Pattern</param>
        /// <returns>文件列表</returns>
        public static List<string> GetFileOrderByDate(string path, string searchPattern)
        {
            List<string> filePathList = new List<string>();

            if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                List<FileInfo> fileInfoList = directoryInfo.GetFiles(searchPattern).ToList(); // 获取所有文件
                fileInfoList = fileInfoList.OrderBy(q => q.LastWriteTime).ToList(); // 按时间排序
                foreach (FileInfo f in fileInfoList)
                {
                    filePathList.Add(f.FullName);
                }
            }

            return filePathList;
        }

        public static List<string> ReadFile(string path, Encoding encoding = null)
        {
            List<string> lineList = new List<string>();

            if (encoding == null)
            {
                encoding = Encoding.Default;
            }

            if (File.Exists(path))
            {
                lineList.AddRange(File.ReadAllLines(path, encoding));
            }

            return lineList;
        }

        public static string ReadFileSingle(string path, Encoding encoding = null)
        {
            string str = string.Empty;
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }

            if (File.Exists(path))
            {
                str = File.ReadAllText(path, encoding);
            }

            return str;
        }

        /// <summary>
        /// 输出到文件(覆盖)
        /// </summary>
        /// <param name="path">FilePath</param>
        /// <param name="lines">Lines</param>
        /// <returns>success</returns>
        public static bool SaveFile(string path, List<string> lines)
        {
            try
            {
                if (lines == null || lines.Count == 0)
                {
                    return true;
                }

                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }

                File.WriteAllLines(path, lines.ToArray());
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 输出到文件(追加)
        /// </summary>
        /// <param name="path">FilePath</param>
        /// <param name="lines">Lines</param>
        /// <returns>success</returns>
        public static bool FileWrite(string path, List<string> lines)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }

                if (lines != null && lines.Count > 0)
                {
                    File.AppendAllLines(path, lines.ToArray());
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool FileWrite(string path, string line)
        {
            return FileWrite(path, new List<string>() { line });
        }
    }
}

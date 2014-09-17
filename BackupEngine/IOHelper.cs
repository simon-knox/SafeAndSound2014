using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace SKnoxConsulting.SafeAndSound.BackupEngine
{
    public class IOHelper
    {
        public static List<DirectoryInfo> GetDirectories(string root)
        {
            try
            {
                List<DirectoryInfo> result = new List<DirectoryInfo>();
                result.Add(new DirectoryInfo(root));

                foreach (string directory in Directory.GetDirectories(root))
                {
                    List<DirectoryInfo> dirs = GetDirectories(directory);
                    if (dirs != null)
                    {
                        foreach (DirectoryInfo di in dirs)
                        {
                            result.Add(new DirectoryInfo(di.FullName));
                        }
                    }

                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<DirectoryInfo> GetDirectoriesPostOrder(string root)
        {
            try
            {
                List<DirectoryInfo> result = new List<DirectoryInfo>();

                foreach (string directory in Directory.GetDirectories(root, "*.*", SearchOption.TopDirectoryOnly))
                {
                    var subResult = GetDirectoriesPostOrder(directory);
                    if (subResult != null)
                    {
                        result.AddRange(subResult);
                    }
                }
                result.Add(new DirectoryInfo(root));
                return result;
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static List<FileInfo> GetFiles(string directory)
        {
            try
            {
                List<FileInfo> result = new List<FileInfo>();
                //result.Add(new DirectoryInfo(root));

                foreach (string file in Directory.GetFiles(directory))
                {
                    try
                    {
                        result.Add(new FileInfo(file));
                    }
                    catch (Exception)
                    {
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<FileInfo> GetAllFiles(string root)
        {
            List<DirectoryInfo> dirs = GetDirectories(root);
            List<FileInfo> files = new List<FileInfo>();
            if (dirs.Count > 0)
            {
                foreach (DirectoryInfo di in dirs)
                {
                    List<FileInfo> filesTemp = GetFiles(di.FullName);
                    if (filesTemp != null)
                    {
                        foreach (FileInfo fi in filesTemp)
                        {
                            files.Add(fi);
                        }
                    }
                }
            }
            return files;
        }

        public static List<FileInfo> GetAllFiles(string root, BackgroundWorker bw)
        {
            List<DirectoryInfo> dirs = GetDirectories(root);
            List<FileInfo> files = new List<FileInfo>();
            if (dirs.Count > 0)
            {
                foreach (DirectoryInfo di in dirs)
                {
                    if (bw.CancellationPending)
                    {
                        return files;
                    }
                    List<FileInfo> filesTemp = GetFiles(di.FullName);
                    if (filesTemp != null)
                    {
                        foreach (FileInfo fi in filesTemp)
                        {
                            files.Add(fi);
                        }
                    }
                }
            }
            return files;
        }

        public static List<FileInfo> GetAllFiles2(string root)
        {
            List<DirectoryInfo> dirs = GetDirectories(root);
            List<FileInfo> files = new List<FileInfo>();
            if (dirs.Count > 0)
            {
                foreach (DirectoryInfo di in dirs)
                {
                    List<FileInfo> filesTemp = GetFiles(di.FullName);
                    if (filesTemp != null)
                    {
                        foreach (FileInfo fi in filesTemp)
                        {
                            files.Add(fi);
                        }
                    }
                }
            }
            return files;
        }

        //public static List<FileInfo> GetMissingFiles(string source, string destination)
        //{
        //    var sf = GetAllFiles(source);
        //    var df = GetAllFiles(destination);

        //    var result = sf.Where(f => !df.Contains(f));

        //    List<DirectoryInfo> dirs = GetDirectories(root);
        //    List<FileInfo> files = new List<FileInfo>();
        //    if (dirs.Count > 0)
        //    {
        //        foreach (DirectoryInfo di in dirs)
        //        {
        //            List<FileInfo> filesTemp = GetFiles(di.FullName);
        //            if (filesTemp != null)
        //            {
        //                foreach (FileInfo fi in filesTemp)
        //                {
        //                    files.Add(fi);
        //                }
        //            }
        //        }
        //    }
        //    return files;
        //}
    }
}



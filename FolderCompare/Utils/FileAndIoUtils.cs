using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace FolderCompare.Utils
{
    public static class ThreadHelper
    {
        public static void InvokeOnCtrl(Control c, Action action)
        {
            if (c.InvokeRequired)
            {
                c.Invoke(new Action(action));
                return;
            }

            action();
        }
    }

    internal static class FileAndIoUtils
    {
        private static char? _directorySeparatorStr;

        public static char DirectorySeparator => _directorySeparatorStr ??
                                                 (_directorySeparatorStr = Path.Combine("4", "4").Replace("4", "")
                                                     .First()).Value;

        public static string CalculateMd5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private static readonly string[] Suffix = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
        public static string BytesToString(long byteCount)
        {
            if (byteCount == 0)
                return "0" + Suffix[0];
            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + Suffix[place];
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    $"Source directory does not exist or could not be found: {sourceDirName}");
            }

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            foreach (var file in dir.GetFiles())
            {
                var tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            foreach (var subdir in dir.GetDirectories())
            {
                var tempPath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, tempPath);
            }
        }
    }
}
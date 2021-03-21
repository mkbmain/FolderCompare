using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace BackUpInSynch
{
    public static class FileHelper
    {
        private static char? _directorySeparatorStr;

        public static char DirectorySeparatorStr => _directorySeparatorStr ??
                                                    (_directorySeparatorStr = Path.Combine("aa", "aa").Replace("aa", "")
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
    }
}
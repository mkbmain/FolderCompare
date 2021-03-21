using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace BackUpInSynch
{
    public static class FileHelper
    {
        private static char? _DirectorySeparatorStr = null;

        public static char DirectorySeparatorStr => _DirectorySeparatorStr ??
                                                    (_DirectorySeparatorStr = Path.Combine("aa", "aa").Replace("aa", "")
                                                        .First()).Value;

        public static string CalculateMD5(string filename)
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
using System.IO;
using System.Linq;

namespace BackUpInSynch
{
    public static class FileHelper
    {
        private static char? _DirectorySeparatorStr = null;

        public static char DirectorySeparatorStr => _DirectorySeparatorStr ?? (_DirectorySeparatorStr = Path.Combine("aa", "aa").Replace("aa", "").First()).Value;
    }
}
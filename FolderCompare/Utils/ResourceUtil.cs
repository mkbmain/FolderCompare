using System.Drawing;
using System.Reflection;

namespace FolderCompare.Utils
{
    public class ResourceUtil
    {
        public static Image GetImageFromResource(string fileName)
        {
            var image = Image.FromStream(Assembly.GetEntryAssembly().GetManifestResourceStream(fileName));
            return image;
        }
    }
}
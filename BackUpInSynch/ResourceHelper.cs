using System.Drawing;
using System.Reflection;

namespace BackUpInSynch
{
    public class ResourceHelper
    {
        public static Image GetImageFromResource(string fileName)
        {
            var image = Image.FromStream(Assembly.GetEntryAssembly().GetManifestResourceStream(fileName));
            return image;
        }
    }
}
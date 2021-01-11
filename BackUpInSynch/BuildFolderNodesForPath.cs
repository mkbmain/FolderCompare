using System.Linq;
using System.Windows.Forms;

namespace BackUpInSynch
{
    public static class BuildFolderNodesForPath
    {
        public static FolderNode BuildPath(string path)
        {
            var name = "\\" + NameCleaner(path);
            var node = new FolderNode
            {
                Name = name, Text = name,
            };

            foreach (var item in System.IO.Directory.GetDirectories(path))
            {
                node.Nodes.Add(BuildPath(item));
            }

            foreach (var item in System.IO.Directory.GetFiles(path))
            {
                var names = NameCleaner(item);
                node.Nodes.Add(names, names);
            }

            return node;
        }

        private static string NameCleaner(string path)
        {
            var s = path.Split("\\");
            return s.Last();
        }
    }
}
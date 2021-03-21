using System.Linq;
using BackUpInSynch.DirectoryStructure;

namespace BackUpInSynch
{
    public static class BuildFolderNodesForPath
    {
        public static DirectoryNode BuildPath(string path,bool calcHash = false)
        {
            var name = $"{FileHelper.DirectorySeparatorStr}{NameCleaner(path)}";
            var node = new DirectoryNode
            {
                Name = name, FullLocation = path
            };

            foreach (var item in System.IO.Directory.GetDirectories(path))
            {
                node.SubDirectories.Add(BuildPath(item));
            }

            foreach (var item in System.IO.Directory.GetFiles(path))
            {
                var names = NameCleaner(item);
                node.Files.Add(new FileNode{ Name = names, FullLocation = item, Hash = calcHash ? FileHelper.CalculateMd5(item) :null});
            }

            return node;
        }

        private static string NameCleaner(string path)
        {
            return path.Split(FileHelper.DirectorySeparatorStr).Last();
        }
    }
}
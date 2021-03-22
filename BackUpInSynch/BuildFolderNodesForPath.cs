using System.IO;
using System.Linq;
using BackUpInSynch.Models.ScanStructure;
using BackUpInSynch.Utils;

namespace BackUpInSynch
{
    internal static class BuildFolderNodesForPath
    {
        public static DirectoryNode BuildPath(string basePath ,string path, bool calcHash = false)
        {
            var name = $"{FileAndIoUtils.DirectorySeparator}{NameCleaner(path)}";
            var node = new DirectoryNode
            {
                Name = name, 
                BasePath = basePath,
                FullLocation = path
            };

            foreach (var item in Directory.GetDirectories(path))
            {
                node.SubDirectories.Add(BuildPath(basePath,item));
            }

            foreach (var item in Directory.GetFiles(path))
            {
                var names = NameCleaner(item);
                node.Files.Add(new FileNode
                {
                    Name = names, 
                    BasePath = basePath,
                    FullLocation = item,
                    Hash = calcHash ? FileAndIoUtils.CalculateMd5(item) : string.Empty
                });
            }

            return node;
        }

        private static string NameCleaner(string path)
        {
            return path.Split(FileAndIoUtils.DirectorySeparator).Last();
        }
    }
}
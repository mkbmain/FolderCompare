using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FolderCompare.Models.ScanStructure
{
    public class DirectoryNode : NodeBase
    {
        public List<FileNode> Files { get; }
        public List<DirectoryNode> SubDirectories { get; }

        public DirectoryNode()
        {
            Files = new List<FileNode>();
            SubDirectories = new List<DirectoryNode>();
        }

        public TreeNode ToTreeNode()
        {
            var node = new TreeNode(Name);
            node.Nodes.AddRange(SubDirectories.Select(f => f.ToTreeNode()).ToArray());
            node.Nodes.AddRange(Files.Select(f => f.ToTreeNode()).ToArray());
            return node;
        }
    }
}
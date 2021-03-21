using System.Collections.Generic;
using System.Windows.Forms;

namespace BackUpInSynch.DirectoryStructure
{
    public class DirectoryNode : NodeBase
    {
        public DirectoryNode()
        {
            Files = new List<FileNode>();
            SubDirectories = new List<DirectoryNode>();
        }

        public TreeNode ToTreeNode()
        {
            var node = new TreeNode();
            foreach (var directory in SubDirectories)
            {
                node.Nodes.Add(directory.ToTreeNode());
            }

            foreach (var file in Files)
            {
                node.Nodes.Add(file.ToTreeNode());
            }

            return node;
        }

        public List<FileNode> Files { get; set; }
        public List<DirectoryNode> SubDirectories { get; set; }
    }
}
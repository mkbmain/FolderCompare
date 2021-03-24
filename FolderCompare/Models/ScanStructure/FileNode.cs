using System.IO;
using System.Windows.Forms;

namespace FolderCompare.Models.ScanStructure
{
    public class FileNode : NodeBase
    {
        public string Hash { get; set; }

        public FileInfo FileInfo { get; set; }

        public TreeNode ToTreeNode()
        {
            return new TreeNode(Name);
        }
    }
}
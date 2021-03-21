using System.Windows.Forms;

namespace BackUpInSynch.Models.ScanStructure
{
    public class FileNode : NodeBase
    {
        public string Hash { get; set; }

        public TreeNode ToTreeNode()
        {
            return new TreeNode(Name);
        }
    }
}
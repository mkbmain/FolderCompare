using System.Windows.Forms;

namespace BackUpInSynch.DirectoryStructure
{
    public class FileNode :NodeBase
    {
        public string Hash { get; set; }

        public TreeNode ToTreeNode()
        {
            return new TreeNode(Name);
        }
    }
}
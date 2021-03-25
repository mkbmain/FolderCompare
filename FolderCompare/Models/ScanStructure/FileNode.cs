using System.IO;
using System.Windows.Forms;
using FolderCompare.Utils;

namespace FolderCompare.Models.ScanStructure
{
    public class FileNode : NodeBase
    {
        private string _hash = null;

        public string Hash()
        {
            return _hash ?? (_hash = FileAndIoUtils.CalculateMd5(FullLocation));
        }

        public FileInfo FileInfo { get; set; }

        public TreeNode ToTreeNode()
        {
            return new TreeNode(Name);
        }
    }
}
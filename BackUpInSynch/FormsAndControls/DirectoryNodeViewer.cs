using System.Drawing;
using System.Windows.Forms;
using BackUpInSynch.Models.ScanStructure;

namespace BackUpInSynch.FormsAndControls
{
    public class DirectoryNodeViewer : Form
    {
        private DirectoryNode _folderNodeOne;
        private DirectoryNode _folderNodeTwo;

        private TreeView _treeViewFolderOne;
        private TreeView _treeViewFolderTwo;

        public DirectoryNodeViewer(DirectoryNode folderOne, DirectoryNode folderTwo)
        {
            Size = new Size(800, 450);
            Text = "FolderComparer";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Size = new Size(640, 480);

            _folderNodeOne = folderOne;
            _folderNodeTwo = folderTwo;
            _treeViewFolderOne = new TreeView {Nodes = {_folderNodeOne.ToTreeNode()}, Size = new Size(300, 350)};
            _treeViewFolderTwo = new TreeView {Nodes = {_folderNodeTwo.ToTreeNode()}, Size = new Size(300, 350)};

            _treeViewFolderOne.Location = new Point(1, 1);
            _treeViewFolderTwo.Location = new Point(_treeViewFolderOne.Right + 15, 1);
            Controls.Add(_treeViewFolderOne);
            Controls.Add(_treeViewFolderTwo);
        }
    }
}
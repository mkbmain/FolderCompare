using System.Drawing;
using System.Windows.Forms;
using BackUpInSynch.Models.ScanStructure;

namespace BackUpInSynch.FormsAndControls
{
    public class DirectoryNodeViewer : Form
    {
        public DirectoryNodeViewer(DirectoryNode folderOne, DirectoryNode folderTwo)
        {
            Size = new Size(800, 450);
            Text = "FolderComparer";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Size = new Size(640, 480);

            var treeViewFolderOne = new TreeView {Nodes = {folderOne.ToTreeNode()}, Size = new Size(300, 350)};
            var treeViewFolderTwo = new TreeView {Nodes = {folderTwo.ToTreeNode()}, Size = new Size(300, 350)};

            treeViewFolderOne.Location = new Point(1, 1);
            treeViewFolderTwo.Location = new Point(treeViewFolderOne.Right + 15, 1);
            Controls.Add(treeViewFolderOne);
            Controls.Add(treeViewFolderTwo);
        }
    }
}
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BackUpInSynch.DirectoryStructure;

namespace BackUpInSynch.FormsAndControls.MainForm
{
    public class MainForm : Form
    {
        private DirectoryPanel FolderOne = new DirectoryPanel();
        private DirectoryPanel FolderTwo = new DirectoryPanel();
        private Button RunBtn = new Button();

        public MainForm()
        {
            this.Controls.Add(RunBtn);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            FolderTwo.Top = FolderOne.Bottom + 5;
            this.Controls.Add(FolderOne);
            this.Controls.Add(FolderTwo);
            RunBtn.Location  = new Point(33 + FolderTwo.Bottom, this.Width / 2);
            this.Width = 640;
            RunBtn.Click += RunBtn_Click;
            var r = new ResultsForm.ResultsForm(new FileNode[0], new DirectoryNode[0], new FileNode[0]);
            r.Show();
        }


        private void RunBtn_Click(object sender, EventArgs e)
        {
            if (RunBtn.Text == "Results")
            {
                var fc = new DirectoryNodeViewer(_folderNodeOne, _folderNodeTwo);
                fc.Show();
                return;
            }

            var pathOne = FolderOne.GetPathIfValid;
            var pathTwo = FolderTwo.GetPathIfValid;
            if (pathOne == null || pathTwo == null)
            {
                MessageBox.Show("Please check paths");
                return;
            }

            RunBtn.Text = "Results";
            RunBtn.Enabled = false;

            RunInBackground.Run((pathOne, pathTwo), DoWork, (a, b) => { RunBtn.Enabled = true; }, null);
        }

        private static DirectoryNode _folderNodeOne;
        private DirectoryNode _folderNodeTwo;

        private void DoWork(object o, DoWorkEventArgs args)
        {
            var (pathOne, pathTwo) = (ValueTuple<string, string>) args.Argument;
            _folderNodeOne = BuildFolderNodesForPath.BuildPath(pathOne);
            _folderNodeTwo = BuildFolderNodesForPath.BuildPath(pathTwo);
            MessageBox.Show("Results available");
        }
    }
}
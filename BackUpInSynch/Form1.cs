using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackUpInSynch
{
    public partial class Form1 : Form
    {
        private DirectoryPanel FolderOne = new();
        private DirectoryPanel FolderTwo = new();
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            FolderTwo.Top = FolderOne.Bottom + 5;
            this.Controls.Add(FolderOne);
            this.Controls.Add(FolderTwo);
        }


        private void RunBtn_Click(object sender, EventArgs e)
        {
            if (RunBtn.Text == "Results")
            {
                var fc = new FolderComparer(_folderNodeOne, _folderNodeTwo);
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
            
            RunInBackground.Run((pathOne,pathTwo), DoWork, (_, _) =>
            {
                RunBtn.Enabled = true;
            }, null);
        }

        private static FolderNode _folderNodeOne;
        private FolderNode _folderNodeTwo;
        private void DoWork(object o, DoWorkEventArgs args)
        {
            var (pathOne, pathTwo) = (ValueTuple< string,string>)args.Argument;
            _folderNodeOne = BuildFolderNodesForPath.BuildPath(pathOne);
            _folderNodeTwo  = BuildFolderNodesForPath.BuildPath(pathTwo);
            MessageBox.Show("Results available"); 
        }
    }
}
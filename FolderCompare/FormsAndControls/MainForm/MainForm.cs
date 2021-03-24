using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FolderCompare.CalculateMissMatches;
using FolderCompare.Models.ScanStructure;
using FolderCompare.Utils;

namespace FolderCompare.FormsAndControls.MainForm
{
    public class MainForm : Form
    {
        private readonly DirectoryPanel _folderOne = new DirectoryPanel();
        private readonly DirectoryPanel _folderTwo = new DirectoryPanel();
        private readonly Button _runBtn = new Button {Text = "Calculate", BackColor = GlobalColor.Get(ColorFor.Button)};

        private readonly Label _waringLabel = new Label
        {
            Text = "Check contents:", Size = new Size(95, 20), TextAlign = ContentAlignment.MiddleCenter,
            AutoSize = false
        };

        private readonly CheckBox _checkBox = new CheckBox
            {Text = "Checking contents will take a long time", Size = new Size(300, 20)};

        public MainForm()
        {
            BackColor = GlobalColor.Get(ColorFor.Window);
            Controls.Add(_runBtn);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            _folderTwo.Top = _folderOne.Bottom + 5;
            Controls.Add(_folderOne);
            Controls.Add(_folderTwo);
            Size = new Size(410, 150);
            this.Text = "Folder comparer";
            _runBtn.Click += RunBtn_Click;
            _waringLabel.Location = new Point(10, _folderTwo.Bottom + 5);
            _checkBox.Location = new Point(_waringLabel.Right + 10, _waringLabel.Top + 3);
            _runBtn.Location = new Point(33 + _folderTwo.Bottom, _checkBox.Bottom+15);
            Controls.Add(_checkBox);
            Controls.Add(_waringLabel);
        }


        private void RunBtn_Click(object sender, EventArgs e)
        {
            var pathOne = _folderOne.GetPathIfValid;
            var pathTwo = _folderTwo.GetPathIfValid;
            if (pathOne == null || pathTwo == null)
            {
                MessageBox.Show("Please check paths");
                return;
            }

            _runBtn.Text = "Calculating";
            _runBtn.Enabled = false;

            BackgroundGenerator.Run(
                new BackgroundWorkerInfo {PathOne = pathOne, PathTwo = pathTwo, CheckContents = _checkBox.Checked},
                DoWork, (a, b) =>
                {
                    _runBtn.Enabled = true;
                    var issues = CalculateDifferences.Issues(_folderNodeOne.BasePath, _folderNodeTwo.BasePath,
                        _folderNodeOne, _folderNodeTwo);
                    var fc = new ResultsForm.ResultsForm(issues);
                    fc.Show();
                    _runBtn.Text = "Calculate";
                }, null);
        }

        private DirectoryNode _folderNodeOne;
        private DirectoryNode _folderNodeTwo;

        private void DoWork(object o, DoWorkEventArgs args)
        {
            var info = (BackgroundWorkerInfo) args.Argument;
            _folderNodeOne = BuildFolderNodesForPath.BuildPath(info.PathOne, info.PathOne, info.CheckContents);
            _folderNodeTwo = BuildFolderNodesForPath.BuildPath(info.PathTwo, info.PathTwo, info.CheckContents);
            MessageBox.Show("Results available");
        }
    }
}
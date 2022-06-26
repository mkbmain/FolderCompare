using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FolderCompare.CalculateMissMatches;
using FolderCompare.FormsAndControls.MainForm.Controls;
using FolderCompare.Models;
using FolderCompare.Models.ScanStructure;
using FolderCompare.Utils;

namespace FolderCompare.FormsAndControls.MainForm
{
    public class MainForm : Form
    {
        private readonly DirectoryPanel _folderOne = new DirectoryPanel();
        private readonly DirectoryPanel _folderTwo = new DirectoryPanel();
        private readonly Button _runBtn = new Button {Text = "Calculate", BackColor = GlobalColor.Get(ColorFor.Button)};

        private readonly ProgressBar _progressBar = new ProgressBar
            {Maximum = 100, Size = new Size(100, 25), Visible = false};

        private readonly CheckBox _checkBox = new CheckBox
            {Text = "Checking contents will take a long time", Size = new Size(300, 20)};

        private readonly Label _waringLabel = new Label
            {Text = "Check contents:", Size = new Size(95, 20), AutoSize = false};

        private DirectoryNode _folderNodeOne;
        private DirectoryNode _folderNodeTwo;

        public MainForm()
        {
            BackColor = GlobalColor.Get(ColorFor.Window);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Text = "Folder comparer";
            Size = new Size(410, 160);
            _folderTwo.Top = _folderOne.Bottom + 5;
            _runBtn.Click += RunBtn_Click;
            _waringLabel.Location = new Point(10, _folderTwo.Bottom + 5);
            _checkBox.Location = new Point(_waringLabel.Right + 10, _waringLabel.Top + 3);
            _runBtn.Location = new Point(33 + _folderTwo.Bottom, _checkBox.Bottom + 15);
            _progressBar.Location = new Point(_runBtn.Left + _runBtn.Size.Width + 10, _runBtn.Top);
            Controls.Add(_runBtn);
            Controls.Add(_progressBar);
            Controls.Add(_folderOne);
            Controls.Add(_folderTwo);
            Controls.Add(_checkBox);
            Controls.Add(_waringLabel);
        }

        private void RunBtn_Click(object sender, EventArgs e)
        {
            var pathOne = _folderOne.GetPathIfValid;
            var pathTwo = _folderTwo.GetPathIfValid;
            if (pathOne == null || pathTwo == null)
            {
                MessageBox.Show("Please check paths", nameof(FolderCompare));
                return;
            }

            if (pathOne.Contains(pathTwo) || pathTwo.Contains(pathOne))
            {
                MessageBox.Show("can not check folders that contain a child that it iss being compared to",
                    nameof(FolderCompare));
                return;
            }

            _runBtn.Text = "Calculating";
            _runBtn.Enabled = false;
            _progressBar.Visible = true;
            var backGroundTaskArgs = new BackgroundWorkerInfo
                {PathOne = pathOne, PathTwo = pathTwo, CheckContents = _checkBox.Checked};
            BackgroundGenerator.Run(backGroundTaskArgs, DoWork, WhenComplete, null);
        }

        private void WhenComplete(object o, RunWorkerCompletedEventArgs completedEventArgs)
        {
            var issues = CalculateDifferencesDirectories.Issues(_folderNodeOne.BasePath, _folderNodeTwo.BasePath,
                _folderNodeOne, _folderNodeTwo, _checkBox.Checked);
            _progressBar.Invoke(new MethodInvoker(delegate { SetPercent(85, false); }));
            var results = new ResultsForm.ResultsForm(issues);
            results.Show();
            _runBtn.Enabled = true;
            _runBtn.Text = "Calculate";
            _progressBar.Invoke(new MethodInvoker(delegate { SetPercent(0, false); }));
            MessageBox.Show("Done results available", nameof(FolderCompare));
        }

        private void SetPercent(int percent,bool visible)
        {
            _progressBar.Value = percent;
            _progressBar.Visible = visible;
        }
        private void DoWork(object o, DoWorkEventArgs args)
        {
            try
            {
                var info = (BackgroundWorkerInfo) args.Argument;
                _folderNodeOne = BuildFolderNodesForPath.BuildPath(info.PathOne, info.PathOne);
                _progressBar.Invoke(new MethodInvoker(delegate { SetPercent(info.CheckContents ? 25 : 45, true); }));
                _folderNodeTwo = BuildFolderNodesForPath.BuildPath(info.PathTwo, info.PathTwo);
                _progressBar.Invoke(new MethodInvoker(delegate { SetPercent(info.CheckContents ? 55 : 85, true); }));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace);
                Application.Exit();
            }
        }
    }
}
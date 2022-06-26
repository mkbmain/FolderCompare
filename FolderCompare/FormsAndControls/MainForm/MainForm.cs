using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using FolderCompare.FormsAndControls.MainForm.Controls;
using FolderCompare.Utils;
using FolderCompare.Utils.Background;

namespace FolderCompare.FormsAndControls.MainForm
{
    public class MainForm : Form
    {
        private readonly DirectoryPanel _folderOne = new DirectoryPanel();
        private readonly DirectoryPanel _folderTwo = new DirectoryPanel();
        private readonly Button _runBtn = new Button { Text = "Calculate", BackColor = GlobalColor.Get(ColorFor.Button) };
        private readonly FolderCompareTaskRunner _threadHelper = new FolderCompareTaskRunner();
        private readonly ProgressBar _progressBar = new ProgressBar
        { Maximum = 100, Size = new Size(100, 25), Visible = false };

        private readonly CheckBox _checkBox = new CheckBox
        { Text = "Checking contents will take a long time", Size = new Size(300, 20) };

        private readonly Label _waringLabel = new Label
        { Text = "Check contents:", Size = new Size(95, 20), AutoSize = false };


        public MainForm()
        {
            BackColor = GlobalColor.Get(ColorFor.Window);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Text = "Folder comparer";
            Size = new Size(410, 160);
            _folderTwo.Top = _folderOne.Bottom + 5;
            _threadHelper.SetProgressBarPercent += ThreadHelperOnSetProgressBarPercent;
            _threadHelper.Done += ThreadHelperOnDone;
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

        private void ThreadHelperOnDone(object sender, FolderCompareTaskRunner.DoneEventArgs e)
        {
            ThreadHelper.InvokeOnCtrl(this, () =>
            {
                var results = new ResultsForm.ResultsForm(e.Issues);
                results.Show();
                MessageBox.Show("Done results available", nameof(FolderCompare));
            });

            ThreadHelper.InvokeOnCtrl(_progressBar, () =>
            {
                _progressBar.Visible = false;
                _progressBar.Value = 0; ;
            });
            ThreadHelper.InvokeOnCtrl(_runBtn, () =>
            {
                _runBtn.Enabled = true;
                _runBtn.Text = "Calculate";
            });
            ;
        }


        private void ThreadHelperOnSetProgressBarPercent(object sender, FolderCompareTaskRunner.SetPercentEventArgs e)
        {
            ThreadHelper.InvokeOnCtrl(_progressBar, () =>
            {
                _progressBar.Value = e.Percent;
                _progressBar.Update();
                _progressBar.Refresh();
            });
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
            Task.Run(() => _threadHelper.DoWork(pathOne, pathTwo, _checkBox.Checked));
        }
    }
}
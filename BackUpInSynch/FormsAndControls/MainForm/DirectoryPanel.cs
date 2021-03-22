using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BackUpInSynch.Utils;

namespace BackUpInSynch.FormsAndControls.MainForm
{
    internal class DirectoryPanel : Panel
    {
        private const int MyHeight = 22;

        private readonly Label _title = new Label
            {Text = "Open Folder:", AutoSize = false, Size = new Size(77, MyHeight)};

        private readonly TextBox _pathTxtBox = new TextBox
            {Multiline = false, AutoSize = false, Size = new Size(280, MyHeight - 2), ReadOnly = false};

        private readonly Button _openButton = new Button {Text = "..", AutoSize = false, Size = new Size(23, MyHeight)};

        public string GetPathIfValid => Directory.Exists(_pathTxtBox.Text)
            ? _pathTxtBox.Text.EndsWith(FileAndIoUtils.DirectorySeparator.ToString()) ? _pathTxtBox.Text :
            $"{_pathTxtBox.Text}{FileAndIoUtils.DirectorySeparator}"
            : null;

        private readonly FolderBrowserDialog _folderBrowserDialog = new FolderBrowserDialog();

        public DirectoryPanel()
        {
            Size = new Size(400, MyHeight);
            _title.Location = new Point(1, 1);
            _pathTxtBox.Location = new Point(_title.Right + 2, 1);
            _openButton.Location = new Point(_pathTxtBox.Right + 2, 1);

            _openButton.Click += (sender, args) =>
            {
                if (_folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    _pathTxtBox.Text = _folderBrowserDialog.SelectedPath;
                }
            };

            Controls.Add(_title);
            Controls.Add(_pathTxtBox);
            Controls.Add(_openButton);
        }
    }
}
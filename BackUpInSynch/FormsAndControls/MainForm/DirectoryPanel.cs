using System.Drawing;
using System.Windows.Forms;

namespace BackUpInSynch.FormsAndControls.MainForm
{
    public class DirectoryPanel : Panel
    {
        private const int MyHeight = 22;
        
        private Label Title = new Label {Text = "Open Folder:", AutoSize = false,Size = new Size(77,MyHeight)};
        private TextBox PathTxtBox = new TextBox {Multiline = false, AutoSize = false,Size = new Size(280, MyHeight-2), ReadOnly = false};
        private Button OpenButton = new Button {Text = "..",AutoSize = false, Size = new Size(23, MyHeight)};

        public string GetPathIfValid => System.IO.Directory.Exists(PathTxtBox.Text) ? PathTxtBox.Text : null;
        private FolderBrowserDialog _folderBrowserDialog = new FolderBrowserDialog();

        public DirectoryPanel()
        {
            this.Size = new Size(400, MyHeight);
            Title.Location = new Point(1, 1);
            PathTxtBox.Location = new Point(Title.Right + 2, 1);
            OpenButton.Location = new Point(PathTxtBox.Right + 2, 1);

            OpenButton.Click += (sender, args) =>
            {
                if (_folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    PathTxtBox.Text = _folderBrowserDialog.SelectedPath;
                }
            };
            
            this.Controls.Add(Title);
            this.Controls.Add(PathTxtBox);
            this.Controls.Add(OpenButton);
        }
    }
}
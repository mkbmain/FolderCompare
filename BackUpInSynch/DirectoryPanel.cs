using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace BackUpInSynch
{
    public class DirectoryPanel : System.Windows.Forms.Panel
    {
        private const int Height = 22;
        
        private Label Title = new Label {Text = "Open Folder:", AutoSize = false,Size = new Size(77,Height)};
        private TextBox PathTxtBox = new TextBox {Multiline = false, AutoSize = false,Size = new Size(280, Height-2), ReadOnly = true};
        private Button OpenButton = new Button {Text = "..",AutoSize = false, Size = new Size(23, Height)};

        public string GetPathIfValid => System.IO.Directory.Exists(PathTxtBox.Text) ? PathTxtBox.Text : null;
        private FolderBrowserDialog _folderBrowserDialog = new FolderBrowserDialog();

        public DirectoryPanel()
        {
            this.Size = new Size(400, Height);
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
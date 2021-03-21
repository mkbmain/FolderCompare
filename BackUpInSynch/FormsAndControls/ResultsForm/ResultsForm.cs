using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BackUpInSynch.DirectoryStructure;
using BackUpInSynch.FormsAndControls.MainForm;

namespace BackUpInSynch.FormsAndControls.ResultsForm
{
    public partial class ResultsForm : Form
    {
        private List<FileNode> _missingFiles { get; set; }
        private List<DirectoryNode> _missingDirectoryNodes { get; set; }
        private List<FileNode> _missMatchFiles { get; set; }

        public ResultsForm(IEnumerable<FileNode> missingFiles, IEnumerable<DirectoryNode> missingDirectoryNodes,
            IEnumerable<FileNode> missMatchFiles)
        {
            this.AutoSize = false;
            this.Size = new Size(450, 600);
            _missingFiles = missingFiles as List<FileNode> ?? missingFiles.ToList();
            _missingDirectoryNodes = missingDirectoryNodes as List<DirectoryNode> ?? missingDirectoryNodes.ToList();
            _missMatchFiles = missMatchFiles as List<FileNode> ?? missMatchFiles.ToList();
            InitializeComponent();

            var p = new Panel {Size = new Size(this.Width - 30, this.Height - 50), AutoScroll = true};
            var location = 0;
            
            foreach (var item in _missingDirectoryNodes)
            {
                var directory = new DirectoryView(item) {Top = location};
                location += directory.Height + 5;
                p.Controls.Add(directory);
            }
            

            this.Controls.Add(p);
        }
    }
}
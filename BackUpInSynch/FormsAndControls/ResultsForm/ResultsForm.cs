using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BackUpInSynch.CalculateMissMatches;
using BackUpInSynch.Models.ResultStructure;
using BackUpInSynch.Models.ScanStructure;

namespace BackUpInSynch.FormsAndControls.ResultsForm
{
    public partial class ResultsForm : Form
    {
        private List<DirectoryResultDetails> _directories { get; set; }
        private List<FileResultDetails> _files { get; set; }
        private Panel Panel;

        public ResultsForm(DirectoryNode source, DirectoryNode destination)
        {
            this.AutoSize = false;
            this.Size = new Size(450, 600);
            InitializeComponent();
            Panel = new Panel {Name = "M", Size = new Size(this.Width - 30, this.Height - 50), AutoScroll = true};

            var issue = CalculateDiffrences.Issues(source.BasePath, destination.BasePath, source, destination);
            _directories = issue.DirectoryResultDetailsList;
            _files = issue.FileResultDetailsList;
            DrawWindow();
            this.Controls.Add(Panel);
        }


        private void DrawWindow()
        {
            var location = 0;
            foreach (var item in Panel.Controls.Cast<Control>())
            {
                Panel.Controls.Remove(item);
            }

            foreach (var directoryView in _directories.Select(item => new DirectoryView(item) {Top = location}))
            {
                directoryView.PathChosen += DirectoryOnPathChosen;
                location += directoryView.Height + 5;
                Panel.Controls.Add(directoryView);
            }

            foreach (var fileView in _files.Select(item => new FileView(item) {Top = location}))
            {
                fileView.PathChosen += FileOnPathChosen;
                location += fileView.Height + 5;
                Panel.Controls.Add(fileView);
            }
        }

        private void DirectoryOnPathChosen(object sender, EventArgs e)
        {
            var item = e as DirectoryResultDetails;
            _directories = _directories.Where(f => f.Data.Id != item.Data.Id).
                Where(f => f.Linked == null || f.Linked.Id != item.Data.Id).ToList();
        }

        private void FileOnPathChosen(object sender, EventArgs e)
        {
            var item = e as FileResultDetails;
            _files = _files.Where(f => f.Data.Id != item.Data.Id).
                            Where(f => f.Linked == null || f.Linked.Id != item.Data.Id).ToList();
        }
    }
}
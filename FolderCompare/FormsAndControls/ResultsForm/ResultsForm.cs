using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FolderCompare.FormsAndControls.ResultsForm.Controls;
using FolderCompare.Models.ResultCalcModels;
using FolderCompare.Models.ResultStructure;
using FolderCompare.Utils;

namespace FolderCompare.FormsAndControls.ResultsForm
{
    public class ResultsForm : Form
    {
        private List<DirectoryResultDetails> Directories { get; set; }
        private List<FileResultDetails> Files { get; set; }
        private Panel _panel;

        public ResultsForm(ResultPotentialIssues resultPotentialIssue)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Text = "Results";
            AutoSize = false;
            Size = new Size(750, 600);
            BackColor = GlobalColor.Get(ColorFor.Window);
            Directories = resultPotentialIssue.DirectoryResultDetailsList;
            Files = resultPotentialIssue.FileResultDetailsList;
            DrawWindow();
        }

        private void RemoveAndRedrawPanel()
        {
            if (this._panel != null && this.Controls.Contains(_panel))
            {
                this.Controls.Remove(_panel);
            }

            _panel = new Panel
            {
                Location = new Point(15, 15),
                Name = "M",
                Size = new Size(Width - 30, Height - 50),
                AutoScroll = true
            };
            if (Directories.Any() || Files.Any())
            {
                Controls.Add(_panel);
            }
            else
            {
                Controls.Add(new Label
                {
                    Text = "No Differences found",
                    AutoSize = false,
                    Size = new Size(300, 100),
                    Font = new Font(FontFamily.GenericMonospace, 20, FontStyle.Bold)
                });
            }
        }


        private void DrawWindow()
        {
            ThreadHelper.InvokeOnCtrl(this, RemoveAndRedrawPanel);

            ThreadHelper.InvokeOnCtrl(_panel, () =>
            {
                foreach (var item in _panel.Controls.Cast<Control>())
                {
                    _panel.Controls.Remove(item);
                }

                var location = 0;
                foreach (var item in Directories.OrderBy(f => !f.Source))
                {
                    var directoryView = new DirectoryView(item) { Top = location };
                    directoryView.PathChosen += DirectoryOnPathChosen;
                    directoryView.BackGroundTask += DirectoryViewOnBackGroundTask;
                    location += directoryView.Height + 5;
                    _panel.Controls.Add(directoryView);
                }

                foreach (var item in Files.OrderBy(f => !f.Source).ThenByDescending(x => x.Data.FileInfo.Length))
                {
                    var fileView = new FileView(item) { Top = location };
                    fileView.PathChosen += FileOnPathChosen;
                    fileView.BackGroundTask += DirectoryViewOnBackGroundTask;
                    location += fileView.Height + 5;
                    _panel.Controls.Add(fileView);
                }
            });
        }

        private void DirectoryViewOnBackGroundTask(object sender, EventArgs e)
        {
            ThreadHelper.InvokeOnCtrl(this, () => this._panel.Enabled = false);
        }

        private void DirectoryOnPathChosen(object sender, EventArgs e)
        {
            var item = e as DirectoryResultDetails;
            Directories = Directories.Where(f => f.Data.Id != item.Data.Id).ToList();
            DrawWindow();
        }

        private void FileOnPathChosen(object sender, EventArgs e)
        {
            var item = e as FileResultDetails;
            Files = Files.Where(f => f.Data.Id != item.Data.Id)
                .Where(f => f.Linked == null || f.Linked.Id != item.Data.Id).ToList();
            DrawWindow();
        }
    }
}
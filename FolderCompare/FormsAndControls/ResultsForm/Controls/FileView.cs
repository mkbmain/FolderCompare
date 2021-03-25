using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FolderCompare.Models.ResultStructure;
using FolderCompare.Utils;

namespace FolderCompare.FormsAndControls.ResultsForm.Controls
{
    internal class FileView : NodeViewBase
    {
        private readonly FileResultDetails _fileResultDetails;
        public event EventHandler PathChosen;
        public event EventHandler BackGroundTask;

        private static string GetDescription(bool missMatch, bool source)
        {
            return missMatch
                ? $"Different from {(source ? "destination" : "source")}"
                : $"Missing from {(source ? "destination" : "source")}";
        }

        private Panel FilePanel(FileResultDetails node)
        {
            var panel = new Panel
            {
                Width = MyDefaultSize.Width - 65,
                Height = 220,
                BackColor = GlobalColor.Get(node.Source ? ColorFor.SourceInfo : ColorFor.DestinationInfo)
            };
            var label = new TextBox
            {
                AutoSize = false,
                Multiline = true,
                Size = new Size(panel.Width - 65, 100),
            };
            DropDownBox = new ComboBox
                {Location = new Point(1, label.Bottom + 5), Size = new Size(80, 33)};
            foreach (var actionHandlerWithText in node.ActionHandlerWithTexts)
            {
                DropDownBox.Items.Add(actionHandlerWithText.Text);
            }

            DropDownBox.SelectedIndex = 0;
            var button = new Button
            {
                BackColor = GlobalColor.Get(ColorFor.Button),
                Top = DropDownBox.Top,
                Left = DropDownBox.Right + 5,
                Text = "Fix",
            };

            button.Click += (sender, args) =>
            {
                var item = DropDownBox.SelectedItem.ToString();

                var actionHandler = _fileResultDetails.ActionHandlerWithTexts.First(f => f.Text == item);
                if (actionHandler != null)
                {
                    BackgroundGenerator.Run(null, (o, args2) =>
                        {
                            BackGroundTask?.Invoke(o, args2);
                            actionHandler.Action.Invoke();
                        },
                        (o, args2) => { PathChosen?.Invoke(sender, _fileResultDetails); }, null);
                }
            };

            panel.Height = label.Height + DropDownBox.Height + button.Height;
            panel.Controls.Add(label);
            panel.Controls.Add(DropDownBox);
            panel.Controls.Add(button);
            label.Text = $"{node.Data.FullLocation} is {GetDescription(node.Linked != null, node.Source)}";
            label.ReadOnly = true;
            return panel;
        }


        public FileView(FileResultDetails node)
        {
            _fileResultDetails = node;
            DrawMe(MyDefaultSize, ResourceUtil.GetImageFromResource("FolderCompare.FileIcon.png"),
                $"{FileAndIoUtils.BytesToString(node.Data.FileInfo.Length)} {node.Data.Name}",
                $"File Is {(node.Linked != null ? "different" : "missing")} in {(node.Source ? "destination" : "source")}",
                FilePanel(node), node.Source
            );
        }
    }
}
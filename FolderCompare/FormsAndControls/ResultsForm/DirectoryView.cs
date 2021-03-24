using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FolderCompare.Models.ResultStructure;
using FolderCompare.Utils;

namespace FolderCompare.FormsAndControls.ResultsForm
{
    internal class DirectoryView : NodeViewBase
    {
        private readonly DirectoryResultDetails _directoryResultDetails;
        public event EventHandler BackGroundTask;
        public event EventHandler PathChosen;

        private Panel DirectoryPanel(DirectoryResultDetails node)
        {
            var panel = new Panel
            {
                Width = MyDefaultSize.Width - 65,
                Height = 220,
                BackColor = GlobalColor.Get(node.Source ? ColorFor.SourceInfo : ColorFor.DestinationInfo)
            };
            var label = new Label {Text = node.Data.FullLocation, AutoSize = true};

            var treeView = new TreeView
                {Top = label.Bottom + 5, Nodes = {node.Data.ToTreeNode()}, Size = new Size(panel.Width - 90, 150)};
            treeView.ExpandAll();
            DropDownBox = new ComboBox
                {Location = new Point(treeView.Left+5, treeView.Bottom+5), Size = new Size(130, 44)};
            foreach (var actionHandlerWithText in node.ActionHandlerWithTexts)
            {
                DropDownBox.Items.Add(actionHandlerWithText.Text);
            }

            DropDownBox.SelectedIndex = 0;
            var button = new Button
            {
                Top = DropDownBox.Top,
                Left = DropDownBox.Right+10,
                BackColor = GlobalColor.Get(ColorFor.Button),
                Text = "Fix",
            };

            button.Click += ButtonOnClick;

            panel.Height = button.Bottom + 5;
            panel.Controls.Add(label);
            panel.Controls.Add(DropDownBox);
            panel.Controls.Add(button);
            panel.Controls.Add(treeView);
            return panel;
        }

        private void ButtonOnClick(object sender, EventArgs e)
        {
            var actionHandler = _directoryResultDetails.ActionHandlerWithTexts
                .FirstOrDefault(f => f.Text == DropDownBox.SelectedItem.ToString());
            if (actionHandler != null)
            {
                BackgroundGenerator.Run(null, (o, args) =>
                    {
                        BackGroundTask?.Invoke(o, args);
                        actionHandler.Action.Invoke();
                    },
                    (o, args) => { PathChosen?.Invoke(sender, _directoryResultDetails); }, null);
            }
        }

        public DirectoryView(DirectoryResultDetails node)
        {
            DrawMe(MyDefaultSize, ResourceUtil.GetImageFromResource("FolderCompare.openedfolder.png"), node.Data.Name,
                $"Directory is missing {(node.Source ? "in Destination" : "in source")}", DirectoryPanel(node),
                node.Source);
            _directoryResultDetails = node;
        }
    }
}
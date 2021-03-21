using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BackUpInSynch.Models.ResultStructure;
using BackUpInSynch.Utils;

namespace BackUpInSynch.FormsAndControls.ResultsForm
{
    public class DirectoryView : NodeViewBase
    {
        private DirectoryResultDetails _directoryResultDetails;
        public event EventHandler PathChosen;


        private Panel DirectoryPanel(DirectoryResultDetails node)
        {
            var panel = new Panel{Width = MyDefaultSize.Width-65,Height = 220};
            var label = new Label {Text = node.Data.FullLocation, AutoSize = true};
            var treeView = new TreeView
                {Top = label.Bottom + 5, Nodes = {node.Data.ToTreeNode()}, Size = new Size(panel.Width - 90, 200)};
            DropDownBox = new ComboBox
                {Location = new Point(treeView.Right + 7, treeView.Top), Size = new Size(80, 33)};
            foreach (var actionHandlerWithText in node.ActionHandlerWithTexts)
            {
                DropDownBox.Items.Add(actionHandlerWithText.Text);
            }

            DropDownBox.SelectedIndex = 0;
            var button = new Button
            {
                Top = DropDownBox.Bottom + 5,
                Left = DropDownBox.Left,
                Text = "Fix",
            };

            button.Click += ButtonOnClick;

            panel.Height = label.Height + treeView.Height;
            panel.Controls.Add(label);
            panel.Controls.Add(DropDownBox);
            panel.Controls.Add(button);
            panel.Controls.Add(treeView);
            return panel;
        }

        private void ButtonOnClick(object sender, EventArgs e)
        {
            var item = DropDownBox.SelectedItem.ToString();
            var action = _directoryResultDetails.ActionHandlerWithTexts.First(f => f.Text == item);
            action.Action.Invoke();
            PathChosen?.Invoke(sender, _directoryResultDetails);
        }

        public DirectoryView(DirectoryResultDetails node)
        {
            DrawMe(MyDefaultSize,ResourceUtil.GetImageFromResource("BackUpInSynch.openedfolder.png"), node.Data.Name,
                "Directory is missing", DirectoryPanel(node));
            _directoryResultDetails = node;
        }
    }
}
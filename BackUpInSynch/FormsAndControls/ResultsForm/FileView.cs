using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BackUpInSynch.Models.ResultStructure;
using BackUpInSynch.Utils;

namespace BackUpInSynch.FormsAndControls.ResultsForm
{
    public class FileView : NodeViewBase
    {
        
        private readonly FileResultDetails _fileResultDetails;
        public event EventHandler PathChosen;

        private static string GetDescription(bool missMatch)
        {
            return missMatch ? "Different from destination" : "Missing from destination";
        }

        private Panel FilePanel(FileResultDetails node)
        {
            var panel = new Panel{Width = _size.Width-65,Height = 220};
            var label = new TextBox()
            {
                Text = $"{node.Data.FullLocation} is {GetDescription(node.Linked != null)}",
                AutoSize = false,
                Multiline = true,
                Size = new Size(panel.Width- 65,100),
                
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
                Top = DropDownBox.Top,
                Left = DropDownBox.Right +5,
                Text = "Fix",
            };

            button.Click += (sender, args) =>
            {
                var item = DropDownBox.SelectedItem.ToString();
                var action = _fileResultDetails.ActionHandlerWithTexts.First(f => f.Text == item);
                action.Action.Invoke();
                PathChosen?.Invoke(sender, _fileResultDetails);
            };

            panel.Height = label.Height + DropDownBox.Height + button.Height;
            panel.Controls.Add(label);
            panel.Controls.Add(DropDownBox);
            panel.Controls.Add(button);
            return panel;
        }
        

        public FileView(FileResultDetails node)
        {
            _fileResultDetails = node;
            DrawMe(_size,ResourceUtil.GetImageFromResource("BackUpInSynch.FileIcon.png"), node.Data.Name, "File Is Missing",
                FilePanel(node)
            );
        }
    }
}
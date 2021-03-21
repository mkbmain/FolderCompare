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
        private FileResultDetails fileResultDetails;
        public event EventHandler PathChosen;

        private static string GetDescription(bool missMatch)
        {
            return missMatch ? "Different from destination" : "Missing from destination";
        }

        private Panel FilePanel(FileResultDetails node)
        {
            var panel = new Panel();
            var label = new Label
            {
                Text = $"{node.Data.FullLocation} is {GetDescription(node.Linked != null)}",
                AutoSize = true
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
                Top = DropDownBox.Bottom + 5,
                Left = DropDownBox.Left,
                Text = "Fix",
            };

            button.Click += ButtonOnClick;

            panel.Height = label.Height + DropDownBox.Height + button.Height;
            panel.Controls.Add(label);
            panel.Controls.Add(button);
            return panel;
        }

        private void ButtonOnClick(object sender, EventArgs e)
        {
            var item = DropDownBox.SelectedItem.ToString();
            var action = fileResultDetails.ActionHandlerWithTexts.First(f => f.Text == item);
            action.Action.Invoke();
            PathChosen?.Invoke(sender, fileResultDetails);
        }


        public FileView(FileResultDetails node)
        {
            DrawMe(ResourceUtil.GetImageFromResource("BackUpInSynch.FileIcon.png"), node.Data.Name, "File Is Missing",
                FilePanel(node)
            );
        }
    }
}
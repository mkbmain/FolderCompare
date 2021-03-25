using System.Drawing;
using System.Windows.Forms;
using FolderCompare.Utils;

namespace FolderCompare.FormsAndControls.ResultsForm.Controls
{
    internal abstract class NodeViewBase : Panel
    {
        protected ComboBox DropDownBox;
        protected Size MyDefaultSize = new Size(700, 56);
        private Size _withControl;

        protected void DrawMe(Size size, Image image, string title, string description, Control expandControl,
            bool source)
        {
            MyDefaultSize = size;
            AutoScroll = false;
            AutoSize = false;
            Size = MyDefaultSize;
            _withControl = new Size(Width, Height + expandControl.Height + 10);
            BorderStyle = BorderStyle.FixedSingle;
            var pictureBox = new PictureBox
            {
                Location = new Point(3, 3),
                BorderStyle = BorderStyle.FixedSingle,
                Image = image,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(48, 48)
            };

            var titleLabel = new Label
            {
                Location = new Point(pictureBox.Right + 35, pictureBox.Top - 3),
                Text = title,
                AutoSize = true,
                Font = new Font(FontFamily.GenericMonospace, 20, FontStyle.Bold)
            };

            var descriptionLabel = new Label
            {
                Location = new Point(pictureBox.Right + 5, titleLabel.Bottom + 15),
                Width = Width,
                Text = description,
                Font = new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold)
            };
            var expander = new Label
            {
                Text = "+",
                Location = new Point(Width - 25, 15),
                Font = new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold)
            };
            expandControl.Top = pictureBox.Bottom + 5;
            expandControl.Left = 1;
            Controls.Add(expandControl);
            Controls.Add(expander);
            Controls.Add(pictureBox);
            Controls.Add(titleLabel);
            Controls.Add(descriptionLabel);

            titleLabel.SendToBack();

            BackColor = GlobalColor.Get(source ? ColorFor.SourceInfo : ColorFor.DestinationInfo);
            expander.Click += (sender, args) =>
            {
                expander.Text = expander.Text == "+" ? "-" : "+";
                Size = (Size == MyDefaultSize) ? _withControl : MyDefaultSize;
            };
        }
    }
}
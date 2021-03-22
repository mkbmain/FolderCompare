using System.Drawing;
using System.Windows.Forms;

namespace BackUpInSynch.FormsAndControls.ResultsForm
{
    internal abstract class NodeViewBase : Panel
    {
        protected ComboBox DropDownBox;


        protected Size MyDefaultSize = new Size(700, 56);
        private Size _withControl;


        protected Color GetColor(bool source)
        {
            return source ? Color.LimeGreen : Color.Maroon;
            
        }

        protected void DrawMe(Size size, Image image, string title, string description, Control expandControl, bool source)
        {
            MyDefaultSize = size;
            AutoScroll = false;
            AutoSize = false;
            Size = MyDefaultSize;
            _withControl = new Size(Width, Height + expandControl.Height + 10);
            BorderStyle = BorderStyle.FixedSingle;
            var pictureBox = new PictureBox
            {
                Location = new Point(5, 5),
                BorderStyle = BorderStyle.FixedSingle,
                Image = image,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(50, 50)
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
            Controls.Add(expander);
            expandControl.Top = pictureBox.Bottom;
            expandControl.Left = 1;
            Controls.Add(pictureBox);
            Controls.Add(titleLabel);
            Controls.Add(descriptionLabel);
            Controls.Add(expandControl);
            titleLabel.SendToBack();

            BackColor = GetColor(source);
            expander.Click += (sender, args) =>
            {
                expander.Text = expander.Text == "+" ? "-" : "+";
                Size = (Size == MyDefaultSize) ? _withControl : MyDefaultSize;
            };
        }
    }
}
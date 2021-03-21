using System;
using System.Drawing;
using System.Windows.Forms;

namespace BackUpInSynch.FormsAndControls.ResultsForm
{
    public abstract class NodeViewBase : Panel
    {
        protected ComboBox DropDownBox;

        protected void DrawMe(Image image, string title, string description, Control expandControl)
        {
            DrawMe(new Size(400, 56), image, title, description, expandControl);
        }

        private Size _size;
        private Size _withControl;


        protected void DrawMe(Size size, Image image, string title, string description, Control expandControl)
        {
            _size = size;
            this.AutoScroll = false;
            this.AutoSize = false;
            this.Size = _size;
            _withControl = new Size(this.Width, this.Height + expandControl.Height + 10);
            this.BorderStyle = BorderStyle.FixedSingle;
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
                Width = this.Width,
                Text = description,
                Font = new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold)
            };
            var expander = new Label
            {
                Text = "+",
                Location = new Point(this.Width - 25, 15),
                Font = new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold)
            };
            this.Controls.Add(expander);
            expandControl.Top = pictureBox.Bottom;
            expandControl.Left = 1;
            this.Controls.Add(pictureBox);
            this.Controls.Add(titleLabel);
            this.Controls.Add(descriptionLabel);
            this.Controls.Add(expandControl);
            titleLabel.SendToBack();


            expander.Click += (sender, args) =>
            {
                expander.Text = expander.Text == "+" ? "-" : "+";
                this.Size = (this.Size == _size) ? _withControl : _size;
            };
        }
    }
}
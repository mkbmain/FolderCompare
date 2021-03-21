using System;
using System.Drawing;
using System.Windows.Forms;
using BackUpInSynch.DirectoryStructure;

namespace BackUpInSynch.FormsAndControls.ResultsForm
{
    public class FileView : NodeViewBase
    {
        private static string description(bool missMatch)
        {
            return missMatch ? "Different from destination" : "Missing from destination";
        }

        public FileView(FileNode node, bool missMatch) : base(
            ResourceHelper.GetImageFromResource("BackUpInSynch.FileIcon.png"), node.Name, "File Is Missing", new Label
            {
                Text = $"{node.FullLocation} is {description(missMatch)}",
                AutoSize = true
            })
        {
            
        }
    }

    public class DirectoryView : NodeViewBase
    {
        private static Panel DirectoryPanel(DirectoryNode node)
        {
            var panel = new Panel();
            var label = new Label {Text = node.FullLocation, AutoSize = true};
            var treeView = new TreeView
                {Top = label.Bottom + 5, Nodes = {node.ToTreeNode()}, Size = new Size(300, 200)};
            panel.Height = label.Height + treeView.Height;
            panel.Controls.Add(label);
            panel.Controls.Add(treeView);
            return panel;
        }

        public DirectoryView(DirectoryNode node) : base(
            ResourceHelper.GetImageFromResource("BackUpInSynch.openedfolder.png"), node.Name,
            "Directory is missing", DirectoryPanel(node))
        {
        }
    }

    public abstract class NodeViewBase : Panel
    {
        public readonly Guid MyId;

        protected NodeViewBase(Image image, string title, string description, Control expandControl) : this(
            new Size(400, 56), image, title, description, expandControl)
        {
        }

        private Size _size;
        private Size _withControl;

        protected NodeViewBase(Size size, Image image, string title, string description, Control expandControl)
        {
            MyId = Guid.NewGuid();
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
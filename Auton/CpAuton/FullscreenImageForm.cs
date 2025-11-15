using System;
using System.Drawing;
using System.Windows.Forms;

namespace CpAuton
{
    public class FullscreenImageForm : Form
    {
        private PictureBox pictureBox;
        private Image image;

        public FullscreenImageForm(Image img)
        {
            this.image = img ?? throw new ArgumentNullException(nameof(img));
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.Black;
            this.StartPosition = FormStartPosition.CenterScreen;

            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                Image = image,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Black
            };

            this.Controls.Add(pictureBox);

            // Close on click, double-click or Esc
            pictureBox.Click += (s, e) => this.Close();
            pictureBox.DoubleClick += (s, e) => this.Close();
            this.KeyPreview = true;
            this.KeyDown += FullscreenImageForm_KeyDown;
            this.MouseClick += (s, e) => this.Close();
        }

        private void FullscreenImageForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (pictureBox != null)
                {
                    pictureBox.Image = null;
                    pictureBox.Dispose();
                    pictureBox = null;
                }

                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}

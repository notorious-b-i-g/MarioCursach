#nullable enable
using System.ComponentModel;
using System.Drawing;

namespace MarioWinForms
{
    partial class Form1
    {
        private IContainer? components = null;
        private System.Windows.Forms.Timer gameTimer = null!;
        private System.Windows.Forms.Label scoreLabel = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components is not null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            gameTimer = new System.Windows.Forms.Timer(components)
            {
                Interval = 16          // ~60 FPS
            };
            scoreLabel = new System.Windows.Forms.Label
            {
                AutoSize = true,
                Location = new Point(5, 5),
                ForeColor = Color.White,
                BackColor = Color.Black,
                Text = "Score: 0"
            };
            Controls.Add(scoreLabel);

            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(800, 480);
            DoubleBuffered = true;                     // убирает мерцание
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Mini-Mario (WinForms)";
        }
    }
}

#nullable enable
using System.ComponentModel;

namespace MarioWinForms
{
    partial class Form1
    {
        private IContainer? components = null;
        private System.Windows.Forms.Timer gameTimer = null!;

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

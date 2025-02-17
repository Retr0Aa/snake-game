using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Resources;
using System.Text.Json;
using System.Windows.Forms;

namespace SnakeGame.MainMenu
{
    public class MainMenu : Form
    {
        Size defaultSize = new Size(720, 480);

        PictureBox logo = new PictureBox();

        Stats stats = new Stats();

        public MainMenu()
        {
            this.Text = "Snake Game";
            this.ClientSize = defaultSize;
            this.MaximizeBox = false;
            this.MaximumSize = defaultSize;
            this.MinimumSize = defaultSize;
            this.FormClosed += MainMenu_FormClosed;

            stats = StatsLoader.LoadStats();
            StatsLoader.SaveStats(stats);

            DrawControls();
        }

        private void MainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }

        private void DrawControls()
        {
            // Credits Draw
            Label lblCredits = new Label
            {
                Text = "Game made by Alexander Buchkov",
                Font = new Font("Arial", 10),
                ForeColor = Color.Black,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Width = this.Width,
                Location = new Point(0, 130)
            };

            this.Controls.Add(lblCredits);

            // Logo Draw
            int logoWidth = 57 * 9;
            int logoHeight = 9 * 9;
            logo.SizeMode = PictureBoxSizeMode.StretchImage;
            logo.ClientSize = new Size(logoWidth, logoHeight);
            logo.SizeMode = PictureBoxSizeMode.Normal;
            logo.Paint += Logo_Paint;

            int logoX = (this.ClientSize.Width - logoWidth) / 2;
            int logoY = 50;
            logo.Location = new Point(logoX, logoY);

            this.Controls.Add(logo);

            // Button Draw
            string[] buttonTexts = { "Play", "Skins", "Stats" };
            int buttonWidth = 150;
            int buttonHeight = 50;
            int spacing = 20;
            int totalHeight = (buttonHeight + spacing) * buttonTexts.Length - spacing;
            int startY = (this.ClientSize.Height - totalHeight) / 2 + 50;

            for (int i = 0; i < buttonTexts.Length; i++)
            {
                Button btn = new Button
                {
                    Text = buttonTexts[i],
                    Size = new Size(buttonWidth, buttonHeight),
                    Location = new Point((this.ClientSize.Width - buttonWidth) / 2, startY + i * (buttonHeight + spacing)),
                    Font = new Font("Arial", 14)
                };
                btn.Click += Btn_Click;

                this.Controls.Add(btn);
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Text)
            {
                case "Play":
                    BaseSnake.SnakeGame game = new BaseSnake.SnakeGame();
                    game.Show();

                    this.Hide();
                    break;
                case "Skins":
                    MessageBox.Show("Feature not implemented yet!");
                    break;
                case "Stats":
                    MessageBox.Show($"Your Stats:\n Best Score: {stats.BestScore}", "Stats", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    break;
                default:
                    break;
            }
        }

        private void Logo_Paint(object sender, PaintEventArgs e)
        {
            if (logo == null) return;

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.None;
            e.Graphics.DrawImage(Resources.Logo, new Rectangle(0, 0, logo.Width, logo.Height));
        }
    }
}

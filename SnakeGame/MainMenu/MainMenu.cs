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
        PictureBox logo_skins = new PictureBox();

        Stats stats = new Stats();

        int currentMenu = 0;
        int currentSkinChoseIndex = 0;

        List<string> availableSkins = new List<string>()
        {
            "green",
            "yellow",
            "blue",
            "purple",
            "pink",
            "lightgreen"
        };

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

            RedrawControls();
        }

        private void MainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }

        private void RedrawControls()
        {
            this.Controls.Clear();

            switch (currentMenu)
            {
                case 0:
                    LoadMainMenuScreen();
                    break;
                case 1:
                    LoadSkinsScreen();
                    break;
                default:
                    break;
            }
        }

        private void LoadMainMenuScreen()
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

            int resetScoreButtonWidth = 130;
            int resetScoreButtonHeight = 30;
            Button resetScoreButton = new Button
            {
                Text = "Reset Best Score",
                BackColor = Color.Red,
                ForeColor = Color.White,
                Size = new Size(resetScoreButtonWidth, resetScoreButtonHeight),
                Location = new Point((this.ClientSize.Width - resetScoreButtonWidth) - 10, (this.ClientSize.Height - resetScoreButtonHeight) - 10)
            };
            resetScoreButton.Click += ResetScoreButton_Click;

            this.Controls.Add(resetScoreButton);
        }

        private void ResetScoreButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to reset your best score ({stats.BestScore})?", "Reset Best Score", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                // Reset Score
                stats.BestScore = 0;
                StatsLoader.SaveStats(stats);
            }
        }

        private void LoadSkinsScreen()
        {
            currentSkinChoseIndex = availableSkins.IndexOf(stats.CurrentSkin.ToLower());

            // Skins Text Draw
            Label lblSkins = new Label
            {
                Text = "Customize your snake",
                Font = new Font("Arial", 10),
                ForeColor = Color.Black,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Width = this.Width,
                Location = new Point(0, 130)
            };

            this.Controls.Add(lblSkins);

            // Selected Skin Text Draw
            Label lblSelectedSkin = new Label
            {
                Text = availableSkins[currentSkinChoseIndex],
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Width = this.Width,
                Location = new Point(0, 300)
            };

            this.Controls.Add(lblSelectedSkin);

            // Logo Draw
            int logoWidth = 28 * 9;
            int logoHeight = 9 * 9;
            logo_skins.SizeMode = PictureBoxSizeMode.StretchImage;
            logo_skins.ClientSize = new Size(logoWidth, logoHeight);
            logo_skins.SizeMode = PictureBoxSizeMode.Normal;
            logo_skins.Paint += Logo_Skins_Paint;

            int logoX = (this.ClientSize.Width - logoWidth) / 2;
            int logoY = 50;
            logo_skins.Location = new Point(logoX, logoY);

            this.Controls.Add(logo_skins);

            // Button Draw
            string[] buttonTexts = { "Select" };
            int buttonWidth = 150;
            int buttonHeight = 50;
            int spacing = 20;
            int totalHeight = (buttonHeight + spacing) * buttonTexts.Length - spacing;
            int startY = (this.ClientSize.Height - totalHeight) / 2 + 50;

            Button selectBtn = new Button
            {
                Text = "Select",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point((this.ClientSize.Width - buttonWidth) / 2, 350),
                Font = new Font("Arial", 14)
            };
            selectBtn.Click += Btn_Click;

            this.Controls.Add(selectBtn);

            // Preview Draw
            Panel previewPanel = new Panel
            {
                Size = new Size(260, 100),
                Location = new Point((this.ClientSize.Width - 260) / 2, (this.ClientSize.Height - 100) / 2 + 10),
                BackColor = Color.SkyBlue
            };
            previewPanel.Paint += PreviewPanel_Paint;

            this.Controls.Add(previewPanel);

            // Change Buttons Draw
            Button changePreviousBtn = new Button
            {
                Text = "<-",
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(50, buttonHeight),
                Location = new Point((this.ClientSize.Width - buttonWidth) / 2 - 130, (this.ClientSize.Height - buttonHeight) / 2),
                Font = new Font("Arial", 14),
                Enabled = currentSkinChoseIndex > 0
            };

            Button changeNextBtn = new Button
            {
                Text = "->",
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(50, buttonHeight),
                Location = new Point((this.ClientSize.Width - buttonWidth) / 2 + 230, (this.ClientSize.Height - buttonHeight) / 2),
                Font = new Font("Arial", 14),
                Enabled = currentSkinChoseIndex < availableSkins.Count - 1
            };

            changePreviousBtn.Click += (object sender, EventArgs e) =>
            {
                currentSkinChoseIndex--;

                lblSelectedSkin.Text = availableSkins[currentSkinChoseIndex];

                changePreviousBtn.Enabled = currentSkinChoseIndex > 0;
                changeNextBtn.Enabled = currentSkinChoseIndex < availableSkins.Count - 1;

                previewPanel.Refresh();
            };


            changeNextBtn.Click += (object sender, EventArgs e) =>
            {
                currentSkinChoseIndex++;

                lblSelectedSkin.Text = availableSkins[currentSkinChoseIndex];

                changeNextBtn.Enabled = currentSkinChoseIndex < availableSkins.Count - 1;
                changePreviousBtn.Enabled = currentSkinChoseIndex > 0;

                previewPanel.Refresh();
            };

            this.Controls.Add(changeNextBtn);
            this.Controls.Add(changePreviousBtn);
        }

        private void PreviewPanel_Paint(object sender, PaintEventArgs e)
        {
            const int GridSize = 20;
            const int GridWidth = 20;
            const int GridHeight = 20;

            Graphics g = e.Graphics;

            using (Pen gridPen = new Pen(Color.LightBlue, 1))
            {
                for (int x = 0; x < GridWidth; x++)
                {
                    for (int y = 0; y < GridHeight; y++)
                    {
                        g.DrawRectangle(gridPen, x * GridSize, y * GridSize, GridSize, GridSize);
                    }
                }
            }

            Point position = new Point(10, 2);

            g.FillRectangle(Brushes.Red, position.X * GridSize, position.Y * GridSize, GridSize, GridSize);

            g.DrawLine(new Pen(Brushes.Green, 2),
                position.X * GridSize + GridSize / 2,
                position.Y * GridSize,
                position.X * GridSize + GridSize,
            position.Y * GridSize - GridSize * 0.5f);

            for (int x = 1; x < 7; x++)
            {
                g.FillRectangle(new SolidBrush(SkinsManager.GetSkinColor(availableSkins[currentSkinChoseIndex])), x * GridSize, position.Y * GridSize, GridSize, GridSize);
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
                    currentMenu = 1;
                    RedrawControls();
                    break;
                case "Stats":
                    MessageBox.Show($"Your Stats:\n Best Score: {stats.BestScore}", "Stats", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    break;
                case "Select":
                    stats.CurrentSkin = availableSkins[currentSkinChoseIndex];
                    StatsLoader.SaveStats(stats);

                    currentMenu = 0;
                    RedrawControls();
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

        private void Logo_Skins_Paint(object sender, PaintEventArgs e)
        {
            if (logo_skins == null) return;

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.None;
            e.Graphics.DrawImage(Resources.Logo_Skins, new Rectangle(0, 0, logo_skins.Width, logo_skins.Height));
        }
    }
}


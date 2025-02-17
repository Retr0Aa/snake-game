using SnakeGame.MainMenu;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame.BaseSnake
{
    public class SnakeGame : Form
    {
        private const int GridSize = 20;
        private const int GridWidth = 20;
        private const int GridHeight = 20;

        private Timer gameTimer;
        private Snake snake;
        private Food food;
        private Stats stats;

        private bool isGameOver;
        private int currentScore = 0;

        public SnakeGame()
        {
            stats = StatsLoader.LoadStats();

            Size defaultSize = new Size(GridSize * GridWidth, GridSize * GridHeight);

            this.Text = "Snake Game";
            this.ClientSize = defaultSize - new Size(-17, -40);
            this.DoubleBuffered = true;
            this.BackColor = Color.SkyBlue;
            this.MaximizeBox = false;
            this.MaximumSize = defaultSize - new Size(-17, -40);
            this.MinimumSize = defaultSize - new Size(-17, -40);

            gameTimer = new Timer { Interval = 100 };
            gameTimer.Tick += UpdateGame;

            snake = new Snake(GridWidth / 2, GridHeight / 2);
            food = new Food(GridWidth, GridHeight, snake);

            this.KeyDown += OnKeyDown;
            gameTimer.Start();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (isGameOver) return;
            snake.ChangeDirection(e.KeyCode);
        }

        private void UpdateGame(object sender, EventArgs e)
        {
            if (isGameOver) return;

            if (!snake.Move(GridWidth, GridHeight))
            {
                if (stats.BestScore < currentScore)
                {
                    stats.BestScore = currentScore;
                    StatsLoader.SaveStats(stats);
                }

                isGameOver = true;
                gameTimer.Stop();

                if (MessageBox.Show($"You Lost with score: {currentScore}.\n Retry or Main Menu?", "Game Over!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) == DialogResult.Retry)
                {
                    // Retry Game
                    SnakeGame snakeGame = new SnakeGame();
                    snakeGame.Show();

                    this.Close();
                }
                else
                {
                    // Main Menu
                    MainMenu.MainMenu mainMenu = new MainMenu.MainMenu();
                    mainMenu.Show();

                    this.Close();
                }

                return;
            }

            if (snake.Head == food.Position)
            {
                snake.Grow();
                food.GenerateNewPosition(snake);

                currentScore++;
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
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

            snake.Draw(g, GridSize, stats.CurrentSkin);
            food.Draw(g, GridSize);

            Font drawFont = new Font("Arial", 10);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            e.Graphics.DrawString($"Score: {currentScore}", drawFont, drawBrush, 10, 10);
        }
    }
}

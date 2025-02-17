using SnakeGame.MainMenu;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame.BaseSnake
{
    public class Snake
    {
        private List<Point> body;
        private Point direction;

        public Snake(int startX, int startY)
        {
            body = new List<Point> { new Point(startX, startY) };
            direction = new Point(1, 0);
        }

        public Point Head => body[0];

        public void ChangeDirection(Keys key)
        {
            switch (key)
            {
                case Keys.Up when direction.Y == 0:
                    direction = new Point(0, -1);
                    break;
                case Keys.Down when direction.Y == 0:
                    direction = new Point(0, 1);
                    break;
                case Keys.Left when direction.X == 0:
                    direction = new Point(-1, 0);
                    break;
                case Keys.Right when direction.X == 0:
                    direction = new Point(1, 0);
                    break;
            }
        }

        public bool Move(int gridWidth, int gridHeight)
        {
            Point newHead = new Point(Head.X + direction.X, Head.Y + direction.Y);

            if (newHead.X < 0 || newHead.Y < 0 || newHead.X >= gridWidth || newHead.Y >= gridHeight || body.Contains(newHead))
                return false;

            body.Insert(0, newHead);
            body.RemoveAt(body.Count - 1);
            return true;
        }

        public void Grow()
        {
            body.Add(body[body.Count - 1]);
        }

        public void Draw(Graphics g, int gridSize, string skin)
        {
            foreach (Point p in body)
            {
                g.FillRectangle(new SolidBrush(SkinsManager.GetSkinColor(skin)), p.X * gridSize, p.Y * gridSize, gridSize, gridSize);
            }
        }
    }
}

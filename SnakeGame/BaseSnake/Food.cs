using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.BaseSnake
{
    public class Food
    {
        private Point position;
        private Random rand;
        private int gridWidth, gridHeight;

        public Food(int gridWidth, int gridHeight, Snake snake)
        {
            this.gridWidth = gridWidth;
            this.gridHeight = gridHeight;
            rand = new Random();
            GenerateNewPosition(snake);
        }

        public Point Position => position;

        public void GenerateNewPosition(Snake snake)
        {
            do
            {
                position = new Point(rand.Next(gridWidth), rand.Next(gridHeight));
            } while (snake.Head == position);
        }

        public void Draw(Graphics g, int gridSize)
        {
            g.FillRectangle(Brushes.Red, position.X * gridSize, position.Y * gridSize, gridSize, gridSize);
        }
    }
}

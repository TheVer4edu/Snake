using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Timer = System.Timers.Timer;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Game.Launch();
        }
    }

    static class Game
    {
        private static Queue<Point> _snake = new Queue<Point>();
        private static Point _apple;
        private static int _width = Console.WindowWidth, _height = Console.WindowHeight;
        private static Pressed lastKey;
        private static bool _gameOver;
        private static Timer timer;

        public static void Launch()
        {
            _snake.Enqueue(new Point(0,0));
            _apple = GenerateApple();
            timer = new Timer(50);
            timer.Elapsed += (sender, args) =>
            {
                lastKey = Console.ReadKey().Key switch
                {
                    ConsoleKey.W => Pressed.Up,
                    ConsoleKey.S => Pressed.Down,
                    ConsoleKey.A => Pressed.Left,
                    ConsoleKey.D => Pressed.Right
                };
                MoveSnake();
                if (_snake.Last() == _apple)
                    EatApple();
                Redraw();
            };
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Start();
            while (true) { }
        }

        private static void Redraw()
        {
            Console.Clear();
            bool head = true;
            foreach (Point part in _snake.Reverse())
            {
                Console.SetCursorPosition(part.X, part.Y);
                Console.Write(head ? 'ё' : 'O');
                head = false;
            }
            Console.SetCursorPosition(_apple.X, _apple.Y);
            Console.Write('@');
        }


        private static void EatApple()
        {
            _apple = GenerateApple();
            _snake.Enqueue(new Point(_snake.Peek().X, _snake.Peek().Y));
        }

        private static void MoveSnake()
        {
            int dX = 0, dY = 0;
            switch (lastKey)
            {
                case Pressed.Down:
                    dY = 1;
                    break;
                case Pressed.Up:
                    dY = -1;
                    break;
                case Pressed.Left:
                    dX = -1;
                    break;
                case Pressed.Right:
                    dX = 1;
                    break;
            }
            _snake.Enqueue(new Point(_snake.Last().X + dX, _snake.Last().Y + dY));
            _snake.Dequeue();
        }

        private static Point GenerateApple()
        {
            Random rand = new Random();
            Point probableApple = new Point(rand.Next(0, 20), rand.Next(0, 20));
            if (_snake.Contains(probableApple)) return GenerateApple();
            return probableApple;
        }
    }

    enum Pressed
    {
        Up,
        Down,
        Left,
        Right
    }
}
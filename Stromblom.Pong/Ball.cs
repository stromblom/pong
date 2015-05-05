using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stromblom.Pong
{
    public class Ball
    {
        public int Y { get; private set; }
        public int X { get; private set; }

        private bool _directionDown = false;
        private bool _directionRight = false;
        private Random _generator;

        public int Speed { get; private set; }

        public Ball()
        {
            _generator = new Random();

            Reset();
        }
        public void Reset()
        {
            Y = Console.WindowHeight / 2 - 1;
            X = Console.WindowWidth / 2;

            _directionDown = _generator.Next(0, 2) == 1 ? true : false;
            _directionRight = _generator.Next(0, 2) == 1 ? true : false;
        }

        public void Draw(bool clear = false)
        {
            lock (Game.DrawLock)
            {
                if (clear)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.CursorLeft = X;
                Console.CursorTop = Y;

                Console.Write("X");
            }
        }

        public bool Move(Player playerOne, Player playerTwo)
        {
            if (CollisionX())
                return false;

            Draw(clear: true);
            if (CollisionWith(playerOne) || CollisionWith(playerTwo)) // Collision with player
            {
                _directionRight = !_directionRight;
                ChangeSpeed();
            }
            else if (CollisionY())
            {
                _directionDown = !_directionDown;
                ChangeSpeed();
            }

            Y += _directionDown ? 1 : -1;
            X += _directionRight ? 1 : -1;

            Draw();

            return true;
        }

        private void ChangeSpeed()
        {
            var changeSpeed = _generator.Next(0, 2) == 1;

            if (!changeSpeed)
                return;
            var addSpeed = _generator.Next(0, 2) == 1 ? 10 : -10;
            if (Speed + addSpeed > 10 || Speed + addSpeed < -10)
                return;

            Speed += addSpeed;
        }

        private bool CollisionWith(Player player)
        {
            var moveY = _directionDown ? 1 : -1;
            var yCollision = Y + moveY == player.Y - 1 || Y + moveY == player.Y + 1 || Y + moveY == player.Y;

            if(!yCollision)
                return false;

            if (_directionRight && player.IsRightSide)
                return X + 1 >= player.X;
            else if (!_directionRight && !player.IsRightSide)
                return X - 1 <= player.X;

            return false;
        }

        private bool CollisionY()
        {
            return !(Y + 1 < Console.WindowHeight && Y - 1 >= 2);
        }

        private bool CollisionX()
        {
            return !(X + 1 < Console.WindowWidth && X - 1 >= 0);
        }
    }
}

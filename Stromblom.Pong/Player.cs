using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stromblom.Pong
{
    public class Player
    {
        public string Name { get; private set; }
        public int Y { get; set; }
        public int X { get; set; }
        public bool IsRightSide { get; private set; }
        public int Points { get; set; }

        public Player(string name, bool rightSide)
        {
            Name = name;
            IsRightSide = rightSide;

            Reset();

        }
        public void Reset()
        {
            Y = Console.WindowHeight / 2 + 1;

            if (IsRightSide)
                X = Console.WindowWidth - 3;
            else
                X = 2;
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
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                var drawPosition = Y - 1;

                for (int i = 0; i < 3; i++)
                {
                    Console.CursorLeft = X;
                    Console.CursorTop = drawPosition + i;
                    Console.Write("X");
                }
            }
        }

        public void Move(Move move)
        {
            var newPosition = Y + (int)move;
            if (CanMoveTo(newPosition))
            {
                Draw(clear: true);

                Y = newPosition;

                Draw();
            }
        }

        private bool CanMoveTo(int newPosition)
        {
            return newPosition + 1 < Console.WindowHeight && newPosition - 1 >= 2;
        }
    }
}

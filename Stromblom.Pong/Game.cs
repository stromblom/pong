using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stromblom.Pong
{
    public class Game
    {
        public static object DrawLock = new Object();
        private bool _roundEnded = false;
        private Player _playerOne;
        private Player _playerTwo;
        private Ball _ball;

        private int _gameAreaWidth = 40;
        private int _gameAreaHeight = 21;

        public Game()
        {
            Console.CursorVisible = false;
            Console.WindowHeight = _gameAreaHeight;
            Console.WindowWidth = _gameAreaWidth;
            Console.BufferHeight = _gameAreaHeight;
            Console.BufferWidth = _gameAreaWidth;

            _ball = new Ball();
            _playerOne = InitializePlayer();
            _playerTwo = InitializePlayer(secondPlayer: true);

        }

        public void Run()
        {
            while (_playerOne.Points < 3 && _playerTwo.Points < 3)
            {
                _roundEnded = false;
                var handleInputTask = new Task(HandleInput);
                Clear();
                InitialDraw();

                while (!Console.KeyAvailable) { }

                handleInputTask.Start();
                while (_ball.Move(_playerOne, _playerTwo))
                {
                    Thread.Sleep(115 - _ball.Speed);
                }
                _roundEnded = true;
                handleInputTask.Wait();

                if (_ball.X <= _playerOne.X)
                    _playerTwo.Points++;
                else
                    _playerOne.Points++;

                Reset();
            }
            Clear();
            var winner = _playerOne.Points == 3 ? _playerOne : _playerTwo;

            Console.ResetColor();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("{0} won the game!", winner.Name);

        }

        private void InitialDraw()
        {
            _playerOne.Draw();
            _playerTwo.Draw();
            _ball.Draw();
            DrawScoreBoard();
        }

        private void DrawScoreBoard()
        {
            lock (Game.DrawLock)
            {
                Console.SetCursorPosition(0, 0);
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;

                for (int i = 0; i < Console.WindowWidth; i++)
                    Console.Write(" ");
                for (int i = 0; i < Console.WindowWidth; i++)
                    Console.Write(" ");

                Console.SetCursorPosition(18 - _playerOne.Name.Length, 0);
                Console.Write("{0} vs {1}", _playerOne.Name, _playerTwo.Name);

                Console.SetCursorPosition(19 - _playerOne.Points.ToString().Length, 1);
                Console.Write("{0}  {1}", _playerOne.Points, _playerTwo.Points);
            }
        }

        private void Clear()
        {
            Console.ResetColor();
            Console.Clear();
        }

        private void Reset()
        {
            _playerOne.Reset();
            _playerTwo.Reset();
            _ball.Reset();
        }

        private Player InitializePlayer(bool secondPlayer = false)
        {
            Clear();
            var playerName = String.Empty;
            while (true)
            {
                Console.SetCursorPosition(0, 0);
                if (!secondPlayer)
                    Console.WriteLine("Player 1 uses w/s to move up/down");
                else
                    Console.WriteLine("Player 2 uses p/l to move up/down");
                Console.Write("Name of player {0}: ", secondPlayer ? "two" : "one");
                playerName = Console.ReadLine();

                if (playerName.Length < 18)
                    break;

                Clear();

                Console.WriteLine("Player name must be less then 18 characters long.");
            }

            return new Player(playerName, secondPlayer);
        }
        private void HandleInput()
        {
            while (!_roundEnded)
            {
                var key = Console.ReadKey(true).KeyChar;
                Thread.Sleep(10);

                switch (key)
                {
                    case 'w': // move up
                        _playerOne.Move(Move.Up);
                        break;
                    case 's': // move down
                        _playerOne.Move(Move.Down);
                        break;
                    case 'p': // move up
                        _playerTwo.Move(Move.Up);
                        break;
                    case 'l': // move down
                        _playerTwo.Move(Move.Down);
                        break;
                }
            }
        }
    }
}

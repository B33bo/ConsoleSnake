using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;

using ConsoleGames.Games.SnakeGame;

namespace ConsoleGames.Games
{
    public static class Snake
    {
        public static int Score;
        public static void Main(string[] args)
        {
            Game.Screen = new(30, 15);
            Game.AddObject(new SnakeTail());
            Game.AddObject(new Fruit());

            Game.Start();
        }
    }
}

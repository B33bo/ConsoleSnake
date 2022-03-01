using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;
using TextEngine.Colors;
using ConsoleGames.Games;

namespace ConsoleGames.Games.SnakeGame
{
    class Fruit : GameObject
    {
        public Fruit()
        {
            Color = Color.Red;
            Character = 'O';
            Position = RandomNG.Vector(Vector2D.Zero, Vector2D.Zero + Game.Screen);
        }

        public override void OnCollision(GameObject type, Vector2D displacement)
        {
            Game.ToolBar = $"Score: {++Snake.Score}";
            Position = RandomNG.Vector(Vector2D.Zero, Camera.BottomRight);

            SnakeTail newTail = new();
            Game.AddObject(newTail);

            Sound.PlayNote(new Note(750, 250));
        }
    }
}

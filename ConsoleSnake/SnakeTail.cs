using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;

namespace ConsoleGames.Games.SnakeGame
{
    public class SnakeTail : GameObject
    {
        static List<SnakeTail> tails = new();
        public static float timeBetweenMovement = 100;
        static Stopwatch watch;

        Vector2D movementVec;
        Vector2D movementVecPreFrame;

        public int indexFromHead;
        static long lastMovement;
        static bool NoHit = false;
        static int frames = 0;

        string cheatString = "";

        bool GaveGracePeriod;

        public SnakeTail()
        {
            Color = new("#88FF88");

            Character = '@';
            indexFromHead = tails.Count;

            if (indexFromHead == 0)
            {
                Position = new(0, 0);
                watch = new();
                watch.Start();
            }
            else
            {
                movementVec = tails[^1].movementVec;
                Position = tails[^1].Position - movementVec;
            }

            tails.Add(this);
        }

        public override void KeyPress(ConsoleKey key)
        {
            if (indexFromHead != 0)
                return;

            switch (key)
            {
                default:
                    cheatString += ((char)key);

                    if (cheatString.Length > 5)
                        cheatString = cheatString[^5..];

                    if (cheatString.ToLower() == "xyzzy")
                        Cheat();

                    break;
                case ConsoleKey.W:
                    ChangeMoveVec(new(0, -1), frames);
                    break;
                case ConsoleKey.S:
                    ChangeMoveVec(new(0, 1), frames);
                    break;
                case ConsoleKey.A:
                    ChangeMoveVec(new(-1, 0), frames);
                    break;
                case ConsoleKey.D:
                    ChangeMoveVec(new(1, 0), frames);
                    break;
            }
        }

        static void Cheat()
        {
            string cheat = Game.Ask();
            string[] parameters = cheat.Split(',');

            switch (parameters[0])
            {
                default:
                    break;
                case "points":
                    int increase = int.Parse(parameters[1]) - Snake.Score;
                    Snake.Score += increase;
                    break;
                case "speed":
                    timeBetweenMovement = int.Parse(parameters[1]);
                    break;
                case "nohit":
                    NoHit = !NoHit;
                    break;
            }
        }

        private void ChangeMoveVec(Vector2D newVec, int frame)
        {
            if (movementVecPreFrame + newVec == new Vector2D(0, 0))
                return;

            movementVec = newVec;
        }

        public override void OnCollision(GameObject type, Vector2D displacement)
        {
            if (indexFromHead != 0)
                return;

            if (tails.Contains(type))
            {
                if (((SnakeTail)type).indexFromHead == 0)
                    return;
                Die();
            }
        }

        public static void Die()
        {
            if (NoHit)
                return;

            Sound.PlayNote(new(250, 1000));
            Game.Stop();
        }

        public override void Update()
        {
            if (indexFromHead != 0)
                return;

            long timeSinceLastMove = watch.ElapsedMilliseconds - lastMovement;

            if (timeSinceLastMove < timeBetweenMovement)
                return;

            frames++;
            lastMovement = watch.ElapsedMilliseconds;
            Vector2D newPos = Position + movementVec;

            if (!newPos.InScreen())
            {
                if (!GaveGracePeriod)
                {
                    GaveGracePeriod = true;
                    return;
                }

                Die();
                return;
            }

            GaveGracePeriod = false;

            Move(movementVec);

            Character = CharacterFromMoveVec(movementVec);
            for (int i = tails.Count - 1; i >= 1; i--)
            {
                tails[i].Position += tails[i].movementVec;
                tails[i].movementVec = tails[i - 1].movementVec;

                tails[i].Character = CharacterFromMoveVec(tails[i].movementVec);
            }

            movementVecPreFrame = movementVec;
        }

        public static char CharacterFromMoveVec(Vector2D vector)
        {
            if (vector.X > 0)
                return '>';

            if (vector.X < 0)
                return '<';

            if (vector.Y > 0)
                return 'V';

            if (vector.Y < 0)
                return '^';

            return '@';
        }
    }
}

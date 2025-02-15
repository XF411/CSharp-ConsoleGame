using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Learn
{
    public class SankeGameScence:GameScenceBase
    {

        public SankeGameScence()
        {
            e_ScenceType = E_ScenceType.SnakeGameScence;
            mapWidth = ConsoleControler.Instance.DisplayWidth;
            mapHeight = ConsoleControler.Instance.DisplayHeight;
        }

        /// <summary>
        /// map width and height
        /// </summary>
        int mapWidth, mapHeight;
        private Snake Snake;
        private bool isNewGame = true;

        /// <summary>
        /// how many frame per second
        /// </summary>
        private int GameSpeed = 1;

        private DateTime lastUpdateTime;

        public void StartNewGame() 
        {
            isNewGame = true;
            Snake = new Snake(mapWidth/2, mapHeight/2);
            lastUpdateTime = DateTime.Now;
        }

        private void DrawWall() 
        {
            for (int i = 0; i < mapWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("-");
                Console.SetCursorPosition(i, mapHeight - 1);
                Console.Write("-");
            }

            for (int i = 0; i < mapHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("|");
                Console.SetCursorPosition(mapWidth - 2, i);
                Console.Write("|");
            }
        }

        public override void Update()
        {
            CheckKeyInput();
            var now = DateTime.Now;
            if ((now - lastUpdateTime).TotalMilliseconds >= 1000 / GameSpeed)
            {
                lastUpdateTime = now;
                if (isNewGame)
                {
                    DrawWall();
                    isNewGame = false;
                }
                Snake.Move();
                DrawSnake();
            }
        }

        private void DrawSnake()
        {
            Snake.Draw();
        }

        private void CheckKeyInput()
        {
            if (!Console.KeyAvailable)
            {
                return;
            }
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.LeftArrow:
                    Snake.Turn(MoveDirection.Left);
                    break;
                case ConsoleKey.UpArrow:
                    Snake.Turn(MoveDirection.Up);
                    break;
                case ConsoleKey.RightArrow:
                    Snake.Turn(MoveDirection.Right);
                    break;
                case ConsoleKey.DownArrow:
                    Snake.Turn(MoveDirection.Down);
                    break;
                default:
                    break;
            }
        }   

        public override void OnExit()
        {
           
        }

    }
}

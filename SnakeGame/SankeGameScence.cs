using System;
using System.Collections.Generic;
using System.Drawing;
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

            availablePositions = new List<(int, int)>();
            bodyList = new List<(int, int)>();
            foodPosList = new List<(int, int)>();
        }

        /// <summary>
        /// map width and height
        /// </summary>
        int mapWidth, mapHeight;
        int wallWidth, wallHeight;
        private Snake Snake;
        private bool isNewGame = true;

        private SnakeFood? curFood;

        /// <summary>
        /// how many frame per second
        /// </summary>
        private int GameSpeed = 2;

        private DateTime lastUpdateTime;
        List<(int, int)> availablePositions;

        List<(int, int)> bodyList = new List<(int, int)>();
        List<(int, int)> foodPosList = new List<(int, int)>();
        public void StartNewGame() 
        {
            point = 0;
            EndGame = false;
            isNewGame = true;
            Snake = new Snake(mapWidth/2, mapHeight/2);
            lastUpdateTime = DateTime.Now;
            isExit = false;
            curFood = null;
            GameSpeed = 2;
            wallWidth = ConsoleControler.Instance.DisplayWidth - 1;
            wallHeight = ConsoleControler.Instance.DisplayHeight - 2;
            availablePositions.Clear();
            bodyList.Clear();
            foodPosList.Clear();
            for (int i = 1; i < mapWidth - 1; i++) 
            {
                for (int j = 1; j < mapHeight - 1; j++) 
                {
                    availablePositions.Add((i, j));
                } 
            }
        }

        private void DrawWall()
        {

            Console.ForegroundColor = ConsoleColor.Blue;
            for (int i = 0; i < wallWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                if (availablePositions.Contains((i, 0)))
                {
                    availablePositions.Remove((i,0));
                }
                Console.Write("▉");
                Console.SetCursorPosition(i, wallHeight);
                if (availablePositions.Contains((i, wallHeight)))
                {
                    availablePositions.Remove((i, wallHeight));
                }
                Console.Write("▉");
                
            }

            for (int i = 0; i < wallHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                if (availablePositions.Contains((0, i)))
                {
                    availablePositions.Remove((0, i));
                }
                Console.Write("▉");

                Console.SetCursorPosition(wallWidth - 1, i);
                if (availablePositions.Contains((wallWidth - 1, i)))
                {
                    availablePositions.Remove((wallWidth - 1, i));
                }
                Console.Write("▉");
            }
        }

        public override void Update()
        {
            if (isExit) 
            {
                return;
            }
            var now = DateTime.Now;
            if ((now - lastUpdateTime).TotalMilliseconds >= 1000 / GameSpeed)
            {
                
                lastUpdateTime = now;
                if (isNewGame)
                {
                    DrawWall();
                    isNewGame = false;
                }
                GenFood();
                Snake.Move();
                CheckSnakeHit();
                if (!EndGame)
                {
                    DrawSnake();
                }                
            }
            if (EndGame)
            {
                EventBroadcaster.Instance.BroadcastEvent(EventName.EndSnakeGame);
            }
            else
            {
                CheckKeyInput();
            }
        }

        void GenFood()
        {
            if (curFood == null)
            {
                bodyList.Clear();
                foreach (var body in Snake.Bodies)
                {
                    bodyList.Add((body.CurPosX, body.CurPosY));
                    break;
                }

                foodPosList.Clear();
                foreach (var pos in availablePositions) 
                {
                    if (!bodyList.Contains(pos)) 
                    {
                        foodPosList.Add(pos);
                    }
                }

                if (availablePositions.Count > 0)
                {
                    int randomIndex = CommonTools.CommonRandom.Next(foodPosList.Count);
                    var (x, y) = availablePositions[randomIndex];
                    curFood = new SnakeFood();
                    curFood.UpdatePosition(x, y);
                    curFood.Draw();
                }
            }
        }


        bool EndGame = false;

        private void CheckSnakeHit() 
        {
            int x = Snake.Bodies[0].CurPosX;
            int y = Snake.Bodies[0].CurPosY;
            //check for wall,if hit wall,game over

            bool hitBody = false;
            foreach (var body in Snake.Bodies) 
            {
                if (x == body.CurPosX && y == body.CurPosY && body.Type != BodyType.Head)
                {
                    hitBody = true;
                }
            }

            if (x == 0 || x == wallWidth || y == 0 || y == wallHeight || hitBody)
            {
                EndGame = true;
            }
            else 
            {
                //check eat food;
                if (curFood != null && x == curFood.CurPosX && y == curFood.CurPosY)
                {
                    point++;
                    GameSpeed =  Math.Max(2, point/2) + 1;
                    Snake.Grow();
                    curFood = null;
                }
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

        private bool isExit = false;
        public override void OnExit()
        {
            isExit = true;
        }

        private int point;

        internal int GetPoint()
        {
            return point;
        }
    }
}

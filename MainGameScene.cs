using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace C_Learn
{
    public class MainGameScene
    {

        List<MapGrid> mapData = new List<MapGrid>();

        Player player = new Player();

        Player enemy = new Player();

        bool isPlayerTurn = true;
        /// <summary>
        /// map width and height
        /// </summary>
        int mapWidth, mapHeight;

        int mapZeroX, mapZeroY;

        /// <summary>
        /// reset player pos
        /// </summary>
        public void ReSetPlayer() 
        {
            player.curStep = 0;
            enemy.curStep = 0;
        }

        /// <summary>
        /// Draw game scene
        /// </summary>
        public void DrawGameScene(int w,int h)
        {
            Console.Clear();
            mapWidth = w;
            mapHeight = h;
            mapZeroX = mapWidth / 2 / 2;
            mapZeroY = mapHeight / 2 / 2;
            GenMap();
            DrawWall();
            DrawString();
            DrawMap();
            DrawPlayer();
            while (true)
            {
                CheckKeyInput();
                if (player.curStep >= mapData.Count)
                {

                    Console.SetCursorPosition(10, mapHeight - 3);
                    Console.Write("YES,YOU WIN！");
                    break;
                }
                else if (enemy.curStep >= mapData.Count)
                {
                    Console.SetCursorPosition(10, mapHeight - 3);
                    Console.Write("On,no,you lost！");
                    break;
                }
                else 
                {
                    DrawPlayer();
                }
            }
        }

        void CheckKeyInput()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            switch (keyInfo)
            {
                default:
                    MovePlayer();
                   
                    break;
            }
        }

        void MovePlayer() 
        {
            Random random = new Random(DateTime.Now.Millisecond);
            int moveStep = random.Next(1,7);

            if (isPlayerTurn)
            {
                MapGrid playerGrid = mapData[player.curStep];
                Console.SetCursorPosition(mapZeroX + (int)playerGrid.DrawPos.X, mapZeroY + (int)playerGrid.DrawPos.Y);
                mapData[player.curStep].Draw();
                player.curStep += moveStep;
                Console.ForegroundColor = ConsoleColor.Green;
                WriteLog($"player moved {moveStep}，now on {player.curStep}");
            }
            else
            {
                MapGrid enemyGrid = mapData[enemy.curStep];
                Console.SetCursorPosition(mapZeroX + (int)enemyGrid.DrawPos.X, mapZeroY + (int)enemyGrid.DrawPos.Y);
                mapData[enemy.curStep].Draw();
                enemy.curStep += moveStep;
                Console.ForegroundColor = ConsoleColor.Red;
                WriteLog($"enemy moved {moveStep}，now on {enemy.curStep}");
            }
            isPlayerTurn = !isPlayerTurn;
        }

        void WriteLog(string str) 
        {
            Console.SetCursorPosition(1, mapHeight - 4);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < mapWidth; i++)
            {
                sb.Append(" ");
            }
            Console.Write(sb);
            Console.SetCursorPosition(10, mapHeight - 4);
            Console.Write(str);
        }

        private void DrawPlayer() 
        {
            MapGrid playGrid = mapData[player.curStep];
            MapGrid enemyGrid = mapData[enemy.curStep];
            if (enemy.curStep == player.curStep)
            {
                Console.SetCursorPosition(mapZeroX + (int)playGrid.DrawPos.X, mapZeroY + (int)playGrid.DrawPos.Y);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("A");
            }
            else
            {
                Console.SetCursorPosition(mapZeroX + (int)playGrid.DrawPos.X, mapZeroY + (int)playGrid.DrawPos.Y);
                playGrid.DrarPlayer(true);
                Console.SetCursorPosition(mapZeroX + (int)enemyGrid.DrawPos.X, mapZeroY + (int)enemyGrid.DrawPos.Y);
                enemyGrid.DrarPlayer(false);
            }
        }

        private void DrawWall() 
        {
            
            for (int i = 0; i < mapWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("*");
                Console.SetCursorPosition(i, mapHeight - 1);
                Console.Write("*");
                Console.SetCursorPosition(i, mapHeight - 6);
                Console.Write("*");
                Console.SetCursorPosition(i, mapHeight - 11);
                Console.Write("*");
            }

            for (int i = 0; i < mapHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("*");
                Console.SetCursorPosition(mapWidth - 2, i);
                Console.Write("*");
            }
        }

        private void DrawString()
        {
            Console.SetCursorPosition(2, mapHeight - 10);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("O:road");

            Console.SetCursorPosition(26, mapHeight - 10);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("8:bomb");

            Console.SetCursorPosition(2, mapHeight - 9);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("=:random");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(2, mapHeight - 8);
            Console.Write("P:player");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(15, mapHeight - 8);
            Console.Write("E:enemy");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(2, mapHeight - 7);
            Console.Write("A:enemy and player all are here");

            Console.SetCursorPosition(10, mapHeight - 5);
            Console.Write("push any key move random step，form 1 to 6");
        }


        private void GenMap() 
        {
            int maxStepCow = 8;
            int startY = 0;
            int curCowNum = 0;
            bool isAdd = true;
            for (int i = 0; i <= 80; i++)
            {
                MapGrid mapGrid = new MapGrid();
                mapGrid.DrawPos = new Vector2(curCowNum, startY);
                mapGrid.stepNum = i;
                if (i == 80)
                {
                    mapGrid.type = EGirdType.End;
                }
                else
                {
                    mapGrid.RandomType();
                }
                mapData.Add(mapGrid);

                if (isAdd)
                {
                    curCowNum++;
                }
                else
                {
                    curCowNum--;
                }

                if (curCowNum >= maxStepCow)
                {
                    isAdd = true;
                    curCowNum = 0;
                    startY = startY + 1;
                }
            }
        }


        private void DrawMap() 
        {
            foreach (var item in mapData)
            {
                Console.SetCursorPosition(mapZeroX + (int)item.DrawPos.X, mapZeroY + (int)item.DrawPos.Y);
                item.Draw();
            }
        }
    }
}

using System;
using System.Collections.Generic;
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
            DrawWall();
            DrawString();
            GenMap();
            DrawMap();
            while (true)
            {
                CheckKeyInput();
                if (player.curStep >= mapData.Count)
                {
                    Console.SetCursorPosition(10, mapHeight - 3);
                    Console.Write("Yes,YOU WIN！");
                    break;
                }
                else if (enemy.curStep >= mapData.Count)
                {
                    Console.SetCursorPosition(10, mapHeight - 3);
                    Console.Write("On,no,You last！");
                    break;
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
            int moveStep = random.Next(1,6);
            if (isPlayerTurn)
            {
                player.curStep += moveStep;
                Console.SetCursorPosition(10, mapHeight - 3);
                Console.Write($"玩家移动了{moveStep}，当前在{player.curStep}");
            }
            else
            {
                enemy.curStep += moveStep;
                Console.SetCursorPosition(10, mapHeight - 3);
                Console.Write($"敌人移动了{moveStep}，当前在{enemy.curStep}");
            }
            isPlayerTurn = !isPlayerTurn;
            DrawMap();
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
            Console.SetCursorPosition(2, mapHeight - 9);
            Console.Write("O:普通格子");
            Console.SetCursorPosition(26, mapHeight - 9);
            Console.Write("8:炸弹");
            Console.SetCursorPosition(2, mapHeight - 8);
            Console.Write("=:隧道，随机倒退、暂停、交换位置");
            Console.SetCursorPosition(2, mapHeight - 7);
            Console.Write("P:玩家");
            Console.SetCursorPosition(15, mapHeight - 7);
            Console.Write("E:敌人");

            Console.SetCursorPosition(10, mapHeight - 4);
            Console.Write("按任意键开始游戏");
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
            int startPointX = mapHeight / 2 / 2;
            int startPointY = mapHeight / 2 / 2;
            foreach (var item in mapData)
            {
                Console.SetCursorPosition(startPointX + (int)item.DrawPos.X, startPointY + (int)item.DrawPos.Y);
                if (item.stepNum == player.curStep && item.stepNum == enemy.curStep)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("A");
                }
                else if (item.stepNum == player.curStep)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("P");
                }
                else if (item.stepNum == enemy.curStep) 
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("E");
                }
                else 
                {
                    item.Draw();
                }      
            }
        }
    }
}

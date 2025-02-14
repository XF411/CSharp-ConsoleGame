using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace C_Learn
{
    public class RunGameScence : GameScenceBase
    {

        List<RunGameMapGrid> mapData = new List<RunGameMapGrid>();

        RunGamePlayer player = new RunGamePlayer();

        RunGamePlayer enemy = new RunGamePlayer();

        bool isPlayerTurn = true;
        /// <summary>
        /// map width and height
        /// </summary>
        int mapWidth, mapHeight;

        int mapZeroX, mapZeroY;

        bool bombFlag = false;
        bool pauseFlag = false;

        /// <summary>
        /// reset player pos
        /// </summary>
        public void ReSetPlayer() 
        {
            player.curStep = 0;
            enemy.curStep = 0;
        }

        /// <summary>
        /// Draw all game scene
        /// </summary>
        private void DrawAllDisplay()
        {
            Console.Clear();
            mapWidth = ConsoleControler.Instance.DisplayWidth;
            mapHeight = ConsoleControler.Instance.DisplayHeight;
            mapZeroX = mapWidth / 4;
            mapZeroY = mapHeight / 4;
            GenMap();
            DrawWall();
            DrawString();
            DrawMap();
            DrawPlayer();
        }

        void MovePlayer() 
        {
            Random random = new Random(DateTime.Now.Millisecond);
            int moveStep = random.Next(1,7);

            if (isPlayerTurn)
            {
                RunGameMapGrid playerGrid = mapData[player.curStep];
                //Console.SetCursorPosition(mapZeroX + (int)playerGrid.DrawPos.X, mapZeroY + (int)playerGrid.DrawPos.Y);
                //mapData[player.curStep].Draw();
                needReDrawGrid.Add(playerGrid);
                player.curStep = Math.Min(player.curStep + moveStep, mapData.Count - 1);
                switch (playerGrid.type)
                {
                    case EGirdType.Bomb:
                        bombFlag = true;
                        break;
                    case EGirdType.pause:
                        pauseFlag = true;
                        break;
                }
                Console.ForegroundColor = ConsoleColor.Green;
                WriteLog($"player moved {moveStep}，now on {player.curStep}");
            }
            else
            {
                RunGameMapGrid enemyGrid = mapData[enemy.curStep];
                //Console.SetCursorPosition(mapZeroX + (int)enemyGrid.DrawPos.X, mapZeroY + (int)enemyGrid.DrawPos.Y);
                //mapData[enemy.curStep].Draw();
                needReDrawGrid.Add(enemyGrid);
                enemy.curStep = Math.Min(enemy.curStep + moveStep, mapData.Count - 1);
                switch (enemyGrid.type)
                {
                    case EGirdType.Bomb:
                        bombFlag = true;
                        break;
                    case EGirdType.pause:
                        pauseFlag = true;
                        break;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                WriteLog($"enemy moved {moveStep}，now on {enemy.curStep}");
            }
        }

        private List<RunGameMapGrid> needReDrawGrid;

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
            RunGameMapGrid playGrid = mapData[player.curStep];
            RunGameMapGrid enemyGrid = mapData[enemy.curStep];
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
                Console.Write("-");
                Console.SetCursorPosition(i, mapHeight - 1);
                Console.Write("-");
                Console.SetCursorPosition(i, mapHeight - 6);
                Console.Write("-");
                Console.SetCursorPosition(i, mapHeight - 11);
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

        private void DrawString()
        {
            ConsoleControler.DrawString(2, mapHeight - 10, "O:road", ConsoleColor.White);
            ConsoleControler.DrawString(26, mapHeight - 10, "B:bomb", ConsoleColor.Blue);
            ConsoleControler.DrawString(2, mapHeight - 9, "=:random", ConsoleColor.DarkRed);
            ConsoleControler.DrawString(2, mapHeight - 8, "P:player", ConsoleColor.Green);
            ConsoleControler.DrawString(15, mapHeight - 8, "E:enemy", ConsoleColor.Red);
            ConsoleControler.DrawString(2, mapHeight - 7, "A:enemy and player all are here", ConsoleColor.Yellow);
            ConsoleControler.DrawString(10, mapHeight - 5, "push any key move random step，form 1 to 6", ConsoleColor.White);
        }


        private void GenMap() 
        {
            int maxStepCow = 8;
            int startY = 0;
            int curCowNum = 0;
            bool isAdd = true;
            for (int i = 0; i <= 80; i++)
            {
                RunGameMapGrid mapGrid = new RunGameMapGrid();
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

        bool isNewGame = false;
        bool needUpdate = false;

        public void StartNewGame() 
        {
            isNewGame = true;
            if (needReDrawGrid == null)
            {
                needReDrawGrid = new List<RunGameMapGrid>();
            }
            else
            {
                needReDrawGrid.Clear();
            }

            //AddListener();
        }

        //private void AddListener()
        //{
        //    InputControler.Instance.AddListener(ConsoleKey.Enter, OnEnterClick);
        //}
        
        

        private void OnAnyClikClick()
        {

            if (bombFlag || pauseFlag)
            {
                CheckGridFunction();
            }
            else
            {
                MovePlayer();
            }
            //CheckGridFunction();
            //isPlayerTurn = !isPlayerTurn;
            CheckWinOrLast();
            if (!bombFlag && !pauseFlag)
            {
                isPlayerTurn = !isPlayerTurn;
            }
            needUpdate = true;
        }

        public override void Update()
        {
            CheckKeyInput();
            if (isNewGame)
            {
                DrawAllDisplay();
                isNewGame = false;
            }
            if (needUpdate)
            {
                DrawPlayer();
                DrawGrid();
            }
        }

        void DrawGrid() 
        {
            foreach (var item in needReDrawGrid)
            {
                Console.SetCursorPosition(mapZeroX + (int)item.DrawPos.X, mapZeroY + (int)item.DrawPos.Y);
                item.Draw();
            }
            needReDrawGrid.Clear();
        }

        void CheckGridFunction()
        {
            var curPlayerGrid = isPlayerTurn ? mapData[player.curStep] : mapData[enemy.curStep];
            var curPlyaer = isPlayerTurn ? player : enemy;
            if (bombFlag)
            {
                var currentPlayer = isPlayerTurn ? player : enemy;
                int backStep = new Random().Next(1, 7);
                int oldStep = currentPlayer.curStep;

                currentPlayer.curStep = Math.Max(0, currentPlayer.curStep - backStep);
                needReDrawGrid.Add(mapData[oldStep]);
                needReDrawGrid.Add(mapData[currentPlayer.curStep]);

                bombFlag = false;
                TipsBoom();
            }
            else if (pauseFlag)
            {
                isPlayerTurn = !isPlayerTurn;
                pauseFlag = false;
                TipsPause();
            }
        }

        void TipsBoom()
        {
            string playerName = isPlayerTurn ? "player" : "enemy";
            ConsoleControler.DrawString(10, mapHeight - 3, $"Boom!{playerName} will back random step", ConsoleColor.Red);
            //ConsoleKeyInfo keyInfo = Console.ReadKey();
        }

        void TipsPause()
        {
            string playerName = isPlayerTurn ? "player" : "enemy";
            ConsoleControler.DrawString(10, mapHeight - 3, $"Pause!{playerName} will skip next turn", ConsoleColor.Red);
            //ConsoleKeyInfo keyInfo = Console.ReadKey();
        }

        void CheckKeyInput()
        {
            if (!Console.KeyAvailable)
            {
                return;
            }
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            OnAnyClikClick();
        }


        private void CheckWinOrLast()
        {
            if (player.curStep >= mapData.Count - 1) // 检查是否到达第80格
            {
                Console.SetCursorPosition(10, mapHeight - 3);
                Console.Write("YES,YOU WIN！");
            }
            else if (enemy.curStep >= mapData.Count - 1)
            {
                Console.SetCursorPosition(10, mapHeight - 3);
                Console.Write("On,no,you lost！");
            }
        }

        public override void OnExit()
        {
            //InputControler.Instance.RemoveListener(ConsoleKey.Enter, OnEnterClick);
        }
    }
}

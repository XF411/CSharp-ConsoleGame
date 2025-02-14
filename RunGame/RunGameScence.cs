using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace C_Learn
{
    enum TurnType
    {
        NormalMove,
        BombBoom,
        PauseTime,
        Win,
        Last
    }

    public class RunGameScence : GameScenceBase
    {
        private static readonly Random random = new Random();
        List<RunGameMapGrid> mapData;
        private List<RunGameMapGrid> needReDrawGrid;

        RunGamePlayer player; 
        RunGamePlayer enemy;

        TurnType curTurnType = TurnType.NormalMove;
        bool isPlayerTurn = true;


        TurnType nextTurnType = TurnType.NormalMove;
        bool nextIsPlayerTurn = false;

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

        void WriteLog(string str) 
        {
            Console.ForegroundColor = isPlayerTurn ? ConsoleColor.Green : ConsoleColor.Red;
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
            ConsoleControler.DrawString(2, mapHeight - 9, "=:be pause", ConsoleColor.DarkRed);
            ConsoleControler.DrawString(2, mapHeight - 8, "P:player", ConsoleColor.Green);
            ConsoleControler.DrawString(15, mapHeight - 8, "E:enemy", ConsoleColor.Red);
            ConsoleControler.DrawString(2, mapHeight - 7, "A:enemy and player all are here", ConsoleColor.Yellow);
            ConsoleControler.DrawString(10, mapHeight - 5, "push any key move random step，form 1 to 6", ConsoleColor.White);
        }


        private void GenMap() 
        {
            int mapSize = 100;
            int maxStepCow = 10;
            int startY = 0;
            int curCowNum = 0;
            bool isAdd = true;
            for (int i = 0; i < mapSize; i++)
            {
                RunGameMapGrid mapGrid = new RunGameMapGrid();
                mapGrid.DrawPos = new Vector2(curCowNum, startY);
                mapGrid.stepNum = i;
                if (i == mapSize - 1)
                {
                    mapGrid.type = EGirdType.End;
                }
                else if(i == 0)
                {
                    mapGrid.type = EGirdType.Road;
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
            if (mapData != null)
            {
                mapData.Clear();
            }
            else
            {
                mapData = new List<RunGameMapGrid>();
            }

            if (needReDrawGrid == null)
            {
                needReDrawGrid = new List<RunGameMapGrid>();
            }
            else
            {
                needReDrawGrid.Clear();
            }

            player = new RunGamePlayer();
            player.isPlayer = true;
            enemy = new RunGamePlayer();
            enemy.isPlayer = false;

            isNewGame = true;

            curTurnType = TurnType.NormalMove;
            isPlayerTurn = true;


        }


        private void MovePlayer(RunGamePlayer player) 
        {
            //Gen random number for step
            int moveStep = random.Next(1, 7);

            //Get current player grid for redraw
            
            RunGameMapGrid playerGrid = mapData[player.curStep];
            needReDrawGrid.Add(playerGrid);

            //Change player pos data
            player.curStep = Math.Min(player.curStep + moveStep, mapData.Count - 1);

            //Check new grid type and temp
            switch (mapData[player.curStep].type)
            {
                case EGirdType.Bomb:
                    nextTurnType = TurnType.BombBoom;
                    nextIsPlayerTurn = isPlayerTurn;
                    break;
                case EGirdType.pause:
                    nextTurnType = TurnType.PauseTime;
                    nextIsPlayerTurn = isPlayerTurn;
                    break;
                case EGirdType.End:
                    nextTurnType = isPlayerTurn ? TurnType.Win : TurnType.Last;
                    break;
                default:
                    nextIsPlayerTurn = !isPlayerTurn;
                    nextTurnType = TurnType.NormalMove;
                    break;
            }

            //wirte log
            Console.ForegroundColor = ConsoleColor.Green;
            WriteLog($"player moved {moveStep}，now on {player.curStep}");
        }

        void BombBoom() 
        {
            //Gen random number for bomb
            int boomStep = random.Next(1, 7);

            //Get current player
            RunGamePlayer boomPlayer = isPlayerTurn ? player : enemy;

            //Get current player grid for redraw
            RunGameMapGrid playerGrid = mapData[boomPlayer.curStep];
            needReDrawGrid.Add(playerGrid);

            //Change player pos data
            boomPlayer.curStep = Math.Max(0, boomPlayer.curStep - boomStep);

            //Check new grid type and temp
            switch (mapData[boomPlayer.curStep].type)
            {
                case EGirdType.Bomb:
                    nextTurnType = TurnType.BombBoom;
                    nextIsPlayerTurn = isPlayerTurn;
                    break;
                case EGirdType.pause:
                    nextTurnType = TurnType.PauseTime;
                    nextIsPlayerTurn = isPlayerTurn;
                    break;
                case EGirdType.End:
                    nextTurnType = isPlayerTurn ? TurnType.Win : TurnType.Last;
                    break;
                default:
                    nextIsPlayerTurn = isPlayerTurn ? false : true;
                    nextTurnType = TurnType.NormalMove;
                    break;
            }
            //wirte log
            string playerName = isPlayerTurn ? "player" : "enemy";
            WriteLog($"Booooom!{playerName} back {boomStep} step，now on {player.curStep}");
        }

        void PauseTime()
        {
            //Gen random number for step
            int moveStep = random.Next(1, 7);

            //Get can move player,
            //if is player be pause,enemy can move,
            //else player can move
            RunGamePlayer canMovePlayer = isPlayerTurn ? enemy : player;

            //Get current player grid for redraw
            RunGameMapGrid playerGrid = mapData[canMovePlayer.curStep];
            needReDrawGrid.Add(playerGrid);

            //Change player pos data
            canMovePlayer.curStep = Math.Min(canMovePlayer.curStep + moveStep, mapData.Count - 1);

            //Check new grid type and temp
            switch (mapData[canMovePlayer.curStep].type)
            {
                case EGirdType.Bomb:
                    nextTurnType = TurnType.BombBoom;
                    nextIsPlayerTurn = canMovePlayer.isPlayer;
                    break;
                case EGirdType.pause:
                    nextTurnType = TurnType.PauseTime;
                    nextIsPlayerTurn = canMovePlayer.isPlayer;
                    break;
                case EGirdType.End:
                    nextTurnType = canMovePlayer.isPlayer ? TurnType.Win : TurnType.Last;
                    break;
                default:
                    //Attention!
                    //Now, this "canMovePlayer" can move on this turn beacuse the other player be pause,
                    //So, next turn is for this "canMovePlayer"
                    nextIsPlayerTurn = canMovePlayer.isPlayer;
                    nextTurnType = TurnType.NormalMove;
                    break;
            }
            string bePauseName = isPlayerTurn ? "player" : "enemy";
            string canMoveName = canMovePlayer.isPlayer ? "player" : "enemy";
            WriteLog($"{bePauseName} is be pause!{canMoveName} move {moveStep} step,now on {canMovePlayer.curStep}");
        }

        private void OnAnyClikClick()
        {
            //get last turn data
            switch (curTurnType)
            {
                case TurnType.NormalMove:
                    MovePlayer( isPlayerTurn? player : enemy );
                    break;
                case TurnType.BombBoom:
                    BombBoom();
                    break;
                case TurnType.PauseTime:
                    PauseTime();
                    break;
                case TurnType.Win:
                    WriteLog("You win!");
                    break;
                case TurnType.Last:
                    WriteLog("You last!");
                    break;
            }
            needUpdate = true;
            curTurnType = nextTurnType;
            isPlayerTurn = nextIsPlayerTurn;
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

        void CheckKeyInput()
        {
            if (!Console.KeyAvailable)
            {
                return;
            }
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            OnAnyClikClick();
        }

        public override void OnExit()
        {

        }
    }
}

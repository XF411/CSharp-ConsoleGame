using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Learn
{
    public class GameManager
    {
        private static GameManager _instance;
        
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }
                return _instance;
            }
        }
        Dictionary<E_ScenceType, GameScenceBase> GameScenceDic;
        GameScenceBase curGameScenceBase;

        private GameManager()
        {
            ConsoleControler.Instance.Init();
            InitScences();
            AddEventListenler();
        }

        private void InitScences() 
        {
            var mainMenu = new MainMenu();
            var runGameScence = new RunGameScence();
            var snakeGameScence = new SankeGameScence();
            GameScenceDic = new Dictionary<E_ScenceType, GameScenceBase>();
            GameScenceDic.Add(E_ScenceType.Menu, mainMenu);
            GameScenceDic.Add(E_ScenceType.RunGameScence, runGameScence);
            GameScenceDic.Add(E_ScenceType.SnakeGameScence, snakeGameScence);
        }

        private void AddEventListenler() 
        {
            EventBroadcaster.Instance.AddEventListener(EventName.StartRunGame, () => { EnterScence(E_ScenceType.RunGameScence); });
            EventBroadcaster.Instance.AddEventListener(EventName.StartSnakeGame, () => { EnterScence(E_ScenceType.SnakeGameScence); });
            EventBroadcaster.Instance.AddEventListener(EventName.ExitGame, () => { EnterScence(E_ScenceType.EndGame); });

            EventBroadcaster.Instance.AddEventListener(EventName.WinRunGame, () => { 
                EnterScence(E_ScenceType.Menu);
                SetMenuEndRunGame(true);
            });
            EventBroadcaster.Instance.AddEventListener(EventName.LastRunGame, () => { 
                EnterScence(E_ScenceType.Menu);
                SetMenuEndRunGame(false);
            });
            EventBroadcaster.Instance.AddEventListener(EventName.EndSnakeGame, () => { 
                EnterScence(E_ScenceType.Menu);
                SetMenuEndSnakeGame();
            });
        }

        /// <summary>
        /// Main function use,only call once
        /// </summary>
        public void Start() 
        {
            ConsoleControler.Instance.Clear();
            ConsoleControler.Instance.SetDisplaySize();
            ConsoleControler.Instance.SetCursor(false);
            curGameScenceBase = GameScenceDic[E_ScenceType.Menu];
            StartGame();
        }


        #region GameLogic

        public void StartGame()
        {
            while (true)
            {
                if (curGameScenceBase != null)
                {
                    curGameScenceBase.Update();
                }
                //InputControler.Instance.CheckInput();
            }
        }


        private void EnterScence(E_ScenceType type)
        {
            curGameScenceBase.OnExit();
            switch (type)
            {
                case E_ScenceType.Menu:
                    ToMainMenu();
                    break;
                case E_ScenceType.RunGameScence:
                    StartRunGame();
                    break;
                case E_ScenceType.SnakeGameScence:
                    StartSnakeGame();
                    break;
                case E_ScenceType.EndGame:
                    EndGame();
                    break;
            }
        }

        private void SetMenuEndRunGame(bool isWin) 
        {
            (GameScenceDic[E_ScenceType.Menu] as MainMenu).SetForEndRunGame(isWin);
        }

        private void SetMenuEndSnakeGame()
        {
            int point = (GameScenceDic[E_ScenceType.SnakeGameScence] as SankeGameScence).GetPoint();
            (GameScenceDic[E_ScenceType.Menu] as MainMenu).SetForEndSnakeGame(point);
        }
        private void ToMainMenu()
        {
            Console.Clear();
            curGameScenceBase = GameScenceDic[E_ScenceType.Menu];
        }

        private void StartRunGame()
        {
            Console.Clear();
            curGameScenceBase = GameScenceDic[E_ScenceType.RunGameScence];
            if (curGameScenceBase is RunGameScence)
            {
                (curGameScenceBase as RunGameScence).StartNewGame();
            }
        }

        private void StartSnakeGame()
        {
            Console.Clear();
            curGameScenceBase = GameScenceDic[E_ScenceType.SnakeGameScence];
            if (curGameScenceBase is SankeGameScence)
            {
                (curGameScenceBase as SankeGameScence).StartNewGame();
            }
        }

        private void EndGame()
        {
            Console.Clear();
            curGameScenceBase = GameScenceDic[E_ScenceType.Menu];
        }
        #endregion
    }
}

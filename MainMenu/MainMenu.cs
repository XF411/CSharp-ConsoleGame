using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Learn
{
    public class MainMenu: GameScenceBase
    {
        bool needUpdate = true;
        int CurSelect;
        private E_ScenceType lastGameType;

        private MainMenuSelectItem PlayAgainItem;
        private MainMenuSelectItem RunGameItem;
        private MainMenuSelectItem SnakeGameItem;

        string curTitle;

        public MainMenu() 
        {
            e_ScenceType = E_ScenceType.Menu;
            EventBroadcaster.Instance.AddEventListener(EventName.ChangeLanguage, OnChangeLanguage);
            Init();
        }

        List<MainMenuSelectItem> Items = new List<MainMenuSelectItem>();
        public void Init() 
        {
            InitItemList();
            curTitle = LanguageManager.Instance.GetString("MainTitle");
            needUpdate = true;
        }

        private void DrawWall()
        {

            Console.ForegroundColor = ConsoleColor.Blue;
            for (int i = 0; i < ConsoleControler.Instance.DisplayWidth - 1; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("▉");
                Console.SetCursorPosition(i, ConsoleControler.Instance.DisplayHeight - 2);
                Console.Write("▉");
            }

            for (int i = 0; i < ConsoleControler.Instance.DisplayHeight - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("▉");
                Console.SetCursorPosition(ConsoleControler.Instance.DisplayWidth - 2, i);
                Console.Write("▉");
            }
        }


        private void OnChangeLanguage() 
        {
            Console.Clear();
            curTitle = LanguageManager.Instance.GetString("MainTitle");
            foreach (MainMenuSelectItem item in Items) 
            {
                item.ChangeLanguage();
            }
            needUpdate = true;
        }

        private void InitItemList() 
        {
            RunGameItem = new MainMenuSelectItem();
            RunGameItem.textID = "RunGameTitle";
            RunGameItem.selectType = E_ScenceType.RunGameScence;

            SnakeGameItem = new MainMenuSelectItem();
            SnakeGameItem.textID = "SnakeGameTitle";
            SnakeGameItem.selectType = E_ScenceType.SnakeGameScence;

            MainMenuSelectItem ExitGame = new MainMenuSelectItem();
            ExitGame.textID = "ExitGameTitle";
            ExitGame.selectType = E_ScenceType.EndGame;

            MainMenuSelectItem LanguageChange = new MainMenuSelectItem();
            LanguageChange.textID = "ChangeLanguage";
            LanguageChange.selectType = E_ScenceType.ChangeLanguage;

            PlayAgainItem = new MainMenuSelectItem();
            PlayAgainItem.textID = "PlayAgain";
            PlayAgainItem.selectType = E_ScenceType.None;

            Items.Add(RunGameItem);
            Items.Add(SnakeGameItem);
            Items.Add(ExitGame);
            Items.Add(LanguageChange);
            foreach (var item in Items) 
            {
                item.ChangeLanguage();
            }
            PlayAgainItem.ChangeLanguage();
            CurSelect = 0;
        }

        public override void Update()
        {
            if (isExit) 
            {
                return;
            }
            CheckKeyInput();
            if (needUpdate)
            {
                DrawWall();
                var w = Console.WindowWidth;
                var h = Console.WindowHeight/2;
                ConsoleControler.DrawString((w - curTitle.Length)/2 - 2, h - 4, curTitle, ConsoleColor.Blue);
                for (int i = 0; i < Items.Count; i++)
                {
                    ConsoleColor color = CurSelect == i ? ConsoleColor.Red : Items[i].textColor;
                    ConsoleControler.DrawString(Items[i].GetCursorPosX(w) - 2, h + i, Items[i].text, color);
                }
            }
            needUpdate = false;
        }
        private void OnUpClick() 
        {
            if ((CurSelect - 1) < 0)
            {
                CurSelect = Items.Count - 1;
            }
            else
            {
                CurSelect = CurSelect - 1;
            }
            needUpdate = true;
        }
        private void OnDownClick()
        {
            if ((CurSelect + 1) == Items.Count)
            {
                CurSelect = 0;
            }
            else
            {
                CurSelect = CurSelect + 1;
            }
            needUpdate = true;
        }

        void CheckKeyInput()
        {
            if (!Console.KeyAvailable)
            {
                return;
            }
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    OnUpClick();
                    break;
                case ConsoleKey.DownArrow:
                    OnDownClick();
                    break;
                case ConsoleKey.Enter:
                    OnEnterClick();
                    break;
            }
        }

        private void OnEnterClick()
        {
            E_ScenceType type = Items[CurSelect].selectType;
            switch (type)
            {
                case E_ScenceType.RunGameScence:
                    EventBroadcaster.Instance.BroadcastEvent(EventName.StartRunGame);
                    break;
                case E_ScenceType.SnakeGameScence:
                    EventBroadcaster.Instance.BroadcastEvent(EventName.StartSnakeGame);
                    break;
                case E_ScenceType.ChangeLanguage:
                    LanguageManager.Instance.ChangeLanguage();
                    break;
                case E_ScenceType.EndGame:
                    Environment.Exit(0);
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


        internal void SetForEndRunGame(bool isWin)
        {
            isExit = false;
            Console.Clear();
            needUpdate = true;
            CurSelect = 0;

            if (Items.Contains(RunGameItem))
            {
                Items.Remove(RunGameItem);
            }

            if (!Items.Contains(SnakeGameItem))
            {
                SnakeGameItem.ChangeLanguage();
                Items.Insert(1,SnakeGameItem);
            }

            if (!Items.Contains(PlayAgainItem)) 
            {
                Items.Insert(0, PlayAgainItem);
               
            }
            PlayAgainItem.selectType = E_ScenceType.RunGameScence;
            if (isWin) 
            {
                curTitle = LanguageManager.Instance.GetString("WinRunGame");
            }
            else
            {
                curTitle = LanguageManager.Instance.GetString("LastRunGame");
            }
        }

        internal void SetForEndSnakeGame(int point)
        {
            isExit = false;
            Console.Clear();
            needUpdate = true;
            CurSelect = 0;

            if (!Items.Contains(RunGameItem))
            {
                RunGameItem.ChangeLanguage();
                Items.Insert(1,RunGameItem);
            }

            if (Items.Contains(SnakeGameItem))
            {
                Items.Remove(SnakeGameItem);
            }

            if (!Items.Contains(PlayAgainItem))
            {
                Items.Insert(0, PlayAgainItem);
                
            }
            PlayAgainItem.selectType = E_ScenceType.SnakeGameScence;

            curTitle = string.Format(LanguageManager.Instance.GetString("EndSnakeGame"),point.ToString());
        }
    }
}

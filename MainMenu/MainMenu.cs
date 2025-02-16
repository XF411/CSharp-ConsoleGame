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

        string curTitle;

        public MainMenu() 
        {
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


        private void InitItemList() 
        {
            MainMenuSelectItem StartRunGame = new MainMenuSelectItem();
            StartRunGame.text = LanguageManager.Instance.GetString("RunGameTitle");
            StartRunGame.selectType = E_ScenceType.RunGameScence;

            MainMenuSelectItem StartSnakeGame = new MainMenuSelectItem();
            StartSnakeGame.text = LanguageManager.Instance.GetString("SnakeGameTitle");
            StartSnakeGame.selectType = E_ScenceType.SnakeGameScence;

            MainMenuSelectItem ExitGame = new MainMenuSelectItem();
            ExitGame.text = LanguageManager.Instance.GetString("ExitGameTitle");
            ExitGame.selectType = E_ScenceType.EndGame;

            MainMenuSelectItem LanguageChange = new MainMenuSelectItem();
            LanguageChange.text = LanguageManager.Instance.GetString("ChangeLanguage");
            LanguageChange.selectType = E_ScenceType.ChangeLanguage;

            PlayAgainItem = new MainMenuSelectItem();
            PlayAgainItem.text = LanguageManager.Instance.GetString("PlayAgain");
            PlayAgainItem.selectType = E_ScenceType.None;

            Items.Add(StartRunGame);
            Items.Add(StartSnakeGame);
            Items.Add(ExitGame);
            Items.Add(LanguageChange);
            CurSelect = 0;
        }

        public override void Update()
        {
            CheckKeyInput();
            if (needUpdate)
            {
                DrawWall();
                var w = Console.WindowWidth / 2;
                var h = Console.WindowHeight / 2;
                //string title = LanguageManager.Instance.GetString("MainTitle");
                //ConsoleControler.DrawString(w - title.Length, h - 2, title, ConsoleColor.Blue);
                //DrawTitle();
                ConsoleControler.DrawString(w - curTitle.Length, h - 2, curTitle, ConsoleColor.Blue);
                for (int i = 0; i < Items.Count; i++)
                {
                    ConsoleColor color = CurSelect == i ? ConsoleColor.Red : Items[i].textColor;
                    ConsoleControler.DrawString(Items[i].GetCursorPosX(w), h + 1 + i, Items[i].text, color);
                }
            }
            needUpdate = false;
        }

        //private void DrawTitle(int w,int h) 
        //{
            //string title = LanguageManager.Instance.GetString("MainTitle");

        //}

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
                    //TODO
                    break;
                default:
                    break;
            }
        }

        public override void OnExit()
        {
        }


        internal void SetForEndRunGame(bool isWin)
        {
            Console.Clear();
            needUpdate = true;
            CurSelect = 0;
            int needRemove = -1;
            for (int i = 0; i < Items.Count; i++) 
            {
                if (Items[i].selectType == E_ScenceType.RunGameScence) 
                {
                    needRemove = i;
                    break;
                }
            }
            if (needRemove >= 0) 
            {
                Items.RemoveAt(needRemove);
            }

            if (!Items.Contains(PlayAgainItem)) 
            {
                Items.Insert(0, PlayAgainItem);
                PlayAgainItem.selectType = E_ScenceType.RunGameScence;
            }

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
            Console.Clear();
            needUpdate = true;
            CurSelect = 0;
            int needRemove = -1;
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].selectType == E_ScenceType.SnakeGameScence)
                {
                    needRemove = i;
                    break;
                }
            }
            if (needRemove >= 0)
            {
                Items.RemoveAt(needRemove);
            }

            if (!Items.Contains(PlayAgainItem))
            {
                Items.Insert(0, PlayAgainItem);
                PlayAgainItem.selectType = E_ScenceType.SnakeGameScence;
            }

            curTitle = string.Format(LanguageManager.Instance.GetString("EndSnakeGame"),point.ToString());
        }
    }
}

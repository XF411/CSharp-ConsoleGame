using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Learn
{
    public class MainMenu: GameScenceBase
    {
        //bool selectGame = true;
        //bool outMainMenu = false;
        //public void SetTitle() 
        //{
        //Console.SetCursorPosition(w / 2 - 2, h / 2 - 2);
        //Console.WriteLine(LanguageManager.Instance.GetString("MainTitle"));

        ////while (true)
        ////{
        //    Console.SetCursorPosition(w / 2 - 2, h / 2 + 2);
        //    if (selectGame)
        //    {
        //        Console.ForegroundColor = ConsoleColor.Red;
        //    }
        //    else
        //    {
        //        Console.ForegroundColor = ConsoleColor.White;
        //    }

        //    Console.WriteLine(LanguageManager.Instance.GetString("RunGameTitle"));
        //    Console.SetCursorPosition(w / 2 - 2, h / 2 + 3);
        //    if (selectGame)
        //    {
        //        Console.ForegroundColor = ConsoleColor.White;
        //    }
        //    else
        //    {
        //        Console.ForegroundColor = ConsoleColor.Red;
        //    }
        //    Console.WriteLine("ExitGameTitle");
        //    CheckKeyInput();
        //    if (outMainMenu)
        //    {
        //        if (selectGame)
        //        {
        //            EventBroadcaster.Instance.BroadcastEvent(EventName.StartGame);
        //        }
        //        else
        //        {
        //            Environment.Exit(0);
        //        }
        //        //break;
        //    }
        //}

        //}

        bool needUpdate = true;
        int CurSelect;

        public MainMenu() 
        {
            Init();
        }

        List<MainMenuSelectItem> Items = new List<MainMenuSelectItem>();
        public void Init() 
        {
            InitItemList();
            //AddListener();
            needUpdate = true;
        }

        private void InitItemList() 
        {
            MainMenuSelectItem StartRunGame = new MainMenuSelectItem();
            StartRunGame.text = LanguageManager.Instance.GetString("RunGameTitle");
            MainMenuSelectItem StartSnakeGame = new MainMenuSelectItem();
            StartSnakeGame.text = LanguageManager.Instance.GetString("SnakeGameTitle");
            MainMenuSelectItem ExitGame = new MainMenuSelectItem();
            ExitGame.text = LanguageManager.Instance.GetString("ExitGameTitle");
            Items.Add(StartRunGame);
            Items.Add(StartSnakeGame);
            Items.Add(ExitGame);
            CurSelect = 0;
        }

        //private void AddListener()
        //{
        //    InputControler.Instance.AddListener(ConsoleKey.UpArrow, OnUpClick);
        //    InputControler.Instance.AddListener(ConsoleKey.DownArrow, OnDownClick);
        //    InputControler.Instance.AddListener(ConsoleKey.Enter, OnEnterClick);
        //}


        public override void Update()
        {
            CheckKeyInput();
            if (needUpdate)
            {
                var w = Console.WindowWidth / 2;
                var h = Console.WindowHeight / 2;
                string title = LanguageManager.Instance.GetString("MainTitle");
                ConsoleControler.DrawString(w - title.Length / 2, h - 2, title, ConsoleColor.Blue);
                for (int i = 0; i < Items.Count; i++)
                {
                    ConsoleColor color = CurSelect == i ? ConsoleColor.Red : Items[i].textColor;
                    ConsoleControler.DrawString(Items[i].GetCursorPosX(w), h + 1 + i, Items[i].text, color);
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
            ConsoleKeyInfo keyInfo = Console.ReadKey();
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
            switch (CurSelect)
            {
                case 0:
                    EventBroadcaster.Instance.BroadcastEvent(EventName.StartRunGame);
                    break;
                case 1:
                    EventBroadcaster.Instance.BroadcastEvent(EventName.StartSnakeGame);
                    break;
                case 2:
                    EventBroadcaster.Instance.BroadcastEvent(EventName.ExitGame);
                    break;
                default:
                    break;
            }
        }

        public override void OnExit()
        {
            //InputControler.Instance.RemoveListener(ConsoleKey.UpArrow, OnUpClick);
            //InputControler.Instance.RemoveListener(ConsoleKey.DownArrow, OnDownClick);
            //InputControler.Instance.RemoveListener(ConsoleKey.Enter, OnEnterClick);
        }
    }
}

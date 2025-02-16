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

        public MainMenu() 
        {
            Init();
        }

        List<MainMenuSelectItem> Items = new List<MainMenuSelectItem>();
        public void Init() 
        {
            InitItemList();
            needUpdate = true;
        }

        private void DrawWall()
        {

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
            MainMenuSelectItem StartSnakeGame = new MainMenuSelectItem();
            StartSnakeGame.text = LanguageManager.Instance.GetString("SnakeGameTitle");
            MainMenuSelectItem ExitGame = new MainMenuSelectItem();
            ExitGame.text = LanguageManager.Instance.GetString("ExitGameTitle");
            Items.Add(StartRunGame);
            Items.Add(StartSnakeGame);
            Items.Add(ExitGame);
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
                string title = LanguageManager.Instance.GetString("MainTitle");
                ConsoleControler.DrawString(w - title.Length, h - 2, title, ConsoleColor.Blue);
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
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Learn
{
    public class MainMenu
    {
        const string title = "c# mini game";
        const string start = "start game";
        const string end = "exit game";
        bool selectGame = true;
        bool outMainMenu = false;
        public void SetTitle(int w,int h) 
        {
            Console.SetCursorPosition(w / 2 - 2, h / 2 - 2);
            Console.WriteLine(title);

            while (true)
            {
                Console.SetCursorPosition(w / 2 - 2, h / 2 + 2);
                if (selectGame)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine(start);
                Console.SetCursorPosition(w / 2 - 2, h / 2 + 3);
                if (selectGame)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.WriteLine(end);
                CheckKeyInput();
                if (outMainMenu)
                {
                    if (selectGame)
                    {
                        EventBroadcaster.Instance.BroadcastEvent(EventName.StartGame);
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                    break;
                }
            }

        }

        void CheckKeyInput() 
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    selectGame = !selectGame;
                    break;
                case ConsoleKey.DownArrow:
                    selectGame = !selectGame;
                    break;
                case ConsoleKey.Enter:
                    outMainMenu = true;
                    break;
            }
        }



    }
}

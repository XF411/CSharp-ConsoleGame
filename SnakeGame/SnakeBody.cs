using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Learn
{
    public enum BodyType
    {
        Head,
        Body,
        Tail
    }

    public class SnakeBody
    {
        const string headChar = "0";
        const string bodyChar = "O";
        const string tailChar = "o";
        public BodyType Type { get; set; }

        public int CurPosX { get; private set; }
        public int CurPosY { get; private set; }

        public int LastPosX { get; private set; }
        public int LastPosY { get; private set; }
        public void UpdatePosition(int x, int y)
        {
            LastPosX = CurPosX;
            LastPosY = CurPosY;
            CurPosX = x;
            CurPosY = y;
        }

        public void Draw()
        {
            Console.SetCursorPosition(CurPosX, CurPosY);
            switch (Type)
            {
                case BodyType.Head:
                    ConsoleControler.DrawString(CurPosX, CurPosY, headChar, ConsoleColor.Green);
                    break;
                case BodyType.Body:
                    ConsoleControler.DrawString(CurPosX, CurPosY, bodyChar, ConsoleColor.White);
                    break;
                case BodyType.Tail:
                    ConsoleControler.DrawString(CurPosX, CurPosY, tailChar, ConsoleColor.DarkGreen);
                    break;
            }
        }
    }
}

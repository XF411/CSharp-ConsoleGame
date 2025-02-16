using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Learn
{
    class SnakeFood
    {
        const string _char = "★";

        public int CurPosX { get; private set; }
        public int CurPosY { get; private set; }
        public void UpdatePosition(int x, int y)
        {
            CurPosX = x;
            CurPosY = y;
        }

        public void Draw()
        {
            Console.SetCursorPosition(CurPosX, CurPosY);
            ConsoleControler.DrawString(CurPosX, CurPosY, _char, ConsoleColor.Yellow);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Learn
{
    public class ConsoleControler
    {
        int displayWidth = 40;
        int displayHeight = 30;

        public void SetDisplaySize(int width = 40, int height = 30)
        {
            displayWidth = width;
            displayHeight = height;
            Console.SetWindowSize(displayWidth, displayHeight);
        }

        public void GetDisplaySize(out int width, out int height)
        {
            width = displayWidth;
            height = displayHeight;
        }

        public void SetCursor(bool b)
        {
            Console.CursorVisible = b;
        }
    }
}

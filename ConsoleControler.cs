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
            // Ensure buffer size is valid
            if (displayWidth < Console.WindowWidth) displayWidth = Console.WindowWidth;
            if (displayHeight < Console.WindowHeight) displayHeight = Console.WindowHeight;
            // Set buffer size first
            Console.SetBufferSize(displayWidth, displayHeight);
            // Set window size
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

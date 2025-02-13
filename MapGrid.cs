using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace C_Learn
{

    public enum EGirdType
    {
        Road,
        Bomb,
        pause,
        End,
    }

    public struct MapGrid
    {
        public Vector2 DrawPos;
        public EGirdType type;
        public int stepNum;
        public void Draw() 
        {
            switch (type)
            {
                case EGirdType.Bomb:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("8");
                    break;
                case EGirdType.pause:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("=");
                    break;
                case EGirdType.End:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("~");
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("O");
                    break;
            }
        }

        public void RandomType() 
        {
            Random r = new Random(DateTime.Now.Microsecond);
            int num = r.Next(100);
            if (num >= 95)
            {
                type = EGirdType.Bomb;
            }
            else if(num >= 90 && num < 95) 
            {
                type = EGirdType.pause;
            }
            else
            {
                type = EGirdType.Road;
            }
        }

    }
}

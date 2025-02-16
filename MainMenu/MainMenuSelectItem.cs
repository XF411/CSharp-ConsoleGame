using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace C_Learn
{
    public class MainMenuSelectItem
    {
        //public bool isSelect = false;
        public string textID;
        public string text;
        public ConsoleColor textColor = ConsoleColor.White;
        public E_ScenceType selectType;
        /// <summary>
        /// Get cursor position X by display width and text length
        /// </summary>
        /// <param name="displayW"></param>
        /// <returns></returns>
        public int GetCursorPosX(int displayW) 
        {
            int x = (displayW - text.Length)/2;
            return x;
        }
        public void ChangeLanguage()
        {
            text = LanguageManager.Instance.GetString(textID);
        }
    }
}

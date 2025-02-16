using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Learn
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public enum eLanguage
    {
        Chinese,
        English,
    }

    public class LanguageManager
    {
        private static readonly Dictionary<eLanguage, Dictionary<string, string>> _translations;
        private static LanguageManager _instance;

        public eLanguage CurrentLanguage { get; set; } = eLanguage.Chinese;

        static LanguageManager()
        {
            _translations = new Dictionary<eLanguage, Dictionary<string, string>>();

            // get all const string
            var fields = typeof(LanguageManager).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string));

            foreach (var field in fields)
            {
                var parts = field.Name.Split('_');
                if (parts.Length < 2) continue;

                
                var suffix = parts.Last();
                var language = suffix switch
                {
                    "CS" => eLanguage.Chinese,
                    "EN" => eLanguage.English,
                    _ => (eLanguage?)null
                };

                if (!language.HasValue) continue;

                
                var key = string.Join("_", parts.Take(parts.Length - 1));

                
                if (!_translations.TryGetValue(language.Value, out var langDict))
                {
                    langDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    _translations[language.Value] = langDict;
                }

                langDict[key] = (string)field.GetValue(null);
            }
        }

        public static LanguageManager Instance => _instance ??= new LanguageManager();

        public string GetString(string key)
        {
            if (_translations.TryGetValue(CurrentLanguage, out var langDict))
            {
                return langDict.TryGetValue(key, out var value) ? value : $"[{key}]";
            }
            return $"[{key}]";
        }

        #region Chinese const
        public const string MainTitle_CS = "C#控制台小游戏";
        public const string RunGameTitle_CS = "游玩赛跑";
        public const string SnakeGameTitle_CS = "游玩贪吃蛇";
        public const string ExitGameTitle_CS = "退出游戏";
        public const string ChineseTitle_CS = "← 中文 →";
        public const string PlayAgain_CS = "再玩一次？";
        #endregion

        #region English const
        public const string MainTitle_EN = "C# Console Mini Game";
        public const string RunGameTitle_EN = "Play Run Game";
        public const string SnakeGameTitle_EN = "Play Snake Game";
        public const string ExitGameTitle_EN = "Exit Game";
        public const string EnglishTitle_EN = "← English →";
        public const string PlayAgain_EN = "Again？";
        #endregion
    }

}

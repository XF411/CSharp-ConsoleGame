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
        Max
    }

    public class LanguageManager
    {
        private static readonly Dictionary<eLanguage, Dictionary<string, string>> _translations;
        private static LanguageManager _instance;

        public eLanguage CurrentLanguage { get; set; } = eLanguage.Chinese;

        public void ChangeLanguage()
        {
            if (CurrentLanguage + 1 == eLanguage.Max) 
            {
                CurrentLanguage = (eLanguage)0;
            }
            else
            {
                CurrentLanguage = CurrentLanguage + 1;
            }
            EventBroadcaster.Instance.BroadcastEvent(EventName.ChangeLanguage);
        }

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
        public const string MainTitle_CS = "C# 控制台小游戏";
        public const string RunGameTitle_CS = "游玩赛跑";
        public const string SnakeGameTitle_CS = "游玩贪吃蛇";
        public const string ExitGameTitle_CS = "退出游戏";
        public const string ChangeLanguage_CS = "切换语言";
        public const string PlayAgain_CS = "再玩一次？";
        public const string WinRunGame_CS = "你赢下了赛跑游戏!";
        public const string LastRunGame_CS = "你输掉了赛跑游戏!";
        public const string EndSnakeGame_CS = "蛇蛇撞晕掉了，你的得分是 {0}";
        public const string Road_CS = "O:道路";
        public const string Bomb_CS = "B:炸弹";
        public const string Pause_CS = "=:暂停";
        public const string Player_CS = "P:玩家";
        public const string Enemy_CS = "E:敌人";
        public const string PlayerName_CS = "玩家";
        public const string EnemyName_CS = "敌人";
        public const string EnemyAndPlayer_CS = "A:敌人和玩家都在这里";
        public const string MoveRandomStep_CS = "按任意键随机移动一步，\n  从1到6";
        public const string PlayerMoved_CS = "玩家移动了 {0} 步，\n  现在在 {1}";
        public const string BombBoom_CS = "轰炸！{0} 退后了 {1} 步，\n  现在在 {2}";
        public const string PauseMove_CS = "{0} 被暂停！{1} 移动了 {2} 步，\n  现在在 {3}";
        #endregion

        #region English const
        public const string MainTitle_EN = "C# Console Mini Game";
        public const string RunGameTitle_EN = "Play Run Game";
        public const string SnakeGameTitle_EN = "Play Snake Game";
        public const string ExitGameTitle_EN = "Exit Game";
        public const string ChangeLanguage_EN = "Change Language";
        public const string PlayAgain_EN = "Play Again?";
        public const string WinRunGame_EN = "You won the run game!";
        public const string LastRunGame_EN = "You lost the run game!";
        public const string EndSnakeGame_EN = "The snake crashed, your point is {0}";
        public const string Road_EN = "O:road";
        public const string Bomb_EN = "B:bomb";
        public const string Pause_EN = "=:be pause";
        public const string Player_EN = "P:player";
        public const string Enemy_EN = "E:enemy";
        public const string PlayerName_EN = "玩家";
        public const string EnemyName_EN = "敌人";
        public const string EnemyAndPlayer_EN = "A:enemy and player all are here";
        public const string MoveRandomStep_EN = "push any key move random step,\n  from 1 to 6";
        public const string PlayerMoved_EN = "player moved {0} steps，\n  now on {1}";
        public const string BombBoom_EN = "Booooom!{0} back {1} steps，\n  now on {2}";
        public const string PauseMove_EN = "{0} is be pause!{1} move {2} steps,\n  now on {3}";
        #endregion
    }

}

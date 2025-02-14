//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace C_Learn.Input
//{
//    class InputControler : InstanceBase<InputControler>
//    {
//        Dictionary<ConsoleKey,List<Action>> ListenerDictionary = new Dictionary<ConsoleKey, List<Action>>();

//        public void CheckInput()
//        {
//            ConsoleKeyInfo keyInfo = Console.ReadKey();
//            if (ListenerDictionary.ContainsKey(keyInfo.Key))
//            {
//                var list = ListenerDictionary[keyInfo.Key];
//                foreach (var item in list)
//                {
//                    item?.Invoke();
//                }
//            }
//        }

//        public void AddListener(ConsoleKey key, Action action)
//        {
//            if (ListenerDictionary.ContainsKey(key))
//            {
//                if (!ListenerDictionary[key].Contains(action))
//                {
//                    ListenerDictionary[key].Add(action);
//                }
//            }
//            else
//            {
//                ListenerDictionary.Add(key, new List<Action> { action });
//            }
//        }

//        public void RemoveListener(ConsoleKey key, Action action) 
//        {
//            if (ListenerDictionary.ContainsKey(key))
//            {
//                if (ListenerDictionary[key].Contains(action))
//                {
//                    ListenerDictionary[key].Remove(action);
//                }
//            }
//        }
//    }
//}

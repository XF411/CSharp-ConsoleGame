using System;
using System.Collections.Generic;

namespace C_Learn
{
    public class EventBroadcaster
    {
        private static readonly EventBroadcaster instance = new EventBroadcaster();

        private Dictionary<string, List<Action>> eventListeners = new Dictionary<string, List<Action>>();

        // 私有构造函数，防止外部实例化
        private EventBroadcaster() { }

        // 公共静态方法，提供单例实例
        public static EventBroadcaster Instance
        {
            get { return instance; }
        }

        public void AddEventListener(string EventID, Action action)
        {
            if (eventListeners.ContainsKey(EventID))
            {
                eventListeners[EventID].Add(action);
            }
            else
            {
                eventListeners.Add(EventID, new List<Action> { action });
            }
        }

        public void BroadcastEvent(string EventID)
        {
            if (eventListeners.ContainsKey(EventID))
            {
                var list = eventListeners[EventID];
                foreach (var item in list)
                {
                    item?.Invoke();
                }
            }
        }
    }
}
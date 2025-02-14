using System;
using System.Collections.Generic;

namespace C_Learn
{
    public class EventBroadcaster
    {
        private static readonly EventBroadcaster instance = new EventBroadcaster();

        private Dictionary<string, List<Action>> eventListeners = new Dictionary<string, List<Action>>();

        private EventBroadcaster() { }

        public static EventBroadcaster Instance
        {
            get { return instance; }
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

        public void AddEventListener(string EventID, Action action)
        {
            if (eventListeners.ContainsKey(EventID))
            {
                if (!eventListeners[EventID].Contains(action))
                {
                    eventListeners[EventID].Add(action);
                }
            }
            else
            {
                eventListeners.Add(EventID, new List<Action> { action });
            }
        }

        public void RemoveEventListener(string EventID, Action action)
        {
            if (eventListeners.ContainsKey(EventID))
            {
                if (eventListeners[EventID].Contains(action))
                {
                    eventListeners[EventID].Remove(action);
                }
            }
        }

    }
}
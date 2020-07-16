using System;
using System.Collections.Generic;
using SP.Core.World;

namespace SP.Core.GEventSystem
{
    public class GEvent_QuestPlaceEnter
    {
        private readonly List<Action<GEventData_QuestPlace>> _callbacks = new List<Action<GEventData_QuestPlace>>();

        public void Subscribe(Action<GEventData_QuestPlace> callback)
        {
            _callbacks.Add(callback);
        }
        
        public void UnSubscribe(Action<GEventData_QuestPlace> callback)
        {
            _callbacks.Remove(callback);
        }

        public void Invoke(GEventData_QuestPlace data)
        {
            foreach (Action<GEventData_QuestPlace> callback in _callbacks)
                callback(data);
        }
    }
    
    public class GEvent_QuestPlaceLeave
    {
        private readonly List<Action<GEventData_QuestPlace>> _callbacks = new List<Action<GEventData_QuestPlace>>();

        public void Subscribe(Action<GEventData_QuestPlace> callback)
        {
            _callbacks.Add(callback);
        }
        
        public void UnSubscribe(Action<GEventData_QuestPlace> callback)
        {
            _callbacks.Remove(callback);
        }

        public void Invoke(GEventData_QuestPlace data)
        {
            foreach (Action<GEventData_QuestPlace> callback in _callbacks)
                callback(data);
        }
    }

    public class GEventData_QuestPlace
    {
        public QuestPlace questPlace;

        public GEventData_QuestPlace(QuestPlace questPlace)
        {
            this.questPlace = questPlace;
        }
    }
}
using System;
using System.Collections.Generic;

namespace SP.Core.GEventSystem
{
    public class GEvent_WActionButtonClicked
    {
        private readonly List<Action<GEventData_WActionButton>> _callbacks = new List<Action<GEventData_WActionButton>>();

        public void Subscribe(Action<GEventData_WActionButton> callback)
        {
            _callbacks.Add(callback);
        }
        
        public void UnSubscribe(Action<GEventData_WActionButton> callback)
        {
            _callbacks.Remove(callback);
        }

        public void Invoke(GEventData_WActionButton data)
        {
            foreach (Action<GEventData_WActionButton> callback in _callbacks)
                callback(data);
        }
    }

    public class GEventData_WActionButton
    {
        public WActionButton button;

        public GEventData_WActionButton(WActionButton button)
        {
            this.button = button;
        }
    }
}
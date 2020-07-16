using System;
using System.Collections.Generic;
using SP.Core.World;

namespace SP.Core.GEventSystem
{
    public class GEvent_Void
    {
        private readonly List<Action> _callbacks = new List<Action>();

        public void Subscribe(Action callback)
        {
            _callbacks.Add(callback);
        }
        
        public void UnSubscribe(Action callback)
        {
            _callbacks.Remove(callback);
        }

        public void Invoke()
        {
            foreach (Action callback in _callbacks)
                callback();
        }
    }
}
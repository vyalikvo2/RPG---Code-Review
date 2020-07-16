using System;
using UnityEngine;

namespace SP.Core.StateMachine
{
    public class StateMachine<T> : MonoBehaviour
    {
        public Action<T> OnChangeBegin;
        public Action<T> OnChangeCompleted;

        public StateMachine()
        {
        }

        public StateMachine(T startState)
        {
            CurrentState = startState;
        }

        public T CurrentState { get; private set; }

        public T PreviousState { get; private set; }

        public bool Reload(T state)
        {
            OnChangeBegin.Invoke(CurrentState);

            PreviousState = CurrentState;
            CurrentState = state;

            OnChangeCompleted.Invoke(CurrentState);

            return true;
        }

        public bool Change(T state)
        {
            return !state.Equals(CurrentState) && Reload(state);
        }
    }
}
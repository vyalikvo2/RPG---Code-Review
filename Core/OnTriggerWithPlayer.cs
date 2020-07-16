using System;
using UnityEngine;

namespace SP.Core
{
    public class OnTriggerWithPlayer : MonoBehaviour
    {
        public Action OnEnter;
        public Action OnExit;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                OnEnter?.Invoke();
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                OnExit?.Invoke();
        }
    }
}
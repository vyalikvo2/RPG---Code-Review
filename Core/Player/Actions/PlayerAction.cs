
using UnityEngine;
using UnityEngine.Diagnostics;

namespace SP.Core.Player.Actions
{
    public class PlayerAction
    {
        public const int ACTION_NONE = 0;
        public const int ACTION_MOVE = 1;

        public int ActionId { get; }
        
        public float CurrentTime = 0;
        public float MaxTime = 0;
        public bool LimitedTime = true;
        
        public PlayerControllerBase playerController;

        public PlayerAction(int actionId)
        {
            this.ActionId = actionId;
        }
        
        public PlayerAction()
        {
        }

        // override it
        public virtual bool Update(float deltaTime)
        {
            return false;
        }

        public virtual void OnActionComplete()
        {
 
        }
    }
}
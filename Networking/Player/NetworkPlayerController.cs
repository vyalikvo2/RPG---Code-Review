
using SP.Core.Player;
using SP.Core.Player.Actions;
using SP.Utils;
using UnityEngine;

namespace SP.Networking.Player
{
    public class NetworkPlayerController : PlayerControllerBase
    {
        public NetworkPlayerController()
        {
            MoveIgnoreAnimation = true;
        }
        public void FixedUpdate()
        {
            //Trace = true;
            base.FixedUpdate();
        }

        public void SetPlayerEnabled(bool isEnabled)
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = isEnabled;
            gameObject.SetShow(isEnabled);
        }

        public void TeleportTo(Vector2 pos)
        {
            transform.localPosition = new Vector3(pos.x, pos.y, 0);
            ClearActions();
        }

        public void MoveTo(Vector2 pos)
        {
            //if (ActionsCount <= 10) // magic number of current actions (need prediction algorythm)
           // {
                PlayerActionMove actionMove = new PlayerActionMove(pos);
                actionMove.LimitedTime = false;
                actionMove.SetPlayerController(this);
                AddAction(actionMove);
           // }
        }
        
    }
}

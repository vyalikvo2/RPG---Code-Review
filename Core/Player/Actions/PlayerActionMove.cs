using UnityEngine;

namespace SP.Core.Player.Actions
{
    public class PlayerActionMove : PlayerAction
    {
        public Vector2 Position { get; }
        public PlayerActionMove(Vector2 position):base(PlayerAction.ACTION_MOVE)
        {
            Position = position;
        }

        public void SetPlayerController(PlayerControllerBase playerController)
        {
            this.playerController = playerController;
            Vector2 distanceToEnd = (Position - playerController.GetPosition2D());
            MaxTime = distanceToEnd.magnitude / playerController.Speed;
        }
        
        // returns true then action completed
        public override bool Update(float deltaTime)
        {
            if (CurrentTime >= MaxTime && LimitedTime)
            {
                if (playerController.Trace) Debug.Log("ended action by time");
                return true;
            }
            CurrentTime += deltaTime;

            if (playerController == null)
            {
                Debug.Log("NULL PLAYER CONTROLLER IN ACTION");
                return true;
            }

            Vector2 curPos = playerController.GetPosition2D();
            Vector2 distanceToEnd = (Position - curPos);
            Vector2 dir = distanceToEnd.normalized;
            
            Vector2 velocity = dir * playerController.Speed;

            if (playerController.Trace) Debug.Log("velocity : "+velocity+" dist: "+distanceToEnd+" pos : "+Position);

            if (playerController.hasRigidBody)
            {
                if (velocity.sqrMagnitude >= distanceToEnd.sqrMagnitude)
                {
                    playerController.setVelocity(distanceToEnd, deltaTime);
                    if (playerController.Trace) Debug.Log("ended action by GOAL");
                    return true;
                }
                else
                {
                    playerController.setVelocity(velocity, deltaTime);
                }
            }
            else
            {
                if (velocity.sqrMagnitude * deltaTime >= distanceToEnd.sqrMagnitude)
                {
                    playerController.setVelocity(distanceToEnd, deltaTime);
                    if (playerController.Trace) Debug.Log("ended action by GOAL");
                    return true;
                }
                else
                {
                    playerController.setVelocity(velocity, deltaTime);
                }
            }
            
            
            return false;
        }

        public override void OnActionComplete()
        {
            if (playerController != null)
            {
                playerController.setVelocity(Vector2.zero);
            }
        }
    }
}

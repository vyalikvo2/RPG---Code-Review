using System.Collections.Generic;
using SP.Core.Player.Actions;
using SP.Utils.Attributes;
using UnityEngine;


namespace SP.Core.Player 
{
    public class PlayerControllerBase : MonoBehaviour
    {
        private const float ANIM_STOP_WALK_TIME = 0.15f; // Time that should be passed before player walking animation will stop
        public bool Trace = false;
        public bool MoveIgnoreAnimation = false; // ignore animation then player move (usually network player)

        protected float _currentSpeed { get; private set; } = 2f; // speed value for moving engine (do not set)
        public float RunSpeedMultiplier = 1.5f;
        public float Speed = 2f; // speed parameter

        private bool _isRunning;
        public bool isRunning
        {
            get { return _isRunning; }
            set
            {
                if (value == _isRunning) return;
                _isRunning = value;
                _currentSpeed = Speed * (_isRunning ? RunSpeedMultiplier : 1);
            }
        }

        protected List<PlayerAction> _actions;
        protected PlayerAction currentAction;
        public int ActionsCount = 0;
        private bool _lastActionCompleted = false;

        public PlayerAnimationController AnimationController;
        protected bool _canMove = true;
        private Vector2 _lastVelocity;
        private float _notWalkTimer = 0; // time not walking animation compensation
        
        private Rigidbody2D _rigidBody;
        public bool hasRigidBody;

        private new Transform transform;
        
        public PlayerControllerBase()
        {
            _actions = new List<PlayerAction>();
            
            isRunning = false;
            hasRigidBody = false;
            //_animationMoveStarted = false;
        }
        
        public void Awake()
        {
            transform = base.transform;

            AnimationController = GetComponentInChildren<PlayerAnimationController>();
            //AnimationController.OnMoveStarted += _onPlayerMoveStarted;
        }
        
        public void FixedUpdate()
        {
            _processActionStack();
        }

        public void Update()
        {
            _animate();
        }
        
        public void onDestroy()
        {
            //AnimationController.OnMoveStarted -= _onPlayerMoveStarted;
        }
        

        public void AddAction(PlayerAction action)
        {
            ActionsCount++;
            _actions.Add(action);
        }

        public void ClearActions()
        {
            _actions.Clear();
            ActionsCount = 0;
        }

        protected void SetRigidBody(Rigidbody2D rigidBody)
        {
            _rigidBody = rigidBody;
            hasRigidBody = _rigidBody != null;
        }

        public void setVelocity(Vector2 velocity, float deltaTime = 0)
        {
            if (_canMove || MoveIgnoreAnimation)
            {
                if (hasRigidBody)
                {
                    _rigidBody.velocity = velocity;
                }
                else
                {
                    if (velocity != Vector2.zero)
                    {
                        Vector3 pos = transform.localPosition;
                        transform.localPosition = new Vector3(pos.x + velocity.x * deltaTime,
                            pos.y + velocity.y * deltaTime, pos.z);
                    }
                }
            }

            if (Trace)
            {
                Debug.Log("velocity: "+velocity);
            }

            _lastVelocity = velocity;
            _updateScaleByVelocity(velocity);
        }
        
        public Vector2 GetPosition2D()
        {
            if (hasRigidBody)
            {
                return _rigidBody.position;
            }
            else
            {
                return new Vector2(transform.localPosition.x, transform.localPosition.y);
            }
        }

        private void _animate()
        {
            if (Mathf.Abs(_lastVelocity.x) > 0 || Mathf.Abs(_lastVelocity.y) > 0)
            {
                _notWalkTimer = 0;
            }
            else
            {
                _notWalkTimer += Time.deltaTime;
            }

            if (_notWalkTimer > ANIM_STOP_WALK_TIME)
            {
                AnimationController.SetIsWalking(false);
                //_animationMoveStarted = false;
            }
            else
            {
                AnimationController.SetIsWalking(true);
            }
            
        }

        public void _onPlayerMoveStarted()
        {
           // _animationMoveStarted = true;
        }

        private void _processActionStack()
        {
            if (_lastActionCompleted)
            {
                currentAction.OnActionComplete();
                currentAction = null; // override 
                _lastActionCompleted = false;

                _actions.RemoveAt(0);
                ActionsCount--;
            }
            
            if (currentAction == null && _actions.Count > 0)
            {
                currentAction = _actions[0];
            }

            if (currentAction == null) return; // no actions left so return

            _lastActionCompleted = currentAction.Update(Time.fixedDeltaTime); // process current action
        }

        private void _updateScaleByVelocity(Vector2 velocity)
        {
            float absScaleX = Mathf.Abs(transform.localScale.x);
            float newScaleX = 0;
            if (velocity.x > 0)
            {
                newScaleX = absScaleX;
            } else if (velocity.x < 0)
            {
                newScaleX = -absScaleX;
            }
            if (newScaleX != 0 && newScaleX!=transform.localScale.x)
            {
                transform.localScale = new Vector3(newScaleX, transform.localScale.y, transform.localScale.z);
            }
        }
        
    }
}

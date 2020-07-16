using SP.Constants;
using SP.Core.Data;
using SP.Core.GEventSystem;
using SP.Core.Player.Actions;
using SP.Core.StateMachine;
using SP.Core.UIScreens;
using SP.Localization;
using SP.Networking;
using SP.Networking.Player;
using UnityEngine;

namespace SP.Core.Player
{
    public class PlayerController : PlayerControllerBase
    {
        [HideInInspector] private Rigidbody2D _rigidBody;
        [HideInInspector] private NetworkPlayerTransform _networkPlayerTransform;
        
        private Joystick joystick;
        
        private float _movementSpeed;
        private Vector2 _movementDirection;

#if UNITY_EDITOR
        private bool _speedHack;
        private const int SpeedHackMultiplier = 5;
#endif
        
        public void Awake()
        {
            base.Awake();
            
            joystick = FindObjectOfType<Joystick>();
            
            _rigidBody = GetComponent<Rigidbody2D>();
            _rigidBody.freezeRotation = true;
            SetRigidBody(_rigidBody);

            _networkPlayerTransform = gameObject.AddComponent<NetworkPlayerTransform>();
        }

        public void Update()
        {
            base.Update();
            _processInputs(Time.deltaTime);
        }

        public void FixedUpdate()
        {
            base.FixedUpdate();
            _checkMoveAction(Time.fixedDeltaTime);
        }

        private void _checkMoveAction(float deltaTime)
        {
            if (ActionsCount == 0 && _movementSpeed > 0)
            {
                var velocity = _movementSpeed * _currentSpeed * _movementDirection;
                var actionMove = new PlayerActionMove(_rigidBody.position + velocity);
                actionMove.SetPlayerController(this);
                actionMove.CurrentTime = deltaTime; // костыль: не смог разобраться с багом когда идёшь в стенку
                                                    // и меняешь направление - почему то задержка действия дольше чем надо, это время чуть компенсирует хоть
                AddAction(actionMove);
            }
        }

        private void _processInputs(float deltaTime)
        {
            if (UIStateMachine.instance?.CurrentScreenName != ScreenNames.Main)
            {
                _movementDirection = Vector2.zero;
                return;
            }
            
            _movementDirection = joystick.Direction;
            
            if(_movementDirection == Vector2.zero)
                _movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            
            _processPlayerRunning(Input.GetKey(KeyCode.LeftShift) && !_movementDirection.Equals(Vector2.zero), deltaTime);
            
            //_movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            _movementSpeed = Mathf.Clamp(_movementDirection.magnitude, 0.0f, 1.0f);
            _movementDirection.Normalize();
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.S) && Input.GetKey(KeyCode.Space))
            {
                _speedHack = !_speedHack;
                Speed = _speedHack ? Speed * SpeedHackMultiplier : Speed / SpeedHackMultiplier;
            }
#endif
        }
        
        private void _processPlayerRunning(bool inputRunning, float deltaTime)
        {
            GamePlayerStats gameStats = PlayerStatsData.Me.game;
            
            var canRun = inputRunning && gameStats.endurance.Use(Mathf.FloorToInt(deltaTime * GameConstants.RUN_ENDURANCE_PER_SECOND));

            if (!inputRunning) gameStats.endurance.isUsing = false;

            if (isRunning && gameStats.endurance.current == 0) GEvent.STAT_BECOME_ZERO_Endurance.Invoke();
    
            _networkPlayerTransform.isRunning = canRun;
            isRunning = canRun;
            AnimationController.SetIsRunning(canRun);
            
        }
        
    }
}

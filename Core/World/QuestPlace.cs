using SP.Core.GEventSystem;
using SP.Utils.Attributes;
using UnityEngine;

namespace SP.Core.World
{
    public class QuestPlace : MonoBehaviour
    {
        [SerializeField] private Color _initColorTint;
        [SerializeField] private WActionButton _wActionButton;
        [SerializeField] private TextMeshProAnimController _textAnimController;
        
        [Header("Quest Place params")]
        public int placeId; // QuestPlace id
        [Space]
        [SerializeField]  private bool _disableButton = false;
        [Space]
        [SerializeField] public TriggerTextAnimationParams _textAnimationParams;

        // Button action id
        public int gActionId => _wActionButton.gActionId;

        public Color QuestColor
        {
            set
            {
                _backRenderer.material.color = value;
                _frontRenderer.material.color = value;
            }
            get => QuestColor;
        }

        [Header("Dev params:")]
        [DisableEdit] [SerializeField] private SpriteRenderer _backRenderer;
        [DisableEdit] [SerializeField] private SpriteRenderer _frontRenderer;
        
        void Awake()
        {
            if(_textAnimController) _textAnimController.SetAnimParams(_textAnimationParams);
            if(_disableButton) _wActionButton.DisableButton();
        }
        
        void Start()
        {
            var trigger = gameObject.AddComponent<OnTriggerWithPlayer>();
            trigger.OnEnter += OnEnter;
            trigger.OnExit += OnExit;

            QuestColor = _initColorTint;
        }

        private void OnExit()
        {
            if(_wActionButton) _wActionButton.Hide();
            if (_textAnimController) 
                _textAnimController.OnTriggerExit();
            
            GEvent.TRIGGER_LEAVE_QuestPlace.Invoke(new GEventData_QuestPlace(this));
        }

        private void OnEnter()
        {
            if(_wActionButton) _wActionButton.Show();
            if (_textAnimController) 
                _textAnimController.OnTriggerEnter();
            
            GEvent.TRIGGER_ENTER_QuestPlace.Invoke(new GEventData_QuestPlace(this));
        }
    }
}
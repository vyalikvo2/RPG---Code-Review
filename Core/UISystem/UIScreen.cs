using System.Collections.Generic;
using System.Threading.Tasks;
using SP.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace SP.Core.StateMachine
{
    public class UIScreen : MonoBehaviour
    {
        [HideInInspector]
        public ScreenNames Name;

        private CanvasGroup _canvasGroup;
        private List<GraphicRaycaster> _rayCasters;
        
        private Animator _aniInternal;

        protected bool isShowed = false;
        
        public Animator Animator
        {
            get
            {
                if (_aniInternal == null)
                    _aniInternal = GetComponent<Animator>();

                return _aniInternal;
            }
        }

        private List<GraphicRaycaster> RayCasters => _rayCasters ?? (_rayCasters = CollectRayCasters());

        public CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup != null)
                    return _canvasGroup;

                var canvas = GetComponent<CanvasGroup>();
                _canvasGroup = canvas != null ? canvas : gameObject.AddComponent<CanvasGroup>();

                var rayCastersInit = RayCasters;

                return _canvasGroup;
            }
        }

        public bool IsActive => CanvasGroup.alpha > 0;

        public void AddRayCaster(GraphicRaycaster rayCaster)
        {
            RayCasters.Add(rayCaster);
        }

        private List<GraphicRaycaster> CollectRayCasters()
        {
            var rayCaster = new List<GraphicRaycaster>();

            if (!GetComponent<GraphicRaycaster>())
                gameObject.AddComponent<GraphicRaycaster>();

            var childRayCaster = GetComponentsInChildren<GraphicRaycaster>(true);

            if (childRayCaster.Length > 0)
                rayCaster.AddRange(childRayCaster);

            return rayCaster;
        }

        public virtual void Hide()
        {
            isShowed = false;
            
            CanvasGroup.SetAlpha(0);

            ChangeActivityRaycaster(false);
            
            _targetVisible = false;
            _targetAlpha = 0;
        }

        public virtual void Show()
        {
            isShowed = true;
            
            CanvasGroup.SetAlpha(1);

            ChangeActivityRaycaster(true);

            _targetVisible = true;
            _targetAlpha = 1;
        }
        
        public virtual void ShowFullSize()
        {
        }
        
        public virtual void ShowCropSize()
        {
        }
        
        public virtual void ShowSizeReset()
        {
        }

        public virtual void ShowGameObject()
        {
            gameObject.Show();
        }

        public void ChangeActivityRaycaster(bool activity)
        {
            var toRemoveRaycasters = new List<GraphicRaycaster>();
            foreach (var raycaster in _rayCasters)
            {
                if (raycaster.IsAnyNull())
                {
                    toRemoveRaycasters.Add(raycaster);
                    continue;
                }

                raycaster.enabled = activity;
            }

            foreach (var toRemoveRaycaster in toRemoveRaycasters)
                _rayCasters.Remove(toRemoveRaycaster);
        }

        private Task _changeVisibleHandle;
        private float _targetAlpha;
        private bool _targetVisible;
        public bool AnimatedVisible
        {
            set
            {
                _targetVisible = value;
                _targetAlpha = value ? 1 : 0;
                
                if (!_targetVisible)
                    ChangeActivityRaycaster(false);
                
                if (_changeVisibleHandle == null || _changeVisibleHandle.IsCompleted)
                    _changeVisibleHandle = AnimationFade();
            }
        }

        private async Task AnimationFade()
        {
            var currentAlpha = CanvasGroup.alpha;
            while (!this.IsAnyNull())
            {
                currentAlpha = Mathf.Lerp(currentAlpha, _targetAlpha, Time.deltaTime * 2f);
                
                if ((!_targetVisible && currentAlpha < 0.1f) || (_targetVisible && currentAlpha > 0.9f))
                {
                    CanvasGroup.alpha = _targetAlpha;
                    break;
                }

                CanvasGroup.alpha = currentAlpha;
                await Task.Yield();
            }
            
            if (_targetVisible)
                ChangeActivityRaycaster(true);
        }
    }
}
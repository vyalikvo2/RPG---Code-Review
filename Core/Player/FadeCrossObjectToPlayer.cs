using UnityEngine;

namespace SP.Core.Player
{
    public class FadeCrossObjectToPlayer : MonoBehaviour
    {
        private bool _executing;
        private Transform _character;
        private Transform _camera;

        private SpriteRenderer _hiddenObject;
        private float _fadeAlpha = 0.33f;

        public void StartExecute()
        {
            _camera = transform;
            _character = _camera.parent;
            _executing = true;
        }
        
        private void Update()
        {
            if (!_executing)
                return;
            
            var characterPosition = _character.position;
            var camPos= _camera.position;
            var dir = characterPosition - camPos;

            var hitInfo = Physics2D.Raycast(camPos, dir);

            if (hitInfo.collider != null && hitInfo.collider.isTrigger)
            {
                var rnd = hitInfo.collider.gameObject.GetComponent<SpriteRenderer>();
                if (rnd != null)
                {
                    if (rnd != _hiddenObject)
                        UnHideHidden();

                    _hiddenObject = rnd;
                    
                    var color = rnd.color;
                    color.a = Mathf.Lerp(color.a, _fadeAlpha, Time.deltaTime * 2f);
                    rnd.color = color;
                }
                else
                {
                    UnHideHidden();
                }
            }
            else
            {
                UnHideHidden();
            }

            void UnHideHidden()
            {
                if (_hiddenObject == null) return;
                var color1 = _hiddenObject.color;
                color1.a = 1;
                _hiddenObject.color = color1;
            }
        }
    }
}
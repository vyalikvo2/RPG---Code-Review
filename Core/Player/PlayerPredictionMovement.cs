using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP.Core.Player
{
    public class PlayerPredictionMovement : MonoBehaviour
    {
        private new Transform transform;
        public Vector3 positionToMove;

        public float SmoothKoef = 3.0f;
        
        void Start()
        {
            transform = base.transform;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (positionToMove == null) return;
            base.transform.localPosition = Vector3.Lerp(transform.localPosition, positionToMove, Time.fixedDeltaTime * SmoothKoef);
            //transform.localPosition = new Vector3(positionToMove.x,positionToMove.y,0);
        }
    }
}

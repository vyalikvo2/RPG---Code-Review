using SP.Core.Data;
using SP.Networking.Requests;
using SP.Utils.Attributes;
using UnityEngine;

namespace SP.Networking.Player
{
    [RequireComponent(typeof(NetworkInfo))]
    public class NetworkPlayerTransform : MonoBehaviour
    {
        private static readonly float MAX_SEND_POS_INTERVAL = 1.0f; // update static (not changed) position period
        private static readonly float MIN_SEND_DIST = 0.05f;

        [SerializeField] [DisableEdit] [Header("Debug Info")]
        private Vector3 oldPosition;

        private NetworkInfo _networkIdentity;
        private float _sendStaticUpdateTimestamp = 0f;
        
        public bool isRunning = false;

        private new Transform transform;

        public void Start()
        {
            transform = base.transform;
            _networkIdentity = GetComponent<NetworkInfo>();
            oldPosition = transform.localPosition;
        }

        private void Update()
        {
            if (!_networkIdentity.IsControlled()) return;
            
            var localPosition = transform.localPosition;

            if ((localPosition - oldPosition).sqrMagnitude >= MIN_SEND_DIST * MIN_SEND_DIST)
            {
                SendData(localPosition);
            }
            else
            {
                _sendStaticUpdateTimestamp += Time.deltaTime;
                if (_sendStaticUpdateTimestamp >= MAX_SEND_POS_INTERVAL)
                    SendData(localPosition);
            }
        }

        private void SendData(Vector3 localPosition)
        {
            WorldPosition pos = new WorldPosition();
            pos.SetPosition(localPosition.x, localPosition.y);
    
            UpdatePlayerReqParamsPosition posParams = new UpdatePlayerReqParamsPosition(pos.x, pos.y);
  
            posParams.r = isRunning ? 1 : 0;
     
            UpdatePlayerRequest.Send(UpdatePlayerReqAction.UPDATE_POSITION, posParams);
            
            oldPosition = localPosition;
            _sendStaticUpdateTimestamp = 0;
        }
    }
}

using System;
using SP.Utils.Attributes;
using UnityEngine;

namespace SP.Networking
{
    public class NetworkInfo : MonoBehaviour
    {
        [Header("Spawned Object Info")]
        [SerializeField] [DisableEdit]
        private long _id;
        [SerializeField] [DisableEdit]
        private bool _isControlled;
        
        public void Awake()
        {
            _isControlled = false;
        }

        public void SetControllerID(Int64 id)
        {
            _id = id;
            _isControlled = id == NetworkClient.ClientId;
        }

        public long GetID()
        {
            return _id;
        }

        public bool IsControlled()
        {
            return _isControlled;
        }
        
    }
}

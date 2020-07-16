using System;
using System.Collections.Generic;
using SP.Core.StateMachine;
using SP.Networking;
using SP.Utils;
using UnityEngine;

namespace SP.Core.UIScreens
{
    public class PlayerMessagesHolder : UIScreen
    {
        public static PlayerMessagesHolder instance;

        private readonly Dictionary<Int64, PlayerMessagePanel> _panels = new Dictionary<Int64, PlayerMessagePanel>();

        public GameObject template;
        
        private void Awake()
        {
            Name = ScreenNames.PlayersMessages;
        }

        private void Start()
        {
            instance = this;
        }

        private void OnDestroy()
        {
            instance = null;
        }

        public void OnSpawnPlayer(Transform player, Int64 uid)
        {
            var go = Instantiate(template, transform);
            go.name = $"PlayerMessage_{uid}";
            go.Show();
            go.GetComponent<CanvasGroup>().alpha = 0f;
            var cmp = go.GetComponent<PlayerMessagePanel>();

            if (_panels.ContainsKey(uid))
                _panels[uid] = cmp;
            else
                _panels.Add(uid, cmp);
            
            cmp.AssignPlayer(player);
        }
        
        public void OnDestroyPlayer(Int64 uid)
        {
            if (!_panels.ContainsKey(uid)) return;
            
            Destroy(_panels[uid].gameObject);
            _panels.Remove(uid);
        }
        
        public void OnSomebodySay(long uid, string nickname, string message)
        {
            if (!_panels.ContainsKey(uid)) return;

            _panels[uid].OnSay(nickname, message);
        }
        
        public void OnPlayerSay(string message)
        {
            OnSomebodySay(NetworkClient.ClientId, "Me", message);
        }
    }
}
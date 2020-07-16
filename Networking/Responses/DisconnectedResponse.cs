using System.Collections;
using System.Collections.Generic;
using SocketIO;
using SP.Core.UIScreens;
using SP.Core.World;
using UnityEngine;

namespace SP.Networking.Responses
{
    public static class DisconnectedResponse 
    {
        public static void Process(SocketIOEvent ev)
        {
            //if (ResponseChecker.IsErrorResponse(ev)) return;
            
            JSONObject data = NetworkClient.GetResponseParams(ev);
            long uid = long.Parse(data["uid"].ToString2());
            
            SpawnController.DestroyUID(uid);

            PlayerMessagesHolder.instance?.OnDestroyPlayer(uid);
        }
    }
}

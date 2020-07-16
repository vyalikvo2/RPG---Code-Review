using SocketIO;
using SP.Core.World;
using UnityEngine;

namespace SP.Networking.Responses
{
    public static class SpawnResponse 
    {
        public static void Process(SocketIOEvent ev)
        {
            if (ResponseValidator.IsErrorResponse(ev)) return;
            
            JSONObject data = NetworkClient.GetResponseParams(ev);

            long uid =  long.Parse(data["uid"].ToString2());
            
            float x = data["x"].f;
            float y = data["y"].f;

            bool playerEnabled = Mathf.FloorToInt(data["enabled"].f) == 1;
           
            SpawnController.instance.SpawnAt(new Vector2(x, y), uid);
            SpawnController.SetPlayerEnabled(uid, playerEnabled);
        }
    }

    public class PlayerSpawnData
    {
        public bool isEnabled;
    }
}

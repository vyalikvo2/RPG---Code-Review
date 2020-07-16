using SocketIO;
using SP.Core.Data;
using SP.Core.World;
using UnityEngine;

namespace SP.Networking.Responses
{
    public static class UpdatePlayerResponse
    {
        
// -------------------------------------------------------------------------------------------------------- 
        public static void Process(SocketIOEvent ev)
        {
            if (ResponseValidator.IsErrorResponse(ev)) return;
            
            int actionId = NetworkClient.GetResponseActionId(ev);

            switch (actionId)
            {
                case UpdatePlayerReqAction.UPDATE_POSITION:
                    _processUpdatePosition(NetworkClient.GetResponseParams(ev));
                    break;
                case UpdatePlayerReqAction.UPDATE_ENABLED:
                    _processUpdateEnabled(NetworkClient.GetResponseParams(ev));
                    break;
                default:
                    Asshole.Error("Wrong action id");
                    break;
            }
        }
        
// -------------------------------------------------------------------------------------------------------- 
        private static void _processUpdatePosition(JSONObject data)
        {
            long uid = long.Parse(data["uid"].ToString2());

            float x = data["x"].f;
            float y = data["y"].f;
            
            bool isRunning = Mathf.FloorToInt(data["r"].f) == 1;
            SpawnController.SetPlayerRunning(uid, isRunning);
            
            Vector2 pos = WorldPosition.FromServerPosition(x, y);

            SpawnController.MovePlayerToPos(uid, pos);
            
        }
        // -------------------------------------------------------------------------------------------------------- 
        private static void _processUpdateEnabled(JSONObject data)
        {
            long uid = long.Parse(data["uid"].ToString2());
            
            int enabled = Mathf.FloorToInt(data["enabled"].f);

            if (enabled == 1)
            {
                float x = data["x"].f;
                float y = data["y"].f;
                Vector2 pos = WorldPosition.FromServerPosition(x, y);
                SpawnController.SetPlayerEnabled(uid, true, pos);
            }
            else
            {
                SpawnController.SetPlayerEnabled(uid, false, Vector2.zero);
            }
            
            
        }
    }
    
// -------------------------------------------------------------------------------------------------------- 
    public class UpdatePlayerReqAction
    {
        public const int UPDATE_POSITION = 1;
        public const int UPDATE_ENABLED = 2;
    }
}
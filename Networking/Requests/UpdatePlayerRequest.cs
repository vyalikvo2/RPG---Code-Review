using System;
using SP.Networking.Constants;

namespace SP.Networking.Requests
{
    public static class UpdatePlayerRequest
    {
        public static void Send(int actionId, object updateParams = null)
        {
            switch (actionId)
            {
                case UpdatePlayerReqAction.UPDATE_POSITION:
                    NetworkClient.SEND_ACTION(CL.UPDATE_PLAYER, actionId, updateParams);
                    break;
                case UpdatePlayerReqAction.UPDATE_ENABLED:
                    NetworkClient.SEND_ACTION(CL.UPDATE_PLAYER, actionId, updateParams);
                    break;
                default:
                    Asshole.Error("No such action id");
                    break;
            }
        }
        
    }
    
// --------------------------------------------------------------------------------------------------------   
    [Serializable]
    public class UpdatePlayerReqParamsPosition
    {
        public int x;
        public int y;

        public int r; // is running
        
        public UpdatePlayerReqParamsPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    
    // --------------------------------------------------------------------------------------------------------   
    [Serializable]
    public class UpdatePlayerReqParamsEnabled
    {
        public int enabled;
        public UpdatePlayerReqParamsEnabled(bool enabled)
        {
            this.enabled = enabled ? 1: 0;
        }
    }
    
// -------------------------------------------------------------------------------------------------------- 
    public static class UpdatePlayerReqAction
    {
        public const int UPDATE_POSITION = 1;
        public const int UPDATE_ENABLED = 2;
    }
}

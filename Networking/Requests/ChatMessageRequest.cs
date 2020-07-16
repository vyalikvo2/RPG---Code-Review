using SP.Networking.Constants;
using UnityEngine;

namespace SP.Networking.Requests
{
    public static class ChatRequest
    {
        public static void Send(int actionId, object chatParams = null)
        {
            switch (actionId)
            {
                case ChatReqAction.SEND_MESSAGE_TO_ALL:
                    NetworkClient.SEND_ACTION(CL.CHAT, actionId, chatParams); 
                    break;
                default:
                    Asshole.Error("wrong action id");
                    break;
            }
        }
    }
    
    public static class ChatReqAction
    {
        public const int SEND_MESSAGE_TO_ALL = 1;
    }
    
    [SerializeField]
    public class ChatReqParams
    {
        public string message;

        public ChatReqParams(string message)
        {
            this.message = message;
        }
    }
    
}

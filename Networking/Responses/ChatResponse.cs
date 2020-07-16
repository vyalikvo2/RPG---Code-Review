using SocketIO;
using SP.Core.UIScreens;
using SP.Networking.Requests;
using UnityEngine;

namespace SP.Networking.Responses
{
    public static class ChatResponse 
    {
        public static void Process(SocketIOEvent ev)
        {
            if (ResponseValidator.IsErrorResponse(ev)) return;
            
            int actionId = NetworkClient.GetResponseActionId(ev);
            JSONObject data = NetworkClient.GetResponseParams(ev);
            
            switch (actionId)
            {
                case ChatReqAction.SEND_MESSAGE_TO_ALL:
                    long uid =  long.Parse(data["uid"].ToString2());
                    string username =  data["username"].ToString2();
                    string message =  data["message"].ToString2();
                    Debug.LogWarning($"[Chat] {username}: {message}");
                    if (uid == NetworkClient.ClientId)
                        return;
                    PlayerMessagesHolder.instance?.OnSomebodySay(uid, username, message);
                    break;
                default:
                    Asshole.Error("No such action in chat");
                    break;
            }
        }
    }
}
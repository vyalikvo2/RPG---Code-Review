using SocketIO;
using UnityEngine;

namespace SP.Networking.Responses
{
    public class ResponseValidator
    {
        public static bool IsErrorResponse(SocketIOEvent ev)
        {
            if (ev.data && ev.data.HasField("r"))
            {
                int result = Mathf.CeilToInt(ev.data["r"].f);
                switch (result)
                {
                    case RequestResult.OK:
                        return false;
                    case RequestResult.ERROR:
                        Debug.Log("ERROR!:: " + ev.data);
                        return true;
                    default:
                        return false;
                }
            }
            else
            {
                Asshole.Error("BAD STATUS CODE");
                return true;
            }
        }
    }
}
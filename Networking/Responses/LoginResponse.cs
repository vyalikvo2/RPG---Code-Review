
using System;
using SocketIO;
using SP.Constants;
using SP.Core.Data;
using SP.Core.StateMachine;
using SP.Networking.Requests;
using SP.Utils;
using UnityEngine;

namespace SP.Networking.Responses
{
    public static class LoginResponse
    {
        public static void Process(SocketIOEvent ev)
        {
            if (ResponseValidator.IsErrorResponse(ev)) return;
            
            JSONObject data = NetworkClient.GetResponseParams(ev);
            long serverTime = long.Parse(data["timestamp"].ToString2());
            Timestamp.SetServerTime(serverTime);

            string status = data["status"].ToString2();
            long uid =  long.Parse(data["uid"].ToString2());
            
            if (data.HasField("timestamp"))
            {
                NetworkClient.ServerTime = Convert.ToInt64(data["timestamp"].ToString2());
            }
         
            NetworkClient.SetClientId(uid);
            
            switch (status)
            {
                case "created_new":
                    PlayerPrefs.SetString(PrefsConstants.Uid, uid.ToString());
                    Ass.Log("onGeneratedUserId id: "+ uid);
                    NetworkClient.IsLogged = true;
                    break;
                case "logged_in":
                    Ass.Log("Logged in as: "+ uid);
                    NetworkClient.IsLogged = true;
                    break;    
                case "login_error":
                    Ass.Log("No Such User: "+ uid);
                    break;
            }

            if (NetworkClient.IsLogged)
            {
                NetworkClient.CallOnLoggedIn();
                PlayerStatsData playerStats = PlayerStatsData.fromJSON(data["player"]);
                playerStats.game.endurance.SetAutoUpdate();
                PlayerStatsData.SetPlayerStats(playerStats);
                
                if (data["player"] && data["player"]["job"])
                    JobData.InitJSONData(data["player"]["job"]);

                if (JobData.Time != 0 && Timestamp.Get() - JobData.Time < JobData.JobDuration)
                {
                    UIStateMachine.instance?.Reload(ScreenNames.JobInProgress);
                }
            }
        }
    }
}

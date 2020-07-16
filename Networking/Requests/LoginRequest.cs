
using System;
using SocketIO;
using SP.Constants;
using SP.Networking.Constants;
using UnityEngine;

namespace SP.Networking.Requests
{
    public class LoginRequest
    {
        public static void ProcessAndSend(SocketIOEvent ev)
        {
            Send();
        }
        
        public static void Send()
        {
            long uid = NetworkClient.LoginUid;
            LoginReqParams loginParams = new LoginReqParams();
            loginParams.uid = uid.ToString();
        
            if (PlayerPrefs.HasKey(PrefsConstants.Uid) && uid!= null && NetworkClient.CreateNewUser == false )
            {
                loginParams.uid = PlayerPrefs.GetString(PrefsConstants.Uid);
            } 
            
            
            Debug.Log("Login UId " + loginParams.uid);

            NetworkClient.SEND_ACTION(CL.LOGIN, -1, loginParams);
        }
    }
    
    [Serializable]
    public class LoginReqParams
    {
        public string uid; // long
        public string password;
        public bool isNew;
    }
}

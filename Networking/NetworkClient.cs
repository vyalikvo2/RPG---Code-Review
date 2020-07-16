using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using SP.Core.Data;
using SP.Core.GEventSystem;
using SP.Networking.Constants;
using SP.Networking.Player;
using SP.Networking.Requests;
using SP.Networking.Responses;

namespace SP.Networking
{
    public class NetworkClient : SocketIOComponent
    {
        [HideInInspector] public static long LoginUid;
        public static NetworkClient Socket;
        public static bool UseLocalServer = false;
        
        public static bool CreateNewUser = false;
        [Header("Account")]
        [SerializeField] private bool createNewUser = false;
        
        public static long ServerTime;

        public static long ClientId;
        public static bool IsLogged = false;
        
        public delegate void NetworkLoggedInEvent();
        public static event NetworkLoggedInEvent OnLoggedInDelegate;
        
        
// --------------------------------------------------------------------------------------------------------
        public override void Awake()
        {
            base.Awake();

            UseLocalServer = useLocalServer;
            
            if(Socket == null) Socket = this;
            
            CreateNewUser = createNewUser;
            
            _setupEvents();

            InitDataLoader.onAllDataLoaded += _allDataLoaded;
            InitDataLoader.instance.LoadAllData();
        }

        // all data loaded - connect socket
        private void _allDataLoaded()
        {
            Connect();
        }

// --------------------------------------------------------------------------------------------------------
        private void _setupEvents()
        {
            //On(SV.Open, onConnectedToServer); // Do Nothing (Empty receiver)
            On(SV.Disconnected, DisconnectedResponse.Process);
            
            On(SV.INIT, LoginRequest.ProcessAndSend);
            On(SV.LOGIN_RESPONSE, LoginResponse.Process);
            On(SV.SPAWN_HERO, SpawnResponse.Process);
            On(SV.UPDATE_PLAYER, UpdatePlayerResponse.Process);

            On(SV.CHAT, ChatResponse.Process);
            
            On(SV.JOB, JobResponse.Process);
        }
        
// --------------------------------------------------------------------------------------------------------
        public static void SetClientId(long clientId)
        {
            ClientId = clientId;
        }
        
// --------------------------------------------------------------------------------------------------------
        public static void CallOnLoggedIn()
        {
            if (OnLoggedInDelegate != null) OnLoggedInDelegate();
        }
        
// --------------------------------------------------------------------------------------------------------
        public static void SEND_ACTION(string commandId, int actionId, object actionParams = null)
        {
            JSONObject sendJson = new JSONObject();
            sendJson.AddField("a", actionId);
            
            if(actionParams != null)
                sendJson.AddField("p", new JSONObject(JsonUtility.ToJson(actionParams))); // need to add manually
            
            Socket.Emit(commandId, sendJson); 
        }
        
// --------------------------------------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------------  
// parse request helpers
        public static int GetResponseActionId(SocketIOEvent ev)
        {
            return Mathf.RoundToInt(ev.data["a"].f);
        }

        public static JSONObject GetResponseParams(SocketIOEvent ev) 
        {
            return ev.data["p"];
        }
        
    }
    
    
}

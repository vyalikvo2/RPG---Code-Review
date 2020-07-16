using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP.Networking.Constants
{
    public class SV
    {
        public const string Open = "open"; // socket connected event
        public const string Disconnected ="disconnected"; // socket disconnected event
        
        public const string INIT = "init";
        public const string LOGIN_RESPONSE ="login_response";
        public const string SPAWN_HERO ="spawn_hero";
        public const string UPDATE_PLAYER ="update_player";
        
        public const string CHAT ="chat";
        public const string JOB = "job";
        
        public const string DATABASE_ERROR = "database_error";
    }
}
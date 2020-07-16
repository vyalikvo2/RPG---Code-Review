using System.Collections.Generic;
using SP.Core.Data;
using SP.Core.Player;
using SP.Core.UIScreens;
using SP.Networking;
using SP.Networking.Player;
using UnityEngine;

namespace SP.Core.World
{

    public class SpawnController : MonoBehaviour
    {
        public static Dictionary<long, NetworkPlayerController> NetworkPlayerControllers;
        public static Dictionary<long, NetworkInfo> NetworkInfos;

        public static GameObject ReceivedPositionDebug;
        private GameObject _receivedPositionDebug;

        [SerializeField] public GameObject playerPrefab;
        private static SpawnController _instance;
        public static SpawnController instance
        {
            get { return _instance; }
        }

        void Awake()
        {
            if(_instance == null)
                _instance = this;
            
            ReceivedPositionDebug = _receivedPositionDebug;
            NetworkPlayerControllers = new Dictionary<long, NetworkPlayerController>();
            NetworkInfos = new Dictionary<long, NetworkInfo>();
        }
        

        public void SpawnAt(Vector2 spawnPos, long uid)
        {
            GameObject go = Instantiate(playerPrefab, transform);
            
            var trans = go.transform;
            Vector2 pos = WorldPosition.FromServerPosition(spawnPos.x, spawnPos.y);
            trans.localPosition = new Vector3(pos.x, pos.y, 0);
            go.name = "PlayerID: " + uid;
            if (uid == NetworkClient.ClientId)
            {
                go.tag = "Player";
                go.name = "MY PLAYER: " + uid;

                PlayerController playerController = go.AddComponent<PlayerController>();
            }
            else
            {
                go.tag = "PlayerOther";
                Rigidbody2D rigidBody = go.GetComponent<Rigidbody2D>();
                Destroy(rigidBody);

                NetworkPlayerController networkPlayerController = go.AddComponent<NetworkPlayerController>();
                NetworkPlayerControllers[uid] = networkPlayerController;
            }

            NetworkInfo n1 = go.GetComponent<NetworkInfo>();
            n1.SetControllerID(uid);
            
            PlayerMessagesHolder.instance?.OnSpawnPlayer(go.transform, uid);

            if (n1.IsControlled())
            {
                var cam = Camera.main.transform;
                cam.SetParent(n1.gameObject.transform);
                cam.localPosition = new Vector3(0, 2, -10);
                cam.GetComponent<FadeCrossObjectToPlayer>().StartExecute();
            }

            NetworkInfos.Add(uid, n1);
        }

        public static void SetPlayerRunning(long uid, bool isRunning)
        {
            if (NetworkPlayerControllers[uid])
            {
                NetworkPlayerControllers[uid].isRunning = isRunning;
                NetworkPlayerControllers[uid].AnimationController.SetIsRunning(isRunning);
            }
        }

        public static void SetPlayerEnabled(long uid, bool enabled, Vector2 pos)
        {
            NetworkPlayerControllers[uid].SetPlayerEnabled(enabled);
            if(enabled)
                NetworkPlayerControllers[uid].TeleportTo(pos);
        }
        public static void SetPlayerEnabled(long uid, bool enabled)
        {
            NetworkPlayerControllers[uid].SetPlayerEnabled(enabled);
        }
        
        public static void MovePlayerToPos(long uid, Vector2 pos)
        {
            if (NetworkPlayerControllers[uid])
            {
                NetworkPlayerControllers[uid].MoveTo(pos);
                if (ReceivedPositionDebug != null)
                    ReceivedPositionDebug.transform.localPosition = new Vector3(pos.x, pos.y, 0);
            }
        }

        public static void DestroyUID(long uid)
        {
            GameObject go = NetworkInfos[uid].gameObject;
            Destroy(go);
            
            NetworkInfos.Remove(uid);
            if (NetworkPlayerControllers[uid])
            {
                NetworkPlayerControllers.Remove(uid);
            }
        }
    }
}

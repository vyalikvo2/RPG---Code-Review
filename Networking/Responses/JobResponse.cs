using SocketIO;
using SP.Core.Data;
using SP.Networking.Requests;
using UnityEngine;

namespace SP.Networking.Responses
{
    public static class JobResponse
    {
        
// -------------------------------------------------------------------------------------------------------- 
        public static void Process(SocketIOEvent ev)
        {
            if (ResponseValidator.IsErrorResponse(ev)) return;

            int actionId = NetworkClient.GetResponseActionId(ev);

            switch (actionId)
            {
                case JobReqAction.CHOOSE_WORK:
                    _processChoosedJob(NetworkClient.GetResponseParams(ev));
                    break;
                case JobReqAction.BEGIN_WORK:
                    _processBeginWork(NetworkClient.GetResponseParams(ev));
                    break;
                case JobReqAction.SALARY:
                    _processSalary(NetworkClient.GetResponseParams(ev));
                    break;
                default:
                    Asshole.Error("Wrong action id");
                    break;
            }
        }
        
        // -------------------------------------------------------------------------------------------------------- 
        private static void _processChoosedJob(JSONObject data)
        {
            int jobId = Mathf.CeilToInt(data["id"].f);
            JobData.SetJobId(jobId);
        }
        // -------------------------------------------------------------------------------------------------------- 
        private static void _processBeginWork(JSONObject data)
        {
            long timeBegin = long.Parse(data["time"].ToString2());

            int newEnergy = Mathf.FloorToInt(data["energy"].f);
  
            JobData.Time = timeBegin;

            GamePlayerStats gStats = new GamePlayerStats();
            gStats.energy = new RangeStat(newEnergy);
            
            Debug.Log("received energy "+newEnergy);
            
            PlayerStatsData.UpdateGamePlayerStats(gStats);
        }
        // -------------------------------------------------------------------------------------------------------- 
        private static void _processSalary(JSONObject data)
        {
            int newExp = Mathf.CeilToInt(data["job"]["exp"].f);
            
            GamePlayerStats newStats = new GamePlayerStats();
            
            newStats.money = RangeStat.fromJSON(data["money"]);
            
            PlayerStatsData.UpdateGamePlayerStats(newStats);
            
            JobData.Time = 0;
            JobData.SetJobExp(newExp);
            
            //TODO: User money update
        }
    }
}
using System;
using SP.Networking.Constants;

namespace SP.Networking.Requests
{
    public static class JobRequest
    {
        public static void Send(int actionId, object jobParams = null)
        {
            NetworkClient.SEND_ACTION(CL.JOB, actionId, jobParams);
        }
        
    }
    
// --------------------------------------------------------------------------------------------------------   
    [Serializable]
    public class JobReqParams
    {
        public int id;

        public JobReqParams(int vacancy_id)
        {
            id = vacancy_id;
        }
    }
    
// -------------------------------------------------------------------------------------------------------- 
    public static class JobReqAction
    {
        public const int CHOOSE_WORK = 1;
        public const int BEGIN_WORK = 2;
        public const int SALARY = 3;
    }
}

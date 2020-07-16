using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using NetworkClient = SP.Networking.NetworkClient;

namespace SP.Core.Data
{
    public class JobData
    {
        public static string JSON_URL { get { return "http://"+ (NetworkClient.UseLocalServer ? "localhost:5051" : "xn-----6kcbb0azqkn5akhg5f.xn--p1ai") + "/jobs.json"; }} 
       
        public static bool IsLoaded = false;
        
        public static Dictionary<int, JobData> AllJobsDict;
        public static List<JobData> AllJobsList;
        
        // ------------------------------------------------------------------------------------
        // Player current job data

        private static int _jobId;
        public static int JobId // current job id
        {
            set { if (_jobId == value) return;  _jobId = value;
                if (OnCurrentJobUpdated != null) OnCurrentJobUpdated();
            } 
            get { return _jobId; }
        } // current job id
        
        private static int _jobLevel;
        public static int JobLevel // current job level
        {
            set { if (_jobLevel == value) return; _jobLevel = value;
                if (OnCurrentJobUpdated != null) OnCurrentJobUpdated();
            } 
            get { return _jobLevel; }
        } 
        
        private static bool _isJobMaxLevel;
        public static bool IsJobMaxLevel // current job level
        {
            set { if (_isJobMaxLevel == value) return; _isJobMaxLevel = value;
                if (OnCurrentJobUpdated != null) OnCurrentJobUpdated();
            } 
            get { return _isJobMaxLevel; }
        } 
        
        private static int _jobTotalExp; 
        public static int JobTotalExp // current job total exp 
        {
            set { if (_jobTotalExp == value) return; _jobTotalExp = value;
                if (OnCurrentJobUpdated != null) OnCurrentJobUpdated();
            } 
            get { return _jobTotalExp; }
        } 
        
        private static int _jobExp; 
        public static int JobExp // current job level exp 
        {
            set { if (_jobExp == value) return; _jobExp = value;
                if (OnCurrentJobUpdated != null) OnCurrentJobUpdated();
            } 
            get { return _jobExp; }
        } 
        
        private static int _jobExpNext; 
        public static int JobExpNext 
        {
            set { if (_jobExpNext == value) return; _jobExpNext = value;
                if (OnCurrentJobUpdated != null) OnCurrentJobUpdated();
            } 
            get { return _jobExpNext; }
        } // next level job exp
        
        private static int _jobMoney; 
        public static int JobMoney // current job money
        {
            set { if (_jobMoney == value) return; _jobMoney = value;
                if (OnCurrentJobUpdated != null) OnCurrentJobUpdated();
            } 
            get { return _jobMoney; }
        }
        
        private static int _jobMoneyNext; 
        public static int JobMoneyNext  // next level job money 
        {
            set { if (_jobMoneyNext == value) return;  _jobMoneyNext = value;
                if (OnCurrentJobUpdated != null) OnCurrentJobUpdated();
            } 
            get { return _jobMoneyNext; }
        }
        
        private static int _jobEnergy;
        public static int JobEnergy  // job energy need
        {
            set { if (_jobEnergy == value) return; _jobEnergy = value;
                if (OnCurrentJobUpdated != null) OnCurrentJobUpdated();
            } 
            get { return _jobEnergy; }
        }
        
        private static int _jobDuration; 
        public static int JobDuration // job duration 
        {
            set { if (_jobDuration == value) return; _jobDuration = value;
                if (OnCurrentJobUpdated != null) OnCurrentJobUpdated();
            } 
            get { return _jobDuration; }
        }

        private static int _jobDayExp; 
        public static int JobDayExp // job duration 
        {
            set { if (_jobDayExp == value) return; _jobDayExp = value;
                if (OnCurrentJobUpdated != null) OnCurrentJobUpdated();
            } 
            get { return _jobDayExp; }
        }
        
        public delegate void JobUpdateEvent();
        public static event JobUpdateEvent OnCurrentJobUpdated;
        
        private static long _time;
        public static long Time // current job start time (0 if not started)
        {
            set { if (_jobDayExp == value) return; _time = value;
                if (OnCurrentJobUpdated != null) OnCurrentJobUpdated();
            } 
            get { return _time; }
        } 
        
        // ------------------------------------------------------------------------------------
        // static job methods to update params
        public static void SetJobId(int id)
        {
            if (id != _jobId)
            {
                _jobTotalExp = 0;
                _jobId = id;
                _recalculateJobData();
            }
        }
        

        public static void SetJobExp(int exp)
        {
            _jobTotalExp = exp;
            _recalculateJobData();
        }
        

        public static int GetLevelByExp(int exp, JobData jobData)
        {
            int curLevel = jobData.Levels.Length - 1;
            for (int i = jobData.Levels.Length - 1; i >= 0; i--)
            {
                curLevel = i;
                if (exp >= jobData.Levels[i].e) break;
               
            }

            return curLevel;
        }

        public static void InitJSONData(JSONObject data)
        {
            _jobId = Mathf.FloorToInt(data["id"].f);
            _jobTotalExp = Mathf.FloorToInt(data["exp"].f);
            _time = long.Parse(data["timestamp"].ToString2());
            _jobDayExp = Mathf.FloorToInt(data["dexp"].f);
            
            _recalculateJobData();
        }
        
        private static void _recalculateJobData()
        {
            if (!IsLoaded)
            {
                Debug.Log("Recalculate NOT LOADED jobs");
                return;
            }
            
            JobData job = AllJobsDict[JobId];

            int curLevel = GetLevelByExp(_jobTotalExp, job);
            
            _jobLevel = curLevel;
            
            int nextLevel = (curLevel + 1) % job.Levels.Length;
            
            if (nextLevel == 0)
            {
                _isJobMaxLevel = true;
            }
            else
            {
                _isJobMaxLevel = false;
                _jobExpNext = job.Levels[nextLevel].e - job.Levels[curLevel].e;
            }
            
            _jobExp = _jobTotalExp - job.Levels[curLevel].e;
            
            _jobMoney = job.Levels[curLevel].m;
            _jobMoneyNext = job.Levels[nextLevel].m;

            _jobDuration = job.Duration;
            _jobEnergy = job.Energy;
            
            if (OnCurrentJobUpdated != null) OnCurrentJobUpdated();
        }
        
        // ------------------------------------------------------------------------------------
        // single job data
        public int Id;
        public int Duration;
        public int Energy;
        public int IconId;

        public JParJobLevel[] Levels;
        
        public JobData(int id, int iconId, int duration, int energy, JParJobLevel[] levels)
        {
            Id = id;
            IconId = iconId;
            Duration = duration;
            Energy = energy;
            Levels = levels;
        }
    }
    
    
    // ------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------
    // json parse classes
    // ------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------
    [Serializable]
    public class JParJobItems
    {
        public JParJobItem[] items;
    }
    
    [Serializable]
    public class JParJobItem
    {
        public int id;
        public int i;
        public JParJobLevel[] levels;
        public int duration;
        public int energy;
    }
    
    [Serializable]
    public class JParJobLevel
    {
        public int e; // experience
        public int m; // money
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using SP.Core.Data;
using SP.Utils.Attributes;
using UnityEngine;
using UnityEngine.Networking;

public class InitDataLoader : MonoBehaviour
{
    [SerializeField] [Header("Class that loads all data before socket connection:")][DisableEdit]
    public string _ = "";

    public delegate void OnAllDataLoadedDelegate();
    public static event OnAllDataLoadedDelegate onAllDataLoaded;

    private static List<InitDataLoaderBase> _loadActions = new List<InitDataLoaderBase>();
    private static int _loadedCount = 0;

    public static InitDataLoader instance;

    public InitDataLoader()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    

    public void LoadAllData()
    {
        _loadActions.Add(new InitDataLoader_JobData());
        
        StartCoroutine(_loadActions[_loadedCount].Load());
    }

    public void DataPackLoaded()
    {
        _loadedCount++;
        if (_loadedCount >= _loadActions.Count)
        {
            if (onAllDataLoaded != null)
            {
                onAllDataLoaded();
                _clean();
            }
        }
        else
        {
            StartCoroutine(_loadActions[_loadedCount].Load());
        }
        
    }

    private void _clean()
    {
        instance = null;
        enabled = false;
    }
    
        
    // ------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------
    // INIT DATA LOADER
    // ------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------

    public class InitDataLoaderBase
    {
        public virtual IEnumerator Load()
        {
            yield return new WaitForSeconds(0); // override it
        }

        public virtual void OnLoaded()
        {
            InitDataLoader.instance.DataPackLoaded();
        }
    }

    public class InitDataLoader_JobData : InitDataLoaderBase
    {
        
        // ------------------------------------------------------------------------------------
        public override IEnumerator Load()
        {
            JobData.AllJobsDict = new Dictionary<int, JobData>();
            JobData.AllJobsList = new List<JobData>();
            
            UnityWebRequest www = UnityWebRequest.Get(JobData.JSON_URL);
            yield return www.SendWebRequest();
            if (www.error == null)
            {
                //Debug.Log(www.downloadHandler.text);
                JParJobItems parsed = JsonUtility.FromJson<JParJobItems>(www.downloadHandler.text);

                for (int i = 0; i < parsed.items.Length; i++)
                {
                    JParJobItem jsonJob = parsed.items[i];
                    JobData job = new JobData(jsonJob.id, jsonJob.i, jsonJob.duration, jsonJob.energy, jsonJob.levels);
                    JobData.AllJobsDict[job.Id] = job;
                    JobData.AllJobsList.Add(job);

                }
                
                JobData.IsLoaded = true;

                Ass.Log("Loaded Jobs: " + JobData.AllJobsList.Count);
            }
            else
            {
                Ass.Log("ERROR LOADING JOBS: " + www.error);
            }

            OnLoaded();
        }
    }
    
}

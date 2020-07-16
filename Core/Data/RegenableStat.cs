using System;
using System.Collections.Generic;
using SP.Constants;
using SP.Utils;
using UnityEngine;

namespace SP.Core.Data
{

    // class that can autoregen value (see SetAutoUpdate method)
    
    public class RegenableStat
    {
        public int current;
        public int max;
        public int regen;

        private long _lastUseTime = 0;
        
        public float regenDelay = 0;
        private bool _isUsing = false; // regen starts after setting isUsing to false (with delay)

        public bool isUsing
        {
            get { return _isUsing; }
            set
            {
                _isUsing = value;
                if (_isUsing)
                {
                    _lastUseTime = Timestamp.Get();
                }
            }
        }

        private bool _autoUpdate = false;

        public bool autoUpdate
        {
            get { return _autoUpdate; }
        }

        private static List<RegenableStat> _updateList = new List<RegenableStat>();

        public static void Update(float deltaTime)
        {
            long curTime = Timestamp.Get();
            for (int i = 0; i < _updateList.Count; i++)
            {
                RegenableStat stat = _updateList[i];
                
                if (!stat.isUsing && curTime - stat._lastUseTime > stat.regenDelay && stat.current < stat.max &&
                    stat.regen > 0)
                {
                    // regen in percent
                    stat.current =
                        Math.Min(Mathf.FloorToInt(stat.current + deltaTime * (stat.regen * GameConstants.REGEN_TO_SECONDS) * stat.max), stat.max);
                }
            }
        }

        // returns true if have current value > 0 after descending
        public bool Use(int useValue)
        {
            isUsing = true;
            current = Math.Max(0, current - useValue);
            return current != 0;
        }

        public RegenableStat(int current = -1, int max = -1, int regen = -1)
        {
            this.current = current;
            this.max = max;
            this.regen = regen;
        }

        public bool IsEqual(RegenableStat stats)
        {
            return stats.current == current &&
                   stats.max == max &&
                   stats.regen == regen;
        }

        public static RegenableStat fromJSON(JSONObject json)
        {
            int current = json.HasField("current") ? Mathf.FloorToInt(json["current"].f) : -1;
            int max = json.HasField("max") ? Mathf.FloorToInt(json["max"].f) : -1;
            int regen = json.HasField("regen") ? Mathf.FloorToInt(json["regen"].f) : -1;

            return new RegenableStat(current, max, regen);
        }

        public void _updateStats(RegenableStat newStats)
        {
            if (newStats.current != -1)
                current = newStats.current;
            if (newStats.max != -1)
                max = newStats.max;
            if (newStats.regen != -1)
                regen = newStats.regen;
        }

        public void SetAutoUpdate(bool update = true)
        {
            if (update != _autoUpdate)
            {
                _autoUpdate = update;
                if (_autoUpdate)
                {
                    _updateList.Add(this);
                }
                else
                {
                    _updateList.Remove(this);
                }
            }
        }
        
    }
}
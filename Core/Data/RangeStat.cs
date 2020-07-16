using System;
using System.Collections.Generic;
using SP.Constants;
using SP.Utils;
using UnityEngine;

namespace SP.Core.Data
{

    // class that can autoregen value (see SetAutoUpdate method)
    
    public class RangeStat
    {
        public int current;
        public int max;
        
        public RangeStat(int current = -1, int max = -1)
        {
            this.current = current;
            this.max = max;
        }

        public bool IsEqual(RangeStat stat)
        {
            return stat.current == current &&
                   stat.max == max;
        }

        public static RangeStat fromJSON(JSONObject json)
        {
            int current = json.HasField("current") ? Mathf.FloorToInt(json["current"].f) : -1;
            int max = json.HasField("max") ? Mathf.FloorToInt(json["max"].f) : -1;
            
            return new RangeStat(current, max);
        }

        public void _updateStats(RangeStat newStat)
        {
            if (newStat.current != -1)
                current = newStat.current;
            if (newStat.max != -1)
                max = newStat.max;
        }
        
    }
}
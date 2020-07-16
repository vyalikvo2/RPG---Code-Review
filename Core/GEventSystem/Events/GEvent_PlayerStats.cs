using System;
using System.Collections.Generic;
using SP.Core.Data;
using SP.Core.World;

namespace SP.Core.GEventSystem
{
    // -------------------------------------------------------------------------------------------------
    public class GEvent_AllPlayerStats_Updated
    {
        private readonly List<Action<GEventData_AllPlayerStats>> _callbacks = new List<Action<GEventData_AllPlayerStats>>();

        public void Subscribe(Action<GEventData_AllPlayerStats> callback)
        {
            _callbacks.Add(callback);
        }
        
        public void UnSubscribe(Action<GEventData_AllPlayerStats> callback)
        {
            _callbacks.Remove(callback);
        }

        public void Invoke(GEventData_AllPlayerStats data)
        {
            foreach (Action<GEventData_AllPlayerStats> callback in _callbacks)
                callback(data);
        }
    }
    
    // -------------------------------------------------------------------------------------------------
    public class GEvent_GamePlayerStats_Updated
    {
        private readonly List<Action<GEventData_GamePlayerStats>> _callbacks = new List<Action<GEventData_GamePlayerStats>>();

        public void Subscribe(Action<GEventData_GamePlayerStats> callback)
        {
            _callbacks.Add(callback);
        }
        
        public void UnSubscribe(Action<GEventData_GamePlayerStats> callback)
        {
            if(_callbacks.Contains(callback))
                _callbacks.Remove(callback);
        }

        public void Invoke(GEventData_GamePlayerStats data)
        {
            foreach (Action<GEventData_GamePlayerStats> callback in _callbacks)
                callback(data);
        }
    }
    
    // -------------------------------------------------------------------------------------------------
    public class GEvent_BasicPlayerStats_Updated
    {
        private readonly List<Action<GEventData_BasicPlayerStats>> _callbacks = new List<Action<GEventData_BasicPlayerStats>>();

        public void Subscribe(Action<GEventData_BasicPlayerStats> callback)
        {
            _callbacks.Add(callback);
        }
        
        public void UnSubscribe(Action<GEventData_BasicPlayerStats> callback)
        {
            _callbacks.Remove(callback);
        }

        public void Invoke(GEventData_BasicPlayerStats data)
        {
            foreach (Action<GEventData_BasicPlayerStats> callback in _callbacks)
                callback(data);
        }
    }
    
    // -------------------------------------------------------------------------------------------------
    public class GEvent_AdvancedPlayerStats_Updated
    {
        private readonly List<Action<GEventData_AdvancedPlayerStats>> _callbacks = new List<Action<GEventData_AdvancedPlayerStats>>();

        public void Subscribe(Action<GEventData_AdvancedPlayerStats> callback)
        {
            _callbacks.Add(callback);
        }
        
        public void UnSubscribe(Action<GEventData_AdvancedPlayerStats> callback)
        {
            _callbacks.Remove(callback);
        }

        public void Invoke(GEventData_AdvancedPlayerStats data)
        {
            foreach (Action<GEventData_AdvancedPlayerStats> callback in _callbacks)
                callback(data);
        }
    }
    
    // -------------------------------------------------------------------------------------------------
    // -----------------  DATA  ------------------------------------------------------------------------
    // -------------------------------------------------------------------------------------------------
    
    public class GEventData_AllPlayerStats
    {
        public long uid;
        public PlayerStatsData playerStats;

        public GEventData_AllPlayerStats(long uid, PlayerStatsData gamePlayerStats)
        {
            this.uid = uid;
            this.playerStats = gamePlayerStats;
        }
    }
    
    // -------------------------------------------------------------------------------------------------
    public class GEventData_GamePlayerStats
    {
        public long uid;
        public GamePlayerStats gamePlayerStats;

        public GEventData_GamePlayerStats(long uid, GamePlayerStats gamePlayerStats)
        {
            this.uid = uid;
            this.gamePlayerStats = gamePlayerStats;
        }
    }
    
    // -------------------------------------------------------------------------------------------------
    public class GEventData_BasicPlayerStats
    {
        public long uid;
        public BasicPlayerStats basicPlayerStats;

        public GEventData_BasicPlayerStats(long uid, BasicPlayerStats basicPlayerStats)
        {
            this.uid = uid;
            this.basicPlayerStats = basicPlayerStats;
        }
    }
    
    // -------------------------------------------------------------------------------------------------
    public class GEventData_AdvancedPlayerStats
    {
        public long uid;
        public AdvancedPlayerStats advancedPlayerStats;

        public GEventData_AdvancedPlayerStats(long uid, AdvancedPlayerStats advancedPlayerStats)
        {
            this.uid = uid;
            this.advancedPlayerStats = advancedPlayerStats;
        }
    }
    
}
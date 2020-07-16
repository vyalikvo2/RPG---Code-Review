using System;
using System.Collections.Generic;
using SP.Constants;
using SP.Core.GEventSystem;
using SP.Networking;
using SP.Utils;
using UnityEngine;

namespace SP.Core.Data
{
    public class PlayerStatsData
    {
        // ------------------------------------------------------- static methods
        private static Dictionary<long, PlayerStatsData> _allStats = new Dictionary<long, PlayerStatsData>();
        public static Dictionary<long, PlayerStatsData> AllStats { get { return _allStats; } }

        public static PlayerStatsData Me
        {
            get
            {
                AllStats.TryGetValue(NetworkClient.ClientId, out var player);
                return player;
            }
        }

        public static PlayerStatsData fromJSON(JSONObject json)
        {
            JSONObject stats = json["stats"];
            
            GamePlayerStats gameStats = null;
            BasicPlayerStats basicStats = null;
            AdvancedPlayerStats advancedStats = null;
            
            if (stats)
            {
                gameStats = stats["game"] ? GamePlayerStats.fromJSON(stats["game"]) : null;
                basicStats = stats["basic"] ? BasicPlayerStats.fromJSON(stats["basic"]) : null;
                advancedStats = stats["advanced"] ? AdvancedPlayerStats.fromJSON(stats["advanced"]) : null;
            }
         
            PlayerStatsData data = new PlayerStatsData(gameStats, basicStats, advancedStats);
            
            return data;
        }

        public static void SetPlayerStats(PlayerStatsData playerStats)
        {
            SetPlayerStats(NetworkClient.ClientId, playerStats);
        }
        public static void SetPlayerStats(long uid, PlayerStatsData playerStats)
        {
            _allStats[uid] = playerStats;

            // send events
            GEvent.UPDATED_STATS_AllPlayerStats.Invoke(new GEventData_AllPlayerStats(uid, playerStats));
            
            GEvent.UPDATED_STATS_GamePlayerStats.Invoke(new GEventData_GamePlayerStats(uid, playerStats.game));
            GEvent.UPDATED_STATS_BasicPlayerStats.Invoke(new GEventData_BasicPlayerStats(uid, playerStats.basic));
            GEvent.UPDATED_STATS_AdvancedPlayerStats.Invoke(new GEventData_AdvancedPlayerStats(uid, playerStats.advanced));
        }

        // ------------------------------------------------------------------------------------
        public static void UpdateGamePlayerStats(GamePlayerStats gameStats)
        {
            UpdateGamePlayerStats(NetworkClient.ClientId, gameStats);
        }
        
        public static void UpdateGamePlayerStats(long uid, GamePlayerStats gameStats)
        {
            if (_allStats.ContainsKey(uid))
            {
                GamePlayerStats oldGameStats = _allStats[uid].game;
                if (oldGameStats != null && !oldGameStats.IsEqual(gameStats))
                {
                    _allStats[uid].game._updateStats(gameStats);
                    
                    // send event
                    GEventData_GamePlayerStats eventData = new GEventData_GamePlayerStats(uid, _allStats[uid].game);
                    GEvent.UPDATED_STATS_GamePlayerStats.Invoke(eventData);
                }
            }
        }
        
        // ------------------------------------------------------------------------------------
        public static void UpdateBasicPlayerStats(long uid, BasicPlayerStats basicStats)
        {
            if (_allStats.ContainsKey(uid))
            {
                BasicPlayerStats oldBasicStats = _allStats[uid].basic;
                if (oldBasicStats != null && !oldBasicStats.IsEqual(basicStats))
                {
                    _allStats[uid].basic._updateStats(basicStats);
                    
                    // send event
                    GEventData_BasicPlayerStats eventData = new GEventData_BasicPlayerStats(uid, _allStats[uid].basic);
                    GEvent.UPDATED_STATS_BasicPlayerStats.Invoke(eventData);
                }
            }
        }

        public static void UpdateAdvancedPlayerStats(long uid, AdvancedPlayerStats advancedStats)
        {
            if (_allStats.ContainsKey(uid))
            {
                AdvancedPlayerStats oldAdvancedStats = _allStats[uid].advanced;
                if (oldAdvancedStats != null && !oldAdvancedStats.IsEqual(advancedStats))
                {
                    _allStats[uid].advanced._updateStats(advancedStats);
                    
                    // send event
                    GEventData_AdvancedPlayerStats eventData = new GEventData_AdvancedPlayerStats(uid, _allStats[uid].advanced);
                    GEvent.UPDATED_STATS_AdvancedPlayerStats.Invoke(eventData);
                }
            }
        }
        
        // ------------------------------------------------------- 
        // ------------------------------------------------------- class variables
        // ------------------------------------------------------- 
        
        private GamePlayerStats _game; // runtime updateable stats
        public GamePlayerStats game { get { return _game; } } 
        
        private BasicPlayerStats _basic; 
        public BasicPlayerStats basic { get { return _basic; } } 
        
        private AdvancedPlayerStats _advanced;
        public AdvancedPlayerStats advanced { get { return _advanced; } }
        
        public PlayerStatsData(GamePlayerStats __game = null,BasicPlayerStats __basic = null, AdvancedPlayerStats __advanced = null)
        {
            if(__game!=null) this._game = __game;
            if(__basic!=null) this._basic = __basic;
            if(__advanced!=null) this._advanced = __advanced;
        }
    }
    
    // ------------------------------------------------------------------------------------
    // -------------- DATA CLASSES --------------------------------------------------------
    // ------------------------------------------------------------------------------------
    public class GamePlayerStats
    {
        public int gold;
        public RangeStat energy;
        public RangeStat money;
        public RegenableStat health;
        public RegenableStat endurance;

        public GamePlayerStats(RegenableStat health = null, RegenableStat endurance = null, int gold = -1, RangeStat money = null, RangeStat energy = null)
        {
            if(health!=null)
                this.health = health;
            if(endurance!=null)
                this.endurance = endurance;
            
            this.gold = gold;
            
            if(money!=null)
                this.money = money;
            if(energy!=null)
                this.energy = energy;
        }
        
        public bool IsEqual(GamePlayerStats stats)
        {
            return !((stats.health !=null && !stats.health.IsEqual(health)) ||
                   (stats.endurance!=null && !stats.endurance.IsEqual(endurance)) ||
                   (stats.money!=null && !stats.money.IsEqual(money)) ||
                   (stats.energy!=null && !stats.energy.IsEqual(energy)) ||
                    (stats.gold!=-1 && stats.gold != gold));
        }

        public void _updateStats(GamePlayerStats newStats)
        {
            if (newStats.health != null)
                health._updateStats(newStats.health);
            if(newStats.endurance != null)
                endurance._updateStats(newStats.endurance);
            if (newStats.gold != -1)
                gold = newStats.gold;
            if(newStats.money != null)
                money._updateStats(newStats.money);
            if(newStats.energy != null)
                energy._updateStats(newStats.energy);
        }

        public static GamePlayerStats fromJSON(JSONObject json)
        {
            RegenableStat health = json["health"] ? RegenableStat.fromJSON(json["health"]) : null;
            RegenableStat endurance = json["endurance"] ? RegenableStat.fromJSON(json["endurance"]) : null;
            int gold = json.HasField("gold") ? Mathf.FloorToInt(json["gold"].f) : -1;
            RangeStat money = json["money"] ? RangeStat.fromJSON(json["money"]) : null;
            RangeStat energy = json["energy"] ? RangeStat.fromJSON(json["energy"]) : null;
            
            return new GamePlayerStats(health, endurance, gold, money, energy);
        }
    }
    
    // ------------------------------------------------------------------------------------
    public class BasicPlayerStats
    {
        public int strength;
        public int agility;
        public int intuition;
        public int intelligence;
        public int endurance;

        public BasicPlayerStats(int strength = -1, int agility = -1, int intuition = -1, int intelligence = -1, int endurance = -1)
        {
            this.strength = strength;
            this.agility = agility;
            this.intuition = intuition;
            this.intelligence = intelligence;
            this.endurance = endurance;
        }

        public bool IsEqual(BasicPlayerStats stats)
        {
            return !((stats.strength!=-1 && stats.strength == strength) ||
                   (stats.agility!=-1 && stats.agility == agility) ||
                   (stats.intuition!=-1 && stats.intuition == intuition) ||
                   (stats.intelligence!=-1 && stats.intelligence == intelligence) ||
                    (stats.endurance!=-1 && stats.endurance == endurance));
        }
        
        public void _updateStats(BasicPlayerStats newStats)
        {
            if(newStats.strength!=-1)
                strength = newStats.strength;
            if(newStats.agility!=-1)
                agility = newStats.agility;
            if(newStats.intuition!=-1)
                intuition = newStats.intuition;
            if(newStats.intelligence!=-1)
                intelligence = newStats.intelligence;
            if(newStats.endurance!=-1)
                endurance = newStats.endurance;
        }
        
        public static BasicPlayerStats fromJSON(JSONObject json)
        {
            int strength = json.HasField("strength") ? Mathf.FloorToInt(json["strength"].f) : -1;
            int agility = json.HasField("agility") ? Mathf.FloorToInt(json["agility"].f) :  -1;
            int intuition = json.HasField("intuition") ? Mathf.FloorToInt(json["intuition"].f) : -1;
            int intelligence = json.HasField("intelligence") ? Mathf.FloorToInt(json["intelligence"].f) :  -1;
            int endurance = json.HasField("endurance") ? Mathf.FloorToInt(json["endurance"].f) : -1;
            
            return new BasicPlayerStats(strength, agility,intuition, intelligence, endurance);
        }
    }
    
    // ------------------------------------------------------------------------------------
    public class AdvancedPlayerStats
    {
        public int moveSpeed;

        public AdvancedPlayerStats(int moveSpeed = -1)
        {
            this.moveSpeed = moveSpeed;
        }
        
        public bool IsEqual(AdvancedPlayerStats stats)
        {
            return stats.moveSpeed == moveSpeed;
        }
        
        public void _updateStats(AdvancedPlayerStats newStats)
        {
            if(newStats.moveSpeed!=-1)
                moveSpeed = newStats.moveSpeed;
        }
        
        public static AdvancedPlayerStats fromJSON(JSONObject json)
        {
            int moveSpeed =json.HasField("moveSpeed") ? Mathf.FloorToInt(json["moveSpeed"].f) : -1;

            return new AdvancedPlayerStats(moveSpeed);
        }
    }
}

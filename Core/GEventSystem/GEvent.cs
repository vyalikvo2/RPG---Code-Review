
namespace SP.Core.GEventSystem
{
    public static class GEvent
    {
        public static readonly GEvent_WActionButtonClicked UI_BUTTON_CLICKED_WAction = new GEvent_WActionButtonClicked();

        public static readonly GEvent_QuestPlaceEnter TRIGGER_ENTER_QuestPlace = new GEvent_QuestPlaceEnter();
        public static readonly GEvent_QuestPlaceLeave TRIGGER_LEAVE_QuestPlace = new GEvent_QuestPlaceLeave();
        
        public static readonly GEvent_Void STAT_BECOME_ZERO_Endurance = new GEvent_Void();
        
        public static readonly GEvent_AllPlayerStats_Updated UPDATED_STATS_AllPlayerStats = new GEvent_AllPlayerStats_Updated();
        public static readonly GEvent_GamePlayerStats_Updated UPDATED_STATS_GamePlayerStats = new GEvent_GamePlayerStats_Updated();
        public static readonly GEvent_BasicPlayerStats_Updated UPDATED_STATS_BasicPlayerStats = new GEvent_BasicPlayerStats_Updated();
        public static readonly GEvent_AdvancedPlayerStats_Updated UPDATED_STATS_AdvancedPlayerStats = new GEvent_AdvancedPlayerStats_Updated();
    }

}

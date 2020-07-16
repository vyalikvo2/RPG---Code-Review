using SP.Core.Data;
using SP.Core.GEventSystem;
using SP.Core.StateMachine;
using SP.Core.UIScreens;
using SP.Localization;
using SP.Networking.Requests;
using UnityEngine;

namespace SP.Core.World
{
    public class UIScreenController : MonoBehaviour
    {
        public void Awake()
        {
            GEvent.UI_BUTTON_CLICKED_WAction.Subscribe(OnWActionButtonClicked);

            GEvent.STAT_BECOME_ZERO_Endurance.Subscribe(OnEnduranceEnded);
        }

        private void OnEnduranceEnded()
        {
            PlayerMessagesHolder.instance?.OnPlayerSay( LocalizationManager.Localize(Random.Range(0 ,1) == 0 ? "cant_run_anymore" : "fuh"));
        }
        
        private void OnWActionButtonClicked(GEventData_WActionButton data)
        {
            if(data.button.gActionId == GActionId.WINDOW_OPEN_WORK)
            {
                UIStateMachine.instance?.Reload(JobData.JobId > 0 ? ScreenNames.CurrentJob : ScreenNames.JobManagement);
            }
        }
    }
}


using SP.Core.Data;
using SP.Core.StateMachine;
using SP.Networking.Requests;
using SP.Utils;
using SP.Views;
using UnityEngine;

namespace SP.Core.UIScreens.JobManagementScripts
{
    public class JobInProgress : ViewJobInProgress
    {

        private bool _sendedEndWork = false; 
        private void Awake()
        {
            Name = ScreenNames.JobInProgress;
        }

        public override void Show()
        {
            base.Show();
            _initValues();
        }

        public override void Hide()
        {
            base.Hide();
        }

        private void _initValues()
        {
            _sendedEndWork = false;
            LabelReward.text = "+"+JobData.JobMoney + "$";
        }

        public void FixedUpdate()
        {
            if (!isShowed) return;
            
            long timePassed = Timestamp.Get() - JobData.Time;
            int timeLeft = Mathf.FloorToInt(JobData.JobDuration - timePassed);
   
            ImageMask.fillAmount = (timePassed + 0.0f) / (JobData.JobDuration + 0.0f);
            LabelProgress.text = "".MillisecondsToFormattedTime(timeLeft, MsFormatTime.FORMAT_00_00);

            if (timeLeft <= 0 && !_sendedEndWork)
            {
                _sendedEndWork = true;
                UIStateMachine.instance?.Reload(ScreenNames.CurrentJob);
                // enable our player to others
                UpdatePlayerRequest.Send(UpdatePlayerReqAction.UPDATE_ENABLED, new UpdatePlayerReqParamsEnabled(true));
            }
        }
        
    }
}
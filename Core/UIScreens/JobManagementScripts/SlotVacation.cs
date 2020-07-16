using System.Linq;
using SP.Core.Data;
using SP.Localization;
using SP.Networking.Requests;
using SP.Utils;
using SP.Views;
using UnityEngine;
using UnityEngine.UI;

namespace SP.Core.UIScreens.JobManagementScripts
{
    public class SlotVacation : ViewSlotVacation
    {
        private int _jobId;

        private bool _selected;

        void Awake()
        {
            ButtonChoose.onClick.AddListener(_onChooseButtonClick);
        }

        private void _onChooseButtonClick()
        {
            JobRequest.Send(JobReqAction.CHOOSE_WORK, new JobReqParams(_jobId));
        }

        public int GetJobId()
        {
            return _jobId;
        }
        
        public void Init(int jobId)
        {
            _jobId = jobId;
            var job = JobData.AllJobsList.First(j => j.Id == jobId);
            // LabelName.text = LocalizationManager.Localize(job.)
            var sprite = Resources.Load<Sprite>($"job_{job.IconId}");
            ImageVacancyIcon.sprite = sprite;
            if (sprite == null)
                Debug.LogError($"Sprite is null job_{job.IconId}");

            LabelName.GetComponent<LocalizedText>().localeKey = "job_" + job.Id + "_title";
            LabelEnergy.text = job.Energy + "";
            LabelProfit.text = "+"+job.Levels[0].m + "$";
            LabelTime.text =  "".MillisecondsToFormattedTime(job.Duration);

            LabelEnergy.RecalculateRectTransform();
            LabelProfit.RecalculateRectTransform();
            LabelTime.RecalculateRectTransform();
            
            SetSelected(false, true);
        }

        public void SetSelected(bool selected, bool forceUpdate = false)
        {
            if (_selected == selected && !forceUpdate) return;
            
            ImageSelectedBg.SetShow(selected);
            ImageSelectedIconBg.SetShow(selected);
            ImageSelectedIconTop.SetShow(selected);
            
            ButtonChoose.gameObject.SetShow(!selected);
            LabelCurrent.enabled = selected;

            _selected = selected;
        }
        
    }
}

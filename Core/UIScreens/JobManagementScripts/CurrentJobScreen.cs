using System.Collections.Generic;
using SP.Core.Data;
using SP.Core.StateMachine;
using SP.Localization;
using SP.Networking.Requests;
using SP.Utils;
using SP.Views;
using UnityEngine;

namespace SP.Core.UIScreens.JobManagementScripts
{
    public class CurrentJobScreen : ViewCurrentJob
    {
        private int _prevFillExp = 0;

        private void Awake()
        {
            Name = ScreenNames.CurrentJob;
            ButtonClose.onClick.AddListener(() => UIStateMachine.instance?.Reload(ScreenNames.Main));
            ButtonWork.onClick.AddListener(_onWorkClick);
            ButtonSalary.onClick.AddListener(_onSalaryClick);
            ButtonAllJobs.onClick.AddListener(() => UIStateMachine.instance?.Reload(ScreenNames.JobManagement));
        }

        public override void Show()
        {
            base.Show();
            OnShow();

            JobData.OnCurrentJobUpdated += OnJobUpdated;
        }

        public override void Hide()
        {
            base.Hide();
        }

        private void OnShow()
        {
            UpdateValues();
        }

        private void OnJobUpdated()
        {
            UpdateValues();
            
            if (JobData.Time != 0 && Timestamp.Get()-JobData.Time<JobData.JobDuration)
            {
                UIStateMachine.instance?.Reload(ScreenNames.JobInProgress);
            }
        }

        private void UpdateValues()
        {
            
            LabelName.GetComponent<LocalizedText>().localeKey = "job_" + JobData.JobId + "_title";
            LabelTime.text = "".MillisecondsToFormattedTime(JobData.JobDuration);
            LabelProfit.text = "+" + JobData.JobMoney + "$";
            LabelExp.text = "+" + JobData.JobDayExp;
            LabelToWorkEnergy.text = "-" + JobData.JobEnergy;

            LabelSalary.text = LocalizationManager.Localize("job.salary_btn", JobData.JobMoney);
            
            LabelTime.RecalculateRectTransform();
            LabelProfit.RecalculateRectTransform();
            LabelExp.RecalculateRectTransform();
            LabelToWorkEnergy.RecalculateRectTransform();
            LabelSalary.RecalculateRectTransform();
            
            LabelLevel.text = LocalizationManager.Localize("job.level", JobData.JobLevel+1);

            if (!JobData.IsJobMaxLevel)
            { 
                LabelLevelExp.text = JobData.JobExp + "/" + JobData.JobExpNext;
                ImageProgress.fillAmount = (JobData.JobExp+0.0f) / (JobData.JobExpNext+0.0f);
            }
            else
            {
                LabelLevelExp.text = LocalizationManager.Localize("job.max_exp");
                ImageProgress.fillAmount = 1;
            }

            ButtonWork.SetShow(JobData.Time == 0);
            ButtonSalary.SetShow(Timestamp.Get() - JobData.Time > JobData.JobDuration && JobData.Time!=0);
            
            ButtonAllJobs.SetShow(JobData.Time == 0);

            PlayerStatsData playerStats = PlayerStatsData.Me;
            LabelNoEnergy.enabled = JobData.Time==0 && JobData.JobEnergy > playerStats.game.energy.current;
            ButtonWork.interactable = playerStats.game.energy.current >= JobData.JobEnergy;

            PanelStats.RecalculateRectTransform();

        }

        private void _onWorkClick()
        {
            JobRequest.Send(JobReqAction.BEGIN_WORK);
        }
        
        private void _onSalaryClick()
        {
            JobRequest.Send(JobReqAction.SALARY);
        }
        
    }
}
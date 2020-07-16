using System.Collections.Generic;
using SP.Core.Data;
using SP.Core.StateMachine;
using SP.Utils;
using SP.Views;
using UnityEngine;

namespace SP.Core.UIScreens.JobManagementScripts
{
    public class JobManagement : ViewJobManagement
    {
        private readonly List<SlotVacation> _vacations = new List<SlotVacation>();
        private readonly List<SlotNumPage> _pages = new List<SlotNumPage>();

        private int _prevJobId = -1;
        private int _pageIndex = 1;
        
        private void Awake()
        {
            Name = ScreenNames.JobManagement;
            ButtonClose.onClick.AddListener(() => UIStateMachine.instance?.Reload(ScreenNames.Main));
            ButtonBack.onClick.AddListener(() => UIStateMachine.instance?.Reload(ScreenNames.CurrentJob));
        }

        public override void Show()
        {
            base.Show();

            _prevJobId = JobData.JobId;

            JobData.OnCurrentJobUpdated += JobDataUpdated;
            OnShow();
            JobDataUpdated();
        }

        public override void Hide()
        {
            base.Hide();
            JobData.OnCurrentJobUpdated -= JobDataUpdated;
        }

        private void OnShow()
        {
            ClearDynamics();
            ShowVacations();
            ShowPages();
            JobDataUpdated();
        }
        
        private void JobDataUpdated()
        {
            ButtonBack.gameObject.SetShow(JobData.JobId > 0);
            
            foreach (var vacation in _vacations)
            {
                vacation.SetSelected(vacation.GetJobId() == JobData.JobId);
            }

            if (_prevJobId != JobData.JobId)
            {
                _prevJobId = JobData.JobId;
                UIStateMachine.instance?.Reload(ScreenNames.CurrentJob);
            }
        }

        private void ClearDynamics()
        {
            foreach (var panel in _vacations)
                Destroy(panel.gameObject);
            
            foreach (var panel in _pages)
                Destroy(panel.gameObject);
            
            _vacations.Clear();
            _pages.Clear();
        }
        
        private void ShowVacations()
        {
            SlotVacation.Hide();

            int countOnCurrentPage = 4;
            countOnCurrentPage = Mathf.Clamp(countOnCurrentPage, 1, JobData.AllJobsList.Count - _pageIndex * 4 + 4);

            var vacationsOnPage = JobData.AllJobsList.GetRange(_pageIndex * 4 - 4, countOnCurrentPage);

            foreach (var vacation in vacationsOnPage)
            {
                var card = Instantiate(SlotVacation, SlotVacation.transform.parent);
                card.Show();
                var cardView = card.GetComponent<SlotVacation>();
                _vacations.Add(cardView);
            
                cardView.Init(vacation.Id);
            }
        }
        
        private void ShowPages()
        {
            SlotNumPage.Hide();

            var pageCount = Mathf.CeilToInt(JobData.AllJobsList.Count / 4f);

            for (var i = 1; i <= pageCount; i++)
            {
                var card = Instantiate(SlotNumPage, SlotNumPage.transform.parent);
                card.Show();
                var cardView = card.GetComponent<SlotNumPage>();
                _pages.Add(cardView);
            
                cardView.FillSelected(i, i == _pageIndex, newPage =>
                {
                    _pageIndex = newPage;
                    OnShow();
                });
            }
        }
    }
}
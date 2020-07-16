// This file is code generated. Do not edit.

using System.Collections.Generic;
using SP.Core.StateMachine;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SP.Views
{
    public class ViewCurrentJob : UIScreen
    {
        // TextMeshProUGUI 
        public TextMeshProUGUI LabelLevel;
        public TextMeshProUGUI LabelLevelExp;
        public TextMeshProUGUI LabelTime;
        public TextMeshProUGUI LabelProfit;
        public TextMeshProUGUI LabelExp;
        public TextMeshProUGUI LabelNoEnergy;
        public TextMeshProUGUI LabelName;
        public TextMeshProUGUI LabelToWorkEnergy;
        public TextMeshProUGUI LabelSalary;
        // Buttons 
        public Button ButtonClose;
        public Button ButtonWork;
        public Button ButtonSalary;
        public Button ButtonAllJobs;
        // Image 
        public Image ImageProgress;
        public Image ImageVacancyIcon;
        // Panel 
        public GameObject PanelStats;
        
        
        private T GetComponent<T>(string path) where T : Component
        {
            var t = transform.Find(path);
            if (t == null)
            {
                Debug.LogError("Cannot find Transform at "+ path+ " for component " + typeof(T) + " GO Name: " + gameObject.name , gameObject);
                return null;
            }
            var component = t.GetComponent<T>();
            if (component == null) Debug.LogError("Cannot find "+typeof(T)+" at "+path, gameObject);
            return component;
        }
        #if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button("Validate")]
        private void Validate()
        {
            OnValidate();
        }
        #endif
                
        #if UNITY_EDITOR
        protected void OnValidate()
        {
            // Label
            // Labels
            LabelLevel = GetComponent<TextMeshProUGUI>("Level progress/Label - Level");
            LabelLevelExp = GetComponent<TextMeshProUGUI>("Level progress/Label - LevelExp");
            LabelTime = GetComponent<TextMeshProUGUI>("Panel - Stats/Time/value/Label - Time");
            LabelProfit = GetComponent<TextMeshProUGUI>("Panel - Stats/Profit/value/Label - Profit");
            LabelExp = GetComponent<TextMeshProUGUI>("Panel - Stats/Exp/value/Label - Exp");
            LabelNoEnergy = GetComponent<TextMeshProUGUI>("Label - NoEnergy");
            LabelName = GetComponent<TextMeshProUGUI>("Label - Name");
            LabelToWorkEnergy = GetComponent<TextMeshProUGUI>("Button - Work/Label - ToWorkEnergy");
            LabelSalary = GetComponent<TextMeshProUGUI>("Button - Salary/background/Label - Salary");
            // Buttons
            ButtonClose = GetComponent<Button>("Button - Close");
            ButtonWork = GetComponent<Button>("Button - Work");
            ButtonSalary = GetComponent<Button>("Button - Salary");
            ButtonAllJobs = GetComponent<Button>("Button - AllJobs");
            // Images
            ImageProgress = GetComponent<Image>("Level progress/ProgressBarBg/Image - Progress");
            ImageVacancyIcon = GetComponent<Image>("Vacancy Icon/Image - VacancyIcon");
            // Panels
            PanelStats = GetComponent<Transform>("Panel - Stats").gameObject;
        }
        #endif
    }
}


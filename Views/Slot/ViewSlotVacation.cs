// This file is code generated. Do not edit.

using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SP.Views
{
    public class ViewSlotVacation : MonoBehaviour
    {
        // TextMeshProUGUI 
        public TextMeshProUGUI LabelEnergy;
        public TextMeshProUGUI LabelProfit;
        public TextMeshProUGUI LabelTime;
        public TextMeshProUGUI LabelName;
        public TextMeshProUGUI LabelToWork;
        public TextMeshProUGUI LabelCurrent;
        // Buttons 
        public Button ButtonChoose;
        // Image 
        public Image ImageSelectedBg;
        public Image ImageSelectedIconBg;
        public Image ImageVacancyIcon;
        public Image ImageSelectedIconTop;
        
        
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
            LabelEnergy = GetComponent<TextMeshProUGUI>("Energy/value/Label - Energy");
            LabelProfit = GetComponent<TextMeshProUGUI>("Stats/Profit/value/Label - Profit");
            LabelTime = GetComponent<TextMeshProUGUI>("Stats/Time/value/Label - Time");
            LabelName = GetComponent<TextMeshProUGUI>("Label - Name");
            LabelToWork = GetComponent<TextMeshProUGUI>("Button - Choose/background/Label - ToWork");
            LabelCurrent = GetComponent<TextMeshProUGUI>("Label - Current");
            // Buttons
            ButtonChoose = GetComponent<Button>("Button - Choose");
            // Images
            ImageSelectedBg = GetComponent<Image>("Image - SelectedBg");
            ImageSelectedIconBg = GetComponent<Image>("Vacancy Icon/Image - SelectedIconBg");
            ImageVacancyIcon = GetComponent<Image>("Vacancy Icon/Image - VacancyIcon");
            ImageSelectedIconTop = GetComponent<Image>("Vacancy Icon/Image - SelectedIconTop");
        }
        #endif
    }
}


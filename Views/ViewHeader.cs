// This file is code generated. Do not edit.

using System.Collections.Generic;
using SP.Core.StateMachine;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SP.Views
{
    public class ViewHeader : UIScreen
    {
        // TextMeshProUGUI 
        public TextMeshProUGUI LabelEnergyValue;
        public TextMeshProUGUI LabelMoney;
        public TextMeshProUGUI LabelGold;
        // Image 
        public Image ImageProgressMoney;
        
        
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
            LabelEnergyValue = GetComponent<TextMeshProUGUI>("Energy/EnergyValue/Label - EnergyValue");
            LabelMoney = GetComponent<TextMeshProUGUI>("Image/SoftWallet/SoftWalletLabelBg/Label - Money");
            LabelGold = GetComponent<TextMeshProUGUI>("Image/HardWallet/HardWalletLabelBg/Label - Gold");
            // Images
            ImageProgressMoney = GetComponent<Image>("Image/SoftWallet/SoftWalletLabelBg/ProgressBarBg/Image - ProgressMoney");
        }
        #endif
    }
}


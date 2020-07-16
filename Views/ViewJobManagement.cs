// This file is code generated. Do not edit.

using System.Collections.Generic;
using SP.Core.StateMachine;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SP.Views
{
    public class ViewJobManagement : UIScreen
    {
        // TextMeshProUGUI 
        public TextMeshProUGUI LabelWindowName;
        // Buttons 
        public Button ButtonBack;
        public Button ButtonClose;
        // Slot 
        public GameObject SlotNumPage;
        public GameObject SlotVacation;
        
        
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
            LabelWindowName = GetComponent<TextMeshProUGUI>("WindowName/Label - WindowName");
            // Buttons
            ButtonBack = GetComponent<Button>("Button - Back");
            ButtonClose = GetComponent<Button>("Button - Close");
            // Slots
            SlotNumPage = GetComponent<Transform>("WindowPages/Slot - NumPage").gameObject;
            SlotVacation = GetComponent<Transform>("Window/WindowContent/Slot - Vacation").gameObject;
        }
        #endif
    }
}


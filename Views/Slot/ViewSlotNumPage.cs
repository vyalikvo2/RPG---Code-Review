// This file is code generated. Do not edit.

using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SP.Views
{
    public class ViewSlotNumPage : MonoBehaviour
    {
        // TextMeshProUGUI 
        public TextMeshProUGUI LabelPageNum;
        public TextMeshProUGUI LabelPageNumSelected;
        // Buttons 
        public Button ButtonSelect;
        // Image 
        public Image ImageSelected;
        public Image ImageStroke;
        public Image ImageStrokeSelected;
        
        
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
            LabelPageNum = GetComponent<TextMeshProUGUI>("Label - PageNum");
            LabelPageNumSelected = GetComponent<TextMeshProUGUI>("Label - PageNumSelected");
            // Buttons
            ButtonSelect = GetComponent<Button>("Button - Select");
            // Images
            ImageSelected = GetComponent<Image>("Image - Selected");
            ImageStroke = GetComponent<Image>("Image - Stroke");
            ImageStrokeSelected = GetComponent<Image>("Image - StrokeSelected");
        }
        #endif
    }
}


using System;
using SP.Utils;
using SP.Views;

namespace SP.Core.UIScreens.JobManagementScripts
{
    public class SlotNumPage : ViewSlotNumPage
    {
        public void FillSelected(int index, bool selected, Action<int> onChangePage)
        {
            LabelPageNum.text = index.ToRoman() + "";
            LabelPageNumSelected.text = index.ToRoman() + "";
            
            LabelPageNumSelected.gameObject.SetShow(selected);
            ImageStroke.SetShow(!selected);
            ImageStrokeSelected.SetShow(selected);
            ImageSelected.SetShow(selected);
            ButtonSelect.onClick.RemoveAllListeners();
            ButtonSelect.onClick.AddListener(() => onChangePage.Invoke(index));
        }
    }
}

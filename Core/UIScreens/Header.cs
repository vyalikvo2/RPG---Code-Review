using SP.Core.Data;
using SP.Core.GEventSystem;
using SP.Core.StateMachine;
using SP.Networking;
using SP.Utils;
using SP.Views;
using UnityEngine;

public class Header : ViewHeader
{
    private static readonly int ShowFullSizeHash = Animator.StringToHash("ShowFullSize");
    private static readonly int ShowCropSizeHash = Animator.StringToHash("ShowCropSize");
    
    private void Awake()
    {
        Name = ScreenNames.Header;
        
    }
    
    override public void Show()
    {
        base.Show();
        GEvent.UPDATED_STATS_GamePlayerStats.Subscribe(_onPlayerStatsUpdated);
    }
        
    override public void Hide()
    {
        base.Hide();
        GEvent.UPDATED_STATS_GamePlayerStats.Subscribe(_onPlayerStatsUpdated);
    }

    private void _onPlayerStatsUpdated(GEventData_GamePlayerStats stats)
    {
        if (stats.uid != NetworkClient.ClientId) return;
        
        GamePlayerStats gameStats = stats.gamePlayerStats;
        LabelEnergyValue.text = gameStats.energy.current + " / "+ gameStats.energy.max;
        LabelMoney.text = gameStats.money.current + " / "+ gameStats.money.max;
        LabelGold.text = gameStats.gold.ToString();

        ImageProgressMoney.fillAmount = (gameStats.money.current + 0.0f) / gameStats.money.max;
        
        LabelEnergyValue.RecalculateRectTransform();
        LabelMoney.RecalculateRectTransform();
        LabelGold.RecalculateRectTransform();
    }


    public override void ShowCropSize()
    {
        Animator.SetTrigger(ShowCropSizeHash);
    }

    public override void ShowFullSize()
    {
        Animator.SetTrigger(ShowFullSizeHash);
    }

    public override void ShowSizeReset()
    {
        Animator.ResetTrigger(ShowCropSizeHash);
        Animator.ResetTrigger(ShowFullSizeHash);
    }
    
}

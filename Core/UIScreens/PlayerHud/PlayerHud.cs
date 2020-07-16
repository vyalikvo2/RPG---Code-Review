using SP.Core.Data;
using SP.Core.StateMachine;
using SP.Utils;
using SP.Views;
using UnityEngine;

namespace SP.Core.UIScreens.PlayerHud
{
    public class PlayerHud : ViewPlayerHud
    {
        private StaminaHud _stamina;
        private CanvasGroup _staminaCanvas;
        private float _lastStaminaChange = float.MinValue;
        private void Awake()
        {
            Name = ScreenNames.PlayerHud;
            _stamina = SlotStaminaHud.GetComponent<StaminaHud>();
            _staminaCanvas = SlotStaminaHud.GetComponent<CanvasGroup>();
            _staminaCanvas.alpha = 0;
            _stamina.ImageBar.SetFill(1);
        }

        private void Update()
        {
            var playerStat = PlayerStatsData.Me;
            if (playerStat == null) return;
            
            var time = Time.time;
            var delta = Time.deltaTime;

            FillStamina(time, delta, playerStat);
        }

        private void FillStamina(float time, float delta, PlayerStatsData player)
        {
            var stamina = player.game.endurance;
            if (_stamina.ImageBar.SetFill(stamina.current / (float) stamina.max))
                _lastStaminaChange = time;
            
            _staminaCanvas.SetAlpha(Mathf.Lerp(_staminaCanvas.alpha, time - _lastStaminaChange > 2 ? 0 : 0.75f, delta * 2));
        }
    }
}

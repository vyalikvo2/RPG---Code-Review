using System.Collections.Generic;
using System.Linq;
using SP.Utils;

namespace SP.Core.StateMachine
{
    public class UIStateMachine : StateMachine<ScreenNames>
    {
        private readonly List<UIScreen> _screens = new List<UIScreen>();
        
        public static UIStateMachine instance;

        public ScreenNames CurrentScreenName; 

        private void Start()
        {
            instance = this;
            
            var screens = gameObject.GetComponentsInChildren<UIScreen>(true);
            foreach (var screen in screens)
            {
                _screens.Add(screen);
                screen.gameObject.Show();
            }
            
            base.OnChangeBegin += OnChangeBegin;
            base.OnChangeCompleted += OnChangeCompleted;

            Reload(ScreenNames.Main);
        }

        private void OnDestroy()
        {
            instance = null;
        }

        private new void OnChangeCompleted(ScreenNames newScreen)
        {
            _screens.FirstOrDefault(s => s.Name == newScreen)?.Show();
            ShowConditions(newScreen);
        }

        private new void OnChangeBegin(ScreenNames newScreen)
        {
            foreach (var screen in _screens)
                screen.Hide();
        }

        private void ShowConditions(ScreenNames newScreen)
        {
            var screen = newScreen != ScreenNames.Main ? GetScreen(newScreen) : null;
            var header = GetScreen(ScreenNames.Header);
            header.ShowSizeReset();
            switch (newScreen)
            {
                case ScreenNames.Main:
                    header.Show();
                    GetScreen(ScreenNames.CharacterControl).Show();
                    GetScreen(ScreenNames.PlayersMessages).Show();
                    GetScreen(ScreenNames.PlayerHud).Show();
                    GetScreen(ScreenNames.Chat).Show();
                    break;
                case ScreenNames.JobManagement:
                    screen.Show();
                    header.Show();
                    break;
                case ScreenNames.CurrentJob:
                    screen.Show();
                    header.Show();
                    break;
                case ScreenNames.JobInProgress:
                    screen.Show();
                    header.Show();
                    break;
            }

            CurrentScreenName = screen ? screen.Name : ScreenNames.Main;
        }

        private UIScreen GetScreen(ScreenNames screenName)
        {
            return _screens.First(s => s.Name == screenName);
        }
    }
}
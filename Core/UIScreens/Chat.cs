using SP.Core.StateMachine;
using SP.Networking.Requests;
using UnityEngine.UI;

namespace SP.Core.UIScreens
{
    public class Chat : UIScreen
    {
        private const int MaxMessageLength = 100;
        
        public Text text;
        public InputField field;
        
        private void Awake()
        {
            Name = ScreenNames.Chat;
        }
        public void SendMessageOnEndEdit(string message)
        {
            if (message.Length == 0)
            {
                return;
            }
            
            PlayerMessagesHolder.instance?.OnPlayerSay(message); // fake send to self
            ChatRequest.Send(ChatReqAction.SEND_MESSAGE_TO_ALL, new ChatReqParams(message));
            
            field.SetTextWithoutNotify("");
            // if (Application.platform == RuntimePlatform.WindowsPlayer || Application.isEditor)
            // {
            //     field.Select();
            //     field.ActivateInputField();
            // }
        }

        private void Start()
        {
            field.onEndEdit.AddListener(_ =>
            {
                // Debug.LogWarning($"[Chat] Send message: {field.text}");
                SendMessageOnEndEdit(field.text);
            });
            
            field.onValueChanged.AddListener(message =>
            {
                if (message.Length < MaxMessageLength) return;
                field.SetTextWithoutNotify(message.Substring(0, MaxMessageLength));
            });
        }
    }
}

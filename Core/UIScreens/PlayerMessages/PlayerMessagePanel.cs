using System.Collections.Generic;
using System.Threading.Tasks;
using SP.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace SP.Core.UIScreens
{
    public class PlayerMessagePanel : MonoBehaviour
    {
        private readonly Queue<MessageQueue> _messageQueues = new Queue<MessageQueue>();

        private const float ShowLongMessageTimeMultiplier = 0.1f;
        private const int SymbolsToLongMessage = 30;
        private const int ShowMessageMaxTime = 10;
        private const int ShowMessageMinTime = 3;
        
        private Transform _player;
        private Camera _camera;
        private bool _showing;
        
        
        public CanvasGroup canvas;
        public Image pointer;
        public Text message;

        public void AssignPlayer(Transform player)
        {
            _player = player;
            _camera = Camera.main;;
        }
        
        public void OnSay(string nickname, string message)
        {
            var showHandle = ShowMessage(nickname, message);
        }

        private async Task ShowMessage(string nickname, string message)
        {
            if (_showing)
            {
                _messageQueues.Enqueue(new MessageQueue(nickname, message));
                return;
            }
            
            var msgLength = message.Length;
            var showMessageTime = msgLength < SymbolsToLongMessage ? ShowMessageMinTime : Mathf.Clamp(msgLength * ShowLongMessageTimeMultiplier, ShowMessageMinTime, ShowMessageMaxTime);

            _showing = true;

            var movePanelToPlayerHandle = MoveToOwner();
            
            canvas.alpha = 1;
            this.message.text = $"<b><i>{nickname}:</i></b> {message}";

            await Task.Delay((int) showMessageTime * 1000);

            while (!gameObject.IsAnyNull() && canvas.alpha > 0)
            {
                canvas.alpha -= Time.deltaTime;
                await Task.Yield();
            }

            _showing = false;

            if (_messageQueues.Count > 0)
            {
                var nextMessage = _messageQueues.Dequeue();
                ShowMessage(nextMessage);
            }
        }

        private async Task MoveToOwner()
        {
            var color = pointer.color;
            
            while (_showing)
            {
                var position = _camera.WorldToScreenPoint(_player.position);
                position.x += 185;
                position.y += 125;

                transform.position = position;

                color.a = Mathf.PingPong(Time.time * 2f, 1f);
                pointer.color = color;
                
                await Task.Yield();
            }
        }
        
        private class MessageQueue
        {
            public string nickname;
            public string message;

            public MessageQueue(string nickname, string message)
            {
                this.nickname = nickname;
                this.message = message;
            }
        }

        private void ShowMessage(MessageQueue message)
        {
            var showHandle = ShowMessage(message.nickname, message.message);
        }
    }
}

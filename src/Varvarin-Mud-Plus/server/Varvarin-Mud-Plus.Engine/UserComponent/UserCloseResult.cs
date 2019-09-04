using System.Net.WebSockets;

namespace Varvarin_Mud_Plus.Engine.UserComponent
{ 
    public class UserCloseResult
    {
        private readonly WebSocketReceiveResult _webSocketReceiveResult;

        public WebSocketCloseStatus CloseStatus {
            get
            {
                return _webSocketReceiveResult.CloseStatus.Value;
            }
        }

        public string CloseStatusDescription {
            get
            {
                return _webSocketReceiveResult.CloseStatusDescription;
            }
        }

        public UserCloseResult(WebSocketReceiveResult webSocketReceiveResult)
        {
            _webSocketReceiveResult = webSocketReceiveResult;
        }
    }
}

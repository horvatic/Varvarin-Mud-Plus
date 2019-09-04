using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Varvarin_Mud_Plus.Engine.UserComponent
{
    public class User
    {
        private readonly WebSocket _webSocket;
        private readonly int _bufferSize;

        public User(WebSocket webSocket, int bufferSize)
        {
            _webSocket = webSocket;
            _bufferSize = bufferSize;
        }

        public async Task<UserResult> ReceiveMessage()
        {
            var buffer = new byte[_bufferSize];
            UserResult result;
            try
            {
                var message = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                result = new UserResult(message, buffer);
            }
            catch
            {
                var message = new WebSocketReceiveResult(0, WebSocketMessageType.Close, true);
                result = new UserResult(message, buffer);
                result.ConntectionLostHasBeenLost();
            }
            return result;
        }

        public async Task SendMessage(string message)
        {
            await _webSocket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(message)), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task CloseUserConnection(UserCloseResult result)
        {
            await _webSocket.CloseAsync(result.CloseStatus, result.CloseStatusDescription, CancellationToken.None);
        }

        public bool IsAlive()
        {
            var state = _webSocket.State;

            return state == WebSocketState.Closed || state == WebSocketState.Aborted || state == WebSocketState.CloseReceived;
        }
    }
}

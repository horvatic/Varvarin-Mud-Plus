using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Varvarin_Mud_Plus.Engine.UserComponent
{
    public class User : IUser
    {
        private readonly WebSocket _webSocket;
        private readonly int _bufferSize;
        private string name;
        private Guid lobbyContext;

        public User(WebSocket webSocket, int bufferSize, Guid initLobbyContext)
        {
            _webSocket = webSocket;
            _bufferSize = bufferSize;
            name = Guid.NewGuid().ToString();
            lobbyContext = initLobbyContext;
        }
        
        public string GetUserName()
        {
            return name;
        }

        public void SetUserName(string newName)
        {
            name = newName;
        }

        public async Task<IUserResult> ReceiveMessage()
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

        public async Task CloseUserConnection(IUserCloseResult result)
        {
            await _webSocket.CloseAsync(result.CloseStatus, result.CloseStatusDescription, CancellationToken.None);
        }

        public bool IsAlive()
        {
            var state = _webSocket.State;

            return state == WebSocketState.Closed || state == WebSocketState.Aborted || state == WebSocketState.CloseReceived;
        }

        public void SetLobbyContext(Guid lobbyId)
        {
            lobbyContext = lobbyId;
        }

        public Guid GetLobbyContext()
        {
            return lobbyContext;
        }
    }
}

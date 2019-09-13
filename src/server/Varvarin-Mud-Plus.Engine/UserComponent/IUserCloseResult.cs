using System.Net.WebSockets;

namespace Varvarin_Mud_Plus.Engine.UserComponent
{
    public interface IUserCloseResult
    {
        WebSocketCloseStatus CloseStatus { get; }
        string CloseStatusDescription { get; }
    }
}

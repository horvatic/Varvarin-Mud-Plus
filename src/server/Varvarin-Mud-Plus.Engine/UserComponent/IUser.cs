using System;
using System.Threading.Tasks;

namespace Varvarin_Mud_Plus.Engine.UserComponent
{
    public interface IUser
    {
        string GetUserName();
        void SetUserName(string newName);
        Task<IUserResult> ReceiveMessage();
        Task SendMessage(string message);
        Task CloseUserConnection(IUserCloseResult result);
        bool IsAlive();
        void SetLobbyContext(Guid lobbyId);
        Guid GetLobbyContext();
    }
}

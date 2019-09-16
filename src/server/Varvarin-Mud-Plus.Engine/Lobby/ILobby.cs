using System;
using System.Threading;
using System.Threading.Tasks;
using Varvarin_Mud_Plus.Engine.UserComponent;

namespace Varvarin_Mud_Plus.Engine.Lobby
{
    public interface ILobby
    {
        string GetLobbyType();
        Guid GetLobbyId();
        void StopLobby();
        void StartLobby();
        AddUserToLobbyResult AddUserToLobby(IUser user);
        void RemoveUser(IUser user);
        bool IsLobbyEmpty();
        Task ProcessClientMessage(string message, IUser user);
    }
}
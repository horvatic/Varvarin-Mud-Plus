using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Varvarin_Mud_Plus.Engine.Command;
using Varvarin_Mud_Plus.Engine.Extensions;
using Varvarin_Mud_Plus.Engine.UserComponent;

namespace Varvarin_Mud_Plus.Engine.Lobby
{
    public class LobbyCoordinator
    {
        private readonly List<ILobby> _lobbies;
        private readonly Guid _deafultLobbyId;
        private readonly LobbyCoordinatorCommandProcessor _lobbyCoordinatorCommandProcessor;

        public LobbyCoordinator(ILobby deafultLobby, LobbyCoordinatorCommandProcessor lobbyCoordinatorCommandProcessor)
        {
            _lobbyCoordinatorCommandProcessor = lobbyCoordinatorCommandProcessor;
            deafultLobby.StartLobby();
            _deafultLobbyId = deafultLobby.GetLobbyId();
            _lobbies = new List<ILobby>
            {
                deafultLobby
            };
        }

        public async Task RunUserSession(IUser user)
        {
            await user.SendMessage("Welcome To Varvarin Mud!\nType :help for all commands");
            var result = await user.ReceiveMessage();
            var userLobby = _lobbies.GetUserLobby(user);
            if (userLobby == null)
                SetUserLobbyToDeafult(user);
            while (!result.HasConnectionClosed() && !result.IsConntectionLost())
            {
                userLobby = _lobbies.GetUserLobby(user);
                if (result.GetMessage().StartsWith(":"))
                {
                    await _lobbyCoordinatorCommandProcessor.ProcessCommand(user, _lobbies, _deafultLobbyId, result.GetMessage());
                }
                else
                {
                    await userLobby.ProcessClientMessage(result.GetMessage(), user);
                }
                result = await user.ReceiveMessage();
            }
            if(!result.IsConntectionLost())
                await user.CloseUserConnection(result.GetCloseResult());
            userLobby = _lobbies.Where(x => x.GetLobbyId() == _deafultLobbyId).First();
            await userLobby.RemoveUser(user);

            if (userLobby.IsLobbyEmpty() && userLobby.GetLobbyId() != _deafultLobbyId)
                userLobby.StopLobby();
        }

        private void SetUserLobbyToDeafult(IUser user)
        {
            var userLobby = _lobbies.Where(x => x.GetLobbyId() == _deafultLobbyId).First();
            user.SetLobbyContext(_deafultLobbyId);
        }
    }
}

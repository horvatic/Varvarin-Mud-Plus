using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Varvarin_Mud_Plus.Engine.Extensions;
using Varvarin_Mud_Plus.Engine.Lobby;
using Varvarin_Mud_Plus.Engine.UserComponent;

namespace Varvarin_Mud_Plus.Engine.Command
{
    public class LobbyCoordinatorCommandProcessor
    {
        public async Task ProcessCommand(IUser user, List<ILobby> lobbies, Guid deafultLobbyId, string command)
        {
            if (command.ToLower() == ":help")
            {
                await user.SendMessage(GetHelp());
            }
            else if(command.ToLower() == ":list created lobbies")
            {
                var lobbiesNameAndId = lobbies
                    .Select(x => FormatLobbyInfo(x))
                    .Aggregate((current, s) => $"{current}\n{s}");
                await user.SendMessage($"{lobbiesNameAndId}\n");
            }
            else if (command.ToLower() == ":current lobby")
            {
                var userLobby = lobbies.GetUserLobby(user);
                if(userLobby == null)
                    await user.SendMessage($"User not in a lobby\n");
                else
                    await user.SendMessage(FormatLobbyInfo(userLobby) + "\n");
            }
            else if (command.ToLower() == ":list lobby types")
            {
                await user.SendMessage($"{MessageLobby.LOBBY_TYPE}\n");
            }
            else if (command.ToLower().StartsWith(":leave lobby"))
            {
                var lobby = lobbies.FirstOrDefault(x => x.GetLobbyId() == deafultLobbyId);
                RemoveFromLobby(user, lobbies, deafultLobbyId);
                await AddToLobby(user, lobby, deafultLobbyId);
            }
            else if (command.ToLower().StartsWith(":join lobby="))
            {
                var request = command.Substring(12, command.Length - 12);
                try
                {
                    var lobbyRequested = Guid.Parse(request);
                    var lobby = lobbies.FirstOrDefault(x => x.GetLobbyId() == lobbyRequested);
                    if (lobby == null)
                        throw new Exception();
                    RemoveFromLobby(user, lobbies, deafultLobbyId);
                    await AddToLobby(user, lobby, lobbyRequested);
                }
                catch
                {
                    await user.SendMessage($"Could not join {request}\n");
                }

            }
            else if (command.ToLower().StartsWith(":make lobby="))
            {
                var name = command.Substring(12, command.Length - 12);
                if(name.ToLower() == MessageLobby.LOBBY_TYPE.ToLower())
                {
                    await AddUserToMessageLobby(user, lobbies, deafultLobbyId);
                }
                else
                {
                    await user.SendMessage($"Could not make {name}\n");
                }
            }
            else
            {
                await user.SendMessage($"INVAILD COMMAND\n");
            }
        }

        private async Task AddUserToMessageLobby(IUser user, List<ILobby> lobbies, Guid deafultLobbyId)
        {
            RemoveFromLobby(user, lobbies, deafultLobbyId);
            var lobbyId = Guid.NewGuid();
            var newLobby = new MessageLobby(lobbyId, new UserLobbyCommandProcessor());
            await AddToLobby(user, newLobby, lobbyId);
            lobbies.Add(newLobby);
        }

        public async Task AddToLobby(IUser user, ILobby lobby, Guid lobbyId)
        {
            user.SetLobbyContext(lobbyId);
            lobby.AddUserToLobby(user);
            lobby.StartLobby();
            await user.SendMessage($"Joining: {FormatLobbyInfo(lobby)}\n");
        }

        private void RemoveFromLobby(IUser user, List<ILobby> lobbies, Guid deafultLobbyId)
        {
            var userLobby = lobbies.GetUserLobby(user);
            userLobby.RemoveUser(user);
            if (userLobby.IsLobbyEmpty() && userLobby.GetLobbyId() != deafultLobbyId)
            {
                userLobby.StopLobby();
                lobbies.Remove(userLobby);
            }
        }

        private string FormatLobbyInfo(ILobby lobby)
        {
            return $"Lobby Type: {lobby.GetLobbyType()} | Lobby Id: {lobby.GetLobbyId()}";
        }

        private string GetHelp()
        {
            return @"
:help - server commands
!:help - current lobby commands
:logoff - logoff and end session
:clear - clear the screen
:list lobby types - list all lobbies that can be created
:list created lobbies - list all lobbies users have created
:current lobby - get info for the current lobby
:join lobby={LOBBY ID} - join lobby with id
:make lobby={LOBBY TYPE} - join lobby
:leave lobby - leave current lobby
";
        }
    }
}

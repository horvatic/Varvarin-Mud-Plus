using System.Collections.Generic;
using System.Linq;
using Varvarin_Mud_Plus.Engine.Lobby;
using Varvarin_Mud_Plus.Engine.UserComponent;

namespace Varvarin_Mud_Plus.Engine.Extensions
{
    public static class ILobbyListExtensions
    {
        public static ILobby GetUserLobby(this List<ILobby> lobbies, IUser user)
        {
            return lobbies.Where(x => x.GetLobbyId() == user.GetLobbyContext()).FirstOrDefault();
        }
    }
}

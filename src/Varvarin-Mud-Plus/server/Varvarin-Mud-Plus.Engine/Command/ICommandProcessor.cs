using System.Collections.Generic;
using System.Threading.Tasks;
using Varvarin_Mud_Plus.Engine.UserComponent;

namespace Varvarin_Mud_Plus.Engine.Command
{
    public interface ICommandProcessor
    {
        Task ProcessCommand(User mainUser, List<User> allUsers, string command);
    }
}

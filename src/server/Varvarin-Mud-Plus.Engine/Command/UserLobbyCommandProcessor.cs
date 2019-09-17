using System.Collections.Generic;
using System.Threading.Tasks;
using Varvarin_Mud_Plus.Engine.UserComponent;

namespace Varvarin_Mud_Plus.Engine.Command
{
    public class UserLobbyCommandProcessor
    {
        public async Task ProcessCommand(IUser mainUser, List<IUser> allUsers, string command)
        {
            if(command.ToLower() == "!:list all users")
            {
                var userNames = "";
                foreach(var user in allUsers)
                {
                    userNames += $"{user.GetUserName()}\n";
                }
                await mainUser.SendMessage(userNames);
            }
            else if(command.ToLower() == "!:help")
            {
                await mainUser.SendMessage(GetHelp());
            }
            else
            {
                await mainUser.SendMessage($"INVAILD COMMAND\n");
            }
        }

        private string GetHelp()
        {
            return @"
!:help - current lobby commands
!:list all users
";
        }
    }
}

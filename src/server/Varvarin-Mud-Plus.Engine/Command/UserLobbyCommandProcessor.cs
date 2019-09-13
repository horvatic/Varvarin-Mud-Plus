using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Varvarin_Mud_Plus.Engine.UserComponent;

namespace Varvarin_Mud_Plus.Engine.Command
{
    public class UserLobbyCommandProcessor : ICommandProcessor
    {
        public async Task ProcessCommand(IUser mainUser, List<IUser> allUsers, string command)
        {
            if(command.ToLower() == ":list all users")
            {
                var userNames = "";
                foreach(var user in allUsers)
                {
                    userNames += $"{user.GetUserName()}\n";
                }
                await mainUser.SendMessage(userNames);
            }
            else if (command.ToLower().Contains(":set name="))
            {
                var name = command.Substring(10, command.Length - 10 );
                mainUser.SetUserName(name);
                await mainUser.SendMessage($"Name set to {name}");
            }
            else if(command.ToLower() == ":help")
            {
                await mainUser.SendMessage($":list all users\n:set name=NAME\n:logoff\n:clear\n");
            }
            else
            {
                await mainUser.SendMessage("INVAILD COMMAND");
            }
        }
    }
}

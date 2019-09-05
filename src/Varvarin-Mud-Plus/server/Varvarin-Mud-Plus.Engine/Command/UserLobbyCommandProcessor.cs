using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Varvarin_Mud_Plus.Engine.UserComponent;

namespace Varvarin_Mud_Plus.Engine.Command
{
    public class UserLobbyCommandProcessor : ICommandProcessor
    {
        public async Task ProcessCommand(User mainUser, List<User> allUsers, string command)
        {
            if(command.ToLower() == ":list all users")
            {
                var userNames = "";
                foreach(var user in allUsers)
                {
                    userNames += $"{user.Name}\n";
                }
                await mainUser.SendMessage(userNames);
            }
            else if (command.ToLower().Contains(":set name="))
            {
                var name = command.Substring(10, command.Length - 10 );
                mainUser.Name = name;
                await mainUser.SendMessage($"Name set to {name}");
            }
            else
            {
                await mainUser.SendMessage("INVAILD COMMAND");
            }
        }
    }
}

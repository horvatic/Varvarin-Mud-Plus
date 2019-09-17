using System.Collections.Generic;
using System.Threading.Tasks;
using Varvarin_Mud_Plus.Engine.Command;
using Varvarin_Mud_Plus.Engine.Test.Mocks;
using Varvarin_Mud_Plus.Engine.UserComponent;
using Xunit;

namespace Varvarin_Mud_Plus.Engine.Test.Command
{
    public class UserLobbyCommandProcessorTest
    {
        [Fact]
        public async Task ProcessCommand_ListAllUsers()
        {
            var mainUser = new MockUser().StubSetUserName("Mike");
            var otherUser = new MockUser().StubSetUserName("John");
            var userLobby = new UserLobbyCommandProcessor();

            await userLobby.ProcessCommand(mainUser, new List<IUser> { mainUser, otherUser }, "!:list all users");

            mainUser.VerifySendMessageCalledWith("Mike\nJohn\n");
            otherUser.VerifySendMessageWasNotCalled();
        }

        [Fact]
        public async Task ProcessCommand_Help()
        {
            var mainUser = new MockUser();
            var userLobby = new UserLobbyCommandProcessor();

            await userLobby.ProcessCommand(mainUser, new List<IUser> { mainUser }, "!:help");

            mainUser.VerifySendMessageCalledWith(GetHelp());
        }

        [Theory]
        [InlineData(":dwqwqwd")]
        [InlineData("!:fwqffqwfwqs")]
        [InlineData("12343d")]
        public async Task ProcessCommand_RandomCommands(string commands)
        {
            var mainUser = new MockUser();
            var userLobby = new UserLobbyCommandProcessor();

            await userLobby.ProcessCommand(mainUser, new List<IUser> { mainUser }, commands);

            mainUser.VerifySendMessageCalledWith("INVAILD COMMAND\n");
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

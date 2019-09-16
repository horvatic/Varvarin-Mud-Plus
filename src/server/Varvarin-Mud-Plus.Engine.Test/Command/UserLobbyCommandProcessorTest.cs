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
        [Theory]
        [InlineData("Mike")]
        [InlineData("Mike Is Number 1")]
        public async Task ProcessCommand_ChangeUserName(string name)
        {
            var user = new MockUser();
            var userLobby = new UserLobbyCommandProcessor();

            await userLobby.ProcessCommand(user, new List<IUser> { user }, $":set name={name}");

            user.VerifySetUserNameWasCalledWith(name);
        }

        [Fact]
        public async Task ProcessCommand_ListAllUsers()
        {
            var mainUser = new MockUser().StubSetUserName("Mike");
            var otherUser = new MockUser().StubSetUserName("John");
            var userLobby = new UserLobbyCommandProcessor();

            await userLobby.ProcessCommand(mainUser, new List<IUser> { mainUser, otherUser }, ":list all users");

            mainUser.VerifySendMessageCalledWith("Mike\nJohn\n");
            otherUser.VerifySendMessageWasNotCalled();
        }

        [Fact]
        public async Task ProcessCommand_Help()
        {
            var mainUser = new MockUser();
            var userLobby = new UserLobbyCommandProcessor();

            await userLobby.ProcessCommand(mainUser, new List<IUser> { mainUser }, ":help");

            mainUser.VerifySendMessageCalledWith($":list all users\n:set name=NAME\n");
        }
    }
}

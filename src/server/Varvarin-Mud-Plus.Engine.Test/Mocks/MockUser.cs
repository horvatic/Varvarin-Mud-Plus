using System;
using System.Threading.Tasks;
using Moq;
using Varvarin_Mud_Plus.Engine.UserComponent;

namespace Varvarin_Mud_Plus.Engine.Test.Mocks
{
    class MockUser : IUser
    {
        private readonly Mock<IUser> _user;

        public MockUser()
        {
            _user = new Mock<IUser>();
        }

        public async Task CloseUserConnection(IUserCloseResult result)
        {
            await _user.Object.CloseUserConnection(result);
        }

        public string GetUserName()
        {
            return _user.Object.GetUserName();
        }

        public bool IsAlive()
        {
            return _user.Object.IsAlive();
        }

        public async Task<IUserResult> ReceiveMessage()
        {
            return await _user.Object.ReceiveMessage();
        }

        public async Task SendMessage(string message)
        {
            await _user.Object.SendMessage(message);
        }

        public void SetUserName(string newName)
        {
            _user.Object.SetUserName(newName);
        }

        public void VerifySetUserNameWasCalledWith(string name)
        {
            _user.Verify(x => x.SetUserName(name), Times.Once);
        }

        public void VerifySendMessageWasNotCalled()
        {
            _user.Verify(x => x.SendMessage(It.IsAny<string>()), Times.Never);
        }

        public void VerifySendMessageCalledWith(string message)
        {
            _user.Verify(x => x.SendMessage(message), Times.Once);
        }

        public MockUser StubSetUserName(string username)
        {
            _user.Setup(x => x.GetUserName()).Returns(username);
            return this;
        }

        public void SetLobbyContext(Guid lobbyId)
        {
            throw new NotImplementedException();
        }

        public Guid GetLobbyContext()
        {
            throw new NotImplementedException();
        }
    }
}

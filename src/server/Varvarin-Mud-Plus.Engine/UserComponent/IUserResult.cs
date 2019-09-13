namespace Varvarin_Mud_Plus.Engine.UserComponent
{
    public interface IUserResult
    {
        bool HasConnectionClosed();
        IUserCloseResult GetCloseResult();
        string GetMessage();
        void ConntectionLostHasBeenLost();
        bool IsConntectionLost();
    }
}

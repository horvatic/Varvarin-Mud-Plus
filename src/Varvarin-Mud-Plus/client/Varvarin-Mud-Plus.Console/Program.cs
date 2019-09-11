namespace Varvarin_Mud_Plus.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var userSession = new UserSession();
            userSession.Start();
        }
    }
}

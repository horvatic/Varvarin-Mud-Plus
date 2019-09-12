using Microsoft.Extensions.Configuration;
using System.IO;

namespace Varvarin_Mud_Plus.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json", true, true)
          .Build();

            var userSession = new UserSession(config);
            userSession.Start();
        }
    }
}

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Varvarin_Mud_Plus.Engine.UserComponent;

namespace Varvarin_Mud_Plus.Engine.Lobby
{
    public class UserLobby
    {

        private readonly List<User> Users;
        private readonly ConcurrentQueue<string> Messges;
        public UserLobby()
        {
            Users = new List<User>();
            Messges = new ConcurrentQueue<string>();
        }

        public async Task RunUserSession(User user)
        {
            Users.Add(user);
            var result = await user.ReceiveMessage();
            while (!result.HasConnectionClosed() && !result.IsConntectionLost())
            {
                Messges.Enqueue(result.GetMessage());
                result = await user.ReceiveMessage();
            }
            if(!result.IsConntectionLost())
                await user.CloseUserConnection(result.GetCloseResult());
            Users.Remove(user);
        }

        public void StartLobby(CancellationToken token)
        {
            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    while (Messges.Count > 0)
                    {
                        var hasMessage = Messges.TryDequeue(out var message);
                        if (!hasMessage)
                            continue;

                        foreach (var user in Users)
                        {
                            await user.SendMessage(message);
                        }
                    }
                }
            });
        }
    }
}

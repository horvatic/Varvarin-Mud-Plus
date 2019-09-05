﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Varvarin_Mud_Plus.Engine.Command;
using Varvarin_Mud_Plus.Engine.UserComponent;

namespace Varvarin_Mud_Plus.Engine.Lobby
{
    public class UserLobby
    {

        private readonly List<User> Users;
        private readonly ConcurrentQueue<string> Messges;
        private readonly ICommandProcessor _commandProcessor;

        public UserLobby(ICommandProcessor commandProcessor)
        {
            Users = new List<User>();
            Messges = new ConcurrentQueue<string>();
            _commandProcessor = commandProcessor;
        }

        public async Task RunUserSession(User user)
        {
            Users.Add(user);
            var result = await user.ReceiveMessage();
            while (!result.HasConnectionClosed() && !result.IsConntectionLost())
            {
                var message = result.GetMessage();
                if (message.StartsWith(":"))
                {
                    await _commandProcessor.ProcessCommand(user, Users, message);
                }
                else
                {
                    Messges.Enqueue($"User: {user.Name} Message: {message}");
                }
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

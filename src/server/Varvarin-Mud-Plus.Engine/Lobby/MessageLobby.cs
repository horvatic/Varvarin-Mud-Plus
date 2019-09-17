using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Varvarin_Mud_Plus.Engine.Command;
using Varvarin_Mud_Plus.Engine.UserComponent;

namespace Varvarin_Mud_Plus.Engine.Lobby
{
    public class MessageLobby : ILobby
    {
        public const string LOBBY_TYPE = "Group Message";
        private readonly Guid _id;
        private readonly UserLobbyCommandProcessor _commandProcessor;
        private readonly ConcurrentQueue<string> _messges;
        private readonly List<IUser> _allUsers;
        private readonly CancellationTokenSource lobbyCancellationTokenSource;

        public MessageLobby(Guid id, UserLobbyCommandProcessor commandProcessor)
        {
            _allUsers = new List<IUser>();
            _id = id;
            _commandProcessor = commandProcessor;
            _messges = new ConcurrentQueue<string>();
            lobbyCancellationTokenSource = new CancellationTokenSource();
        }

        public string GetLobbyType()
        {
            return LOBBY_TYPE;
        }

        public Guid GetLobbyId()
        {
            return _id;
        }

        public void StopLobby()
        {
            lobbyCancellationTokenSource.Cancel();
        }

        public void StartLobby()
        {
            var token = lobbyCancellationTokenSource.Token;
            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    while (_messges.Count > 0)
                    {
                        var hasMessage = _messges.TryDequeue(out var message);
                        if (!hasMessage)
                            continue;

                        foreach (var user in _allUsers)
                        {
                            await user.SendMessage(message);
                        }
                    }
                }
            });
        }

        public async Task<AddUserToLobbyResult> AddUserToLobby(IUser user)
        {
            _allUsers.Add(user);
            return await Task.FromResult(AddUserToLobbyResult.AddedToLobby);
        }

        public async Task ProcessClientMessage(string message, IUser user)
        {
            if (message.StartsWith("!:"))
            {
                await _commandProcessor.ProcessCommand(user, _allUsers, message);
            }
            else
            {
                _messges.Enqueue($"User: {user.GetUserName()}\nMessage: {message}\n");
            }
        }

        public async Task RemoveUser(IUser user)
        {
            _allUsers.Remove(user);
            await Task.CompletedTask;
        }

        public bool IsLobbyEmpty()
        {
            return _allUsers.Count() <= 0;
        }
    }
}

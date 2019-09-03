using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Varvarin_Mud_Plus.Console
{
    public class UserSession
    {
        private readonly ConcurrentQueue<string> Messges;

        public UserSession()
        {
            Messges = new ConcurrentQueue<string>();
        }

        public void Start()
        {
            System.Console.SetCursorPosition(0, System.Console.WindowHeight - 1);
            var clientGuid = Guid.NewGuid();
            var client = new ClientWebSocket();
            client.ConnectAsync(new Uri("ws://localhost:52479"), CancellationToken.None).GetAwaiter().GetResult();
            var cancellationTokenSource = new CancellationTokenSource();
            Task.Run(async () =>
            {
                await ReadData(client, cancellationTokenSource.Token);
            });
            Task.Run(() =>
            {                
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    while(Messges.Count > 0)
                    {
                        var hasMessage = Messges.TryDequeue(out var message);
                        if (hasMessage)
                            System.Console.WriteLine(message);
                    }
                }
            });

            var userInput = ReadUserInput();

            while (userInput != "q!")
            {
                client.SendAsync(Encoding.ASCII.GetBytes($"{userInput} Id: {clientGuid}"), WebSocketMessageType.Text, true, CancellationToken.None).GetAwaiter().GetResult();
                userInput = ReadUserInput();
            }
            cancellationTokenSource.Cancel();
            client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None).GetAwaiter().GetResult();
        }

        private string ReadUserInput()
        {
            var userInput = "";
            var userkey = System.Console.ReadKey();
            while (userkey.Key != ConsoleKey.Enter)
            {
                if (userkey.Key == ConsoleKey.Backspace)
                {
                    if (userInput.Length > 0)
                    {
                        userInput = userInput.Substring(0, userInput.Length - 1);
                    }
                }
                else
                {
                    userInput += userkey.KeyChar;
                }
                userkey = System.Console.ReadKey();
            }
            return userInput;
        }

        private async Task ReadData(ClientWebSocket client, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var buffer = new byte[1024 * 4];
                var result = await client.ReceiveAsync(buffer, CancellationToken.None);
                var rgx = new Regex("[^a-zA-Z0-9 -]");
                var message = Encoding.ASCII.GetString(buffer).Substring(0, result.Count);
                message = rgx.Replace(message, "");
                Messges.Enqueue(message);
            }
        }
    }
}

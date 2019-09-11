using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
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
            ClearCurrentConsoleLine();
            var client = new ClientWebSocket();
            client.ConnectAsync(new Uri("ws://localhost:58392"), CancellationToken.None).GetAwaiter().GetResult();
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
                        {
                            ClearCurrentConsoleLine();
                            System.Console.WriteLine(message);
                        }
                    }
                }
            });

            var userInput = ReadUserInput();

            while (userInput != ":q!")
            {
                if (userInput == ":clear")
                {
                    System.Console.Clear();
                }
                else
                {
                    client.SendAsync(Encoding.ASCII.GetBytes(userInput), WebSocketMessageType.Text, true, CancellationToken.None).GetAwaiter().GetResult();
                }
                userInput = ReadUserInput();
            }
            cancellationTokenSource.Cancel();
            client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None).GetAwaiter().GetResult();
        }

        public void ClearCurrentConsoleLine()
        {
            int currentLineCursor = System.Console.CursorTop;
            System.Console.SetCursorPosition(0, System.Console.CursorTop);
            System.Console.Write(new string(' ', System.Console.WindowWidth));
            System.Console.SetCursorPosition(0, currentLineCursor);
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
                var message = Encoding.ASCII.GetString(buffer).Substring(0, result.Count);
                Messges.Enqueue(message);
            }
        }
    }
}

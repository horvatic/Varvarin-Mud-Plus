using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Varvarin_Mud_Plus.Engine.Lobby;
using Varvarin_Mud_Plus.Engine.UserComponent;

namespace Varvarin_Mud_Plus.Web
{
    public class Startup
    {
        const int BUFFER_SIZE = 4 * 1024;

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = BUFFER_SIZE
            };
            app.UseWebSockets(webSocketOptions);
            var userLobby = new UserLobby();
            userLobby.StartLobby(new System.Threading.CancellationToken());
            app.Use(async (context, next) =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var socket = await context.WebSockets.AcceptWebSocketAsync();
                    var user = new User(socket, BUFFER_SIZE);
                    await userLobby.RunUserSession(user);
                }
                else
                {
                    context.Response.StatusCode = 405;
                }
            });
        }
    }
}

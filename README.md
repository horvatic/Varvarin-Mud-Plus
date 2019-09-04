# Varvarin-Mud-Plus
A mud engine for running simple games.

## How to run
### Server:
	type: dotnet run in the project Varvarin-Mud-Plus.Web
	
### Client:
	type dotnet run in the project Varvarin-Mud-Plus.Console
	
	Note: For the client the port must match the port on the server
	
	client.ConnectAsync(new Uri("ws://localhost:52479"), CancellationToken.None).GetAwaiter().GetResult();
	
	This will be added in a config soon
	
## Current Session:
### Main Lobby
Acts as a text chat

## Comming soon:

- Config for client
- TicTacToe Lobby
- Much More!

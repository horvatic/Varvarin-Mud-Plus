# Varvarin-Mud-Plus
https://github.com/horvatic/Varvarin-Mud-Plus/workflows/Client/badge.svg
https://github.com/horvatic/Varvarin-Mud-Plus/workflows/Server/badge.svg

A mud engine for running simple games.

## How to run
### Server:
Dot net core
	
	type: dotnet run in the project Varvarin-Mud-Plus.Web
	
Docker:

	Building:
	docker build -t varvarin .	
	
	Running:
	docker run -p 8080:8080 --name varvarin varvarin
	
### Client:
	type dotnet run in the project Varvarin-Mud-Plus.Console
	
	Note: For the client the port must match the port on the server
	
## Current Session:
### Main Lobby
Acts as a text chat

## Comming soon:

- TicTacToe Lobby
- Much More!

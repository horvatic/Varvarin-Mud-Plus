using System;
using System.Threading;
using System.Threading.Tasks;
using Varvarin_Mud_Plus.Engine.Extensions;
using Varvarin_Mud_Plus.Engine.UserComponent;

namespace Varvarin_Mud_Plus.Engine.Lobby
{
    public class TicTacToeLobby : ILobby
    {
        public const string LOBBY_TYPE = "TicTacToe";
        private IUser playerOne;
        private IUser playerTwo;
        private bool isPlayOneTurn;
        private TicTacToeLobbyGameBoardState gameState;
        private readonly Guid _id;
        private readonly CancellationTokenSource lobbyCancellationTokenSource;
        private readonly TicTacToeGameBoardCellState[,] gameboard;

        public TicTacToeLobby(Guid id)
        {
            _id = id;
            playerOne = null;
            playerTwo = null;
            lobbyCancellationTokenSource = new CancellationTokenSource();
            gameboard = new TicTacToeGameBoardCellState[3,3];
            gameboard.SetInitState();
            isPlayOneTurn = true;
            gameState = TicTacToeLobbyGameBoardState.OnGoing;
        }

        public async Task<AddUserToLobbyResult> AddUserToLobby(IUser user)
        {
            var result = AddUserToLobbyResult.FailedToAddLobbyFull;

            if (playerOne == null)
            {
                playerOne = user;
                if (playerTwo != null)
                    await playerTwo.SendMessage("Player One joined");
                if (playerTwo == null)
                    await playerOne.SendMessage("Waiting for Player Two to join");
                result = AddUserToLobbyResult.AddedToLobby;
            }
            else if (playerTwo == null)
            {
                playerTwo = user;
                if (playerOne != null)
                    await playerOne.SendMessage("Player Two joined");
                if (playerOne == null)
                    await playerTwo.SendMessage("Waiting for Player One to join");
                result = AddUserToLobbyResult.AddedToLobby;
            }
            else
            {
                return AddUserToLobbyResult.FailedToAddLobbyFull;
            }

            if (playerTwo != null && playerOne != null)
            {
                await playerOne.SendMessage(gameboard.ToGameString());
                await playerTwo.SendMessage(gameboard.ToGameString());
                await playerOne.SendMessage(isPlayOneTurn ? $"{playerOne.GetUserName()} turn" : $"{playerTwo.GetUserName()} turn");
                await playerTwo.SendMessage(isPlayOneTurn ? $"{playerOne.GetUserName()} turn" : $"{playerTwo.GetUserName()} turn");
            }

            return result;
        }

        public Guid GetLobbyId()
        {
            return _id;
        }

        public string GetLobbyType()
        {
            return LOBBY_TYPE;
        }

        public bool IsLobbyEmpty()
        {
            return playerTwo == null && playerOne == null;
        }

        public async Task ProcessClientMessage(string message, IUser user)
        {
            if(playerOne == null || playerTwo == null)
            {
                await user.SendMessage("Waiting on other player");
                return;
            }

            if (gameState != TicTacToeLobbyGameBoardState.OnGoing)
            {
                await user.SendMessage("The game is over, please leave the lobby with :leave lobby");
                return;
            }

            if ((user == playerTwo && isPlayOneTurn) || (user == playerOne && !isPlayOneTurn))
            {
                await user.SendMessage("Not your turn");
                return;
            }

            bool didMove;
            if (message == "1")
            {
                didMove = TryMoveToLocation(0, 0, user == playerOne ? TicTacToeGameBoardCellState.X : TicTacToeGameBoardCellState.O);
            }
            else if (message == "2")
            {
                didMove = TryMoveToLocation(1, 0, user == playerOne ? TicTacToeGameBoardCellState.X : TicTacToeGameBoardCellState.O);
            }
            else if (message == "3")
            {
                didMove = TryMoveToLocation(2, 0, user == playerOne ? TicTacToeGameBoardCellState.X : TicTacToeGameBoardCellState.O);
            }
            else if (message == "4")
            {
                didMove = TryMoveToLocation(0, 1, user == playerOne ? TicTacToeGameBoardCellState.X : TicTacToeGameBoardCellState.O);
            }
            else if (message == "5")
            {
                didMove = TryMoveToLocation(1, 1, user == playerOne ? TicTacToeGameBoardCellState.X : TicTacToeGameBoardCellState.O);
            }
            else if (message == "6")
            {
                didMove = TryMoveToLocation(2, 1, user == playerOne ? TicTacToeGameBoardCellState.X : TicTacToeGameBoardCellState.O);
            }
            else if (message == "7")
            {
                didMove = TryMoveToLocation(0, 2, user == playerOne ? TicTacToeGameBoardCellState.X : TicTacToeGameBoardCellState.O);
            }
            else if (message == "8")
            {
                didMove = TryMoveToLocation(1, 2, user == playerOne ? TicTacToeGameBoardCellState.X : TicTacToeGameBoardCellState.O);
            }
            else if (message == "9")
            {
                didMove = TryMoveToLocation(2, 2, user == playerOne ? TicTacToeGameBoardCellState.X : TicTacToeGameBoardCellState.O);
            }
            else
            {
                await user.SendMessage("INVAILD MOVE");
                return;
            }

            if (!didMove) { 
                await user.SendMessage($"INVAILD MOVE");
                return;
            }

            await playerOne.SendMessage(gameboard.ToGameString());
            await playerTwo.SendMessage(gameboard.ToGameString());

            if(gameboard.DidTagWin(isPlayOneTurn ? TicTacToeGameBoardCellState.X : TicTacToeGameBoardCellState.O))
            {
                var winningMessage = isPlayOneTurn ? $"{playerOne.GetUserName()} Won!\n" : $"{playerTwo.GetUserName()} Won!\n";
                await playerOne.SendMessage(winningMessage);
                await playerTwo.SendMessage(winningMessage);
                await playerOne.SendMessage("The game is over, please leave the lobby with :leave lobby");
                await playerTwo.SendMessage("The game is over, please leave the lobby with :leave lobby");
                gameState = TicTacToeLobbyGameBoardState.GameOver;
            }
            else if(gameboard.IsTie())
            {
                await playerOne.SendMessage("Game is tie!");
                await playerTwo.SendMessage("Game is tie!");
                await playerOne.SendMessage("The game is over, please leave the lobby with :leave lobby");
                await playerTwo.SendMessage("The game is over, please leave the lobby with :leave lobby");
                gameState = TicTacToeLobbyGameBoardState.GameOver;
            }

            isPlayOneTurn = !isPlayOneTurn;
            await playerOne.SendMessage(isPlayOneTurn ? $"{playerOne.GetUserName()} turn" : $"{playerTwo.GetUserName()} turn");
            await playerTwo.SendMessage(isPlayOneTurn ? $"{playerOne.GetUserName()} turn" : $"{playerTwo.GetUserName()} turn");
        }

        public async Task RemoveUser(IUser user)
        {
            if (playerOne == user)
            {
                playerOne = null;
                if (playerTwo != null)
                    await playerTwo.SendMessage($"{playerOne.GetUserName()} left the lobby");
            }
            else if (playerTwo == user)
            {
                playerTwo = null;
                if (playerOne != null)
                    await playerOne.SendMessage($"{playerTwo.GetUserName()} left the lobby");
            }
        }

        public void StartLobby()
        {
        }

        public void StopLobby()
        {
        }

        private bool TryMoveToLocation(int x, int y, TicTacToeGameBoardCellState playerTag)
        {
            if (gameboard[x, y] != TicTacToeGameBoardCellState.Empty)
                return false;
            gameboard[x, y] = playerTag;
            return true;
        }
    }
}

using Varvarin_Mud_Plus.Engine.Lobby;

namespace Varvarin_Mud_Plus.Engine.Extensions
{
    public static class TicTacToeGameBoardCellState2dArrayExtensions
    {

        public static bool IsTie(this TicTacToeGameBoardCellState[,] game)
        {
            for (var x = 0; x < game.GetLength(0); x++)
            {
                for (var y = 0; y < game.GetLength(1); y++)
                {
                    if (game[x, y] == TicTacToeGameBoardCellState.Empty)
                        return false;
                }
            }
            return true;
        }

        public static bool DidTagWin(this TicTacToeGameBoardCellState[,] game, TicTacToeGameBoardCellState tag)
        {
            if (game[0, 0] == tag && game[0, 1] == tag && game[0, 2] == tag)
                return true;
            if (game[1, 0] == tag && game[1, 1] == tag && game[1, 2] == tag)
                return true;
            if (game[2, 0] == tag && game[2, 1] == tag && game[2, 2] == tag)
                return true;
            if (game[0, 0] == tag && game[1, 0] == tag && game[2, 0] == tag)
                return true;
            if (game[0, 1] == tag && game[1, 1] == tag && game[2, 1] == tag)
                return true;
            if (game[0, 2] == tag && game[1, 2] == tag && game[2, 2] == tag)
                return true;
            if (game[0, 0] == tag && game[1, 1] == tag && game[2, 2] == tag)
                return true;
            if (game[2, 0] == tag && game[1, 1] == tag && game[0, 2] == tag)
                return true;

            return false;
        }

        public static string ToGameString(this TicTacToeGameBoardCellState[,] game)
        {
            return $@"
|---|---|---|
| {GetCellNumberOrTag(0,0, game)} | {GetCellNumberOrTag(1, 0, game)} | {GetCellNumberOrTag(2, 0, game)} |
|---|---|---|
| {GetCellNumberOrTag(0,1, game)} | {GetCellNumberOrTag(1, 1, game)} | {GetCellNumberOrTag(2, 1, game)} |
|---|---|---|
| {GetCellNumberOrTag(0,2, game)} | {GetCellNumberOrTag(1, 2, game)} | {GetCellNumberOrTag(2, 2, game)} |
";
        }

        public static void SetInitState(this TicTacToeGameBoardCellState[,] game)
        {
            for(var x = 0; x < game.GetLength(0); x++)
            {
                for (var y = 0; y < game.GetLength(1); y++)
                {
                    game[x,y] = TicTacToeGameBoardCellState.Empty;
                }
            }
        }

        private static string GetCellNumberOrTag(int x, int y, TicTacToeGameBoardCellState[,] game)
        {

            if(game[x,y] != TicTacToeGameBoardCellState.Empty)
                return game[x, y] == TicTacToeGameBoardCellState.X ? "X" : "O";

            if (x == 0 && y == 0)
                return "1";
            else if (x == 1 && y == 0)
                return "2";
            else if (x == 2 && y == 0)
                return "3";
            else if (x == 0 && y == 1)
                return "4";
            else if (x == 1 && y == 1)
                return "5";
            else if (x == 2 && y == 1)
                return "6";
            else if (x == 0 && y == 2)
                return "7";
            else if (x == 1 && y == 2)
                return "8";
            return "9";
        }
    }
}

using static System.Console;
using System.Collections.Generic;



public class SortingMoves {

    CheckBoard check = new CheckBoard();

    public SortingMoves() { }

    public List<(int, int, int)> GetSortList(int[,,] board, int depth, List<(int, int, int)> freeSquares, 
        int turn, int opponent, out (int, int, int)? bestMove) {

        bestMove = null; 
        (int, int, int)? WinningMove = null; 
        (int, int, int)? BlockingMove = null; 
        (int, int, int)? BlockMove = null; 

        List<KeyValuePair<(int, int, int), int>> BestResults = new List<KeyValuePair<(int, int, int), int>>();

        foreach (var square in freeSquares) {
            int z = square.Item1;
            int x = square.Item2;
            int y = square.Item3;
            BestResults.Add(new KeyValuePair<(int, int, int), int>((z, x, y), 
            check.GetValue(board, turn, opponent, z, x, y, out WinningMove, out BlockingMove)));

            if (WinningMove != null && depth == 0) {
                bestMove = WinningMove;
                return new List<(int, int, int)>();
            }
            if (BlockingMove != null && depth == 0){
                BlockMove = BlockingMove;
            }
        }

        if (BlockMove != null && depth == 0) {
            bestMove = BlockMove;
            return new List<(int, int, int)>();
        }

        var sortedList = BestResults.OrderByDescending(pair => pair.Value).ToList();
        int limit = Math.Min(freeSquares.Count, 10);
        List<(int, int, int)> sortedMoves = new List<(int, int, int)>();

        foreach (var pair in sortedList) {
            if (limit == 0) {
                break;
            }
            var (z, x, y) = pair.Key;
            sortedMoves.Add((z, x, y));
            limit--;
        }
        return sortedMoves;
    }
}
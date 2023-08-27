using static System.Console;
using System.Collections.Generic;



public class SortingMoves {

    private const int maxCount = 10;

    CheckBoard check = new CheckBoard();

    public SortingMoves() { }

    public List<(int, int, int)> GetSortList(int[,,] board, int depth, List<(int, int, int)> freeSquares, 
        int turn, int opponent, out (int, int, int)? bestMove) {
        
        bestMove = null; 

        // BlockingMove and BlockMove fixed a bug, it wasnt updating properly and it was always null
        // because first time BlockingMove is updated is no longer null but then in the next loop
        // in the CheckBoard is becomes null again, so BlockMove is necessary 

        (int, int, int)? WinningMove = null; 
        (int, int, int)? BlockingMove = null; 
        (int, int, int)? BlockMove = null; 

        List<KeyValuePair<(int, int, int), int>> BestResults = new List<KeyValuePair<(int, int, int), int>>();

        // getting the scores of all current free squares
        foreach (var square in freeSquares) {
            int z = square.Item1;
            int x = square.Item2;
            int y = square.Item3;
            BestResults.Add(new KeyValuePair<(int, int, int), int>((z, x, y), 
            check.GetValue(board, turn, opponent, z, x, y, out WinningMove, out BlockingMove)));

            if (WinningMove != null && depth == 0) {
                // if we have a winner move we can end the game and win 
                bestMove = WinningMove;
                return new List<(int, int, int)>();
            }
            if (BlockingMove != null && depth == 0){
                BlockMove = BlockingMove;
            }
        }

        if (BlockMove != null && depth == 0) {
            // give total priority to block the user when there is 5 tokens
            // of the opponent aligned 
            bestMove = BlockMove;
            return new List<(int, int, int)>();
        }

        // sorting by the scores in descencing order
        var sortedList = BestResults.OrderByDescending(pair => pair.Value).ToList();
        // in the case there are less than 10 squares free
        int limit = Math.Min(freeSquares.Count, maxCount);
        List<(int, int, int)> sortedMoves = new List<(int, int, int)>();

        foreach (var pair in sortedList) {
            if (limit == 0) {
                break;
            }
            var (z, x, y) = pair.Key;
            sortedMoves.Add((z, x, y));
            limit--;
        }
        // returning top 10 moves 
        return sortedMoves;
    }
}
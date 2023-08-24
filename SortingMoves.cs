using static System.Console;
using System.Collections.Generic;



public class SortingMoves {

    (int, int, int)? FinalMove; // identify it when in the winner case we have 
    (int, int, int)? FinalBlockingMove; // identify it when the winner case of the oponent 
    int dimention;
    int turn; 
    int opponent;

    //List of KeyValuePairs for 

    public SortingMoves(int dimention, int turn, int opponent) {
        this.dimention = dimention;
        this.turn = turn;
        this.opponent = opponent;
    }

    public List<(int, int, int)> getSortList(int[,,] matrix, int depth, List<(int, int, int)> freeSquares, out (int, int, int)? bestMove) {
        bestMove = null; // ?? is this null?
        FinalMove = null; 
        FinalBlockingMove = null; 

        List<KeyValuePair<(int, int, int), int>> BestResults = new List<KeyValuePair<(int, int, int), int>>();

        foreach (var square in freeSquares) {
            int z = square.Item1;
            int x = square.Item2;
            int y = square.Item3;
            BestResults.Add(new KeyValuePair<(int, int, int), int>((z, x, y), getValue(matrix, z, x, y)));

            if (FinalMove != null && depth == 0) {
                bestMove = FinalMove;
                return new List<(int, int, int)>();
            }
        }

        if (FinalBlockingMove != null && depth == 0) {
            bestMove = FinalBlockingMove;
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

    int getValue(int[,,] matrix, int z, int x, int y){
        // current cell is empty so if row/col/diagonal are empty so it wont sum up
        bool next = false;

        int increment1 = checkRowsCols(matrix, z, x, y, out next);
        if (next) {
            int increment2 = checkAllDiagonals(matrix, z, x, y);
            return increment1 + increment2 + 1; // maybe take this one out 
        }
        else {
            return increment1 + 1;
        }

    }

    int diagonalSameLevel(int[,,] matrix, int z, bool order, int bestz, int bestx, int besty, out bool next) { // we dont need values of rows or cols
        next = true;
        bool mixed = false;
        int winningScore = 0;
        int blockingScore = 0;

        for (int k = 0; k < dimention; ++k) {
            if (order) {
                if (matrix[z, k, k] == turn){ // 00 11 22 
                    winningScore++;
                }
                if (matrix[z, k, k] == opponent){ 
                    blockingScore++;
                }
                if (winningScore >= 1 && blockingScore >= 1){
                    mixed = true;
                }
            }
            if (!order) {
                if (matrix[z, k, ((dimention-1) - k)] == turn){ // 05 14 23 
                    winningScore++;
                }
                if (matrix[z, k, ((dimention-1) - k)] == opponent){ // 05 14 23 
                    blockingScore++;
                }
                if (winningScore >= 1 && blockingScore >= 1){
                    mixed = true;
                }
            }
            if (mixed) {
                return 0;
            }
        }
        if (winningScore == (dimention - 1)) {
            next = false;
            FinalMove = (bestz, bestx, besty);
        }
        if (blockingScore == (dimention - 1)) {
            FinalBlockingMove = (bestz, bestx, besty);
        }
        return Math.Max(winningScore*2, blockingScore);
    }
    
    int diagonalColumnTop_to_ColumnBotton(int[,,] matrix, int x, bool order, int bestz, int bestx, int besty, out bool next) { // true is level == y 
        // checks for any square if its diagonal from row x and level 0 to 5 or level 5 to 0
        next = true;
        bool mixed = false;
        int winningScore = 0;
        int blockingScore = 0;

        int level = order ? 0 : (dimention-1);
        for (int k = 0; k < dimention; ++k) {
            if (matrix[level, x, k] == turn){
                winningScore++;
            }
            if (matrix[level, x, k] == opponent){
                blockingScore++;
            }
            if(winningScore >= 1 && blockingScore >= 1){
                mixed = true;
            }
            if (order) {
                level++;
            }
            else {
                level--;
            }
            if (mixed) {
                return 0;
            }
        }
        if (winningScore == (dimention - 1)) {
            next = false;
            FinalMove = (bestz, bestx, besty);
        }
        if (blockingScore == (dimention - 1)) {
            FinalBlockingMove = (bestz, bestx, besty);
        }
        return Math.Max( winningScore*2, blockingScore);
    }

    int diagonalRowTop_to_RowBotton(int[,,] matrix, int y, bool order, int bestz, int bestx, int besty, out bool next) {
        next = true;
        bool mixed = false;
        int winningScore = 0;
        int blockingScore = 0;
        int level = order ? 0 : (dimention-1);

        for (int k = 0; k < dimention; ++ k) {
            if (matrix[level, k, y] == turn){
                winningScore++;
            }
            if (matrix[level, k, y] == opponent){
                blockingScore++;
            }
            if (winningScore >= 1 && blockingScore >= 1){
                mixed = true;
            }
            if (order) {
                level++;
            }
            else {
                level--;
            }
            if (mixed){
                return 0;
            }
        }
        if (winningScore == (dimention - 1)) {
            next = false;
            FinalMove = (bestz, bestx, besty);
        }
        if (blockingScore == (dimention - 1)) {
            FinalBlockingMove = (bestz, bestx, besty);
        }
        return Math.Max(winningScore*2, blockingScore);
        

    }

    int diagonalBothDirectionsfrom00(int[,,] matrix, bool order, int bestz, int bestx, int besty, out bool next) {
        next = true;
        bool mixed = false;
        int winningScore = 0;
        int blockingScore = 0;
        int level = order ? 0 : (dimention-1);
        for (int k = 0; k < dimention; ++k) {
            if (matrix[level, k, k] == turn) {
                winningScore++;
            }
            if (matrix[level, k, k] == opponent) {
                blockingScore++;
            }
            if (winningScore >= 1 && blockingScore >= 1){
                mixed = true;
            }
            if (order) {
                level++;
            }
            else {
                level--;
            }
            if (mixed){
                return 0;
            }
        }
        if (winningScore == (dimention - 1)) {
            next = false;
            FinalMove = (bestz, bestx, besty);
        }
        if (blockingScore == (dimention - 1)) {
            FinalBlockingMove = (bestz, bestx, besty);
        }
        return Math.Max(winningScore*2, blockingScore);
    }

    int diagonalBothDirectionsfrom05(int[,,] matrix, bool order, int bestz, int bestx, int besty, out bool next) { // z == x
        next = true;
        bool mixed = false;
        int winningScore = 0;
        int blockingScore = 0;
        int level = 0;
        for (int k = (dimention-1); k >= 0; --k) {
            if (order){
                if (matrix[level, level, k] == turn) { // 0 
                    winningScore++;
                }
                if (matrix[level, level, k] == opponent) { // 0 
                    blockingScore++;
                }
            }
            if (!order){
                if (matrix[level, k, level] == turn) {
                    winningScore++;
                }
                if (matrix[level, k, level] == opponent) {
                    blockingScore++;
                }
            }
            if (winningScore >= 1 && blockingScore >= 1){
                mixed = true;
            }
            if (mixed){
                return 0;
            }
            level++;
        }

        if (winningScore == (dimention - 1)) {
            next = false;
            FinalMove = (bestz, bestx, besty);
        }
        if (blockingScore == (dimention - 1)) {
            FinalBlockingMove = (bestz, bestx, besty);
        }
        return Math.Max(winningScore*2, blockingScore);
    }



    int checkRowsCols(int[,,] matrix,  int z, int x, int y, out bool continuebool) {    
        continuebool = true;  
        bool first = true;
        bool second = true;
        bool thrid = true;
        int rowTurn = 0;
        int colTurn = 0;
        int col_by_levelsTurn = 0;
        int rowOpponent = 0;
        int colOpponent = 0;
        int col_by_levelsOpponent = 0;

        for(int k = 0; k < dimention; ++k) {

            if (matrix[z, k, y] == turn){
                rowTurn++;
            }
            if (matrix[z, k, y] == opponent){
                rowOpponent++;
            } 
            if (rowOpponent >= 1 && rowOpponent >= 1){ // it is already blocked and not possible for anyone to advance
                first = false;
            }

            if (matrix[z, x, k] == turn) {
                colTurn++;
            }
            if (matrix[z, x, k] == opponent) {
                colOpponent++;
            }
            if (colTurn >= 1 && colOpponent >= 1){
                second = false;
            }

            if (matrix[k, x, y] == turn){
                col_by_levelsTurn++;
            } 
            if (matrix[k, x, y] == opponent){
                col_by_levelsOpponent++;
            } 
            if (col_by_levelsOpponent >= 1 && col_by_levelsOpponent >= 1){
                thrid = false;
            }
            if (first == false && second == false && thrid == false){
                continuebool = false;
                return 0;
            }
        }

        if (rowTurn == (dimention - 1) || colTurn == (dimention - 1) || col_by_levelsTurn == (dimention - 1)) {
            continuebool = false;
            FinalMove = (z, x, y);
        }
        if (rowOpponent == (dimention - 1) || colOpponent == (dimention - 1) || col_by_levelsOpponent == (dimention - 1)){
            FinalBlockingMove = (z, x, y);
        }
        if (first == false || second == false || thrid == false){ // we might continue to check the diagonals 
            return Math.Max(rowTurn, Math.Max(colTurn, col_by_levelsTurn));
        }
        else {
            int maxTurn = Math.Max(rowTurn, Math.Max(colTurn, col_by_levelsTurn)); 
            int maxOpponent = Math.Max(rowOpponent, Math.Max(colOpponent, col_by_levelsOpponent));
            return Math.Max(maxTurn*2, maxOpponent);
        }
    }
    
    int checkAllDiagonals(int[,,] matrix, int z, int x, int y){

        bool next = true;
        int totalDiagonalwinningScore = 0;

        if ( (x + y) == (dimention-1)  || x == y ) {
            totalDiagonalwinningScore += diagonalSameLevel(matrix, z, x == y, z, x, y, out next);
        }

        if ( ( y == z || ( y + z) == (dimention-1) )  && next ) {
            totalDiagonalwinningScore += diagonalColumnTop_to_ColumnBotton(matrix, x, y == z, z, x, y, out next);
        }

        if (( x == z || (x + z) == (dimention-1) ) && next) {
            totalDiagonalwinningScore += diagonalRowTop_to_RowBotton(matrix, y, x == z, z, x, y, out next);
        }

        if ( (x == y && ( x == z || (z + x) == (dimention-1))) && next){
            totalDiagonalwinningScore += diagonalBothDirectionsfrom00(matrix, x == z, z, x, y, out next);
        }
        if ( ((x + y) == (dimention-1)  && ( z == y || z == x )) && next ) {
            totalDiagonalwinningScore += diagonalBothDirectionsfrom05(matrix, z == x, z, x, y, out next);
        }
        return totalDiagonalwinningScore;
    }

}


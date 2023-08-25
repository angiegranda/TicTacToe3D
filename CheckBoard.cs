using System;
using static System.Console;

public class CheckBoard {

    private int dimention; 
    private (int, int, int)? WinningMove;
    private (int, int, int)? BlockingMove;
    private (int, int, int)? Victory;
    private int turn;
    private int opponent;

    public CheckBoard() { }

    public void Eval(int[,,] board, int currentTurn, int z, int x, int y, 
                    out bool wongame, out int winner) {
        dimention = board.GetLength(1);
        winner = -1;
        wongame = false;
        turn = currentTurn;
        opponent = (3 - turn);
        Victory = null;

        int increment1 = CheckRowCol(board, z, x, y);
        int increment2 = CheckAllDiagonals(board, z, x, y);

        if (Victory != null) {
            winner = turn;
            wongame = true;
        }
    }

    public int GetValue(int[,,] board, int currentTurn, int opponentTurn, int z, int x, int y, 
                        out (int, int, int)? winMove, out (int, int, int)? blockMove){

        dimention = board.GetLength(1);
        WinningMove = null; winMove = null;
        BlockingMove = null; blockMove = null;
        turn = currentTurn;
        opponent = (3 - turn);

        int increment1 = CheckRowCol(board, z, x, y);
        int increment2 = CheckAllDiagonals(board, z, x, y);
        winMove = WinningMove; blockMove = BlockingMove; // updated twice

        return increment1 + increment2 + 1;

    }


    private int DiagonalSameLevel(int[,,] board, int z, bool order, int a, int b, int c) { // we dont need values of rows or cols
        bool mixed = false;
        int winningScore = 0;
        int blockingScore = 0;

        for (int k = 0; k < dimention; ++k) {
            if (order) {
                if (board[z, k, k] == turn){ // 00 11 22 
                    winningScore++;
                }
                if (board[z, k, k] == opponent){ 
                    blockingScore++;
                }
            }
            if (!order) {
                if (board[z, k, ((dimention-1) - k)] == turn){ // 05 14 23 
                    winningScore++;
                }
                if (board[z, k, ((dimention-1) - k)] == opponent){ // 05 14 23 
                    blockingScore++;
                }
            }
            if (winningScore >= 1 && blockingScore >= 1){
                mixed = true;
            }
            if (mixed) {
                return 0;
            }
        }
        if (winningScore == (dimention - 1)) {
            WinningMove = (a, b, c);
        }
        if (winningScore == dimention){
            Victory = (a, b, c);
        }
        if (blockingScore == (dimention - 1)) {
            BlockingMove = (a, b, c);
        }
        return Math.Max(winningScore*2, blockingScore);
    }
    
    private int DiagonalColumnTop_to_ColumnBotton(int[,,] board, int x, bool order, int a, int b, int c) { // true is level == y 
        // checks for any square if its diagonal from row x and level 0 to 5 or level 5 to 0
        bool mixed = false;
        int winningScore = 0;
        int blockingScore = 0;

        int level = order ? 0 : (dimention-1);
        for (int k = 0; k < dimention; ++k) {
            if (board[level, x, k] == turn){
                winningScore++;
            }
            if (board[level, x, k] == opponent){
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
            WinningMove = (a, b, c);
        }
        if (winningScore == dimention) {
            Victory = (a, b, c);
        }
        if (blockingScore == (dimention - 1)) {
            BlockingMove = (a, b, c);
        }
        return Math.Max( winningScore*2, blockingScore);
    }

    private int DiagonalRowTop_to_RowBotton(int[,,] board, int y, bool order, int a, int b, int c) {

        bool mixed = false;
        int winningScore = 0; int blockingScore = 0;
        int level = order ? 0 : (dimention-1);

        for (int k = 0; k < dimention; ++ k) {
            if (board[level, k, y] == turn){
                winningScore++;
            }
            if (board[level, k, y] == opponent){
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
            WinningMove = (a, b, c);
        }
        if (winningScore == dimention){
            Victory = (a, b, c);
        }
        if (blockingScore == (dimention - 1)) {
            BlockingMove = (a, b, c);
        }
        return Math.Max(winningScore*2, blockingScore);
    
    }

    private int DiagonalBothDirectionsfrom00(int[,,] board, bool order, int a, int b, int c) {
        bool mixed = false;
        int winningScore = 0; int blockingScore = 0;
        int level = order ? 0 : (dimention-1);
        
        for (int k = 0; k < dimention; ++k) {
            if (board[level, k, k] == turn) {
                winningScore++;
            }
            if (board[level, k, k] == opponent) {
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
            WinningMove = (a, b, c);
        }
        if (winningScore == dimention){
            Victory = (a, b, c);
        }
        if (blockingScore == (dimention - 1)) {
            BlockingMove = (a, b, c);
        }
        return Math.Max(winningScore*2, blockingScore);
    }

    private int DiagonalBothDirectionsfrom05(int[,,] board, bool order, int a, int b, int c) { // z == x

        bool mixed = false; int winningScore = 0; 
        int blockingScore = 0; int level = 0; 

        for (int k = (dimention-1); k >= 0; --k) {
            if (order){
                if (board[level, level, k] == turn) { // 0 
                    winningScore++;
                }
                if (board[level, level, k] == opponent) { // 0 
                    blockingScore++;
                }
            }
            if (!order){
                if (board[level, k, level] == turn) {
                    winningScore++;
                }
                if (board[level, k, level] == opponent) {
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
            WinningMove = (a, b, c);
        }
        if (winningScore == dimention){
            Victory = (a, b, c);
        }
        if (blockingScore == (dimention - 1)) {
            BlockingMove = (a, b, c);
        }
        return Math.Max(winningScore*2, blockingScore);
    }

    private int CheckRowCol(int[,,] board,  int z, int x, int y) {   

        bool first = true; bool second = true; bool thrid = true;
        int rowTurn = 0; int colTurn = 0; int col_by_levelsTurn = 0;
        int rowOpponent = 0; int colOpponent = 0; int col_by_levelsOpponent = 0;
        int limit = dimention - 1;

        for(int k = 0; k < dimention; ++k) {
            // rows same level
            if (board[z, k, y] == turn){  rowTurn++;  }
            if (board[z, k, y] == opponent){  rowOpponent++; } 
            if (rowTurn >= 1 && rowOpponent >= 1){  first = false; }
            // cols same level
            if (board[z, x, k] == turn) {  colTurn++; }
            if (board[z, x, k] == opponent) {  colOpponent++; }
            if (colTurn >= 1 && colOpponent >= 1){  second = false; }
            // col in different level
            if (board[k, x, y] == turn){  col_by_levelsTurn++; } 
            if (board[k, x, y] == opponent) { col_by_levelsOpponent++;  } 
            if (col_by_levelsTurn >= 1 && col_by_levelsOpponent >= 1){ thrid = false; }

            if ( first == false && second == false && thrid == false ){ 
                return 0;
            }
        }
        if (rowTurn == limit || colTurn == limit || col_by_levelsTurn == limit) {
            WinningMove = (z, x, y);
        }
        if (rowTurn == dimention || colTurn == dimention || col_by_levelsTurn == dimention) {
            Victory = (z, x, y);
        }
        if (rowOpponent == limit || colOpponent == limit|| col_by_levelsOpponent == limit){
            BlockingMove = (z, x, y);
        }
        if ( first == false || second == false || thrid == false ){ // we may continue to check the diagonals 
            return Math.Max(rowTurn, Math.Max(colTurn, col_by_levelsTurn));
        }
        else {
            int maxTurn = Math.Max(rowTurn, Math.Max(colTurn, col_by_levelsTurn)); 
            int maxOpponent = Math.Max(rowOpponent, Math.Max(colOpponent, col_by_levelsOpponent));
            return Math.Max(maxTurn*2, maxOpponent);
        }
    }
    
    private int CheckAllDiagonals(int[,,] board, int z, int x, int y){

        int totalDiagonalScore = 0;
        int limit = dimention - 1;

        if ( (x + y) == limit  || x == y ) {
            totalDiagonalScore += DiagonalSameLevel(board, z, x == y, z, x, y);
        }

        if ( y == z || ( y + z) == limit )  {
            totalDiagonalScore += DiagonalColumnTop_to_ColumnBotton(board, x, y == z, z, x, y);
        }

        if ( x == z || (x + z) == limit )  {
            totalDiagonalScore += DiagonalRowTop_to_RowBotton(board, y, x == z, z, x, y);
        }

        if  (x == y && ( x == z || (z + x) == limit)) {
            totalDiagonalScore+= DiagonalBothDirectionsfrom00(board, x == z, z, x, y);
        }
        if  ((x + y) == limit  && ( z == y || z == x )) {
            totalDiagonalScore += DiagonalBothDirectionsfrom05(board, z == x, z, x, y);
        }

        return totalDiagonalScore;
    }

}


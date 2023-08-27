using System;
using static System.Console;

public class CheckBoard {

    private const int dimention = 6; 
    private (int, int, int)? WinningMove; // Empty square that can lead to a win
    private (int, int, int)? BlockingMove; // Empty square that can block the opponent from winning 
    private (int, int, int)? Victory; // Already filled square that proofs there is a winner, game over
    private int turn;
    private int opponent;

    public CheckBoard() { }

    // this is called from AlphaBetaPruning() - Minimax class - and Move() - Game class - 
    // main purpose is to check is the current (already filled) square 
    // gives a victory to that turn - could be the opponent or AI - 

    public void Eval(int[,,] board, int currentTurn, int z, int x, int y, out int winner) {

        // initializing
        winner = -1;

        // updating variables
        turn = currentTurn;
        opponent = (3 - turn);
        Victory = null;

        // getting all possible scores 
        int increment1 = CheckRowCol(board, z, x, y);
        int increment2 = CheckAllDiagonals(board, z, x, y);

        // if Victory - 6 Tokens of that turn -  has been found
        // AlphaBetaPruning() wants winner value, Move()
        if (Victory != null) {
            winner = turn;
        }

    }

    // this is called from GetSortList()  SortingMoves class
    
    public int GetValue(int[,,] board, int currentTurn, int opponentTurn, int z, int x, int y, 
                        out (int, int, int)? winMove, out (int, int, int)? blockMove){
        
        // initializing

        WinningMove = null; winMove = null;
        BlockingMove = null; blockMove = null;

        turn = currentTurn;
        opponent = (3 - turn);

        // getting the scores  
        int increment1 = CheckRowCol(board, z, x, y);
        int increment2 = CheckAllDiagonals(board, z, x, y);
        winMove = WinningMove; blockMove = BlockingMove; // updated twice

        return increment1 + increment2 + 1;

    }


    private int DiagonalSameLevel(int[,,] board, int z, bool order, int a, int b, int c) { 
        
        bool mixed = false;
        int winningScore = 0;
        int blockingScore = 0;

        for (int k = 0; k < dimention; ++k) {

            if (order) {  // 00 11 22 
                if (board[z, k, k] == turn){ 
                    winningScore++;
                }
                if (board[z, k, k] == opponent){  
                    blockingScore++;
                }
            }

            if (!order) {  // 05 14 23 
                if (board[z, k, ((dimention-1) - k)] == turn){ 
                    winningScore++;
                }
                if (board[z, k, ((dimention-1) - k)] == opponent){ 
                    blockingScore++;
                }
            }

            // if we have mixed Tokes it is not worthy considering this diagonal
            if (winningScore >= 1 && blockingScore >= 1){
                mixed = true;
            }

            if (mixed) { // no increment
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

        // gives twice preference to keep completing diagonal/row/col
        // that can lead to a win, than to block the opponent

        return Math.Max(winningScore * 2, blockingScore);
    }
    
    private int DiagonalColumnTop_to_ColumnBotton(int[,,] board, int x, bool order, int a, int b, int c) { 

        // true is matrix == column , see devdocs.md for examples
        // checks the diagonal that starts at any corner square in a column
        // throught the levels, to the other corner on the opposite level
        // keeping the value of the row unchanged
        
        // initializing 
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
            // Mixed Tokens, no need to keep the loop 
            if(winningScore >= 1 && blockingScore >= 1){
                mixed = true;
            }

            if (order) {
                level++;
            }
            else {
                level--;
            }
            // exit is mixed Tokens
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

        // true if matrix == row , see devdocs.md for examples
        // takes any square in the corners of a row, keep staying in the same column number
        // and goes through the levels to the opposite corner

        // initalizing 
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
            // mixed Tokens
            if (winningScore >= 1 && blockingScore >= 1){
                mixed = true;
            }
            if (order) {
                level++;
            }
            else {
                level--;
            }
            // exit if mixed
            if (mixed){
                return 0;
            }
        }

        // updating variables 
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
        
        // Checks main diagonal that goes through levels from corner 00 to 05 
        // it can be in both directions from level 0 to 5 or level 5 to 0

        //initializing
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
            // Mixed tokens 
            if (winningScore >= 1 && blockingScore >= 1){
                mixed = true;
            }

            if (order) {
                level++;
            }
            else {
                level--;
            }
            // exit if mixed
            if (mixed){
                return 0;
            }
        }

        // updatig variables
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

    private int DiagonalBothDirectionsfrom05(int[,,] board, bool order, int a, int b, int c) { 

        // Checks main diagonal through levels from 05 to 50, from level 0 to 5 or level 5 to 0

        // initializing 
        bool mixed = false; int winningScore = 0; 
        int blockingScore = 0; int level = 0; 

        for (int k = (dimention-1); k >= 0; --k) {

            // from level 0 to 5
            if (order){
                if (board[level, level, k] == turn) { 
                    winningScore++;
                }
                if (board[level, level, k] == opponent) { 
                    blockingScore++;
                }
            }

            // from level 5 to 0 
            if (!order){
                if (board[level, k, level] == turn) {
                    winningScore++;
                }
                if (board[level, k, level] == opponent) {
                    blockingScore++;
                }
            }
            // Mixed tokens
            if (winningScore >= 1 && blockingScore >= 1){
                mixed = true;
            }
            // exit if mixed 
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
        // initializing 
        bool first = true; bool second = true; bool thrid = true;
        int rowTurn = 0; int colTurn = 0; int col_by_levelsTurn = 0;
        int rowOpponent = 0; int colOpponent = 0; int col_by_levelsOpponent = 0;
        int limit = dimention - 1;

        for(int k = 0; k < dimention; ++k) {

            // rows same level
            if (board[z, k, y] == turn){  rowTurn++; }
            if (board[z, k, y] == opponent){  rowOpponent++; } 
            if (rowTurn >= 1 && rowOpponent >= 1){  first = false; } // Mixed Tokens 

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

        // if we have mixed tokens in a row/col, we look for the biggest score
        if ( first == false || second == false || thrid == false ){ 
            return Math.Max(rowTurn, Math.Max(colTurn, col_by_levelsTurn));
        }

        else { // if there is anything mixed, it means it is either full of opponent tokens
                // or full of current player tokens, we choose the most completed row/col
            int maxTurn = Math.Max(rowTurn, Math.Max(colTurn, col_by_levelsTurn)); 
            int maxOpponent = Math.Max(rowOpponent, Math.Max(colOpponent, col_by_levelsOpponent));
            return Math.Max(maxTurn*2, maxOpponent);
        }
    }
    
    private int CheckAllDiagonals(int[,,] board, int z, int x, int y){

        //         z = matrix   x  = row    y = column

        int totalDiagonalScore = 0;
        int limit = dimention - 1;

        // checks all possible directions, edges squares has the most number of diagonals 
        //square [5,0,1] has 4 : 3 in CheckRowCol() and 1 in DiagonalColumnTop_to_ColumnBotton()
        
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


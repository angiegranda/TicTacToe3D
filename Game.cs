using System;
using static System.Console;
using System.Collections.Generic;


// I should use const int for general values 
// I should set a counter value of occupied cells, if they are all occupied
// then it is  DRAW , but we should set a minimum number 
// total number of cells are 216 if 200 are occupied then DRAW

class Game {

    const int size = 6; // svodova advice

    int[,,] board = new int[size,size,size];
    public int turn = 1;  
    public int winner = 0; 
    int count = 0;

    public int this[int z, int x, int y] { // indexer 
        get => board[z, x, y];
    }

    public int? Winner() =>  (winner != 0) ? winner : null;  // COMMIT CHANGE FROM PRIVATE TO PUBLIC

    private bool diagonalSameLevel(int z, bool order) { // we dont need values of rows or cols

        // they don't have any element in common so we can assume it will happen one of another
        for (int k = 0; k < size; ++k) {
            if (order) {
                if (board[z, k, k] != turn){ // 00 11 22 
                    return false;
                }
            }
            if (!order) {
                if (board[z, k, ((size-1) - k)] != turn){ // 05 14 23 
                    return false;
                }
            }
        }
        return true;
    }
    
    private bool diagonalColumnTop_to_ColumnBotton(int x, bool order) { //true is level == y 
        int level = order ? 0 : (size-1);
        for (int k = 0; k < size; ++k) {
            if (board[level, x, k] != turn){
                return false;
            }
            if (order) {
                level++;
            }
            else {
                level--;
            }
        }
        return true;
    }

    private bool diagonalRowTop_to_RowBotton(int y, bool order) {

        int level = order ? 0 : (size-1);
        for (int k = 0; k < size; ++ k) {
            if (board[level, k, y] != turn){
                return false;
            }
            if (order) {
                level++;
            }
            else {
                level--;
            }
        }
        return true;

    }

    private bool diagonalBothDirectionsfrom00(bool order) {
        // UPDATED USE OF ? WASNT WORKING 

        int level = order ? 0 : (size-1);
        for (int k = 0; k < size; ++k) {
            if (board[level, k, k] != turn) {
                return false;
            }
            if (order) {
                level++;
            }
            else {
                level--;
            }
        }

        return true;
    }

    private bool diagonalBothDirectionsfrom05(bool order) { // z == x
        int level = 0;
        for (int k = (size-1); k >= 0; --k) {
            if (order){
                if (board[level, level, k] != turn) { // 0 
                    return false;
                }
            }
            if (!order){
                if (board[level, k, level] != turn) {
                    return false;
                }
            }
            level++;
        }
        return true;
    }

    private bool checkAllDiagonals(int z, int x, int y) {

        bool completedDiagonal;

        if ( (x + y) == (size-1)  || x == y ) {
            completedDiagonal = diagonalSameLevel(z, x == y);
            if (completedDiagonal) {return true; }
        }

        if ( y == z || ( y + z) == (size-1) ) {
            completedDiagonal = diagonalColumnTop_to_ColumnBotton(x, y == z);
            if(completedDiagonal){ return true; }
        }

        if ( x == z || (x + z) == (size-1)) {
            completedDiagonal = diagonalRowTop_to_RowBotton(y, x == z );
            if (completedDiagonal) { return true; }
        }

        if (x == y && ( x == z || (z + x) == (size-1)) ){
            completedDiagonal = diagonalBothDirectionsfrom00(x == z);
            if (completedDiagonal){ return true; }
        }
        if ( (x + y) == (size-1)  && ( z == y || z == x ) ) {
            completedDiagonal = diagonalBothDirectionsfrom05(z == x);
            if (completedDiagonal){ return true; }
        }

        return false;
    }

    private bool checkRowsCols(int z, int x, int y) { // row and column in the same level, 
                                                        // column by different levels
        bool row = true;
        bool col = true;
        bool col_by_levels = true;

        for(int k = 0; k < size; ++k) {
            if (board[z, k, y] != turn){
                row = false;
            }
            if (board[z, x, k] != turn) {
                col = false;
            }
            if (board[k, x, y] != turn){
                col_by_levels = false;
            } 
        }
        return ( row || col || col_by_levels );

    }

    private bool checkVictory(int z, int x, int y) { 
        return  ( checkRowsCols(z, x, y) || checkAllDiagonals(z, x, y) );
    }

    public bool move(int z, int x, int y) {

        if (board[z, x, y] != 0) {
            return false;
        }

        board[z, x, y] = turn;

        if (checkVictory(z, x, y)){
            winner = turn; 
        }

        turn = (turn == 1) ? 2 : 1; 

        count++;
        return true;
    }

    public bool tie() =>  count >= 200;

    public int[,,] getBoard => board;

    public bool isLegal(int z, int x, int y) => (board[z, x, y] == 0);

    public override string ToString() { 

        char[] output = {'-', 'X', 'O'};                  
        for (int z = (size-1); z >= 0; --z) {
            WriteLine($"Level {z+1}: "); 
            for (int x = 0; x < size; ++x){
                for (int y = 0; y < size; ++y){
                    if (y == 0){
                        Write(" |"); 
                    }
                    Write($" {output[board[z, x, y]]} | ");
                }
                WriteLine();
            }
            WriteLine(); 
        }
        return "";

    }

}


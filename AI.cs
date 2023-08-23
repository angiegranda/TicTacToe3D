using System;
using System.Collections.Generic;
using static System.Console;



class AIPlayer {

    Minimax minimax = new Minimax();

    public (int, int, int) BestMove(int[,,] matrix) { // he

        (int z, int x, int y) value = minimax.GetBestMove(matrix);
        return (value.z, value.x, value.y);
    }

}

// strategy for sorting the possible moves,
// 1) get the free squares 
// 2) with each free square, check the potentional for winning, always 1 and if in its row, col, diagonal 
// it has more squares of its turn the ++ for each
// we should have an option in case it is the only square left to fill then we automatically give it and stop any recursion
// this should be in the first loop so if depth is 0. 
// 3) additionally check the potential for blocking at any given square sum ++ for each square that has the opponent turn 
// in a row, col, diagonal it forms part 


class Minimax {

    private int bestMoveZ = 0;
    private int bestMoveX = 0;
    private int bestMoveY = 0;
    const int dimention = 6;
    const int MaxSquaresCount = 200; //factor to get a DRAW !!! NOT IMPLEMENTED YET 

    SortingMoves sorting = new SortingMoves(dimention, 2, 1);
    

    public Minimax() {}

    private int counter = 0;

    public (int, int, int) GetBestMove(int[,,] matrix) {

        bestMoveZ = 0;
        bestMoveX = 0;
        bestMoveY = 0;
        counter = 0;

        AlphaBetaPruning(matrix, 2, true, 0, int.MinValue, int.MaxValue);

        Console.WriteLine("Minimax took " + counter + " tries"); // debugging to know how many loops we do 
        Console.WriteLine($"This is the best move of the computer {bestMoveZ}, {bestMoveX}, {bestMoveY}");
        return (bestMoveZ, bestMoveX, bestMoveY);
    }

    private int AlphaBetaPruning(int[,,] matrix, int turn, bool maximazing, int depth,
    int alpha, int beta, int prev_z = -1, int prev_x = -1, int prev_y = -1) {

        counter++;

        if (prev_z != -1 && prev_x != -1 && prev_y != -1 ){
            int winner = checkWin(matrix, prev_z, prev_x, prev_y);
            if (winner >= 0 || matrixCurrentOccupiedSpots(matrix) == MaxSquaresCount || depth == 5) {
                return getScore(matrix, turn, depth, winner);
            }
        } 

        List<int> scores = new List<int>();

        List<(int, int, int)> freeSquares = sorting.getSortList(matrix, depth, getFreeSquares(matrix),  out (int a, int b, int c)? lastMove);
        if (lastMove != null && depth == 0) {
            (bestMoveZ, bestMoveX, bestMoveY) = (lastMove.Value.a, lastMove.Value.b, lastMove.Value.c);
            return 0;
        }

        int score = 0;

        for (int i = 0; i < freeSquares.Count; i++) {
            int z = freeSquares[i].Item1;
            int x = freeSquares[i].Item2;
            int y = freeSquares[i].Item3;

            int marker = maximazing ? 2 : 1; // change to 1 or 2 
            matrix[z, x, y] = marker;
            //WriteLine($"current turn {marker} and bool {maximazing} current depth {depth}");

            int newTurn = maximazing ? 1 : 2;
            bool newTurnB = maximazing ? false : true;
            score = AlphaBetaPruning(matrix, newTurn, newTurnB, depth + 1, alpha, beta, z, x, y);
            scores.Add(score);
            matrix[z, x, y] = 0; // we are backtracking and changing back to the original state 
            // Pruning
            if (maximazing) {

                int maxValue = Math.Max(int.MinValue, score);
                alpha = Math.Max(alpha, maxValue);
                if (alpha >= beta) {
                    // WriteLine("PRUNING!!!");
                    return alpha;
                }
            }
            else {
                int minValue = Math.Min(int.MaxValue, score);
                beta = Math.Min(beta, minValue);
                if (beta <= alpha) {
                    // WriteLine("PRUNING!!!");
                    return beta;
                }
            }
            //WriteLine($"currently visiting the loop of move  [{z}, {x}, {y} ], depth is {depth}");
        }

        int scoreIndex = 0;

        if (maximazing) {
            scoreIndex = getMax(scores);
        }
        else {
            scoreIndex = getMin(scores);
        }

        (bestMoveZ, bestMoveX, bestMoveY) = freeSquares[scoreIndex];
        return scores[scoreIndex];

    }


    int getMax(List<int> scores) {
        int index = 0;
        int max = int.MinValue;
        for (int i = 0; i < scores.Count; i++) {
            if (scores[i] >= max) {
                index = i;
                max = scores[i];
            }
        }
        return index;
    }

    int getMin(List<int> scores) {
        int index = 0;
        int min = int.MaxValue;
        for (int i = 0; i < scores.Count; i++) {
            if (scores[i] <= min) {
                index = i;
                min = scores[i];
            }
        }
        return index;
    }

    int getScore(int[,,] matrix, int turn, int depth, int result){
        if (result >= 0) {
            return result == turn ? MaxSquaresCount - depth : -MaxSquaresCount + depth;
        }

        return 0;
    }

    List<(int, int, int)>  getFreeSquares(int[,,] matrix) {

        List<(int, int, int)> freeSquares = new List<(int, int, int)>();

        for (int z = 0; z < dimention; z++) {
            for (int x = 0; x < dimention; x++) {
                for (int y = 0; y < dimention; y++) {
                    if (matrix[z, x, y] == 0) {
                        freeSquares.Add((z, x, y));
                    }
                }
            }
        }

        return freeSquares;

    }

    int matrixCurrentOccupiedSpots(int[,,] matrix) {

        int result = 0;
        for (int z = 0; z < dimention; z++) {
            for (int x = 0; x < dimention; x++) {
                for (int y = 0; y < dimention; y++) {
                    if (matrix[z, x, y] != 0) {
                        result++;
                    }
                }
            }
        }
        return result;

    }


    int checkWin(int[,,] matrix, int z, int x, int y) {

        // checkimg each of the possibilities and return the winner 
        
        int currentTurn = matrix[z, x, y];

        int winner = checkRowsCols(matrix, z, x, y, currentTurn); // adding the matrix and the one tha has the turn 
        if (winner != -1) {
            return winner;
        }
        
        int winner2 = checkAllDiagonals(matrix, z, x, y, currentTurn);
        if (winner2 != -1){
            return winner2;
        }

        return -1; // No win
    }

    int diagonalSameLevel(int[,,] matrix, int z, bool order, int turn) { // we dont need values of rows or cols

        // they don't have any element in common so we can assume it will happen one of another
        for (int k = 0; k < dimention; ++k) {
            if (order) {
                if (matrix[z, k, k] != turn){ // 00 11 22 
                    return -1;
                }
            }
            if (!order) {
                if (matrix[z, k, ((dimention-1) - k)] != turn){ // 05 14 23 
                    return -1;
                }
            }
        }
        return turn; 
    }
    
    int diagonalColumnTop_to_ColumnBotton(int[,,] matrix, int x, bool order, int turn) {
        // checks for any square if its diagonal from row x and level 0 to 5 or level 5 to 0
        int level = order ? 0 : (dimention-1);
        for (int k = 0; k < dimention; ++k) {
            
            if (matrix[level, x, k] != turn){
                return -1;
            }
            if (order) {
                level++;
            }
            else {
                level--;
            }
        }
        return turn;
    }

    int diagonalRowTop_to_RowBotton(int[,,] matrix, int y, bool order, int turn) {
        // checks for any square if its diagonal from col y level 0 to 5 or 5 to 0
        int level = order ? 0 : (dimention-1);
        for (int k = 0; k < dimention; ++k) {
            if (matrix[level, k, y] != turn){
                return -1;
            }
            if (order) {
                level++;
            }
            else {
                level--;
            }
        }
        return turn;

    }

    int diagonalBothDirectionsfrom00(int[,,] matrix, bool order, int turn) {

        int level = order ? 0 : (dimention-1);
        for (int k = 0; k < dimention; ++k) {
            if (matrix[level, k, k] != turn) {
                return -1;
            }
            if (order) {
                level++;
            }
            else {
                level--;
            }
        }

        return turn;
    }

    int diagonalBothDirectionsfrom05(int[,,] matrix, bool order, int turn) { // z == x
        int level = 0;
        for (int k = (dimention-1); k >= 0; --k) {
            if (order){
                if (matrix[level, level, k] != turn) {
                    return -1;
                }
            }
            if (!order){
                if (matrix[level, k, level] != turn) {
                    return -1;
                }
            }
            level++;
        }
        return turn;
    }

    int checkAllDiagonals(int[,,] matrix, int z, int x, int y, int turn) {

        int completedDiagonal;

        if ( (x + y) == (dimention-1)  || x == y ) {
            completedDiagonal = diagonalSameLevel(matrix, z, x == y, turn);
            if (completedDiagonal != -1){
                return completedDiagonal;
            }
        }

        if ( y == z || ( y + z) == (dimention-1) ) {
            completedDiagonal = diagonalColumnTop_to_ColumnBotton(matrix, x, y == z, turn);
            if (completedDiagonal != -1){
                return completedDiagonal;
            }
        }

        if ( x == z || (x + z) == (dimention-1)) {
            completedDiagonal = diagonalRowTop_to_RowBotton(matrix, y, x == z, turn);
            if (completedDiagonal != -1){
                return completedDiagonal;
            }
        }

        if (x == y && ( x == z || (z + x) == (dimention-1)) ){
            completedDiagonal = diagonalBothDirectionsfrom00(matrix, x == z, turn);
            if (completedDiagonal != -1){
                return completedDiagonal;
            }
        }
        if ( (x + y) == (dimention-1)  && ( z == y || z == x ) ) {
            completedDiagonal = diagonalBothDirectionsfrom05(matrix, z == x, turn);
            if (completedDiagonal != -1){
                return completedDiagonal;
            }
        }
        return -1;
    }

    int checkRowsCols(int[,,] matrix, int z, int x, int y, int turn) { // row and column in the same level, 
                                                        // column by different levels
        bool row = true;
        bool col = true;
        bool col_by_levels = true;

        for(int k = 0; k < dimention; ++k) {
            if (matrix[z, k, y] != turn){
                row = false;
            }
            if (matrix[z, x, k] != turn) {
                col = false;
            }
            if (matrix[k, x, y] != turn){
                col_by_levels = false;
            } 
        }

        if (row || col || col_by_levels) {
            return turn;
        }
        return -1;
    }

}







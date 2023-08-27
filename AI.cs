using System;
using System.Collections.Generic;
using static System.Console;


public class AIPlayer {

    Minimax minimax;
    int turn;

    // initialize player
    public AIPlayer(int turn) {
        this.turn = turn;
        minimax = new Minimax();
    }
    // call minimax
    public (int, int, int) BestMove(int[,,] board) { 
        (int z, int x, int y) value = minimax.GetBestMove(board, turn);
        return (value.z, value.x, value.y);
    }

}

public class Minimax {

    private const int maxSquaresCount = 216; 
    // maxDepth is 6 => unitests take 15 secs
    // maxDepth is 5 => unitests take 3 secs
    private const int maxDepth = 6;  
    private const int dimention = 6;
    private int bestMoveZ;
    private int bestMoveX;
    private int bestMoveY;
    SortingMoves sorting;
    CheckBoard check;
    
    public Minimax() {
        sorting = new SortingMoves();
        check = new CheckBoard();
    }

    public (int, int, int) GetBestMove(int[,,] board, int turn) {

        bestMoveZ = 0;
        bestMoveX = 0;
        bestMoveY = 0;

        // maximazing current AI player, minimizing the opponent 
        AlphaBetaPruning(board, turn, true, 0, int.MinValue, int.MaxValue);
        return (bestMoveZ, bestMoveX, bestMoveY);

    }

    // previous coordinates are initialized as -1 
    // necessary to explore any column, row or diagonal that that specific square is part of

    private int AlphaBetaPruning(int[,,] board, int turn, bool maximazing, int depth,
    int alpha, int beta, int prev_z = -1, int prev_x = -1, int prev_y = -1) {

        int opponent;
        
        // checking if :
        // 1) the move in the previous depth is a winner move 
        // 2) we have reached the maximum depth
        // 3) we have filled all squares 

        if (prev_z != -1 && prev_x != -1 && prev_y != -1 ) {

            int currentVal = board[prev_z, prev_x, prev_y];
            int opponentVal = (3 - currentVal);

            check.Eval(board, currentVal, prev_z, prev_x, prev_y, out int winner);

            if (winner >= 0 || OccupiedSquares(board) == maxSquaresCount || depth == maxDepth) {
                if (winner >= 0) {
                    if (maximazing) { // AI current turn, but in the previous turn opponent won
                        return -maxSquaresCount + depth;
                    }
                    else { // oppoent current turn, in the previous AI won
                        return maxSquaresCount - depth;
                    }
                }
                else {
                    return 0;
                }
            }


        }

        // getting the top 10 best moves from this current state 
        // call SortingMoves class 

        opponent = maximazing ? (3 - turn) : turn;
       	List<(int, int, int)> freeSquares = sorting.GetSortList(board, depth, 
        Minimax.GetFreeSquares(board), turn, opponent, out (int a, int b, int c)? lastMove);


        // lastMove is if there is a winner move or blocking move
        // At this moment we dont need the alpha beta pruning since 
        // only this move will prevent the opponent to success or make the AI to win

        if (lastMove != null && depth == 0) {
            (bestMoveZ, bestMoveX, bestMoveY) = (lastMove.Value.a, lastMove.Value.b, lastMove.Value.c);
            return 0;
        }

        List<int> scores = new List<int>();
        int score = 0;

        for (int i = 0; i < freeSquares.Count; i++) {

            int z = freeSquares[i].Item1;
            int x = freeSquares[i].Item2;
            int y = freeSquares[i].Item3;

            int currentTurn = maximazing ? turn : (3 - turn); 
            board[z, x, y] = currentTurn; // filling the board 

            opponent = maximazing ? (3 - turn) : turn;
            bool newTurnBool = maximazing ? false : true;

            score = AlphaBetaPruning(board, opponent, newTurnBool, depth + 1, alpha, beta, z, x, y);

            scores.Add(score); 
            board[z, x, y] = 0; // backtracking 

            // Pruning
            if (maximazing) {

                alpha = Math.Max(alpha, score);
                // prune since alpha has a equal or better score than the opponent
                if (alpha >= beta) {
                    return alpha;
                }
            }
            else {

                beta = Math.Min(beta, score);
                // prune since opponent (minimazing) has smallest (equal greater) than AI
                if (beta <= alpha) {
                    return beta;
                }
            }
        }

        int scoreIndex = 0;

        if (maximazing) {
            scoreIndex = GetMax(scores, true);
        }
        else {
            scoreIndex = GetMax(scores, false);
        }
        // at the end when we are backtraking we maximaxing is true
        // the highest score is selected (from the initial best 10 moves)
        (bestMoveZ, bestMoveX, bestMoveY) = freeSquares[scoreIndex];
        return scores[scoreIndex];

    }

    // GetMax if true GetMin if false;
    // I wanted to save code since is the same process with 3 lines different
    private int GetMax(List<int> scores, bool order) {
        int index = 0;
        int max = int.MinValue;
        int min = int.MaxValue;
        for (int i = 0; i < scores.Count; i++) {
            if (order) {
                if (scores[i] >= max) {
                    index = i;
                    max = scores[i];
                }
            }
            if (!order) {
                if (scores[i] <= min) {
                    index = i;
                    min = scores[i];
                }
            }
        }
        return index;
    }


    // outputs a lits of the squares that are free in that specific state 
    public static List<(int, int, int)>  GetFreeSquares(int[,,] board) {

        List<(int, int, int)> freeSquares = new List<(int, int, int)>();

        for (int z = 0; z < board.GetLength(1); z++) {
            for (int x = 0; x < board.GetLength(1); x++) {
                for (int y = 0; y < board.GetLength(1); y++) {
                    if (board[z, x, y] == 0) {
                        freeSquares.Add((z, x, y));
                    }
                }
            }
        }
        return freeSquares;

    }

    // number of squares occupied in that specific state 
    private int OccupiedSquares(int[,,] board) {
        int result = 0;
        for (int z = 0; z < dimention; z++) {
            for (int x = 0; x < dimention; x++) {
                for (int y = 0; y < dimention; y++) {
                    if (board[z, x, y] != 0) {
                        result++;
                    }
                }
            }
        }
        return result;
    }

}









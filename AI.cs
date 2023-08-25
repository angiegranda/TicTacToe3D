using System;
using System.Collections.Generic;
using static System.Console;


public class AIPlayer {

    Minimax minimax;
    int turn;

    public AIPlayer(int turn) {
        this.turn = turn;
        minimax = new Minimax();
    }

    public (int, int, int) BestMove(int[,,] board) { // he
        (int z, int x, int y) value = minimax.GetBestMove(board, turn);
        return (value.z, value.x, value.y);
    }

}

public class Minimax {

    private int maxSquaresCount = 216; 
    private int maxDepth = 5; 
    private int dimention = 6;
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

        AlphaBetaPruning(board, turn, true, 0, int.MinValue, int.MaxValue);
        return (bestMoveZ, bestMoveX, bestMoveY);

    }

    private int AlphaBetaPruning(int[,,] board, int turn, bool maximazing, int depth,
    int alpha, int beta, int prev_z = -1, int prev_x = -1, int prev_y = -1) {

        int opponent;

        if (prev_z != -1 && prev_x != -1 && prev_y != -1 ) {

            int currentVal = board[prev_z, prev_x, prev_y];
            int opponentVal = (3 - currentVal);
            check.Eval(board, currentVal, prev_z, prev_x, prev_y, out bool won, out int winner);
            if (winner >= 0 || OccupiedSquares(board) == maxSquaresCount || depth == maxDepth) {
                return GetScore(turn, depth, winner);
            }
        }

        opponent = maximazing ? (3 - turn) : turn;
        List<int> scores = new List<int>();
       	List<(int, int, int)> freeSquares = sorting.GetSortList(board, depth, 
        Minimax.GetFreeSquares(board), turn, opponent, out (int a, int b, int c)? lastMove);

        if (lastMove != null && depth == 0) {
            (bestMoveZ, bestMoveX, bestMoveY) = (lastMove.Value.a, lastMove.Value.b, lastMove.Value.c);
            return 0;
        }

        int score = 0;

        for (int i = 0; i < freeSquares.Count; i++) {
            int z = freeSquares[i].Item1;
            int x = freeSquares[i].Item2;
            int y = freeSquares[i].Item3;

            int currentTurn = maximazing ? turn : (3 - turn); 
            board[z, x, y] = currentTurn;

            opponent = maximazing ? (3 - turn) : turn;
            bool newTurnB = maximazing ? false : true;
            score = AlphaBetaPruning(board, opponent, newTurnB, depth + 1, alpha, beta, z, x, y);

            scores.Add(score);
            board[z, x, y] = 0; // backtracking

            // Pruning
            if (maximazing) {
                int maxValue = Math.Max(int.MinValue, score);
                alpha = Math.Max(alpha, maxValue);
                if (alpha >= beta) {
                    return alpha;
                }
            }
            else {
                int minValue = Math.Min(int.MaxValue, score);
                beta = Math.Min(beta, minValue);
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

        (bestMoveZ, bestMoveX, bestMoveY) = freeSquares[scoreIndex];
        return scores[scoreIndex];

    }


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

    private int GetScore(int currentTurn, int depth, int previousWinner){ // turn can be different in each recursion
        if (previousWinner >= 0) {
            return previousWinner == currentTurn ? maxSquaresCount - depth : -maxSquaresCount + depth;
        }
        return 0;
    }

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









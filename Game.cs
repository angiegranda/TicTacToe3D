using System;
using static System.Console;
using System.Collections.Generic;

public class Game {

    const int TieFactor = 200;
    const int size = 6; 
    int[,,] board = new int[size,size,size];
    public int turn = 1;                      // start
    public int winner = 0;                    // 0 if none
    int count = 0;                            // # of squares occupied 
    CheckBoard check = new CheckBoard();

    public Game() {}

    public bool Tie() =>  count >= TieFactor; 

    public int[,,] GetBoard => board;

    public int? Winner() =>  (winner != 0) ? winner : null;  // COMMIT CHANGE FROM PRIVATE TO PUBLIC

    public bool Move(int z, int x, int y) {
        
        // checks if is a legal move
        if (board[z, x, y] != 0) {
            return false;
        }

        // writes the turn in the board
        board[z, x, y] = turn;

        // checks if current player has won
        check.Eval(board, turn, z, x, y, out int winNumber);

        if (turn == winNumber){
            winner = turn; 
        }

        // updates next turn number and updates the counter
        turn = (turn == 1) ? 2 : 1; 
        count++;

        return true;
    }

    public override string ToString() { 
        
        // writes the board 
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


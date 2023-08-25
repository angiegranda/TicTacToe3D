using System;
using static System.Console;
using System.Collections.Generic;

public class Game {

    const int size = 6; 
    int[,,] board = new int[size,size,size];
    public int turn = 1;  
    public int winner = 0; 
    int count = 0;
    CheckBoard check = new CheckBoard();

    public Game() {}

    public int this[int z, int x, int y] { // indexer 
        get => board[z, x, y];
    }

    public int? Winner() =>  (winner != 0) ? winner : null;  // COMMIT CHANGE FROM PRIVATE TO PUBLIC

    public bool Move(int z, int x, int y) {

        if (board[z, x, y] != 0) {
            return false;
        }

        board[z, x, y] = turn;

        int opponent = (3-turn);
        bool wonGame = false;
        check.Eval(board, turn, z, x, y, out wonGame, out int winNumber);
        if (wonGame){
            winner = turn; 
        }

        turn = (turn == 1) ? 2 : 1; 
        count++;
        return true;
    }

    public bool Tie() =>  count >= 200;

    public int[,,] GetBoard => board;

    public bool IsLegal(int z, int x, int y) => (board[z, x, y] == 0);

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


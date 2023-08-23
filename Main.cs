using System;
using static System.Console;


class Program {

    static void Main() {
        // here starts the game

        WriteLine("Welcome to TicTacToe game against the computer! ^.^");
        WriteLine("Introduce your name: ");
        string playerName = ReadLine()!;
        Game g = new Game();
        AIPlayer computer = new AIPlayer();
        // created for keeping tack of how many ways it has been played
        // could also be a variable outside 
        while (true) { 
            if (g.tie()){
                break;
            }
            if (g.turn == 1) {
                 WriteLine("Introduce level: ");
                int matrix = int.Parse(ReadLine()!);
                WriteLine("Introduce row: ");
                int row = int.Parse(ReadLine()!);
                WriteLine("Introduce column: ");
                int col = int.Parse(ReadLine()!);
                if (g.isLegal(matrix-1, row-1, col-1)) {
                    WriteLine($"Move of {playerName}: ");
                    g.move(matrix-1, row-1, col-1); //  knowing that the user will start with 1 
                    WriteLine(g);
                    if (g.Winner() == 1){
                        break;
                    }
                }
                else {
                    WriteLine("That square is ocuppied already, choose another one");
                }
            } 
            else { // turn of the computer 
                WriteLine("Move of the computer: ");
                (int z, int x, int y) value = computer.BestMove(g.getBoard);
                g.move(value.z, value.x, value.y);
                WriteLine(g);
                if (g.Winner() == 2) {
                    break;
                }
            }
        }

        // Once there is a winner 
        WriteLine();
        if (g.Winner() == 1) {
            WriteLine($"{playerName} has won! ");
        }
        else if (g.Winner() == 2) {
            WriteLine("Computer has won!");
        }
        else {
            WriteLine("Anyone won, game ended in tie");
        }
    }
}


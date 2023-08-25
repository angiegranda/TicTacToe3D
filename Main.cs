using System;
using static System.Console;


class Program {

    static void Main() {
        // here starts the game

        WriteLine("Welcome to TicTacToe game against the computer! ^.^");
        WriteLine("Introduce your name: ");
        string playerName = ReadLine()!;

        int userTurn = 1;
        int computerTurn = 2;
        Game g = new Game();
        AIPlayer computer = new AIPlayer(computerTurn);

        while (true) { 
            if (g.Tie()){
                break;
            }
            if (g.turn == userTurn) {
                try{
                     WriteLine("Introduce level: ");
                    int matrix = int.Parse(ReadLine()!);
                    WriteLine("Introduce row: ");
                    int row = int.Parse(ReadLine()!);
                    WriteLine("Introduce column: ");
                    int col = int.Parse(ReadLine()!);
                    if (g.IsLegal(matrix-1, row-1, col-1)) {
                        WriteLine($"Move of {playerName}: ");
                        g.Move(matrix-1, row-1, col-1); //  knowing that the user will start with 1 
                        WriteLine(g);
                        if (g.Winner() is int currentWinner){
                            if (currentWinner == userTurn){
                                break;
                            }
                        }
                    }
                    else {
                        WriteLine("That square is ocuppied already, choose another one");
                    }
                } catch (System.FormatException){
                    WriteLine("Please enter a legal level-row-column input correctly");
                }
            } 
            else { // turn of the computer 
                WriteLine("Move of the computer: ");
                (int z, int x, int y) value = computer.BestMove(g.GetBoard);
                g.Move(value.z, value.x, value.y);
                WriteLine(g);
                if (g.Winner() is int currentWinner) {
                    if (currentWinner == computerTurn){
                        break;
                    }
                }
            }
        }

        // Once there is a winner 
        WriteLine();
        if (g.Winner() == userTurn) {
            WriteLine($"{playerName} has won! ");
        }
        else if (g.Winner() == computerTurn) {
            WriteLine("Computer has won!");
        }
        else {
            WriteLine("Anyone won, game ended in tie");
        }
    }
}


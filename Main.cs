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

                    WriteLine($"It is {playerName} turn: ");
                    Write("Introduce matrix {1, 2, 3, 4, 5, 6}: ");
                    int matrix = int.Parse(ReadLine()!);
                    Write("Introduce row {1, 2, 3, 4, 5, 6}: ");
                    int row = int.Parse(ReadLine()!);
                    Write("Introduce column {1, 2, 3, 4, 5, 6}: ");
                    int col = int.Parse(ReadLine()!);

                    if (g.IsLegal(matrix-1, row-1, col-1)) {
                        g.Move(matrix-1, row-1, col-1); //  knowing that the user will start with 1 
                        if (g.Winner() is int currentWinner){
                            if (currentWinner == userTurn){
                                break;
                            }
                        }
                    }
                    else {
                        WriteLine("That square is ocuppied already, choose again");
                    }
                } catch (System.FormatException){
                    WriteLine("Please enter a legal level-row-column input");
                }
            } 
            else { // turn of the computer 
                Write("It is the computer turn : ");
                (int z, int x, int y) value = computer.BestMove(g.GetBoard);
                g.Move(value.z, value.x, value.y);
                WriteLine($"move is matrix: {value.z + 1}, row: {value.x + 1}, column :{value.y + 1}");
                if (g.Winner() is int currentWinner) {
                    if (currentWinner == computerTurn){
                        break;
                    }
                }
            }
            WriteLine(g);
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
            WriteLine("Game ended in a tie");
        }
    }
}


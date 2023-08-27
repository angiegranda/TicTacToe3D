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

            // Tie will be reached if there is no winner and there are 16 squares left, 216 are the total 

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

                    if (g.Move(matrix-1, row-1, col-1)) {
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
                } catch (System.IndexOutOfRangeException) {
                    WriteLine("Please introduce numbers in the range 1 to 6 inclusive");
                }
            }

            // turn of the computer 
            else { 
                Write("It is the computer turn : ");
                (int z, int x, int y) value = computer.BestMove(g.GetBoard);
                g.Move(value.z, value.x, value.y);
                WriteLine($"move is matrix: {value.z + 1}, row: {value.x + 1}, column :{value.y + 1}");
                if (g.Winner() is int currentWinner) {
                    if (currentWinner == computerTurn){
                        break;
                    }
                }
                WriteLine(g); // matrix will be printed when both user and computer have made a move
            }
           
        }

        WriteLine();
        // There is a winner or a tie

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


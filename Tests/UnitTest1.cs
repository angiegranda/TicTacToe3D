using Microsoft.VisualBasic;
using Xunit;
using System;
namespace tests;

public class UnitTest1 {

    [Fact]

    public void BlockingTest() {
        // Setup
        var matrix = new int[6,6,6];
        int userTurn = 1;
        int computerTurn = 2;
        
        // Populate our matrix
        matrix[0,0,0] = userTurn;
        matrix[0,0,1] = userTurn;
        matrix[0,0,2] = userTurn;
        matrix[0,0,3] = userTurn;
        matrix[0,0,4] = userTurn;

        // Blocking move
        var blocking_move = (0,0,5);
        var test = new SortingMoves();
        test.GetSortList(matrix, 0, Minimax.GetFreeSquares(matrix), computerTurn, userTurn, out var bestMove);
    
        Assert.Equal(blocking_move, bestMove);

    }
}

public class UniTest2 {

    [Fact]

    public void UserAI(){

        (int, int, int)[] userMoves = {
        (1,1,1), (2,2,2), (3,3,3), (4,4,4), (2,2,1), (2,2,3),
        (1,3,5), (2,2,4), (1,2,6), (2,2,6), (6,2,2), (5,2,2),
        (3,2,2), (6,6,1), (1,6,1), (1,4,2), (4,2,5), (3,6,1),
        (1,6,6), (2,3,3), (1,3,3), (1,6,5), (1,1,5), (1,5,5),
        (2,5,2), (6,2,1), (1,6,2), (3,3,2), (3,2,4)};

        int index = -1;

        int userTurn = 1;
        int computerTurn = 2;

        Game g = new Game();
        AIPlayer computer = new AIPlayer(computerTurn);

        while(true){

            if (g.Tie()){
                break;
            }
            if (g.turn == userTurn){
                index++;
                (int z, int x, int y) move = userMoves[index];
                g.Move(move.z-1, move.x-1, move.y-1);

                if (g.Winner() is int currentWinner){
                    if (currentWinner == userTurn){
                        break;
                    }
                }
            }
            else {
                (int a, int b, int c) cMove = computer.BestMove(g.GetBoard);
                g.Move(cMove.a, cMove.b, cMove.c);

                if (g.Winner() is int currentWinner){
                    if (currentWinner == computerTurn){
                        break;
                    }
                }
            }
            //Console.WriteLine(g);
        }
        // end of the game, there is a Winner or the winner is null 
        // if it is a tie
        if (g.Winner() is int finalWinner){
            Assert.Equal(computerTurn, finalWinner);
        }
        bool notaTie = g.Tie();
        bool expectedresult = false;
        Assert.Equal(expectedresult, notaTie);
        // Computer has won by filling square = [k, 3, 4] where k are all the levels

    }
}

public class UniTest3 {

    [Fact]

    public void TwoAIPlayers() {

        int firstTurn = 1;
        int secondTurn = 2;

        Game g = new Game();
        AIPlayer first = new AIPlayer(firstTurn);
        AIPlayer second = new AIPlayer(secondTurn);


        while(true) {
            if (g.Tie()){
                break;
            }
            if (g.turn == firstTurn) {
                (int z, int x, int y) move1 = first.BestMove(g.GetBoard);
                g.Move(move1.z, move1.x, move1.y);
                //Console.WriteLine(g);
                if (g.Winner() is int currentWinner){
                    if (currentWinner == firstTurn){
                        break;
                    }
                }
            }
            else {
                (int a, int b, int c) move2 = second.BestMove(g.GetBoard);
                g.Move(move2.a, move2.b, move2.c);
                //Console.WriteLine(g);
                if (g.Winner() is int currentWinner){
                    if (currentWinner == secondTurn){
                        break;
                    }
                }
            }
        }
        Console.WriteLine(g.Winner());
        bool expectedSolution = true;

        Assert.Equal(expectedSolution, g.Tie());
    }
}

public class UniTest4 {

    [Fact] 

    public void RandomTest() {

        int randomTurn = 1;
        int computerTurn = 2;

        Game g = new Game();
        AIPlayer computer = new AIPlayer(computerTurn);
        Random random = new Random();

        while (true) {
            if (g.Tie()){
                break;
            }
            if (g.turn == randomTurn) {
                try {
                    int a = random.Next(0, 6);
                    int b = random.Next(0, 6);
                    int c = random.Next(0, 6);
                    g.Move(a, b, c);
                    if (g.Winner() is int winnerNumber){
                        if (winnerNumber == randomTurn){
                            break; 
                        }
                    }
                }
                catch(System.FormatException) { }
            }
            else {
                (int z, int x, int y) move = computer.BestMove(g.GetBoard);
                g.Move(move.z, move.x, move.y);
                if (g.Winner() is int winnerNumber){
                    if (winnerNumber == computerTurn){
                        break;
                    }
                }
            }
        }

        if (g.Winner() is int finalWinner) {
            Assert.Equal(finalWinner, computerTurn);
        }
        bool notaTie = g.Tie();
        bool expectedresult = false;
        Assert.Equal(notaTie, expectedresult);

    }
}


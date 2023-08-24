using Microsoft.VisualBasic;
using Xunit;
namespace tests;

public class UnitTest1 {
    [Fact]
    public void BlockingTest() {
        // Setup
        var test = new SortingMoves(6, 2, 1);
        var matrix = new int[6,6,6];
        
        // Populate our matrix
        matrix[0,0,0] = 1;
        matrix[0,0,1] = 1;
        matrix[0,0,2] = 1;
        matrix[0,0,3] = 1;
        matrix[0,0,4] = 1;

        // Blocking move
        var blocking_move = (0,0,5);

        test.getSortList(matrix, 0, Minimax.getFreeSquares(matrix), out var bestMove);
    
        Assert.Equal(blocking_move, bestMove);
    }
}

public class UniTest2 {

    [Fact]

    public void UserAI(){

        Game g = new Game();
        AIPlayer computer = new AIPlayer();

        (int, int, int)[] userMoves = {
        (1,1,1), (2,2,2), (3,3,3), (4,4,4), (5,5,5), (2,2,1),
        (2,2,3), (2,2,4), (2,1,1), (3,3,1), (4,4,1), (5,5,1),
        (1,1,3), (4,4,3), (5,5,3), (4,4,2), (1,1,2), (2,3,1) };

        int index = -1;

        int userTurn = 1;
        int computerTurn = 2;

        while(true){
            if (g.tie()){
                break;
            }
            if (g.turn == userTurn){
                index++;
                (int z, int x, int y) move = userMoves[index];
                g.move(move.z-1, move.x-1, move.y-1);

                if (g.Winner() is int currentWinner){
                    if (currentWinner == userTurn){
                        break;
                    }
                }
            }
            else {

                (int a, int b, int c) cMove = computer.BestMove(g.getBoard);
                g.move(cMove.a, cMove.b, cMove.c);

                if (g.Winner() is int currentWinner){
                    if (currentWinner == computerTurn){
                        break;
                    }
                }
            }
        
        }
        // end of the game, there is a Winner or the winner is null 
        // if it is a tie
        if (g.Winner() is int finalWinner){
            Assert.Equal(computerTurn, finalWinner);
        }
        bool notaTie = g.tie();
        bool expectedresult = false;
        Assert.Equal(expectedresult, notaTie);
    }
}

public class UniTest3 {

    [Fact]

    public void TwoAIPlayers() {

        Game g = new Game();
        AIPlayer first = new AIPlayer();
        AIPlayer second = new AIPlayer();

        int firstTurn = 1;
        int secondTurn = 2;

        while(true) {
            if (g.tie()){
                break;
            }
            if (g.turn == firstTurn) {
                (int z, int x, int y) move1 = first.BestMove(g.getBoard);
                g.move(move1.z, move1.x, move1.y);
                if (g.Winner() is int currentWinner){
                    if (currentWinner == firstTurn){
                        break;
                    }
                }
            }
            else {
                (int a, int b, int c) move2 = second.BestMove(g.getBoard);
                g.move(move2.a, move2.b, move2.c);
                if (g.Winner() is int currentWinner){
                    if (currentWinner == secondTurn){
                        break;
                    }
                }
            }
        }

        bool expectedSolution = true;

        Assert.Equal(expectedSolution, g.tie());
    }
}

public class UniTest4 {

    [Fact] 

    public void RandomTest() {

        Game g = new Game();
        AIPlayer computer = new AIPlayer();

        int randomTurn = 1;
        int computerTurn = 2;
        Random random = new Random();

        while (true) {
            if (g.tie()){
                break;
            }
            if (g.turn == randomTurn) {
                try {
                    int a = random.Next(0, 6);
                    int b = random.Next(0, 6);
                    int c = random.Next(0, 6);
                    g.move(a, b, c);
                    if (g.Winner() is int winnerNumber){
                        if (winnerNumber == randomTurn){
                            break;
                        }
                    }
                }
                catch(System.FormatException) { }
            }
            else {
                (int z, int x, int y) move = computer.BestMove(g.getBoard);
                g.move(move.z, move.x, move.y);
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
        bool notaTie = g.tie();
        bool expectedresult = false;
        Assert.Equal(notaTie, expectedresult);

    }
}


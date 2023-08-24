using Microsoft.VisualBasic;
using Xunit;
namespace tests;

public class UnitTest1
{
    [Fact]
    public void TestIfBlockWinning()
    {
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

        /* The input for the test where the player placed 5 in one row on one layer
        x  [] [] [] [] []
        x  [] [] [] [] []
        x  [] [] [] [] []
        x  [] [] [] [] []
        x  [] [] [] [] []
        [] [] [] [] [] []
        */

        test.getSortList(matrix, 0, Minimax.getFreeSquares(matrix), out var bestMove);
      
        Assert.Equal(blocking_move, bestMove);


        /* The output for the test, the ai blocked the player from winning
        x  [] [] [] [] []
        x  [] [] [] [] []
        x  [] [] [] [] []
        x  [] [] [] [] []
        x  [] [] [] [] []
        O  [] [] [] [] []
        */
    }
}
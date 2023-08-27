# UniTest using Xunit 

### Tests/UniTests1.cs

##### Located at the folder Tests, we have 4 Tests:

**All 4 test Passed! To check it, run the command 'dotnet test' in the TicTacToe3D folder in your console.** 

**Time the all tests together took:**
- If variable maxDepth = 5, it took 3 seconds
- If variable maxDepth = 6; it took 15 seconds.

### Tests: 

- **BlockingTest()** : checks if the function GetSortList() from SortingMoves class (AlphaBetaPruning() needs it) returns the move to block the user from winning.
  
- **UserAI()** : I took the inputs of users of one the matches I had and take a move in each time the user needed to move. It is curious that in a game where we are blocking computers future winning it takes between 30-50 moves to win, since it checks all possiblilities and human eye won't check easily all possible vectors.

- **TwoAIPlayers()** : in a optimal match in TicTacToe there won't be a winner but a tie. This test shows that two AIPlayers each one with either 1 or 2 assigned as their turn number, can't win since both are choosing the best moves possible.
  
- **RandomTest()** : in this test AIPlayer plays against a random player, it should check that AIPlayer win and it does succeed. 

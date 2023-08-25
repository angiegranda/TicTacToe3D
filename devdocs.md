# Developer Documentation 

-------------------------------------------------------------------------------------------

## Introduction:

There is a predefined numbers that will be written in the board to identify the user and the computer. 
User is number 1 and computer is number 2. However, since the those numbers were hardwire and difficult to keep a track of them, the code is flexible and the user can be 2 too. If the match is two AIPlayers (as we do in one of the unitest) it wont matter which of them is assigned to numbers 1 or 2. 

-------------------------------------------------------------------------------------------

## Files:

#### - Main.cs
#### - Game.cs
#### - AI.cs
#### - CheckBoard.cs
#### - SortingMoves.cs
#### - Tests/UniTest1.cs

--------------------------------------------------------------------------------------------
## Main.cs

Handle I/O

#### Main Functions: 

- ##### void Main() : have the intance of the objects of Game() AIPlayer() and writes in the [6,6,6] board the user input.
There is a while (true) loop that will end when there is a winner or a tie. If the user makes a type while introducing the coordinates of the square, there is a catch System.FromatException, so the user can try again introducing the input. If the square is occupied then it will get a message informing about it and asking for a new input. 

--------------------------------------------------------------------------------------------

## Game.cs

Have the board[6,6,6], have a instance of the object CheckBoard() which will be called when a new legal move 
is written in the board and it checks if it completes a row, column or diagonal so there will be a winner and the game ends. 

#### Main Functions:

- ##### public bool  Winner() :returns 1 or 2 if there is a winner else null.
-  ##### public bool Move(int z, int x, int y) : check if the board[z,x,y] is free, if it is not then return true and the user is asked to write again the input. If it is free (0), then writes the token of the player, and checks if it is a victory move (it has completed a row, column or diagonal), if it does changes Winner(). Increases the counter of the number of squares are occupied. 
- ##### public bool Tie() : return true if the squares occupied 200 or more.
- ##### public int[,,] GetBoard : returns the board.
- ##### public override string ToString() : outputs the board with the current moves. It is called at the end of each while loop of Main.cs.


--------------------------------------------------------------------------------------------

## AI.cs

### It has two classes: 

### AIPlayer 

#### Main Functions: 

- ##### Constructor AIPlayer(int turn): saves the number-turn it will use in the game. Creates a instance of the Minimax class.
- ##### public (int, int, int) BestMove(int[,,] board) : passes the board and its turn to the the function GetBestMove from Minimax and return the output.

### Minimax 

It has many private variables. Maximum depth in the recursion of alpha beta pruning is set to 5, in each depth 10 best moves are considered. maxSquaresCount is the number of squares possible which is 216. Since the board is 6-dimensional, dimension = 6. We have track of the bestMove(Z/X/Y) coordinates that will be updated in the alpha beta pruning algortihm. We have instances of SortingMoves and CheckBoard. 

#### Main Functions: 

- ##### Constructor Minimax() : initialized SortingMoves and CheckBoard instances.
- ##### public (int, int, int) GetBestMove(int[,,] board, int turn) : it is called by the AI player, initialized the BestMove(Z/X/Y) variables to 0 and call the AlphaBetaPruning() function to modify BestMove(Z/X/Y). 
- ##### private int AlphaBetaPruning() :It takes as parameters: board, turn, true, depth, alpha, beta, previous coordinate in the recusion z, x, y set to -1 if none is provided. In each layer of depth the maximaxing bool and turn int is exchange to the opponent, so we create a tree of sequencial moves.
##### We are maximizing for the current player and minimizing for the opponent. Alpha will start with the value int.MinValue and beta int.MaxValue.
##### Parts of this function 
##### If previous coordinates z, x, y are not -1, has been provided, then it checks if there was a winner in the previous recursion call in the turn of the opponent. If there was a winner, or all the board is completed, or the depth has reached the maximum depth set call the function GetScore().
##### Call GetFreeSquares() fuction to get a list of the current free squares and calls GetSortedList from the SortingMoves class to get the top 10 best moves (see in CheckBoard explanations), if the depth is 0 and there is a victory move then it sets BestMove(Z/X/Y) with those coordinates and exit the function without any recursion. 
##### Iteration over the the top 10 moves, write in the board the turn, next recursion of alpha beta pruning (it is supposed to reach the depth and then backtrack updating from botton to top the results), and then free the square again. 
##### PRUNING: If maximaxing then we update alpha and if alpha >= beta we prune that subtree of future chooses and return alpha. If minimizing we only update beta and prune if beta <= alpha returning beta. 
##### We keep track of the int outputs from AlphaBetaPruning in each layer of depth and add them to the scores list. Suppose we are in depth 3, we have new 10 best moves to explore from there, each one will be choosen and call next depth of AlphaBetaPruning() which will return a score for each of them, those scores are saved in the list, each score having the same index as the coordinates each respective coordinates in freeSquares(top10), when we are maximazing we wants the coordinates that has the highest score and minimazin the minimum, so we call the getMax(list, true/false) to get that index and set the BestMove(Z,X,Y). 
- ##### private int GetMax(list, true/false) : looks for the highest element in the list if true or the minimum if false. GetMin() GetMax() could be possible too but since they were the same code now they are unified. 
- ##### private int GetScore(currentTurn, depth, previousWinner) : if the previousWinner is the current player then we return a positive score, else a negative score. 
- ##### public static GetFreeSquares() : it does not belong to any instance of minimax, used in the Test1() of UniTest1.cs file. Gets the board (we may assume the matrix will be filled differently in each depth and for iteration of the AlphaBetaPruning()) and returns a list with the free squares. 
- #### private int OccupiedSquares(): It is called to compared if the currentMatrix is completely filled or not, so it returns the current number of occupied cells by any player. 


-------------------------------------------------------------------------------------------

## SortingMoves.cs

It has an instance of CheckBoard() class. Main fucntion is to return the top 10 best moves.

#### Main Functions: 

- ##### SortingMoves() : It takes many parameters : board, depth, freeSquares list provided by Minimax, the player's turn, opponent and it outputs a list with top 10 moves and a (int, int, int)? bestMove variable.
##### Creates a List<KeyValuePair>, call the GetValue() function from CheckBoard() to get that square score, the higher the more importance it has for winning the match or blocking the opponent. Sorts the List and returns the top 10 best moves.


-------------------------------------------------------------------------------------------

## CheckBoard.cs

Since in a board[6x6x6] we have 3^(6x6x6) possible combinations of a game, not only the AlphaBetaPruning can stop at subtrees that are not successful but another process to optimize the search is to sort the possible moves giving preferences to blocking the opponent for winning and filling more squares, each of those actions get extra points that are all sumed up. 

However, in a row where there is 3 'O' tokens and 'X' token, 4 so far, it is unecessary continuing filling that row with 'O' in the AI turn, so that row must be ignore, we return 0 in that case. The goal is to align 6 Tokens of the current player, when cheking a coordinate z, x, y and finding any row, column or diagonal that has exactly 5 Tokens of that player, then that free square z, x, y is the final to fill in and get a Victory.

There are 2 (int, int, int)? private variables as part of the class, WinningMove and BlockingMoves, those are necessary since passing this parameter to all the functions wouldn't be efficient. This class is used for 2 purposes 1) check if a square completes a row, column or diagonal by Eval(), and 2) check for a square what is its score for potential winning moves or blocking by GetValue().


#### Main Functions: 

- ##### public void Eval() : Takes parameters : board, turn, coordinates of the current square, out bool won, and out winner. This function is called from Game() class and Minimax(). This function will be called when the board[z, x, y] has already been filled. It calls CheckRow() and CheckAllDiagonals().

##### Called from Game() : It is called in the Move() function from Game() to check if the new move gives a victory to that player. Main interest is if the coordinate z, x, y has a competed row, column or diagonal, if it did then there is a winner and the bool won is set to true. Bool won is the desired variable. 

##### Called from Minimax() : It is called in the AlphaBetaPruning() and as before, we want to check is those coordinates complete a row, column or diagonal. The variable that will be needed is winner which will be passed GetScore() function of Minimax. 

- ##### public int GetValue() : Takes parameters : board, currentTurn, opponent, current coordinate, out (int, int, int) ? WinMove, out (int, int, int) ? blockMove. It calls CheckRow() and CheckAllDiagonals().
#####  All squares has a minimum score value of 1 and a maximum of 1 + CheckRow() +  CheckAllDiagonals(), 
##### both functions return 0 if it is not worth to continue filling that square because there are mixed tokens or the +1 for either 1) each square it has the turn's token or 2) each square that has the opponent token, it gets the maximum number of both. The output is that sum that will be given to SortingMoves() class.

All the fucntions above has few things in common: 

- if they have at any row, columm, diagonal mixed tokens i.e. winningScore >= 1 && blockingScore >= 1 then they will exit and return 0.
- It will return the maximum number between winningScore and blockingScore.
- Most of them will take the same parameters : board, bool order, current coordinate.
- At any row, column, diagonal we have 5 squares fill with the current player / opponent, then WinningMove or BlockingMove will be set, respectively. If there are 6 filled then the Victory Move will be set. 

- ##### private int DiagonalSameLevel(): Bool is true when checking (row-col) => (0,0), (1,1), (2,2)... and false => (0,5), (1, 4), (2, 3), (3, 4) in the same matrix. Notice that they dont have any square in common.
- ##### private int DiagonalColumnTop_to_ColumnBotton() : Bool is true if we go => (5, k, 5), (4, k, 4), (3, k, 3), (2, k, 2), (1, k, 1) and (0, k, 0)  and false if we go => (0, k, 5), (1, k, 4), (2, k, 3), (3, k, 2), (4, k, 1), (5, k, 0), both k is the same row value.
- ##### private int DiagonalRowTop_to_RowBotton() : Bool is true if we go => (0, 0, y), (1, 1, y), (2, 2, y)... and false if we go => (5, 0, y), (4, 1, y), (3, 2, y), (2, 3, y)...
- ##### private int DiagonalBothDirectionsfrom05() : Bool true if we go (0, 0, 5), (1, 1, 4), (2, 2, 3)... and false if (0, 5, 0), (1, 4, 1), (2, 3, 2)...
- ##### private int DiagonalBothDirectionsfrom00() : Bool true if we go (0, 0, 0), (1, 1, 1)... and false if we go (0, 0, 5), (1, 1, 4), (2, 2, 3)...
- ##### private int CheckRowCol() : checking the row, column in the same matrix where that coordinate is located and the column betweent dimensions. This one is always checked since it is basic.
- ##### private int CheckAllDiagonals() : calls all the other diagonal functions and collect the scores each one returns.

-------------------------------------------------------------------------------------------

## Tests/UniTest1.cs

### [Click here for information of Unitest](https://github.com/angiegranda/TicTacToe3D/blob/main/manualTests.md)



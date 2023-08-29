## TicTacToe-3D User vs AI using Alpha-Beta Pruning in a multi-dimensional array 6x6x6 in C#
### Autor: Angie Aslhy Granda Nuñez

### User documentation: 

The game is played in the console. First you would need to install .NET [link for MacOS Windows and Linux](https://dotnet.microsoft.com/en-us/download). 
If you are installing .NET, I would recommend to create a testing folder and and run 'dotnet new console', a new project will be created. It is not necessary for the game but it proofs that it was correctly installed. 

You will need .NET 0.6 version.

To start, clone the game via HTTPS or SSH. Access the folder TicTacToe3D and run 'dotnet run'

### Introduction:

The goal is be the first who add tokens,'X' for user and 'O' for the AI, in any possible row, column or diagonal. The board is 6-dimensional so those are possible winning moves: 

- Tokens in row 2, column 1 and in all it's matrix from 1 to 6
- Tokens in (matrix, row, col) format => (1, 1, 1), (2, 2, 2), (3, 3, 3), (4, 4, 4), (5, 5, 5) and (6, 6, 6) ; 
- another version could be  (6, 1, 1), (5, 2, 2), (4, 3, 3), (3, 4, 4), (2, 5, 5) and (1, 6, 6). 
- Forming a row with a token in each matrix => (1, 1, 1), (2, 1, 2), (3, 1, 3), (4, 1, 4), (5, 1, 5) and (6, 1, 6)
- Forming a column with a token in each matrix => (1, 1, 1),(2, 2, 1), (3, 3, 1), (4, 4, 1), (5, 5, 1) and (6, 6, 1)

Any possible diagonal, row, column in which 6 tokens of the same player User-AI are placed will be a victory. 

### Rules:

- The game starts with the command 'dotnet run'.
- It will ask your name, write in console the username you wish.
  
**Introduce your name:**
**Answer below**
  
- AI plays under the username 'computer'
- Token for user is **'X'** and for computer **'O'**
- User will be asked the coordinates of the matrix, row, column to place its token

#### Introduce matrix {1, 2, 3, 4, 5, 6}: Number here ####
#### Introduce row {1, 2, 3, 4, 5, 6}:  ... ####
#### Introduce column {1, 2, 3, 4, 5, 6}: ... ####
  
- If the user makes a typo, it will ask the coordinates again.
- when the user has introduced the input square, there will be a sentence written after saying:
#### It is the computer turn : move is matrix: --, row: --, column: --" ####
So the user can identify quickly the computer's more in a more advanced game.
- After the user input the coordinates of the square, the computer will make its move and the matrix will be printed with the two new tokens.
- There are 216 squares to fill, if anyone has won by the moment it reaches 200 then the game will end in a tie.
- If you wish to end the game press Ctrl+C.

### [Developer Documentation here](https://github.com/angiegranda/TicTacToe3D/blob/main/devdocs.md)

### [Unitest Results](https://github.com/angiegranda/TicTacToe3D/blob/main/manualTests.md)


  








using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SOS
{
    public class ComputerGameLogic : GameLogic
    {
        public ComputerGameLogic(int boardSize, string gameMode, bool player1Human, bool player2Human) : base(boardSize, gameMode, player1Human, player2Human)
        {
        }

        public void automaticSOSGame(ref List<Button> buttons)
        {
            while (!GameDone)
            {
                if (CurrentPlayer == "BLUE")
                    computerPlayerMove(ref buttons, bluePlayer, (Color)ColorConverter.ConvertFromString("#FF0D80FF"));
                else
                    computerPlayerMove(ref buttons, redPlayer, Colors.Red);
            }
        }

        public List<int> checkForPointCPUMove(string gameMode, CellData playerInfo, Position position)
        {
            List<int> cases = new List<int>();
            if (playerInfo.value == "S")
                cases = checkSPlacement(position);
            else
                cases = checkOPlacement(position);
            return cases;
        }

        public bool checkForWinningCPUMove(Player currentPlayer, ref Position winPosition)
        {
            Position tempPosition = new Position(0, 0);
            List<int> cases = new List<int>();
            CellData[,] testBoard = new CellData[BoardSize, BoardSize];
            
            CellData cellData;
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    testBoard = Board.Clone() as CellData[,]; 
                    if (testBoard[i, j].value != null)                                                      //If the current cell is already occupied, skip to the next loop iteration
                        continue;
                    tempPosition = new Position(i, j);                                                      //Set temp position to current loop iteration

                    currentPlayer.placementType = "S";                                                      //Update the currentPlayers placement type to be S to test if S is a winning move in the current cell
                    cellData = new CellData(currentPlayer.placementType, currentPlayer.playerColor); 
                                                                  //Deep copy the Board variable to create a testingBoard without affecting actual Board
                    testBoard[i, j] = cellData;                                                             //Make move as S
                    cases = checkForPointCPUMove(CurrentGameMode, cellData, tempPosition);                  //Determing if point was made
                    if (cases.Last() > 0)
                    {
                        winPosition = new Position(i, j);
                        return true;
                    }

                    cases.Clear(); 
                    currentPlayer.placementType = "O";                                                      //Update the currentPlayers placement type to be O to test if O is a winning move in the current cell
                    cellData = new CellData(currentPlayer.placementType, currentPlayer.playerColor);
                    testBoard = Board.Clone() as CellData[,];                                               //Reset the testing board
                    testBoard[i, j] = cellData;                                                             //Make move as O
                    cases = checkForPointCPUMove(CurrentGameMode, cellData, tempPosition);                  //Determine if point was made
                    if(cases.Last() > 0)
                    {
                        winPosition = new Position(i, j);
                        return true;
                    }
                }
            }
            return false;
        }

        public void computerPlayerMove(ref List<Button> buttons, Player currentPlayer, Color drawColor)
        {
            bool turnOver = false;
            bool validPlacement = false;
            List<int> cases = new List<int>();
            Position winMovePosition = new Position(0, 0);
            //Check if the first move (board is empty). If so make random move.
            if (BoardCount == 0)
            {
                int i = 0;
                Random random = new Random();
                i = random.Next(buttons.Count());

                CellData cellData = new CellData(currentPlayer.placementType, currentPlayer.playerColor);

                var coordinates = buttons.ElementAt(i).Tag.ToString().Split(',');
                var xValue = int.Parse(coordinates[0]);
                var yValue = int.Parse(coordinates[1]);
                var ButtonPosition = new Position(xValue, yValue);

                buttons.ElementAt(i).Foreground = currentPlayer.colorValue;
                buttons.ElementAt(i).Content = currentPlayer.placementType;

                updateBoard(ButtonPosition, cellData);
                SetNextPlayer();
                ((MainWindow)Application.Current.MainWindow).updatePlayerTurnDisplay();
                return;
            }
            else
            {
                while (!turnOver)//Ensures that a computer player can play more than once if point is scored during general game
                {
                    //Check if a winning move can be made
                    if (checkForWinningCPUMove(currentPlayer, ref winMovePosition))
                    {
                        CellData cellData = new CellData(currentPlayer.placementType, currentPlayer.playerColor);
                        int count = -1;
                        for (int i = 0; i < BoardSize; i++)
                        {
                            for (int j = 0; j < BoardSize; j++)
                            {
                                count++;
                                if (i == winMovePosition.x && j == winMovePosition.y)
                                {
                                    goto LoopEnd;
                                }
                            }
                        }
                    LoopEnd:

                        buttons.ElementAt(count).Foreground = currentPlayer.colorValue;
                        buttons.ElementAt(count).Content = currentPlayer.placementType;

                        updateBoard(winMovePosition, cellData);

                        cases.Clear();
                        cases = checkForWinOrPoint(cellData, winMovePosition);
                        ((MainWindow)Application.Current.MainWindow).DrawCases(cases, drawColor, BoardSize, winMovePosition);
                        ((MainWindow)Application.Current.MainWindow).updatePlayerPointsDisplay();

                        if (GameDone)
                        {
                            ((MainWindow)Application.Current.MainWindow).winScreen.Text = WinMessage;
                            ((MainWindow)Application.Current.MainWindow).winScreen.Visibility = Visibility.Visible;
                            return;
                        }
                    }
                    else
                    {
                        int i = 0;
                        Random random = new Random();
                        while (!validPlacement)
                        {
                            //generates a random cell value to make a move in
                            i = random.Next(buttons.Count());
                            if (!String.IsNullOrWhiteSpace(buttons.ElementAt(i).Content?.ToString()))
                                continue;
                            else
                                validPlacement = true;
                        }
                        //Generate a random placement type for the move
                        setRandomPlacementTypeCPU(currentPlayer);

                        CellData cellData = new CellData(currentPlayer.placementType, currentPlayer.playerColor);

                        var coordinates = buttons.ElementAt(i).Tag.ToString().Split(',');
                        var xValue = int.Parse(coordinates[0]);
                        var yValue = int.Parse(coordinates[1]);
                        var ButtonPosition = new Position(xValue, yValue);

                        //Adjust button to display correct information
                        buttons.ElementAt(i).Foreground = currentPlayer.colorValue;
                        buttons.ElementAt(i).Content = currentPlayer.placementType;

                        //Update the gameLogic Board variable
                        updateBoard(ButtonPosition, cellData);

                        //Check if a point was scored
                        cases.Clear();
                        cases = checkForWinOrPoint(cellData, ButtonPosition);
                        ((MainWindow)Application.Current.MainWindow).DrawCases(cases, drawColor, BoardSize, ButtonPosition);
                        ((MainWindow)Application.Current.MainWindow).updatePlayerPointsDisplay();

                        //If game is done display winner message
                        if (GameDone)
                        {
                            ((MainWindow)Application.Current.MainWindow).winScreen.Text = WinMessage;
                            ((MainWindow)Application.Current.MainWindow).winScreen.Visibility = Visibility.Visible;
                            return;
                        }
                    }
                    if (CurrentGameMode == "SIMPLE" || cases.Count <= 1)
                        turnOver = true;
                }
            }
            SetNextPlayer();
            ((MainWindow)Application.Current.MainWindow).updatePlayerTurnDisplay();
        }
        protected void setRandomPlacementTypeCPU(Player currentPlayer)
        {
            Random rand = new Random();
            int i = rand.Next(0, 2);

            if (i == 0)
                currentPlayer.placementType = "S";
            else
                currentPlayer.placementType = "O";
        }
    }
}

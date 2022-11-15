using Accessibility;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace SOS
{
    public class GameLogic
    {
        public GameLogic(int boardSize, string gameMode, bool player1Human, bool player2Human)
        {
            Board = new CellData[boardSize, boardSize];
            BoardSize = boardSize;
            BoardCount = 0;
            CurrentGameMode = gameMode;
            WinMessage = "";

            if (player1Human == true && player2Human == true) //H v H
            {
                bluePlayer = new Player(blue, blueBrush);
                redPlayer = new Player(red, redBrush);
                CurrentPlayerType = player;
            }
            else if (player1Human == true && player2Human == false) //H v C
            {
                bluePlayer = new Player(blue, blueBrush);
                redPlayer = new ComputerPlayer(red, redBrush);
                CurrentPlayerType = player;
            }
            else if (player1Human == false && player2Human == true) // C v H
            {
                bluePlayer = new ComputerPlayer(blue, blueBrush);
                redPlayer = new Player(red, redBrush);
                CurrentPlayerType = computer;
            }
            else //C v C
            {
                bluePlayer = new ComputerPlayer(blue, blueBrush);
                redPlayer = new ComputerPlayer(red, redBrush);
                CurrentPlayerType = computer;
            }
        }

        public static SolidColorBrush blueBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0D80FF"));
        public SolidColorBrush redBrush = new SolidColorBrush(Colors.Red);

        //Create new constructor for the different player type scenarios
        // H v H  H v C  C v H  C v C
        //Potentially need to 

        public Player bluePlayer; //= new Player(blue, blueBrush);
        public Player redPlayer; //= new Player(red, redBrush);

        public string CurrentPlayer { get; set; } = blue;
        private const string blue = "BLUE";
        private const string red = "RED";

        public string WinMessage { get; set; }

        public string CurrentPlayerType { get; set; }
        private const string player = "PLAYER";
        private const string computer = "COMPUTER";

        public string CurrentGameMode { get; set; }
        private const string simple = "SIMPLE";
        private const string general = "GENERAL";

        public CellData[,] Board = new CellData[3, 3];
        public int BoardCount;
        public int BoardSize;
        public bool GameDone = false;

        public void updateBoard(Position position, CellData value)
        {
            Board[position.x, position.y] = value;
            BoardCount++;
        }

        /*public void updateBoardVariableSize(int boardSize)
        {
            if (boardSize != 3)
            {
                Board = new CellData[boardSize, boardSize];
                return;
            }
            return;
        }*/
        /*public void updateGameMode(string gameMode)
        {
            if (CurrentGameMode != gameMode)
            {
                CurrentGameMode = gameMode;
                return;
            }
            return;
        }*/

        public void SetNextPlayer()
        {
            if (CurrentPlayer == blue)
                CurrentPlayer = red;
            else
                CurrentPlayer = blue;
        }

        public void updatePlayerPlacementType(string playerColor, string value)
        {
            if (playerColor == blue)
                bluePlayer.placementType = value;
            else if (playerColor == red)
                redPlayer.placementType = value;
        }

        public List<int> checkForWinOrPoint(string gameMode, CellData playerInfo, Position position)
        {
            List<int> cases = new List<int>();
            if (gameMode == simple)
            {
                if (playerInfo.value == "S")
                {
                    cases = checkSPlacement(position);
                    if (cases.Last() > 0)
                    {
                        GameDone = true;
                        WinMessage = CurrentPlayer + " WINS!";
                    }
                }
                else
                {
                    cases = checkOPlacement(position);
                    if (cases.Last() > 0)
                    {
                        GameDone = true;
                        WinMessage = CurrentPlayer + " WINS!";
                    }
                }

                if (Board.Length == BoardCount)
                {
                    GameDone = true;
                    WinMessage = "DRAW";
                }

                return cases;
            }
            else
            {
                if (playerInfo.value == "S")
                {
                    cases = checkSPlacement(position);

                    if(CurrentPlayer == blue)
                        bluePlayer.totalPoints += cases.Last();
                    else
                        redPlayer.totalPoints += cases.Last();
                }
                else
                {
                    cases = checkOPlacement(position);

                    if (CurrentPlayer == blue)
                        bluePlayer.totalPoints += cases.Last();
                    else
                        redPlayer.totalPoints += cases.Last();
                }

                if (Board.Length == BoardCount)
                {
                    GameDone = true;

                    if(bluePlayer.totalPoints > redPlayer.totalPoints)
                        WinMessage = blue + " WINS!";
                    else if (redPlayer.totalPoints > bluePlayer.totalPoints)
                        WinMessage = red + " WINS!";
                    else
                        WinMessage = "DRAW";
                }
                return cases;
            }
        }

        public List<int> checkSPlacement(Position position)
        {
            List<int> cases= new List<int>();
            int pointsScored = 0;

            for (int i = 0; i < 8; i++)
            {
                try
                {
                    switch (i)
                    {
                        case 0: //Top left corner
                            if (Board[position.x - 1, position.y - 1].value == "O")
                                if (Board[position.x - 2, position.y - 2].value == "S")
                                {
                                    pointsScored++;
                                    cases.Add(0);
                                }
                            break;
                        case 1: //Top middle cell
                            if (Board[position.x, position.y - 1].value == "O")
                                if (Board[position.x, position.y - 2].value == "S")
                                {
                                    pointsScored++;
                                    cases.Add(1);
                                }
                            break;
                        case 2: //Top right corner
                            if (Board[position.x + 1, position.y - 1].value == "O")
                                if (Board[position.x + 2, position.y - 2].value == "S")
                                {
                                    pointsScored++;
                                    cases.Add(2);
                                }
                            break;
                        case 3: //Left side cell
                            if (Board[position.x - 1, position.y].value == "O")
                                if (Board[position.x - 2, position.y].value == "S")
                                {
                                    pointsScored++;
                                    cases.Add(3);
                                }
                            break;
                        case 4: //Right side cell
                            if (Board[position.x + 1, position.y].value == "O")
                                if (Board[position.x + 2, position.y].value == "S")
                                {
                                    pointsScored++;
                                    cases.Add(4);
                                }
                            break;
                        case 5: //Bottom left corner
                            if (Board[position.x - 1, position.y + 1].value == "O")
                                if (Board[position.x - 2, position.y + 2].value == "S")
                                {
                                    pointsScored++;
                                    cases.Add(5);
                                }
                            break;
                        case 6: //Bottom middle cell
                            if (Board[position.x, position.y + 1].value == "O")
                                if (Board[position.x, position.y + 2].value == "S")
                                {
                                    pointsScored++;
                                    cases.Add(6);
                                }
                            break;
                        case 7: //Bottom right corner
                            if (Board[position.x + 1, position.y + 1].value == "O")
                                if (Board[position.x + 2, position.y + 2].value == "S")
                                {
                                    pointsScored++;
                                    cases.Add(7);
                                }
                            break;
                        default:
                            break;
                    }
                }
                catch(IndexOutOfRangeException e)
                {
                    continue;
                }
            }
            
            cases.Add(pointsScored);
            return cases;
        }

        public List<int> checkOPlacement(Position position)
        {
            List<int> cases = new List<int>();
            int pointsScored = 0;
            
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    switch (i)
                    {
                        case 0: //Check for S's above and below
                            if(Board[position.x, position.y - 1].value == "S" && Board[position.x, position.y + 1].value == "S")
                            {
                                pointsScored++;
                                cases.Add(8);
                            }
                            break;
                        case 1: //Check for S's on left and right
                            if (Board[position.x - 1, position.y].value == "S" && Board[position.x + 1, position.y].value == "S")
                            {
                                pointsScored++;
                                cases.Add(9);
                            }
                            break;
                        case 2: //Check for S/s on top left and bottome right
                            if (Board[position.x - 1, position.y - 1].value == "S" && Board[position.x + 1, position.y + 1].value == "S")
                            {
                                pointsScored++;
                                cases.Add(10);
                            }
                            break;
                        case 3: //Check for S's on top right and bottom left
                            if (Board[position.x + 1, position.y - 1].value == "S" && Board[position.x - 1, position.y + 1].value == "S")
                            {
                                pointsScored++;
                                cases.Add(11);
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch(IndexOutOfRangeException e)
                {
                    continue;
                }
            }
            cases.Add(pointsScored);
            return cases;
        }

        public void computerPlayerMove(ref List<Button> buttons,Player currentPlayer, Color drawColor)
        {
            bool turnOver = false;
            bool validPlacement = false;
            List<int> cases = new List<int>();
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
                turnOver = true;
                SetNextPlayer();
                ((MainWindow)Application.Current.MainWindow).updatePlayerTurnDisplay();
            }
            //Check if a winning move can be made
            //Check if blocking move is possible
            //make random move
            else
            {
                while(!turnOver)
                {
                    int i = 0;
                    Random random = new Random();
                    while (!validPlacement)
                    {
                        
                        i = random.Next(buttons.Count());
                        if (!String.IsNullOrWhiteSpace(buttons.ElementAt(i).Content?.ToString()))
                            continue;
                        else
                            validPlacement = true;
                    }

                    setRandomPlacementTypeCPU(currentPlayer);

                    CellData cellData = new CellData(currentPlayer.placementType, currentPlayer.playerColor);

                    var coordinates = buttons.ElementAt(i).Tag.ToString().Split(',');
                    var xValue = int.Parse(coordinates[0]);
                    var yValue = int.Parse(coordinates[1]);
                    var ButtonPosition = new Position(xValue, yValue);

                    buttons.ElementAt(i).Foreground = currentPlayer.colorValue;
                    buttons.ElementAt(i).Content = currentPlayer.placementType;

                    updateBoard(ButtonPosition, cellData);

                    cases = checkForWinOrPoint(CurrentGameMode, cellData, ButtonPosition);
                    ((MainWindow)Application.Current.MainWindow).DrawCases(cases, drawColor, BoardSize, ButtonPosition);
                    ((MainWindow)Application.Current.MainWindow).updatePlayerPointsDisplay();
                
                    if(GameDone)
                    {
                        ((MainWindow)Application.Current.MainWindow).winScreen.Text = WinMessage;
                        ((MainWindow)Application.Current.MainWindow).winScreen.Visibility = Visibility.Visible;
                        return;
                    }
                    if (CurrentGameMode == "SIMPLE")
                        turnOver = true;
                    else if (cases.Count <= 1)
                        turnOver = true;
                }

                SetNextPlayer();
                ((MainWindow)Application.Current.MainWindow).updatePlayerTurnDisplay();
            }
        }

        private void setRandomPlacementTypeCPU(Player currentPlayer)
        {
            Random rand = new Random();
            int i = rand.Next(0,2);

            if (i == 0)
                currentPlayer.placementType = "S";
            else
                currentPlayer.placementType = "O";
        }

        //Test Method usage only
        public void generateLogicBoard()
        {
            CellData[] cellData = new CellData[6];
            for(int i = 0; i < 6; i++)
            {
                if (i % 2 == 0)
                    cellData[i] = new CellData("S", "BLUE");
                else
                    cellData[i] = new CellData("O", "RED"); 
            }
            int count = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Board[i, j] = cellData[count];
                    count++;
                    BoardCount++;
                }
            }
        }

        public void generateLogicBoard2()
        {
            CellData[] cellData = new CellData[6];
            for (int i = 0; i < 6; i++)
            {
                if (i % 2 == 0)
                    cellData[i] = new CellData("S", "BLUE");
                else
                    cellData[i] = new CellData("O", "RED");
            }
            int count = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if(i == 1 && j == 1)
                    {
                        Board[i, j] = new CellData("S", "RED");
                        count++;
                        BoardCount++;
                    }
                    else
                    {
                        Board[i, j] = cellData[count];
                        count++;
                        BoardCount++;
                    }
                }
            }
        }

        public void setGameMode(string gameMode)
        {
            CurrentGameMode = gameMode;
        }

        public void makeMove(string type, string playerColor, Position position)
        {
            if (!String.IsNullOrEmpty(Board[position.x, position.y].value))
                return;
            else
            {
                CellData cellData = new CellData(type, playerColor);
                Board[position.x, position.y] = cellData;
                BoardCount++;
                List<int> temp = new List<int>();
                temp = checkForWinOrPoint(CurrentGameMode, cellData, position);

                if (temp.Count > 1 && CurrentGameMode == "GENERAL")
                    return;

                SetNextPlayer();
            }
        }

        public CellData getCellData(Position position)
        {
            return Board[position.x, position.y];
        }
    }
}

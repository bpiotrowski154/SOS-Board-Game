using Accessibility;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace SOS
{
    public class GameLogic
    {

        private static SolidColorBrush blueBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0D80FF"));
        private static SolidColorBrush redBrush = new SolidColorBrush(Colors.Red);

        public Player bluePlayer = new Player(blue, blueBrush);
        public Player redPlayer = new Player(red, redBrush);

        public string CurrentPlayer { get; set; } = blue;
        private const string blue = "BLUE";
        private const string red = "RED";

        public string WinMessage { get; set; }

        public string CurrentGameMode { get; set; } = simple;
        private const string simple = "SIMPLE";
        private const string general = "GENERAL";


        public CellData[,] Board = new CellData[3, 3];
        public int BoardCount = 0;
        public bool GameDone = false;


        public void updateBoard(Position position, CellData value)
        {
            Board[position.x, position.y] = value;
            BoardCount++;
        }

        public void updateBoardVariableSize(int boardSize)
        {
            if (boardSize != 3)
            {
                Board = new CellData[boardSize, boardSize];
                return;
            }
            return;
        }
        public void updateGameMode(string gameMode)
        {
            if (CurrentGameMode != gameMode)
            {
                CurrentGameMode = gameMode;
                return;
            }
            return;
        }

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
            {
                bluePlayer.placementType = value;
            }
            else if (playerColor == red)
            {
                redPlayer.placementType = value;
            }
        }

        public void checkForWinOrPoint(string gameMode, CellData playerInfo, Position position)
        {
            if (gameMode == simple)
            {
                if (playerInfo.value == "S")
                {
                    if(checkSPlacement(position) > 0)
                    {
                        GameDone = true;
                        WinMessage = CurrentPlayer + " WINS!";
                    }
                }
                else if (playerInfo.value == "O")
                {
                    if (checkOPlacement(position) > 0)
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
            }
            else
            {
                if (playerInfo.value == "S")
                {
                    if(CurrentPlayer == blue)
                    {
                        bluePlayer.totalPoints += checkSPlacement(position);
                    }
                    else
                    {
                        redPlayer.totalPoints += checkSPlacement(position);
                    }
                }
                else
                {
                    if (CurrentPlayer == blue)
                    {
                        bluePlayer.totalPoints += checkOPlacement(position);
                    }
                    else
                    {
                        redPlayer.totalPoints += checkOPlacement(position);
                    }
                }

                if (Board.Length == BoardCount)
                {
                    GameDone = true;
                    if(bluePlayer.totalPoints > redPlayer.totalPoints)
                    {
                        WinMessage = blue + "WINS!";
                    }
                    else if (redPlayer.totalPoints > bluePlayer.totalPoints)
                    {
                        WinMessage = red + "WINS!";
                    }
                    else
                    {
                        WinMessage = "DRAW";
                    }
                }
            }
        }

        public int checkSPlacement(Position position)
        {
            int pointsScored = 0;
            for (int i = 0; i < 8; i++)
            {
                try
                {
                    switch (i)
                    {
                        case 0: //Top left corner
                            if (Board[position.x - 1, position.y - 1].value == "O")
                            {
                                if (Board[position.x - 2, position.y - 2].value == "S")
                                {
                                    pointsScored++;
                                    //DrawLineFunction()
                                }
                            }
                            break;
                        case 1: //Top middle cell
                            if (Board[position.x, position.y - 1].value == "O")
                            {
                                if (Board[position.x, position.y - 2].value == "S")
                                {
                                    pointsScored++;
                                    //DrawLineFunction()
                                }
                            }
                            break;
                        case 2: //Top right corner
                            if (Board[position.x + 1, position.y - 1].value == "O")
                            {
                                if (Board[position.x + 2, position.y - 2].value == "S")
                                {
                                    pointsScored++;
                                    //DrawLineFunction()
                                }
                            }
                            break;
                        case 3: //Left side cell
                            if (Board[position.x - 1, position.y].value == "O")
                            {
                                if (Board[position.x - 2, position.y].value == "S")
                                {
                                    pointsScored++;
                                    //DrawLineFunction()
                                }
                            }
                            break;
                        case 4: //Right side cell
                            if (Board[position.x + 1, position.y].value == "O")
                            {
                                if (Board[position.x + 2, position.y].value == "S")
                                {
                                    pointsScored++;
                                    //DrawLineFunction()
                                }
                            }
                            break;
                        case 5: //Bottom left corner
                            if (Board[position.x - 1, position.y + 1].value == "O")
                            {
                                if (Board[position.x - 2, position.y + 2].value == "S")
                                {
                                    pointsScored++;
                                    //DrawLineFunction()
                                }
                            }
                            break;
                        case 6: //Bottom middle cell
                            if (Board[position.x, position.y + 1].value == "O")
                            {
                                if (Board[position.x, position.y + 2].value == "S")
                                {
                                    pointsScored++;
                                    //DrawLineFunction()
                                }
                            }
                            break;
                        case 7: //Bottom right corner
                            if (Board[position.x + 1, position.y + 1].value == "O")
                            {
                                if (Board[position.x + 2, position.y + 2].value == "S")
                                {
                                    pointsScored++;
                                    //DrawLineFunction()
                                }
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
                
            return pointsScored;
        }

        public int checkOPlacement(Position position)
        {
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
                                //DrawLineFunction()
                            }
                            break;
                        case 1: //Check for S's on left and right
                            if (Board[position.x - 1, position.y].value == "S" && Board[position.x + 1, position.y].value == "S")
                            {
                                pointsScored++;
                                //DrawLineFunction()
                            }
                            break;
                        case 2: //Check for S/s on top left and bottome right
                            if (Board[position.x - 1, position.y - 1].value == "S" && Board[position.x + 1, position.y + 1].value == "S")
                            {
                                pointsScored++;
                                //DrawLineFunction()
                            }
                            break;
                        case 3: //Check for S's on top right and bottom left
                            if (Board[position.x + 1, position.y - 1].value == "S" && Board[position.x - 1, position.y + 1].value == "S")
                            {
                                pointsScored++;
                                //DrawLineFunction()
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
            return pointsScored;
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
            {
                return;
            }
            else
            {
                Board[position.x, position.y] = new CellData(type, playerColor);
                SetNextPlayer();
            }
                

        }

        public CellData getCellData(Position position)
        {
            return Board[position.x, position.y];
        }
    }
}

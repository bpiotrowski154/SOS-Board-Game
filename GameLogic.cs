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

        public string CurrentGameMode { get; set; } = simple;
        private const string simple = "SIMPLE";
        private const string general = "GENERAL";


        public CellData[,] Board = new CellData[3, 3];
        public bool GameDone = false;


        public void updateBoard(Position position, CellData value)
        {
            Board[position.x, position.y] = value;
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
                    if(checkSPlacement(position) > 1)
                    {
                        GameDone = true;
                    }
                }
                else
                {

                }
            }
            else
            {

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
            /*int rowIndx = Math.Max(0, position.y - 1);
             int colIndx = Math.Max(0, position.x - 1);
             int rowLength = 3;
             int colLength = 3;
             int iCounter = 0;
             int jCounter = 0;

             //If the cell clicked was in the anywhere in the left column only check two spots over instead
             if(rowIndx == 0)
             {
                 rowLength = 2;
             }
             if(colIndx == 0)
             {
                 colLength = 2;
             }

             for (int i = rowIndx; i < (rowIndx+3); i++)
             {
                 for(int j = colIndx; j < (colIndx+3); j++)
                 {
                     if (i == position.x && j == position.y)
                         continue;
                     try
                     {
                         if (Board[i,j].value == "O")
                         {
                             if (iCounter == 0) // Checking the three cells in row above
                             {
                                 if (jCounter == 0)//Checking top left corner
                                 {

                                 }
                                 else if(jCounter == 1)//Checking cell above
                                 {

                                 }
                                 else//checking top right corner
                                 {

                                 }
                             }
                             else if (iCounter == 1) //Checking the two side cells
                             {

                             }
                             else // Checking the three cells in row below
                             {

                             }
                         }
                     }
                     catch(IndexOutOfRangeException e)
                     {
                         continue;
                     }

                     jCounter++;
                 }
                 iCounter++;
             }
             //Check two side cells
             //Check bottom three cells*/
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

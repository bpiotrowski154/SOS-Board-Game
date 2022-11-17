using Accessibility;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
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
            WinMessage = null;

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

        public List<int> checkForWinOrPoint(CellData playerInfo, Position position)
        {
            List<int> cases = new List<int>();
            if (CurrentGameMode == simple)
            {
                if (playerInfo.value == "S") //If game mode is simple and the player placed an S
                {
                    cases = checkSPlacement(position); //Check if any points were made from the placement
                    if (cases.Last() > 0) //If a point was made the Game is over and the Current player wins
                    {
                        if (CurrentPlayer == blue)
                            bluePlayer.totalPoints += cases.Last();
                        else
                            redPlayer.totalPoints += cases.Last();
                        GameDone = true;
                        WinMessage = CurrentPlayer + " WINS!";
                        return cases;
                    }
                }
                else //If game mode is simple and the player placed an O
                {
                    cases = checkOPlacement(position); //Check if any points were made from the placement
                    if (cases.Last() > 0) //If a point was made the Game is over and the current player win
                    {
                        if (CurrentPlayer == blue)
                            bluePlayer.totalPoints += cases.Last();
                        else
                            redPlayer.totalPoints += cases.Last();
                        GameDone = true;
                        WinMessage = CurrentPlayer + " WINS!";
                        return cases;
                    }
                }

                if (Board.Length == BoardCount) //If no player scored a point and the Board is full the game is over and its a draw
                {
                    GameDone = true;
                    WinMessage = "DRAW";
                }

                return cases;
            }
            else
            {
                if (playerInfo.value == "S") //If game mode is general and the player placed an S
                {
                    cases = checkSPlacement(position); //Check if any points were made from the placement
                    if (CurrentPlayer == blue)
                        bluePlayer.totalPoints += cases.Last(); //Add total number of points scored from move for blue player
                    else
                        redPlayer.totalPoints += cases.Last(); //Add total number of points scored from move for red player
                }
                else //If game mode is general and the player placed an O
                {
                    cases = checkOPlacement(position); //Check if any points were made from the placement
                    if (CurrentPlayer == blue)
                        bluePlayer.totalPoints += cases.Last(); //Add total number of points scored from move for blue player
                    else
                        redPlayer.totalPoints += cases.Last(); //Add total number of points scored from move for red player
                }

                if (Board.Length == BoardCount) 
                {
                    GameDone = true; //If the board is full the game is over and the winner is whoever has the most points

                    if (bluePlayer.totalPoints > redPlayer.totalPoints)
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
            List<int> cases = new List<int>();
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
                catch (IndexOutOfRangeException e)
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
                            if (Board[position.x, position.y - 1].value == "S" && Board[position.x, position.y + 1].value == "S")
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
                catch (IndexOutOfRangeException e)
                {
                    continue;
                }
            }
            cases.Add(pointsScored);
            return cases;
        }
    }
}

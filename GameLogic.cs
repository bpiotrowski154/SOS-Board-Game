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
using System.IO;

namespace SOS
{
    public class GameLogic
    {
        public GameLogic(int boardSize, string gameMode, bool player1Human, bool player2Human)
        {
            board = new CellData[boardSize, boardSize];
            this.boardSize = boardSize;
            boardCount = 0;
            currentGameMode = gameMode;
            winMessage = null;

            if (player1Human == true && player2Human == true) //H v H
            {
                bluePlayer = new Player(blue, blueBrush);
                redPlayer = new Player(red, redBrush);
                currentPlayerType = player;
            }
            else if (player1Human == true && player2Human == false) //H v C
            {
                bluePlayer = new Player(blue, blueBrush);
                redPlayer = new ComputerPlayer(red, redBrush);
                currentPlayerType = player;
            }
            else if (player1Human == false && player2Human == true) // C v H
            {
                bluePlayer = new ComputerPlayer(blue, blueBrush);
                redPlayer = new Player(red, redBrush);
                currentPlayerType = computer;
            }
            else //C v C
            {
                bluePlayer = new ComputerPlayer(blue, blueBrush);
                redPlayer = new ComputerPlayer(red, redBrush);
                currentPlayerType = computer;
            }
        }

        public static SolidColorBrush blueBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0D80FF"));
        public SolidColorBrush redBrush = new SolidColorBrush(Colors.Red);

        public Player bluePlayer;
        public Player redPlayer;

        public string currentPlayer { get; set; } = blue;
        private const string blue = "BLUE";
        private const string red = "RED";

        public string winMessage { get; set; }

        public string currentPlayerType { get; set; }
        private const string player = "PLAYER";
        private const string computer = "COMPUTER";

        public string currentGameMode { get; set; }
        private const string simple = "SIMPLE";
        private const string general = "GENERAL";

        public CellData[,] board = new CellData[3, 3];
        public int boardCount;
        public int boardSize;
        public bool gameDone = false;

        private List<string> gameRecording = new List<string>();

        public void updateBoard(Position position, CellData value)
        {
            board[position.x, position.y] = value;
            boardCount++;

            string previousMove = $"{value.colorOfPlayer.ToUpper()} player placed {value.value} at ({position.x},{position.y})";
            gameRecording.Add(previousMove);
        }

        public void SetNextPlayer()
        {
            if (currentPlayer == blue)
                currentPlayer = red;
            else
                currentPlayer = blue;
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
            if (currentGameMode == simple)
            {
                if (playerInfo.value == "S") //If game mode is simple and the player placed an S
                {
                    cases = checkSPlacement(position); //Check if any points were made from the placement
                    if (cases.Last() > 0) //If a point was made the Game is over and the Current player wins
                    {
                        if (currentPlayer == blue)
                            bluePlayer.totalPoints += cases.Last();
                        else
                            redPlayer.totalPoints += cases.Last();
                        gameDone = true;
                        winMessage = currentPlayer + " WINS!";

                        File.WriteAllLines("GameRecording.txt", gameRecording.ToArray());

                        return cases;
                    }
                }
                else //If game mode is simple and the player placed an O
                {
                    cases = checkOPlacement(position); //Check if any points were made from the placement
                    if (cases.Last() > 0) //If a point was made the Game is over and the current player win
                    {
                        if (currentPlayer == blue)
                            bluePlayer.totalPoints += cases.Last();
                        else
                            redPlayer.totalPoints += cases.Last();
                        gameDone = true;
                        winMessage = currentPlayer + " WINS!";

                        File.WriteAllLines("GameRecording.txt", gameRecording.ToArray());

                        return cases;
                    }
                }

                if (board.Length == boardCount) //If no player scored a point and the Board is full the game is over and its a draw
                {
                    gameDone = true;
                    File.WriteAllLines("GameRecording.txt", gameRecording.ToArray());
                    winMessage = "DRAW";
                }

                return cases;
            }
            else
            {
                if (playerInfo.value == "S") //If game mode is general and the player placed an S
                {
                    cases = checkSPlacement(position); //Check if any points were made from the placement
                    if (currentPlayer == blue)
                        bluePlayer.totalPoints += cases.Last(); //Add total number of points scored from move for blue player
                    else
                        redPlayer.totalPoints += cases.Last(); //Add total number of points scored from move for red player
                }
                else //If game mode is general and the player placed an O
                {
                    cases = checkOPlacement(position); //Check if any points were made from the placement
                    if (currentPlayer == blue)
                        bluePlayer.totalPoints += cases.Last(); //Add total number of points scored from move for blue player
                    else
                        redPlayer.totalPoints += cases.Last(); //Add total number of points scored from move for red player
                }

                if (board.Length == boardCount) 
                {
                    gameDone = true; //If the board is full the game is over and the winner is whoever has the most points

                    if (bluePlayer.totalPoints > redPlayer.totalPoints)
                        winMessage = blue + " WINS!";
                    else if (redPlayer.totalPoints > bluePlayer.totalPoints)
                        winMessage = red + " WINS!";
                    else
                        winMessage = "DRAW";

                    File.WriteAllLines("GameRecording.txt", gameRecording.ToArray());
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
                            if (board[position.x - 1, position.y - 1].value == "O")
                                if (board[position.x - 2, position.y - 2].value == "S")
                                {
                                    pointsScored++;
                                    cases.Add(0);
                                }
                            break;
                        case 1: //Top middle cell
                            if (board[position.x, position.y - 1].value == "O")
                                if (board[position.x, position.y - 2].value == "S")
                                {
                                    pointsScored++;
                                    cases.Add(1);
                                }
                            break;
                        case 2: //Top right corner
                            if (board[position.x + 1, position.y - 1].value == "O")
                                if (board[position.x + 2, position.y - 2].value == "S")
                                {
                                    pointsScored++;
                                    cases.Add(2);
                                }
                            break;
                        case 3: //Left side cell
                            if (board[position.x - 1, position.y].value == "O")
                                if (board[position.x - 2, position.y].value == "S")
                                {
                                    pointsScored++;
                                    cases.Add(3);
                                }
                            break;
                        case 4: //Right side cell
                            if (board[position.x + 1, position.y].value == "O")
                                if (board[position.x + 2, position.y].value == "S")
                                {
                                    pointsScored++;
                                    cases.Add(4);
                                }
                            break;
                        case 5: //Bottom left corner
                            if (board[position.x - 1, position.y + 1].value == "O")
                                if (board[position.x - 2, position.y + 2].value == "S")
                                {
                                    pointsScored++;
                                    cases.Add(5);
                                }
                            break;
                        case 6: //Bottom middle cell
                            if (board[position.x, position.y + 1].value == "O")
                                if (board[position.x, position.y + 2].value == "S")
                                {
                                    pointsScored++;
                                    cases.Add(6);
                                }
                            break;
                        case 7: //Bottom right corner
                            if (board[position.x + 1, position.y + 1].value == "O")
                                if (board[position.x + 2, position.y + 2].value == "S")
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
                            if (board[position.x, position.y - 1].value == "S" && board[position.x, position.y + 1].value == "S")
                            {
                                pointsScored++;
                                cases.Add(8);
                            }
                            break;
                        case 1: //Check for S's on left and right
                            if (board[position.x - 1, position.y].value == "S" && board[position.x + 1, position.y].value == "S")
                            {
                                pointsScored++;
                                cases.Add(9);
                            }
                            break;
                        case 2: //Check for S/s on top left and bottome right
                            if (board[position.x - 1, position.y - 1].value == "S" && board[position.x + 1, position.y + 1].value == "S")
                            {
                                pointsScored++;
                                cases.Add(10);
                            }
                            break;
                        case 3: //Check for S's on top right and bottom left
                            if (board[position.x + 1, position.y - 1].value == "S" && board[position.x - 1, position.y + 1].value == "S")
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

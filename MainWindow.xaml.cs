﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace SOS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        ComputerGameLogic _gameLogic = new ComputerGameLogic(3, "SIMPLE",true,true);
        List<Button> gameBoardButtons = new List<Button>();
        List<int> cases = new List<int>();

        //Method that determines what happens whenever a cell is clicked on the GUI
        private void PlayerClicksCell(object sender, RoutedEventArgs e)
        {
            var cell = (Button)sender;

            //Checks if current cell is already occupied by an S or O or if it is the new game button
            if (!String.IsNullOrWhiteSpace(cell.Content?.ToString()))
                return;

            //Checks if the game has already been won to prevent placement of more S's or O's
            if (_gameLogic.gameDone)
                return;

            //Creates a coordinate value and assigns it to a position parameter
            //to be pass to the Board variable in gameLogic.cs
            var coordinates = cell.Tag.ToString().Split(',');
            var xValue = int.Parse(coordinates[0]);
            var yValue = int.Parse(coordinates[1]);
            var ButtonPosition = new Position(xValue, yValue);

            //Checks the current player and displays what goes into the current button based on
            //the color of the player and the placement type, then updates the board variable in
            //gameLogic.cs
            if (_gameLogic.currentPlayer == "BLUE")
            {
                updateCurrentBoardDisplay(_gameLogic.bluePlayer, ref cell, ButtonPosition, (Color)ColorConverter.ConvertFromString("#FF0D80FF"), ref cases);
            }
            else
            {
                updateCurrentBoardDisplay(_gameLogic.redPlayer, ref cell, ButtonPosition, Colors.Red, ref cases);
            }

            if (_gameLogic.gameDone == true)
            {
                winScreen.Text = _gameLogic.winMessage;
                winScreen.Visibility = Visibility.Visible;
                return;
            }

            if (cases.Count > 1 && _gameLogic.currentGameMode == "GENERAL")
                return;

            _gameLogic.SetNextPlayer();
            updatePlayerTurnDisplay();

            if (_gameLogic.currentPlayer == "BLUE" && _gameLogic.bluePlayer.isComputer == true)
            {
                _gameLogic.computerPlayerMove(ref gameBoardButtons, _gameLogic.bluePlayer, (Color)ColorConverter.ConvertFromString("#FF0D80FF"));
            }
            else if (_gameLogic.currentPlayer == "RED" && _gameLogic.redPlayer.isComputer == true)
            {
                _gameLogic.computerPlayerMove(ref gameBoardButtons, _gameLogic.redPlayer, Colors.Red);
            }
        }

        //Method that updates everything on the game board
        private void updateCurrentBoardDisplay(Player currentPlayer, ref Button cell, Position buttonPosition, Color drawColor, ref List<int> cases)
        {
            CellData cellData = new CellData(currentPlayer.placementType, currentPlayer.playerColor);
            cell.Foreground = currentPlayer.colorValue;
            cell.Content = currentPlayer.placementType;
            _gameLogic.updateBoard(buttonPosition, cellData);
            cases = _gameLogic.checkForWinOrPoint(cellData, buttonPosition);
            DrawCases(cases, drawColor, (int)boardSize.Value, buttonPosition);
            updatePlayerPointsDisplay();
        }

        //Method that starts a new game whenever the new game button is clicked.
        private void newGameBtn_Clicked(object sender, RoutedEventArgs e)
        {
            gameBoardButtons.Clear();
            bool blueIsHuman = (bool)blueHumanBtn.IsChecked;
            bool redIsHuman = (bool)redHumanBtn.IsChecked;
            generateNewGameBoard();

            if(blueIsHuman == true && redIsHuman == true)
            {
                _gameLogic = new ComputerGameLogic((int)boardSize.Value, getGameMode(),blueIsHuman,redIsHuman);
            }
            else
            {
                _gameLogic = new ComputerGameLogic((int)boardSize.Value, getGameMode(), blueIsHuman, redIsHuman);
            }

            setBluePlayerInitPlacementType();
            setRedPlayerInitPlacementType();
            updateGameModeDisplay();
            updatePlayerTurnDisplay();
            updatePlayerPointsDisplay();
            winScreen.Visibility = Visibility.Collapsed;
            MainCanvas.Children.Clear();

            if((bool)blueHumanBtn.IsChecked == false && (bool)redHumanBtn.IsChecked == false)
            {
                _gameLogic.automaticSOSGame(ref gameBoardButtons);
            }
            else if ((bool)blueHumanBtn.IsChecked == false && (bool)redHumanBtn.IsChecked == true)
            {
                _gameLogic.computerPlayerMove(ref gameBoardButtons, _gameLogic.bluePlayer, (Color)ColorConverter.ConvertFromString("#FF0D80FF"));
            }
        }

        //Generates a new game board by removing all of the elements of the current
        //grid and adding new columns, rows, and buttons, based on the value of
        //the board size slider
        private void generateNewGameBoard()
        {
            int fontSize = 42;

            gameBoardGrid.Children.Clear();
            gameBoardGrid.ColumnDefinitions.Clear();
            gameBoardGrid.RowDefinitions.Clear();

            //Sets fontsize depending on size of board
            if (boardSize.Value == 3)
                fontSize = 78;
            else if (boardSize.Value == 4)
                fontSize = 66;
            else if (boardSize.Value == 5)
                fontSize = 54;

            for (int i = 0; i < boardSize.Value; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                RowDefinition row = new RowDefinition();
                gameBoardGrid.ColumnDefinitions.Add(column);
                gameBoardGrid.RowDefinitions.Add(row);
            }
            for (int i = 0; i < boardSize.Value; i++) //i traverses the "x" values of the grid
            {
                for (int j = 0; j < boardSize.Value; j++) //j traverses the "y" values of the grid
                {
                    Button button = new Button();
                    button.FontSize = fontSize;
                    button.SetValue(TagProperty, $"{i},{j}");
                    button.SetValue(BackgroundProperty, Brushes.White);
                    button.SetValue(Grid.ColumnProperty, i);
                    button.SetValue(Grid.RowProperty, j);
                    gameBoardGrid.Children.Add(button);
                    gameBoardButtons.Add(button);
                }
            }
        }

        //Creates a variable to display the gameMode currently being played and creates a dependency property
        //so that it can be updated on the GUI when the updateGameModeDisplay() method is called
        public string gameModeDisplay
        {
            get { return (string)GetValue(gameModeProperty); }
            set { SetValue(gameModeProperty, value); }
        }
        public static readonly DependencyProperty gameModeProperty = DependencyProperty.Register("gameModeDisplay", typeof(string), typeof(MainWindow), new PropertyMetadata(null));

        //Creates a variable to display which player's turn it is and creates a dependency property
        //so that it can be updated on teh GUI when the updatePlayerTurnDisplay() method is called
        public string playerTurnDisplay
        {
            get { return (string)GetValue(playerTurnProperty); }
            set { SetValue(playerTurnProperty, value); }
        }
        public static readonly DependencyProperty playerTurnProperty = DependencyProperty.Register("playerTurnDisplay", typeof(string), typeof(MainWindow), new PropertyMetadata(null));

        public string bluePointsDisplay
        {
            get { return (string)GetValue(bluePointsProperty); }
            set { SetValue(bluePointsProperty, value); }
        }
        public static readonly DependencyProperty bluePointsProperty = DependencyProperty.Register("bluePointsDisplay", typeof(string), typeof(MainWindow), new PropertyMetadata(null));

        public string redPointsDisplay
        {
            get { return (string)GetValue(redPointsProperty); }
            set { SetValue(redPointsProperty, value); }
        }
        public static readonly DependencyProperty redPointsProperty = DependencyProperty.Register("redPointsDisplay", typeof(string), typeof(MainWindow), new PropertyMetadata(null));

        //Method to update the gameModeDisplay variable and display the update on GUI
        private void updateGameModeDisplay()
        {
            if (simpleGameBtn.IsChecked == true)
                gameModeDisplay = "Current Game Mode: Simple";
            else
                gameModeDisplay = "Current Game Mode: General";
        }

        //Method to update the playerTurnDisplay variable and display the update on GUI.
        public void updatePlayerTurnDisplay()
        {
            if (_gameLogic.currentPlayer == "RED")
                playerTurnDisplay = "Current Turn: Red Player";
            else
                playerTurnDisplay = "Current Turn: Blue Player";
        }

        //Method to update the redPointsDisplay and bluePointsDisplay variables and display the update on GUI
        public void updatePlayerPointsDisplay()
        {
            redPointsDisplay = $"Points: {_gameLogic.redPlayer.totalPoints}";
            bluePointsDisplay = $"Points: {_gameLogic.bluePlayer.totalPoints}";
        }

        //Method to return the new gameMode to the _gameLogic gameMode member
        private string getGameMode()
        {
            if (simpleGameBtn.IsChecked == true)
                return "SIMPLE";
            else
                return "GENERAL";
        }

        //Method to update the placement type of the blue player
        private void setBluePlayerPlacementType(object sender, RoutedEventArgs e)
        {
            if (blueSBtn.IsChecked == true)
                _gameLogic.updatePlayerPlacementType("BLUE", "S");
            else
                _gameLogic.updatePlayerPlacementType("BLUE", "O");
        }

        //Method to update the initial placement type of the blue player when the game starts
        private void setBluePlayerInitPlacementType()
        {
            if (blueSBtn.IsChecked == true)
                _gameLogic.updatePlayerPlacementType("BLUE", "S");
            else
                _gameLogic.updatePlayerPlacementType("BLUE", "O");
        }

        //Method to update the placement type of the red player
        private void setRedPlayerPlacementType(object sender, RoutedEventArgs e)
        {
            if (redSBtn.IsChecked == true)
                _gameLogic.updatePlayerPlacementType("RED", "S");
            else
                _gameLogic.updatePlayerPlacementType("RED", "O");
        }

        //Method to update the inital placement type of the blue player when the game starts
        private void setRedPlayerInitPlacementType()
        {
            if (redSBtn.IsChecked == true)
                _gameLogic.updatePlayerPlacementType("RED", "S");
            else
                _gameLogic.updatePlayerPlacementType("RED", "O");
        }

        //Method that takes in all of the different point cases that the player made. For example getting two points in one move
        //then two lines would need to be drawn.
        public void DrawCases(List<int> cases, Color color, int boardSize, Position position)
        {
            if (cases.Count == 1)
                return;
            
            Position topLeftCorner = new Position(position.x * 500 / boardSize, position.y * 500 / boardSize);
            Position topMiddle = new Position((position.x * 500 / boardSize) + (250 / boardSize), position.y * 500 / boardSize);
            Position topRightCorner = new Position((position.x * 500 / boardSize) + (500 / boardSize), position.y * 500 / boardSize);
            Position leftMiddle = new Position(position.x * 500 / boardSize, (position.y * 500 / boardSize) + (250 / boardSize));
            Position rightMiddle = new Position((position.x * 500 / boardSize) + (500 / boardSize), (position.y * 500 / boardSize) + (250 / boardSize));
            Position bottomLeftCorner = new Position(position.x * 500 / boardSize, (position.y * 500 / boardSize) + (500 / boardSize));
            Position bottomMiddle = new Position((position.x * 500 / boardSize) + (250 / boardSize), (position.y * 500 / boardSize) + (500 / boardSize));
            Position bottomRightCorner = new Position((position.x * 500 / boardSize) + (500 / boardSize), (position.y * 500 / boardSize) + (500 / boardSize));


            for(int i = 0; i < cases.Count-1; i++)
            {
                switch (cases.ElementAt(i))
                {
                    case 0: //Clicked S go up and left twice
                        DrawLine(bottomRightCorner.x, bottomRightCorner.y, topLeftCorner.x - (2 * 500 / boardSize), topLeftCorner.y - (2 * 500 / boardSize), color);
                        break;
                    case 1: //Clicked S go up twice
                        DrawLine(bottomMiddle.x, bottomMiddle.y, topMiddle.x, topMiddle.y - (2 * 500 / boardSize), color);
                        break;
                    case 2: //Clicked S go up and right twice
                        DrawLine(bottomLeftCorner.x, bottomLeftCorner.y, topRightCorner.x + (2 * 500 / boardSize), topRightCorner.y - (2 * 500 / boardSize), color);
                        break;
                    case 3: //Clicked S go left twice
                        DrawLine(rightMiddle.x, rightMiddle.y, leftMiddle.x - (2 * 500 / boardSize), leftMiddle.y, color);
                        break;
                    case 4: //Clicked S go right twice
                        DrawLine(leftMiddle.x, leftMiddle.y, rightMiddle.x + (2 * 500 / boardSize), rightMiddle.y, color);
                        break;
                    case 5: //Clicked S go down and left twice
                        DrawLine(topRightCorner.x, topRightCorner.y, bottomLeftCorner.x - (2 * 500 / boardSize), bottomLeftCorner.y + (2 * 500 / boardSize), color);
                        break;
                    case 6: //Clicked S go down twice
                        DrawLine(topMiddle.x, topMiddle.y, bottomMiddle.x, bottomMiddle.y + (2 * 500 / boardSize), color);
                        break;
                    case 7: //Clicked S go down and right twice
                        DrawLine(topLeftCorner.x, topLeftCorner.y, bottomRightCorner.x + (2 * 500 / boardSize), bottomRightCorner.y + (2 * 500 / boardSize), color);
                        break;
                    case 8: //Clicked O go up one and down one
                        DrawLine(topMiddle.x, topMiddle.y - (500 / boardSize), bottomMiddle.x, bottomMiddle.y + (500 / boardSize), color);
                        break;
                    case 9: //Clicked O go left one and right one
                        DrawLine(leftMiddle.x - (500 / boardSize), leftMiddle.y, rightMiddle.x + (500 / boardSize), rightMiddle.y, color);
                        break;
                    case 10: //Clicked O go up and left one and down and right one
                        DrawLine(topLeftCorner.x - (500 / boardSize), topLeftCorner.y - (500 / boardSize), bottomRightCorner.x + (500 / boardSize), bottomRightCorner.y + (500 / boardSize), color);
                        break;
                    case 11: //Clicked O go up and right one and down and left one
                        DrawLine(topRightCorner.x + (500 / boardSize), topRightCorner.y - (500 / boardSize), bottomLeftCorner.x - (500 / boardSize), bottomLeftCorner.y + (500 / boardSize), color);
                        break;
                    default:
                        break;
                }
            }
        }


        //Draws a line on the game board when given the coordinates of the start and end of the line, as well as the color
        public void DrawLine(int x1, int y1, int x2, int y2, Color color)
        {
            Line line = new Line();
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;

            SolidColorBrush lineBrush = new SolidColorBrush(color);

            line.Stroke = lineBrush;
            line.StrokeThickness = 5;

            MainCanvas.Children.Add(line);
        }
    }
}

using System;
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

        GameLogic _gameLogic = new GameLogic();

        private void PlayerClicksCell(object sender, RoutedEventArgs e)
        {
            var cell = (Button)sender;

            //Checks if current cell is already occupied by an S or O or if it is the new game button
            if (!String.IsNullOrWhiteSpace(cell.Content?.ToString()))
                return;

            //Checks if the game has already been won to prevent placement of more S's or O's
            if (_gameLogic.GameDone)
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
            if (_gameLogic.CurrentPlayer == "BLUE")
            {
                CellData cellData = new CellData(_gameLogic.bluePlayer.placementType, _gameLogic.bluePlayer.playerColor);
                cell.Foreground = _gameLogic.bluePlayer.colorValue;
                cell.Content = _gameLogic.bluePlayer.placementType;
                _gameLogic.updateBoard(ButtonPosition, cellData);
                _gameLogic.checkForWinOrPoint(_gameLogic.CurrentGameMode, cellData, ButtonPosition);
            }
            else
            {
                CellData cellData = new CellData(_gameLogic.redPlayer.placementType, _gameLogic.redPlayer.playerColor);
                cell.Foreground = _gameLogic.redPlayer.colorValue;
                cell.Content = _gameLogic.redPlayer.placementType;
                _gameLogic.updateBoard(ButtonPosition, cellData);
            }

            _gameLogic.SetNextPlayer();
            updatePlayerTurnDisplay();
            
        }


        private void newGameBtn_Clicked(object sender, RoutedEventArgs e)
        {
            generateNewGameBoard();
            _gameLogic=new GameLogic();
            _gameLogic.updateBoardVariableSize((int)boardSize.Value);
            _gameLogic.updateGameMode(getGameMode());
            setBluePlayerInitPlacementType();
            setRedPlayerInitPlacementType();
            updateGameModeDisplay();
            updatePlayerTurnDisplay();
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
            
            for(int i = 0; i < boardSize.Value; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                RowDefinition row = new RowDefinition();
                gameBoardGrid.ColumnDefinitions.Add(column);
                gameBoardGrid.RowDefinitions.Add(row);
            }
            for(int i = 0; i < boardSize.Value; i++) //i traverses the "x" values of the grid
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

        //Method to update the gameModeDisplay variable and display the update on GUI
        private void updateGameModeDisplay()
        {
            if (simpleGameBtn.IsChecked == true)
            {
                gameModeDisplay = "Current Game Mode: Simple";
            }
            else
            {
                gameModeDisplay = "Current Game Mode: General";
            }
        }

        //Method to update the playerTurnDisplay variable and display the update on GUI.
        private void updatePlayerTurnDisplay()
        {
            if (_gameLogic.CurrentPlayer == "RED")
                playerTurnDisplay = "Current Turn: Red Player";
            else
                playerTurnDisplay = "Current Turn: Blue Player";
        }

        //Method to return the new gameMode to the _gameLogic gameMode member
        private string getGameMode()
        {
            if (simpleGameBtn.IsChecked == true)
            {
                return "SIMPLE";
            }
            else
            {
                return "GENERAL";
            }
                
        }

        //Method to update the placement type of the blue player
        private void setBluePlayerPlacementType(object sender, RoutedEventArgs e)
        {
            if (blueSBtn.IsChecked == true)
                _gameLogic.updatePlayerPlacementType("BLUE", "S");
            else
                _gameLogic.updatePlayerPlacementType("BLUE", "O");
        }
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
        private void setRedPlayerInitPlacementType()
        {
            if (redSBtn.IsChecked == true)
                _gameLogic.updatePlayerPlacementType("RED", "S");
            else
                _gameLogic.updatePlayerPlacementType("RED", "O");
        }
    }
}

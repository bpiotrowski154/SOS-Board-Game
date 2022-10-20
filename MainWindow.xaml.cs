using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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

            if (_gameLogic.CurrentPlayer == "BLUE")
            {
                cell.Foreground = _gameLogic.bluePlayer.colorValue;
                cell.Content = _gameLogic.bluePlayer.placementType;
            }
            else
            {
                cell.Foreground = _gameLogic.redPlayer.colorValue;
                cell.Content = _gameLogic.redPlayer.placementType;
            }

            _gameLogic.SetNextPlayer();
            
        }


        private void newGameBtn_Clicked(object sender, RoutedEventArgs e)
        {
            generateNewGameBoard();
            _gameLogic=new GameLogic();
            _gameLogic.updateBoardVar((int)boardSize.Value);
            _gameLogic.updateGameMode(getGameMode());
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
            for(int i = 0; i < boardSize.Value; i++)
            {
                for (int j = 0; j < boardSize.Value; j++)
                {
                    Button button = new Button();
                    button.FontSize = fontSize;
                    button.SetValue(Button.BackgroundProperty, Brushes.White);
                    button.SetValue(Grid.ColumnProperty, i);
                    button.SetValue(Grid.RowProperty, j);
                    gameBoardGrid.Children.Add(button);
                }
            }
        }

        private string getGameMode()
        {
            if (simpleGameBtn.IsChecked == true)
                return "SIMPLE";
            else
                return "GENERAL";
        }

        private void setBluePlayerPlacementType(object sender, RoutedEventArgs e)
        { 
            if (blueSBtn.IsChecked == true)
                _gameLogic.bluePlayer.placementType = "S";
            else
                _gameLogic.bluePlayer.placementType = "O";
        }

        private void setRedPlayerPlacementType(object sender, RoutedEventArgs e)
        {
            if (redSBtn.IsChecked == true)
                _gameLogic.redPlayer.placementType = "S";
            else
                _gameLogic.redPlayer.placementType = "O";
        }
    }
}

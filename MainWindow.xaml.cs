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


        private void newGameBtn_Clicked(object sender, RoutedEventArgs e)
        {
            generateNewGameBoard();
            _gameLogic=new GameLogic();
        }

        private void generateNewGameBoard()
        {
            gameBoardGrid.Children.Clear();
            gameBoardGrid.ColumnDefinitions.Clear();
            gameBoardGrid.RowDefinitions.Clear();
            
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
                    button.SetValue(Grid.ColumnProperty, i);
                    button.SetValue(Grid.RowProperty, j);
                    gameBoardGrid.Children.Add(button);
                }
            }
        }
    }
}

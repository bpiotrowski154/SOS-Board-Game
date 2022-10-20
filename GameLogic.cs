using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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


        private string[,] Board = new string[3, 3];
        public bool GameDone = false;



        public void updateBoardVar(int boardSize)
        {
            if (boardSize != 3)
            {
                Board = new string[boardSize, boardSize];
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



    }
}

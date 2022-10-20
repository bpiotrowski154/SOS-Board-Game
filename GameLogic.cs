﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;

namespace SOS
{
    public class GameLogic
    {
        public string CurrentPlayer { get; set; } = blue;
        private const string blue = "BLUE";
        private const string red = "RED";
        private string[,] Board = new string[3, 3];


        public void updateBoardVar(int boardSize)
        {
            if (boardSize != 3)
            {
                Board = new string[boardSize, boardSize];
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOS
{
    public class TestingGameLogic : ComputerGameLogic
    {
        public TestingGameLogic(int boardSize, string gameMode, bool player1Human, bool player2Human) : base(boardSize, gameMode, player1Human, player2Human)
        {
        }
        public void generateLogicBoard()
        {
            CellData[] cellData = new CellData[6];
            for (int i = 0; i < 6; i++)
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
                    BoardCount++;
                }
            }
        }

        public void generateLogicBoard2()
        {
            CellData[] cellData = new CellData[6];
            for (int i = 0; i < 6; i++)
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
                    if (i == 1 && j == 1)
                    {
                        Board[i, j] = new CellData("S", "RED");
                        count++;
                        BoardCount++;
                    }
                    else
                    {
                        Board[i, j] = cellData[count];
                        count++;
                        BoardCount++;
                    }
                }
            }
        }

        public void makeMove(string type, string playerColor, Position position)
        {
            if (!String.IsNullOrEmpty(Board[position.x, position.y].value))
                return;
            else
            {
                CellData cellData = new CellData(type, playerColor);
                Board[position.x, position.y] = cellData;
                BoardCount++;
                List<int> temp = new List<int>();
                temp = checkForWinOrPoint(cellData, position);

                if (temp.Count > 1 && CurrentGameMode == "GENERAL")
                    return;

                SetNextPlayer();
            }
        }

        public CellData getCellData(Position position)
        {
            return Board[position.x, position.y];
        }
    }
}


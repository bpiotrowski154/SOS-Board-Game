using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Controls;
using System.Windows.Media;

namespace SOS
{
    public class TestingGameLogic : ComputerGameLogic
    {
        public TestingGameLogic(int boardSize, string gameMode, bool player1Human, bool player2Human) : base(boardSize, gameMode, player1Human, player2Human)
        {
        }

        public bool winMovePossible = false;
        public bool madeRandomMove = false;
        public int numTurns = 0;
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
                updateBoard(position, cellData);
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

        public void computerPlayerMakesMove(Player currentPlayer)
        {
            bool turnOver = false;
            bool validPlacement = false;
            List<int> cases = new List<int>();
            Position winMovePosition = new Position(0, 0);
            if (BoardCount == 0)
            { Random random = new Random();
                int i = random.Next(BoardSize);
                int j = random.Next(BoardSize);

                CellData cellData = new CellData(currentPlayer.placementType, currentPlayer.playerColor);

                Position randomPosition = new Position(i, j);
                updateBoard(randomPosition, cellData);
                SetNextPlayer();
                return;
            }
            else
            {   
                //winMovePossible = checkForWinningCPUMove(currentPlayer, ref winMovePosition);
                while (!turnOver) 
                {
                    numTurns++;
                    cases.Clear();
                    if (checkForWinningCPUMove(currentPlayer, ref winMovePosition))
                    {
                        CellData cellData = new CellData(currentPlayer.placementType, currentPlayer.playerColor);

                        updateBoard(winMovePosition, cellData);
                        cases = checkForWinOrPoint(cellData, winMovePosition);
                        winMovePossible = true;

                        if (GameDone)
                            return;
                    }
                    else
                    {
                        int i = 0, j = 0;
                        Random random = new Random();
                        while (!validPlacement)
                        {
                            i = random.Next(BoardSize);
                            j = random.Next(BoardSize);
                            if (Board[i, j].value != null)
                                continue;
                            else
                                validPlacement = true;
                        }

                        setRandomPlacementTypeCPU(currentPlayer);

                        CellData cellData = new CellData(currentPlayer.placementType, currentPlayer.playerColor);
                        Position randomPosition = new Position(i, j);

                        updateBoard(randomPosition, cellData);
                        cases = checkForWinOrPoint(cellData, randomPosition);
                        madeRandomMove = true;

                        if (GameDone)
                            return;
                    }
                    if (CurrentGameMode == "SIMPLE" || cases.Count <= 1)
                        turnOver = true;
                }
            }
            SetNextPlayer();
        }
        public void automaticSOSTestGame()
        {
            while (!GameDone)
            {
                if (CurrentPlayer == "BLUE")
                    computerPlayerMakesMove(bluePlayer);
                else
                    computerPlayerMakesMove(redPlayer);
            }
        }
    }
}


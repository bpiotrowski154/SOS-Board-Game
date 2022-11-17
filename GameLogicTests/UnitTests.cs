using SOS;
using System.Reflection.Metadata.Ecma335;
using System.Security.Permissions;

namespace GameLogicTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void ValidSOrOPlacement_SimpleGame()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "SIMPLE", true, true);

            Position position1 = new Position(1, 1);

            string initialPlayer = gameLogic.CurrentPlayer;

            gameLogic.makeMove("S", initialPlayer, position1);

            CellData expectedCellData = new CellData("S", initialPlayer);
            CellData actualCellData = gameLogic.Board[position1.x, position1.y];
            string expectedPlayer = "RED";
            string actualPlayer = gameLogic.CurrentPlayer;

            Assert.AreEqual(expectedCellData, actualCellData);
            Assert.AreEqual(expectedPlayer, actualPlayer);
        }

        [TestMethod]
        public void InvalidSPlacement_SimpleGame()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "SIMPLE", true, true);
            gameLogic.generateLogicBoard();

            Position position1 = new Position(1, 1);
            Position position2 = new Position(2, 1);

            gameLogic.makeMove("S", gameLogic.CurrentPlayer, position1);
            gameLogic.makeMove("S", gameLogic.CurrentPlayer, position2);

            CellData expectedCellData = new CellData("S", gameLogic.CurrentPlayer);
            CellData actualCellData1 = gameLogic.Board[position1.x, position1.y];
            CellData actualCellData2 = gameLogic.Board[position2.x, position2.y];
            string expectedPlayer = "BLUE";
            string actualPlayer = gameLogic.CurrentPlayer;

            Assert.AreNotEqual(expectedCellData, actualCellData1);
            Assert.AreNotEqual(expectedCellData, actualCellData2);
            Assert.AreEqual(expectedPlayer, actualPlayer);          
        }

        [TestMethod]
        public void InvalidOPlacement_SimpleGame()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "SIMPLE", true, true);
            gameLogic.generateLogicBoard();

            Position position1 = new Position(1, 1);
            Position position2 = new Position(2, 1);

            gameLogic.makeMove("O", gameLogic.CurrentPlayer, position1);
            gameLogic.makeMove("O", gameLogic.CurrentPlayer, position2);

            CellData expectedCellData = new CellData("O", gameLogic.CurrentPlayer);
            CellData actualCellData1 = gameLogic.Board[position1.x, position1.y];
            CellData actualCellData2 = gameLogic.Board[position2.x, position2.y];
            string expectedPlayer = "BLUE";
            string actualPlayer = gameLogic.CurrentPlayer;

            Assert.AreNotEqual(expectedCellData, actualCellData1);
            Assert.AreNotEqual(expectedCellData, actualCellData2);
            Assert.AreEqual(expectedPlayer, actualPlayer);
        }
        [TestMethod]
        public void FirstSOSFormed_SimpleGame()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "SIMPLE", true, true);
            gameLogic.generateLogicBoard();

            Position position = new Position(0, 2);

            gameLogic.makeMove("S", gameLogic.CurrentPlayer, position);

            bool expectedGameDone = true;
            bool actualGameDone = gameLogic.GameDone;
            string expectedWinner = "BLUE WINS!";
            string actualWinner = gameLogic.WinMessage;

            Assert.AreEqual(expectedGameDone, actualGameDone);
            Assert.AreEqual(expectedWinner, actualWinner);

        }

        [TestMethod]
        public void ContinuingGameAfterSorOMove_SimpleGame()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "SIMPLE", true, true);
            gameLogic.generateLogicBoard();

            Position position1 = new Position(0, 2);

            gameLogic.makeMove("O", gameLogic.CurrentPlayer, position1);

            bool expectedGameDone = false;
            bool actualGameDone = gameLogic.GameDone;
            string actualWinner = gameLogic.WinMessage;

            Assert.AreEqual(expectedGameDone, actualGameDone);
            Assert.IsNull(actualWinner);
        }

        [TestMethod]
        public void DrawGame_SimpleGame()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "SIMPLE", true, true);
            gameLogic.generateLogicBoard();

            Position position1 = new Position(0, 2);
            Position position2 = new Position(1, 2);
            Position position3 = new Position(2, 2);
            
            gameLogic.makeMove("O", gameLogic.CurrentPlayer, position1);
            gameLogic.makeMove("O", gameLogic.CurrentPlayer, position2);
            gameLogic.makeMove("O", gameLogic.CurrentPlayer, position3);

            bool expectedGameDone = true;
            bool actualGameDone = gameLogic.GameDone;
            string expectedWinner = "DRAW";
            string actualWinner = gameLogic.WinMessage;

            Assert.AreEqual(expectedWinner, actualWinner);
            Assert.AreEqual(expectedGameDone, actualGameDone);
        }

        [TestMethod]
        public void ValidSOrOPlacement_GeneralGame()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "GENERAL", true, true);

            Position position1 = new Position(1, 1);

            string initialPlayer = gameLogic.CurrentPlayer;

            gameLogic.makeMove("S", initialPlayer, position1);

            CellData expectedCellData = new CellData("S", initialPlayer);
            CellData actual = gameLogic.Board[position1.x, position1.y];
            string expectedPlayer = "RED";
            string actualPlayer = gameLogic.CurrentPlayer;

            Assert.AreEqual(expectedCellData, actual);
            Assert.AreEqual(expectedPlayer, actualPlayer);
        }

        [TestMethod]
        public void InvalidSPlacement_GeneralGame()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "GENERAL", true, true);
            gameLogic.generateLogicBoard();

            Position position1 = new Position(1, 1);
            Position position2 = new Position(2, 1);

            gameLogic.makeMove("S", gameLogic.CurrentPlayer, position1);
            gameLogic.makeMove("S", gameLogic.CurrentPlayer, position2);

            CellData expectedCellData = new CellData("S", gameLogic.CurrentPlayer);
            CellData actualCellData1 = gameLogic.Board[position1.x, position1.y];
            CellData actualCellData2 = gameLogic.Board[position2.x, position2.y];
            string expectedPlayer = "BLUE";
            string actualPlayer = gameLogic.CurrentPlayer;


            Assert.AreNotEqual(expectedCellData, actualCellData1);
            Assert.AreNotEqual(expectedCellData, actualCellData2);
            Assert.AreEqual(expectedPlayer, actualPlayer);
        }

        [TestMethod]
        public void InvalidOPlacement_GeneralGame()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "GENERAL", true, true);
            gameLogic.generateLogicBoard();

            Position position1 = new Position(1, 1);
            Position position2 = new Position(2, 1);

            gameLogic.makeMove("O", gameLogic.CurrentPlayer, position1);
            gameLogic.makeMove("O", gameLogic.CurrentPlayer, position2);

            CellData expectedCellData = new CellData("O", gameLogic.CurrentPlayer);
            CellData actualCellData1 = gameLogic.Board[position1.x, position1.y];
            CellData actualCellData2 = gameLogic.Board[position2.x, position2.y];
            string expectedPlayer = "BLUE";
            string actualPlayer = gameLogic.CurrentPlayer;

            Assert.AreNotEqual(expectedCellData, actualCellData1);
            Assert.AreNotEqual(expectedCellData, actualCellData2);
            Assert.AreEqual(expectedPlayer, actualPlayer);
        }

        [TestMethod]
        public void ValidSorOPlacementWithSOSFormed_GeneralGame()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "GENERAL", true, true);
            gameLogic.generateLogicBoard();

            Position position1 = new Position(0, 2);

            gameLogic.makeMove("S", gameLogic.CurrentPlayer, position1);

            bool expectedGameDone = false;
            bool actualGameDone = gameLogic.GameDone;
            string expectedPlayerTurn = "BLUE";
            string actualPlayerTurn = gameLogic.CurrentPlayer;
            string actualWinMessage = gameLogic.WinMessage;

            Assert.AreEqual(expectedGameDone, actualGameDone);
            Assert.AreEqual(expectedPlayerTurn, actualPlayerTurn);
            Assert.IsNull(actualWinMessage);
        }

        [TestMethod]
        public void BoardBecomesFullAndNotDrawGame_GeneralGame()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "GENERAL", true, true);
            gameLogic.generateLogicBoard();

            Position position1 = new Position(0, 2);
            Position position2 = new Position(1, 2);
            Position position3 = new Position(2, 2);

            gameLogic.makeMove("S", gameLogic.CurrentPlayer, position1);
            gameLogic.makeMove("S", gameLogic.CurrentPlayer, position2);
            gameLogic.makeMove("S", gameLogic.CurrentPlayer, position3);

            bool expectedGameDone = true;
            bool actualGameDone = gameLogic.GameDone;
            string expectedWinMessage = "BLUE WINS!";
            string actualWinMessage = gameLogic.WinMessage;

            Assert.AreEqual(expectedGameDone, actualGameDone);
            Assert.AreEqual(expectedWinMessage, actualWinMessage);
        }

        [TestMethod]
        public void ContinuingGameAfterSorOMove_GeneralGame()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "GENERAL", true, true);
            gameLogic.generateLogicBoard();

            Position position = new Position(0, 2);

            gameLogic.makeMove("O", gameLogic.CurrentPlayer, position);

            bool expectedGameDone = false;
            bool actualGameDone = gameLogic.GameDone;
            string expectedCurrentPlayer = "RED";
            string actualCurrentPlayer = gameLogic.CurrentPlayer;
            string actualWinMessage = gameLogic.WinMessage;

            Assert.AreEqual(expectedGameDone, actualGameDone);
            Assert.AreEqual(expectedCurrentPlayer, actualCurrentPlayer);
            Assert.IsNull(actualWinMessage);
        }
        [TestMethod]
        public void DrawGame_Generalgame()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "GENERAL", true, true);
            gameLogic.generateLogicBoard2();

            Position position1 = new Position(0, 2);
            Position position2 = new Position(1, 2);
            Position position3 = new Position(2, 2);

            gameLogic.makeMove("S", gameLogic.CurrentPlayer, position1);
            gameLogic.makeMove("S", gameLogic.CurrentPlayer, position2);
            gameLogic.makeMove("S", gameLogic.CurrentPlayer, position3);

            bool expectedGameDone = true;
            bool actualGameDone = gameLogic.GameDone;
            string expectedWinMessage = "DRAW";
            string actualWinMessage = gameLogic.WinMessage;

            Assert.AreEqual(expectedGameDone, actualGameDone);
            Assert.AreEqual(expectedWinMessage, actualWinMessage);
        }

        [TestMethod]
        public void FirstMoveByComputerPlayer()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "SIMPLE", false, true);
            gameLogic.computerPlayerMakesMove(gameLogic.bluePlayer);

            bool expectedGameDone = false;
            bool actualGameDone = gameLogic.GameDone;
            int expectedBoardCount = 1;
            int actualBoardCount = gameLogic.BoardCount;
            string expectedCurrentPlayer = "RED";
            string actualCurrentPlayer = gameLogic.CurrentPlayer;

            Assert.AreEqual(expectedGameDone, actualGameDone);
            Assert.AreEqual(expectedBoardCount, actualBoardCount);
            Assert.AreEqual(expectedCurrentPlayer, actualCurrentPlayer);
        }

        [TestMethod]
        public void SecondMoveByComputerPlayer()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "SIMPLE", true, false);
            Position position1 = new Position(0, 2);

            gameLogic.makeMove("S", gameLogic.CurrentPlayer, position1);
            gameLogic.computerPlayerMakesMove(gameLogic.redPlayer);

            bool expectedAttemptedWinMove = false;
            bool actualAttemptedWinMove = gameLogic.winMovePossible;
            bool expectedGameDone = false;
            bool actualGameDone = gameLogic.GameDone;
            string expectedCurrentPlayer = "BLUE";
            string actualCurrentPlayer = gameLogic.CurrentPlayer;

            Assert.AreEqual(expectedGameDone, actualGameDone);
            Assert.AreEqual(expectedAttemptedWinMove, actualAttemptedWinMove);
            Assert.AreEqual(expectedCurrentPlayer, actualCurrentPlayer);

        }
        [TestMethod]
        public void RandomMoveByComputerPlayer()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "SIMPLE", true, false);
            Position position1 = new Position(1, 1);

            gameLogic.makeMove("S", gameLogic.CurrentPlayer, position1);
            gameLogic.computerPlayerMakesMove(gameLogic.redPlayer);

            bool expectedMadeRandomMove = true;
            bool actualMadeRandomMove = gameLogic.madeRandomMove;
            bool expectedGameDone = false;
            bool actualGameDone = gameLogic.GameDone;
            string expectedCurrentPlayer = "BLUE";
            string actualCurrentPlayer = gameLogic.CurrentPlayer;
            bool expectedAttemptedWinMove = false;
            bool actualAttemptedWinMove = gameLogic.winMovePossible;

            Assert.AreEqual(expectedMadeRandomMove, actualMadeRandomMove);
            Assert.AreEqual(expectedGameDone, actualGameDone);
            Assert.AreEqual(expectedCurrentPlayer, actualCurrentPlayer);
            Assert.AreEqual(expectedAttemptedWinMove, actualAttemptedWinMove);
        }

        [TestMethod]
        public void WinningMoveByComputerPlayer_SimpleGame()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "SIMPLE", true, false);
            Position position1 = new Position(1, 1);
            Position position2 = new Position(2, 1);
            Position position3 = new Position(0,0);

            gameLogic.makeMove("O", gameLogic.CurrentPlayer, position1);
            gameLogic.makeMove("S", gameLogic.CurrentPlayer, position2);
            gameLogic.makeMove("S", gameLogic.CurrentPlayer, position3);
            gameLogic.computerPlayerMakesMove(gameLogic.redPlayer);

            bool expectedAttemptedWinMove = true;
            bool actualAttemptedWinMove = gameLogic.winMovePossible;
            bool expectedGameDone = true;
            bool actualGameDone = gameLogic.GameDone;

            Assert.AreEqual(expectedGameDone, actualGameDone);
            Assert.AreEqual(expectedAttemptedWinMove, actualAttemptedWinMove);
        }

        [TestMethod]
        public void WinningMoveOrPointScoredByComputerPlayer_GeneralGame()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "GENERAL", true, false);
            Position position1 = new Position(1, 1);
            Position position2 = new Position(2, 1);
            Position position3 = new Position(0, 0);

            gameLogic.makeMove("O", gameLogic.CurrentPlayer, position1);
            gameLogic.makeMove("S", gameLogic.CurrentPlayer, position2);
            gameLogic.makeMove("S", gameLogic.CurrentPlayer, position3);
            gameLogic.computerPlayerMakesMove(gameLogic.redPlayer);

            bool expectedAttemptedWinMove = true;
            bool actualAttemptedWinMove = gameLogic.winMovePossible;
            bool expectedGameDone = false;
            bool actualGameDone = gameLogic.GameDone;
            int numTurnsPlayedByComputer = gameLogic.numTurns;
            string expectedCurrentPlayer = "BLUE";
            string actualCurrentPlaeyr = gameLogic.CurrentPlayer;

            Assert.AreEqual(expectedAttemptedWinMove, actualAttemptedWinMove);
            Assert.AreEqual(expectedGameDone, actualGameDone);
            Assert.AreNotEqual(numTurnsPlayedByComputer, 1);
            Assert.AreEqual(expectedCurrentPlayer, actualCurrentPlaeyr);
        }

        [TestMethod]
        public void BothPlayersAreComputerPlayers_SimpleGame()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "SIMPLE", false, false);
            gameLogic.automaticSOSTestGame();

            bool expectedGameDone = true;
            bool actualGameDone = gameLogic.GameDone;

            Assert.AreEqual(expectedGameDone, actualGameDone);
        }
        public void BothPlayersAreComputerPlayers_GeneralGame()
        {
            TestingGameLogic gameLogic = new TestingGameLogic(3, "GENERAL", false, false);
            gameLogic.automaticSOSTestGame();

            bool expectedGameDone = true;
            bool actualGameDone = true;
            int expectedBoardCount = 9;
            int actualBoardCount = gameLogic.BoardCount;

            Assert.AreEqual(expectedGameDone, actualGameDone);
            Assert.AreEqual(expectedBoardCount, actualBoardCount);
        }
    }
}
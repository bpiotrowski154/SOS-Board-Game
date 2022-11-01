using SOS;

namespace GameLogicTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void ValidSOrOPlacement_SimpleGame()
        {
            GameLogic gameLogic = new GameLogic();
            gameLogic.setGameMode("SIMPLE");

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
            GameLogic gameLogic = new GameLogic();
            gameLogic.setGameMode("SIMPLE");
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
            GameLogic gameLogic = new GameLogic();
            gameLogic.setGameMode("SIMPLE");
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
        public void ValidSOrOPlacement_GeneralGame()
        {
            GameLogic gameLogic = new GameLogic();
            gameLogic.setGameMode("GENERAL");

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
            GameLogic gameLogic = new GameLogic();
            gameLogic.setGameMode("GENERAL");
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
            GameLogic gameLogic = new GameLogic();
            gameLogic.setGameMode("GENERAL");
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
    }
}
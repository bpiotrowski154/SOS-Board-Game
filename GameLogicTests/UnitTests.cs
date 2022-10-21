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

            CellData expected = new CellData("S", initialPlayer);
            CellData actual = gameLogic.Board[position1.x, position1.y];

            Assert.AreEqual(expected, actual);


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

            CellData expected = new CellData("S", gameLogic.CurrentPlayer);
            CellData actual1 = gameLogic.Board[position1.x, position1.y];
            CellData actual2 = gameLogic.Board[position2.x, position2.y];

            Assert.AreNotEqual(expected, actual1);
            Assert.AreNotEqual(expected, actual2);
            
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

            CellData expected = new CellData("O", gameLogic.CurrentPlayer);
            CellData actual1 = gameLogic.Board[position1.x, position1.y];
            CellData actual2 = gameLogic.Board[position2.x, position2.y];

            Assert.AreNotEqual(expected, actual1);
            Assert.AreNotEqual(expected, actual2);
        }
        [TestMethod]
        public void ValidSOrOPlacement_GeneralGame()
        {
            GameLogic gameLogic = new GameLogic();
            gameLogic.setGameMode("GENERAL");

            Position position1 = new Position(1, 1);

            string initialPlayer = gameLogic.CurrentPlayer;

            gameLogic.makeMove("S", initialPlayer, position1);

            CellData expected = new CellData("S", initialPlayer);
            CellData actual = gameLogic.Board[position1.x, position1.y];

            Assert.AreEqual(expected, actual);
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

            CellData expected = new CellData("S", gameLogic.CurrentPlayer);
            CellData actual1 = gameLogic.Board[position1.x, position1.y];
            CellData actual2 = gameLogic.Board[position2.x, position2.y];

            Assert.AreNotEqual(expected, actual1);
            Assert.AreNotEqual(expected, actual2);
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

            CellData expected = new CellData("O", gameLogic.CurrentPlayer);
            CellData actual1 = gameLogic.Board[position1.x, position1.y];
            CellData actual2 = gameLogic.Board[position2.x, position2.y];

            Assert.AreNotEqual(expected, actual1);
            Assert.AreNotEqual(expected, actual2);
        }
    }
}
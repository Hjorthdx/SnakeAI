using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SnakeGameTests {
    [TestClass]
    public class GameTests {
        //Denne test skal måske væk
        //[TestMethod]
        //public void MoveTest_TestForIfTheSnakeMovesAwayFromTheStartLocation_MoveAway() {
        //    Game game = new Game(15, 10, 20);
        //    Field SnakeStartLocation = game.Grid.GetSnakeHead();
        //    game.DirectionToMove = Game.Direction.Up;
        //    game.Move();
        //    game.Move();
        //    Field SnakeNewLocation = game.Grid.GetSnakeHead();
        //    Assert.IsTrue(SnakeStartLocation.Column != SnakeNewLocation.Column || SnakeStartLocation.Row != SnakeNewLocation.Row);
        //}

        [TestMethod]
        public void IsSnakeDead_TestIfTheSnakeDiesWhenMeetingAnObstacleMovingLeft_ReturnsTrue() {
            int numColumns = 10;
            int numRows = 10;
            int Sidelength = 20;
            Game game = new Game(numColumns, numRows, Sidelength);
            game.DirectionToMove = Game.Direction.Left;
            for (int i = 0; i < numColumns / 2; i++) {
                game.Move();
            }
            Assert.IsTrue(game.IsSnakeDead());
        }

        [TestMethod]
        public void IsSnakeDead_TestIfTheSnakeDiesWhenMeetingAnObstacleMovingRight_ReturnsTrue() {
            int numColumns = 10;
            int numRows = 10;
            int Sidelength = 20;
            Game game = new Game(numColumns, numRows, Sidelength);
            game.DirectionToMove = Game.Direction.Right;
            for (int i = 0; i < numColumns / 2; i++) {
                game.Move();
            }
            Assert.IsTrue(game.IsSnakeDead());
        }

        [TestMethod]
        public void IsSnakeDead_TestIfTheSnakeDiesWhenMeetingAnObstacleMovingUp_ReturnsTrue() {
            int numColumns = 10;
            int numRows = 10;
            int Sidelength = 20;
            Game game = new Game(numColumns, numRows, Sidelength);
            game.DirectionToMove = Game.Direction.Up;
            for (int i = 0; i < numRows / 2; i++) {
                game.Move();
            }
            Assert.IsTrue(game.IsSnakeDead());
        }

        [TestMethod]
        public void IsSnakeDead_TestsIfTheSnakeDiesWhenMeetingAnObstacleMovingDown_ReturnsTrue() {
            int numColumns = 10;
            int numRows = 10;
            int Sidelength = 20;
            Game game = new Game(numColumns, numRows, Sidelength);
            game.DirectionToMove = Game.Direction.Down;
            for (int i = 0; i < numRows / 2; i++) {
                game.Move();
            }
            Assert.IsTrue(game.IsSnakeDead());
        }

        [TestMethod]
        public void Reset_TestsIfTheGridIsResetAfterTheSnakeIsDead_AreReset() {
            int numColumns = 10;
            int numRows = 10;
            int Sidelength = 20;
            Game game = new Game(numColumns, numRows, Sidelength);
            game.DirectionToMove = Game.Direction.Left;
            for (int i = 0; i < numColumns / 2; i++) {
                game.Move();
            }
            game.Reset();
            Assert.IsFalse(game.IsSnakeDead());
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SnakeGameTests {
    [TestClass]
    public class GridTests {
        [TestMethod]
        public void GridField_TestTheConstrustionAndInitializenOfObstaclesInTheGrid_ReturnTrue() {
            int numColumns = 15;
            int numRows = 10;
            int Sidelength = 20;
            Grid Grid = new Grid(numColumns, numRows, Sidelength);
            bool expected = true;
            for (int i = 0; i < numColumns - 1; i++) {
                for (int j = 0; j < numRows - 1; j++) {
                    if (Grid.GridField[i,j].State == State.Obstacle && ((i != 0 && j != 0) && (i != numColumns - 1 && j != numRows - 1))) {
                        expected = false;
                    } 
                }
            }
            Assert.IsTrue(expected);
        }

        [TestMethod]
        public void GridField_TestTheConstrustionAndInitializenOfObstaclesInTheGridWithAWrongObstacle_ReturnFalse() {
            int numColumns = 15;
            int numRows = 10;
            int Sidelength = 20;
            Grid Grid = new Grid(numColumns, numRows, Sidelength);
            Random Random = new Random();
            int RanColumn = Random.Next(1, Grid.GridField.GetLength(0) - 2);
            int RanRow = Random.Next(1, Grid.GridField.GetLength(1) - 2);
            Grid.GridField[RanColumn, RanRow] = new Obstacle(RanColumn, RanRow, 20);
            bool expected = true;
            for (int i = 0; i < numColumns - 1; i++) {
                for (int j = 0; j < numRows - 1; j++) {
                    if (Grid.GridField[i, j].State == State.Obstacle && ((i != 0 && j != 0) && (i != numColumns - 1 && j != numRows - 1))) {
                        expected = false;
                    } 
                }
            }
            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void GridField_TestIfThereIsOnlyOneSnakeFieldAtInitialization_AreEqual() {
            int numColumns = 15;
            int numRows = 10;
            int Sidelength = 20;
            Grid Grid = new Grid(numColumns, numRows, Sidelength);
            int expected = 1;
            int counter = 0;
            for (int i = 0; i < numColumns - 1; i++) {
                for (int j = 0; j < numRows - 1; j++) {
                    if (Grid.GridField[i, j].State == State.Snake) {
                        counter++;
                    }
                }
            }
            Assert.AreEqual(expected, counter);
        }

        [TestMethod]
        public void GridField_TestIfThereIsOnlyOneFoodFieldAtInitialization_AreEqual() {
            int numColumns = 15;
            int numRows = 10;
            int Sidelength = 20;
            Grid Grid = new Grid(numColumns, numRows, Sidelength);
            int expected = 1;
            int counter = 0;
            for (int i = 0; i < numColumns - 1; i++) {
                for (int j = 0; j < numRows - 1; j++) {
                    if (Grid.GridField[i, j].State == State.Food) {
                        counter++;
                    }
                }
            }
            Assert.AreEqual(expected, counter);
        }

        [TestMethod]
        public void FindNewLocation_TestIfTheFoodDoesOnlySpawnOnAnEmptyFieldWhenEaten_ReturnsTrue() {
            int numColumns = 15;
            int numRows = 10;

            int Sidelength = 20;
            bool expected = false;
            Random Random = new Random();
            int RanColumn = Random.Next(1, numColumns - 2);
            int RanRow = Random.Next(1, numRows - 2);
            Grid Grid = new Grid(numColumns, numRows, Sidelength);
            //Has to initialize snake head, will be testet for, if new snakehead has been found.
            Field snake = null;
            //Finds snakeheads location
            for (int i = 0; i < numColumns - 1; i++) {
                for (int j = 0; j < numRows - 1; j++) {
                    if (Grid.GridField[i, j].State == State.Snake) {
                        snake = Grid.GridField[i, j];
                    }
                }
            }

            if (snake != null) {
                Grid.GridField[snake.Column + 1, snake.Row] = new Food(snake.Column + 1, snake.Row, snake.Sidelength);
                Field temp = Grid.GridField[snake.Column + 1, snake.Row];
                for (int i = 0; i < numColumns - 1; i++) {
                    for (int j = 0; j < numRows - 1; j++) {
                        if ((snake.Column != i || snake.Row != j) && (temp.Column != i || temp.Row != j) && (RanColumn != i || RanRow != j)) {
                            Grid.GridField[i, j] = new Obstacle(i, j, 20);
                        }
                    }
                }
                Grid.MoveSnake(1, 0);
                if (Grid.GridField[RanColumn, RanRow].State == State.Food) {
                    expected = true;
                }
            }
            Assert.IsTrue(expected);
        }
    }
}

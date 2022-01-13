using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGameNS;
using NeuralNetworkNS;
using GeneticAlgorithmNS;
using System.Diagnostics;

namespace SnakeAI {
  // Gem grids + andre opl. i csv fil?

  /// <summary>
  /// Describes relevant information about a round in a fitness calculation: current grid, score and if snake is alive. 
  /// </summary>
  [Serializable]
  public class FitnessRoundInfo {

    // Laves til properties???

    // Game info for snake GUI
    public readonly Grid grid;
    public readonly int score;
    public readonly bool isAlive;
    public Point SnakeHeadPoint { get; }

    // Info for AI GUI
    public readonly int movesSincePoint;
    public readonly int totalMoves;
    public readonly double determinationReward;
    public readonly double[] inputNeuralNetwork; // skal være double
    public readonly Dictionary<int, double> outputNeuralNetwork;
    public readonly Direction currentDirection;

    /// <summary>
    /// Copies recieved information to its properties.
    /// </summary>
    public FitnessRoundInfo(SnakeGame snakeGame, NeuralNetwork neuralNetwork, double[] inputNeuralNetwork, 
                            double determinationReward, int movesSincePoint, int totalMoves, Direction currentDirection,
                            Point snakeHeadPoint) {

      score = snakeGame.Score;
      isAlive = snakeGame.Snake.IsAlive;
      this.movesSincePoint = movesSincePoint;
      this.totalMoves = totalMoves;
      this.determinationReward = determinationReward;
      this.inputNeuralNetwork = new double[inputNeuralNetwork.Length];
      Array.Copy(inputNeuralNetwork, this.inputNeuralNetwork, inputNeuralNetwork.Length);
      outputNeuralNetwork = neuralNetwork.GetOutputValues();
      this.currentDirection = currentDirection;
      SnakeHeadPoint = snakeHeadPoint;
      grid = GetGridCopy(snakeGame.Grid, this.currentDirection);
    }

    // Copies a snake game grid
    private Grid GetGridCopy(Grid gridToCopy, Direction currentDirection) {
      int rowCount = gridToCopy.RowCount;
      int columnCount = gridToCopy.ColumnCount;
      Grid gridCopy = new Grid(rowCount, columnCount);

      for(int i = 0; i < rowCount; i++) {
        for(int j = 0; j < columnCount; j++) {
          SnakeGameNS.Point currentPoint = new Point(i, j);
          gridCopy[currentPoint] = GetFieldCopy(gridToCopy[currentPoint], currentPoint, currentDirection);
        }
      }
      return gridCopy;
    }

    // Returns a copy of recieved field
    private Field GetFieldCopy(Field fieldToCopy, SnakeGameNS.Point currentPoint, Direction currentDirection ) {
      Field fieldCopy;

      switch(fieldToCopy) {
        case Wall w:
          fieldCopy = new Wall(currentPoint);
          break;
        case Food f:
          fieldCopy = new Food(currentPoint);
          break;
        case SnakeHead h:
          fieldCopy = new SnakeHead(currentPoint, currentDirection);
          break;
        case SnakeBodyPart b:
          fieldCopy = new SnakeBodyPart(currentPoint);
          break;
        case Empty e:
          fieldCopy = new Empty(currentPoint);
          break;
        default:
          throw new Exception("Unable to find type");
      }
      return fieldCopy;
    }
  }
}

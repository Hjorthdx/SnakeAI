using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneticAlgorithmNS;
using SnakeGameNS;
using NeuralNetworkNS;

namespace SnakeAI {
  // Husk denne klasse er unik for vores løsning

  /// <summary>
  /// Represents methods used for calculating the fitness value of a snake game simulation.
  /// </summary>
  public class FitnessCalculator : IFitnessCalculator {

    // Ryk alle ej relevante metoder ud herfra!
    private NeuralNetwork neuralNetwork;
    private NetworkSettings networkSettings;
    private SnakeGame snakeGame;
    private SnakeSettings snakeSettings;

    private bool record; // view
    private Agent agentToRecord; // view
    private FitnessCalculatorRecorder recorder;

    public FitnessCalculator(NetworkSettings networkSettings, SnakeSettings snakeSettings) {
      this.networkSettings = networkSettings;
      this.snakeSettings = snakeSettings;
    }

   /// <summary>
   /// Records a fitness calculation of the passed agent and returns the recorder containing all relevant info about the calculation
   /// </summary>
    public FitnessCalculatorRecorder RecordCalculation(Agent agent) {
      record = true;
      agentToRecord = agent;
      CalculateFitness(agent.Chromosome.ToArray());
      record = false;
      return recorder;
    } 

    /// <summary>
    /// Calculates the resulting fitness value by using the passed weights 
    /// and returns the value in a FitnessInfo class together with other relevant info about the calculation.
    /// </summary>
    public IFitnessInfo CalculateFitness(double[] weightsForNeuralNetwork) {
      int action                 = 0;
      int currentScore           = 0;
      int movesSincePoint        = 0;
      int movesTotal             = 0;
      double fitness             = 0;
      double averageMovesPerFood = 0;
      double determinationReward = 0;
      int oldDistance            = 0;
      double[] input = new double[ProgramSettings.NUMBER_OF_INPUT_NEURONS];
      // If showGUI:
      EndGameInfo endGameInfo;


      // Setup new snake game and neural network
      neuralNetwork = new NeuralNetwork(networkSettings, weightsForNeuralNetwork);
      snakeGame = new SnakeGame(snakeSettings);

      // Make recorder
      if(record) {
        recorder = new FitnessCalculatorRecorder(agentToRecord, networkSettings, snakeSettings);
        recorder.TakeSnapShot(new FitnessRoundInfo(snakeGame, neuralNetwork, input, // Lav fitroundinfo i metode, undgå head direation
                      determinationReward, movesSincePoint, movesTotal, snakeGame.Snake.Head.Direction,
                      new Point(snakeGame.Snake.Head.Point.Row, snakeGame.Snake.Head.Point.Column)));
      }

      // Simulation begins
      do {
        input = ConvertGridToInput(snakeGame.Grid, snakeGame.Snake, snakeGame.Food);
        action = neuralNetwork.CalculateOutput(input);
        oldDistance = DistanceToPoint(snakeGame.Snake.Head.Point, snakeGame.Food.Point);
        snakeGame.UpdateDirection(action);
        determinationReward += CalculateDetermination(oldDistance); // fjernes hvis vi ikke bruger determinationReward
        // Check if got point
        if(snakeGame.Score != currentScore) {
          movesSincePoint = 0;
          currentScore = snakeGame.Score;
        }
        else {
          movesSincePoint++;
        }
        movesTotal++;
        if(record) { // Save round info.
        recorder.TakeSnapShot(new FitnessRoundInfo(snakeGame, neuralNetwork, input,
                              determinationReward, movesSincePoint, movesTotal, snakeGame.Snake.Head.Direction,
                              new Point(snakeGame.Snake.Head.Point.Row, snakeGame.Snake.Head.Point.Column)));
        }
      } while(snakeGame.Snake.IsAlive && movesSincePoint < GetMaxMoves(snakeGame.Snake.Lenght));

      if(snakeGame.FoodEaten != 0) {
        averageMovesPerFood = movesTotal / (double)snakeGame.FoodEaten;
      }
      //fitness = determinationReward - averageMovesPerFood;
      fitness = snakeGame.FoodEaten;

      //// if dead by game, fitness is 0.
      //if(!snakeGame.snake.isalive) {
      //  fitness = 0;
      //}

      endGameInfo = new EndGameInfo(snakeGame, fitness, averageMovesPerFood, movesTotal);

      if(record) {
        recorder.TakeSnapShot(endGameInfo);
      }
      // Recorder.SaveToFile(); // Gem CSV fil.
      
      return endGameInfo;
    }

    /// <summary>
    /// Gets the maximum number of moves the snake is allowed to move. 
    /// </summary>
    /// <param name="snakeLenght"></param>
    /// <returns></returns>
    private int GetMaxMoves(int snakeLenght) {
      int maxStepsSincePoint;
      // If not recording, set specified limit // Evt. lav det til noget man selv kan bestemme
      if(!record) {
        maxStepsSincePoint = snakeLenght + ProgramSettings.MAX_STEPS;
      }
      else {
        maxStepsSincePoint = 500;
      }
      return maxStepsSincePoint;
    }

    // Input skal egentlig være doubles... ??
    private static double[] ConvertGridToInput(Grid grid, Snake snake, Food food) {
      double[] input = new double[ProgramSettings.NUMBER_OF_INPUT_NEURONS];

      int snakeRow = snake.Head.Point.Row;
      int snakeColumn = snake.Head.Point.Column;
      int foodRow = food.Point.Row;
      int foodColumn = food.Point.Column;

      // Look after obstacles in 4 directions
      input[0] = Convert.ToInt32((grid.GetNextFieldInDirection(snake.Head.Point, Direction.Up) is Wall)); // Wall up
      input[1] = Convert.ToInt32((grid.GetNextFieldInDirection(snake.Head.Point, Direction.Down) is Wall)); // Wall up
      input[2] = Convert.ToInt32((grid.GetNextFieldInDirection(snake.Head.Point, Direction.Left) is Wall)); // Wall up
      input[3] = Convert.ToInt32((grid.GetNextFieldInDirection(snake.Head.Point, Direction.Right) is Wall)); // Wall up

      input[4] = Convert.ToInt32((grid.GetNextFieldInDirection(snake.Head.Point, Direction.Up) is SnakeBodyPart)); // Wall up
      input[5] = Convert.ToInt32((grid.GetNextFieldInDirection(snake.Head.Point, Direction.Down) is SnakeBodyPart)); // Wall up
      input[6] = Convert.ToInt32((grid.GetNextFieldInDirection(snake.Head.Point, Direction.Left) is SnakeBodyPart)); // Wall up
      input[7] = Convert.ToInt32((grid.GetNextFieldInDirection(snake.Head.Point, Direction.Right) is SnakeBodyPart)); // Wall up

      // Look for food
      input[8] = Convert.ToInt32(foodRow < snakeRow); // Is food above snake
      input[9] = Convert.ToInt32(foodRow > snakeRow); // is food below snake
      input[10] = Convert.ToInt32(foodColumn < snakeColumn); // Is food left of snake
      input[11] = Convert.ToInt32(foodColumn > snakeColumn); // Is food right of snake

      input[12] = Convert.ToInt32(IsFieldInDirection(grid, snake.Head, typeof(SnakeBodyPart), Direction.Up));
      input[13] = Convert.ToInt32(IsFieldInDirection(grid, snake.Head, typeof(SnakeBodyPart), Direction.Down));
      input[14] = Convert.ToInt32(IsFieldInDirection(grid, snake.Head, typeof(SnakeBodyPart), Direction.Left));
      input[15] = Convert.ToInt32(IsFieldInDirection(grid, snake.Head, typeof(SnakeBodyPart), Direction.Right));

      return input;
    } 

    // Search for field type in unknown location
    private static bool IsFieldInDirection(Grid grid, Field currentField, Type targetFieldType, Direction direction) { //fitnesscalc 
      bool targetFound = false;

      Point currentPoint = new Point(currentField.Point.Row, currentField.Point.Column);

      currentPoint.Move(direction);

      while(grid.PointWithinGrid(currentPoint) && !targetFound) {
        if(grid[currentPoint].GetType() == targetFieldType) { // ændres
          targetFound = true;
        }
        else {
          currentPoint.Move(direction);
        }
      }

      return targetFound;
    }  // fitness





    private double CalculateDetermination(int oldDistance) {
      double newDistance = DistanceToPoint(snakeGame.Snake.Head.Point, snakeGame.Food.Point);
      if(newDistance > oldDistance) {
        return -1.3;
      }
      else {
        return 1;
      }
    } // I fitnessCalculator 

    // Search for field type in unknown location
    // kan også ligge i field, field.DistanceTo
    private static int StepsToFieldType(Grid grid, Field currentField, Type targetFieldType, Direction direction) {
      int steps = 0;
      bool targetFound = false;

      Point currentPoint = new Point(currentField.Point.Row, currentField.Point.Column); // Starting point

      currentPoint.Move(direction);

      while(grid.PointWithinGrid(currentPoint) && !targetFound) {
        if(grid[currentPoint.Row, currentPoint.Column].GetType() == targetFieldType) {
          targetFound = true;
        }
        else {
          steps++;
          currentPoint.Move(direction);
        }
      }

      if(!targetFound) {
        steps = 0;
      }

      return steps;
    }  // fitness

    // Make binary input for all grid fields, for alle states it can take. // Bruges ikke
    private static int[] ConvertGridToInput2(Field[,] grid, Snake snake, Food food, SnakeGame snakeGame) {
      int[] input = new int[ProgramSettings.NUMBER_OF_INPUT_NEURONS];
      int counter = 0;

      // Define arrays
      int[] inputWalls = new int[ProgramSettings.GRID_ROWS * ProgramSettings.GRID_COLUMNS];
      int[] inputFood = new int[ProgramSettings.GRID_ROWS * ProgramSettings.GRID_COLUMNS];
      int[] inputHead = new int[ProgramSettings.GRID_ROWS * ProgramSettings.GRID_COLUMNS];
      int[] inputBody = new int[ProgramSettings.GRID_ROWS * ProgramSettings.GRID_COLUMNS];
      int[] inputGridField = new int[ProgramSettings.GRID_ROWS * ProgramSettings.GRID_COLUMNS];

      // Grid er 20x30
      // 600 input for én state = 600*4 = 2400 inputs i alt
      // States er: wall, food, head, body
      // 4 states

      // Dvs. gennemgå hele grid. Spørg hvert field hvilket state det er. Lav en switch, en global counter

      Type typeOfCurrentField;

      for(int i = 0; i < grid.GetLength(0); i++) {

        for(int j = 0; j < grid.GetLength(1); j++) {

          inputWalls[counter] = 0;
          inputFood[counter] = 0;
          inputHead[counter] = 0;
          inputBody[counter] = 0;
          inputGridField[counter] = 0;
          // Skal gøres til switch! Se skrald.
          if(grid[i,j] is Wall) {
            inputWalls[counter] = 1;
          }
          else if(grid[i, j] is Food) {
            inputFood[counter] = 1;
          }
          else if(grid[i, j] is SnakeHead) {
            inputHead[counter] = 1;
          }
          else if(grid[i, j] is SnakeBodyPart) {
            inputBody[counter] = 1;
          }
          else if(grid[i, j] is Empty) {
            inputGridField[counter] = 1;
          }
          else {
            Console.WriteLine("Not found???");
            Console.WriteLine("was == " + grid[i, j].GetType());
            Console.ReadKey();
          }
          counter++;

          #region #skrald
          /*

          object obj = grid[i, j].GetType();

          switch(obj) {

            case Wall w:
              inputWalls[counter] = 1;
              break;
            case Food f:
              inputFood[counter] = 1;
              break;
            case SnakeHead h:
              inputHead[counter] = 1;
              break;
            case SnakeBody b:
              inputBody[counter] = 1;
              break;
            default:
              throw new Exception("Could not identify type");
          }
          counter++;
          Console.WriteLine(counter);
        */
          #endregion  
        }
      }

      //DisplayInputArrays(inputWalls, inputFood, inputHead, inputBody, inputGridField, snakeGame);

      Array.Copy(inputWalls, input, inputWalls.Length);
      Array.Copy(inputFood, 0, input, inputWalls.Length, inputFood.Length);
      Array.Copy(inputHead, 0, input, inputFood.Length, inputHead.Length);
      Array.Copy(inputBody, 0, input, inputHead.Length, inputBody.Length);
      Array.Copy(inputGridField, 0, input, inputBody.Length, inputGridField.Length);

      return input;
    } // fitness

    private static int DistanceToPoint(Point current, Point target) {
      int distance = Math.Abs(current.Row - target.Row) + Math.Abs(current.Column - target.Column);
      return distance;
    } 

    private void DisplayInputArrays(int[] inputWalls, int[] inputFood, int[] inputHead, int[] inputBody, int[] inputGridField, SnakeGame snakeGame ) {
      Console.WriteLine("Wall");
      snakeGame.Display();
      DisplayInputArray(inputWalls);
      Console.ReadKey();

      Console.WriteLine("FOod");
      snakeGame.Display();
      DisplayInputArray(inputFood);
      Console.ReadKey();

      Console.WriteLine("Head");
      snakeGame.Display();
      DisplayInputArray(inputHead);
      Console.ReadKey();

      Console.WriteLine("Body");
      snakeGame.Display();
      DisplayInputArray(inputBody);
      Console.ReadKey();

      Console.WriteLine("GridField");
      snakeGame.Display();
      DisplayInputArray(inputGridField);
      Console.ReadKey();
    }  // view

    // For visualising input grid
    private void DisplayInputArray(int[] inputArray) {
      int counter = 0;
      for(int i = 0; i < inputArray.Length; i++) {
        Console.Write(inputArray[i]);
        counter++;
        if(counter == 30) {
          Console.WriteLine();
          counter = 0;
        }
      }
    } // View
  }
}

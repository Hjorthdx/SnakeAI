using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameNS { 
  public class SnakeGame {

    public Grid Grid { get; set; }
    public Food Food { get; private set; }
    public Snake Snake { get; private set; }
    public int FoodEaten { get; private set; }
    public static Direction previousDirection { get; private set; } // Prev direction of snake to check if opposite action chosen
    public int Score {
      get { return FoodEaten * snakeSettings.pointsPerFoodEaten; }
    }

    private SnakeSettings snakeSettings;
    private SnakeGameGUI snakeGameGUI;
    private bool withGUI;
    private bool foundFood;

    // Constructor
    public SnakeGame(SnakeSettings snakeSettings) {
      Console.OutputEncoding = Encoding.GetEncoding(28591);
      this.snakeSettings = snakeSettings;

      // Make grid
      Grid = new Grid(snakeSettings.rowCount, snakeSettings.columnCount);
      // Make snake
      Snake = new Snake(Grid.GetCentrePoint()); 
      // Make food
      Food = new Food(Grid.GetPointOfRandomEmptyField(snakeSettings.randomNumber));
      foundFood = false;
      // Place objects in grid
      Grid.PlaceNewObject(Snake);
      Grid.PlaceNewObject(Food);

      // END OF CONSTRUCTOR
    }

    // Runs a GUI window of the snake game
    public void StartGUI() {
      withGUI = true; 
      snakeGameGUI = new SnakeGameGUI(snakeSettings, this); 
      snakeGameGUI.OpenGameWindow();
    }

    // Kan der abstraheres mere? 
    /// <summary>
    /// Updates game grid as a result of passed action.
    /// </summary>
    /// <param name="action"> 
    /// Left = 1, Right = 2, Up = 3, Down = 4.
    /// </param>
    public void UpdateDirection(int action) {
      // Convert action recived by caller to enum
      Direction directionToMove = GetDirectionToMove(action);

      // Prevent from going against itself
      if(isOppositeDirections(directionToMove, previousDirection)) {
        directionToMove = previousDirection;
      }

      // Investigate the field snake is moving to 
      Field targetField = Grid.GetNextFieldInDirection(Snake.Head.Point, directionToMove);

      // Check if snake is dead (obstacle or body hit)
      if(DeadlyField(targetField)) {
        Snake.KillSnake();
      }
      else { // If not dead, do a normal round
        // Check if food --> extend snake before moving!
        if(targetField is Food) {
          FoodEaten++;
          Snake.Extend();
          foundFood = true;
        }

        // Move snake and place on grid
        Snake.Move(directionToMove);
        Grid.UpdateObjectPosition(Snake, foundFood);

        // Make new food AFTER snake has been moved and placed in grid, to avoid placing food where the snake will move to
        if(foundFood) {
          // Check if game has been completed
          if(Score != snakeSettings.maxScore) { // Check if max points, else method GetRandomField will never find empty field = infinite loop
            Food = new Food(Grid.GetPointOfRandomEmptyField(snakeSettings.randomNumber));
            Grid.UpdateObjectPosition(Food);
          }
          else {
            Snake.KillSnake();
          }
          foundFood = false;
        }
        // Save direction for next round to check if going against itself
        previousDirection = directionToMove;
      }
      if(withGUI) {
        snakeGameGUI.UpdateView(this);
      }

      // END OF UPDATE DIRECTION
    }
    
    // Check if passed directions is opposite of eachother 
    private bool isOppositeDirections(Direction direction1, Direction direction2) {
      bool isOpposite = false;
      switch(direction1) {
        case Direction.Up:
          if(direction2 == Direction.Down) {
            isOpposite = true;
          }
          break;
        case Direction.Down:
          if(direction2 == Direction.Up) {
            isOpposite = true;
          }
          break;
        case Direction.Left:
          if(direction2 == Direction.Right) {
            isOpposite = true;
          }
          break;
        case Direction.Right:
          if(direction2 == Direction.Left) {
            isOpposite = true;
          }
          break;
        default:
          isOpposite = false;
          break;
      }
      return isOpposite;
    }

    // Display game screen // Console, fjernes i final
    public void Display() {
      Grid.Display();
      Console.WriteLine($"Score: {Score} ");
    } 

    // Check if field will make snake die
    private bool DeadlyField(Field newField) {
      if(newField is Obstacle ||
         newField is SnakeBodyPart) {
        return true;
      }
      else {
        return false;
      }
    }

    // Console, fjernes final
    private void DisplayScreen<T>(T obj, string message) {
      for(int i = 0; i < snakeSettings.rowCount; i++) {
        for(int j = 0; j < snakeSettings.columnCount; j++) {
          Console.Write(obj.ToString());
        }
        Console.Write('\n');
      }
      Console.WriteLine($"\n{message}");
    }
    // Console, fjernes final
    public void DisplayEndScreen() {
      if(Score == snakeSettings.maxScore) {
        DisplayScreen('$', "CONGRATULATIONS, YOU WON!!! :D");
      }
      else {
        DisplayScreen('*', "GAME OVER :-(");
      }
      Console.WriteLine($"Total score: {Score} ");
    }

    // Converts an integer to correct snake action
    private Direction GetDirectionToMove(int action) {
      Direction directionToMove;
      directionToMove = (Direction)action;

      if(!(directionToMove == Direction.Up || directionToMove == Direction.Down ||
          directionToMove == Direction.Left || directionToMove == Direction.Right)) {
        throw new Exception("Invalid action passed to SnakeGame");
      }
      return directionToMove;
    }
  }
}

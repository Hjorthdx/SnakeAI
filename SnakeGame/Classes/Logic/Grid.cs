using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameNS {
  public class Grid {

    public Field[,] Fields { get; set; }

    // Accessor to access Grid index by point
    public Field this[Point point] {
      get { return Fields[point.Row, point.Column]; }
      set { Fields[point.Row, point.Column] = value; }
    }
  
    // Acessor to accees Grid index by integers
    public Field this[int row, int column] {
      get { return Fields[row, column]; }
      set { Fields[row, column] = value; }
    }
   
    public int RowCount { get; }
    public int ColumnCount { get;  }

    // Keep track of previous tail position to know which field to delete every round
    private Point previousSnakeTailPosition; 
    private Point previousSnakeHeadPosition;

    // Constructor to generate grid
    //ddf
    public Grid(int rowCount, int columnCount) {
      RowCount    = rowCount;
      ColumnCount = columnCount;
      // Make empty grid with walls
      InitializeGrid();
    }

    // Fill grid with walls and empty fields
    private void InitializeGrid() {
      Fields = new Field[RowCount, ColumnCount];

      for(int i = 0; i < RowCount; i++) {
        for(int j = 0; j < ColumnCount; j++) {
          if((i == 0 || j == 0) || (i == RowCount - 1 || j == ColumnCount - 1)) {
            Fields[i, j] = new Wall(new Point(i, j));
          }
          else {
            Fields[i, j] = new Empty(new Point(i, j));
          }
        }
      }
    }

    public void PlaceNewObject(Snake snake) {
      if(RowCount * ColumnCount < SnakeConstants.MIN_SNAKE_FIELDS) {
        throw new Exception($"Grid too small - no room for snake and food");
      }

      this[snake.Head.Point] = snake.Head;

      if(snake.HasBody) {
        for(int i = 0; i < snake.Body.Count; i++) {
          this[snake.Body[i].Point] = snake.Body[i];
        }
      }

      // Copy position of game objects
      previousSnakeHeadPosition = new Point(snake.Head.Point.Row, snake.Head.Point.Column);
      previousSnakeTailPosition = new Point(snake.Head.Point.Row, snake.Head.Point.Column);
    }

  public void PlaceNewObject(Food food) {
      this[food.Point] = food;
    }

    // Search for empty field
    public Point GetPointOfRandomEmptyField(Random r) {
      Point randomPoint;
      do {
        randomPoint = new Point(r.Next(RowCount), r.Next(ColumnCount));
      } while(!(this[randomPoint] is Empty));
      return randomPoint;
    }

    public Point GetCentrePoint() {
      Point centrePoint;
      centrePoint = new Point((RowCount - 1 - (RowCount / 2)), ColumnCount - 1 - (ColumnCount / 2));
      return centrePoint; 
    }
    
    public Field GetNextFieldInDirection(Point currentPoint, Direction directionToMove) {
      Point nextFieldPoint = new Point(currentPoint.Row, currentPoint.Column);
      nextFieldPoint.Move(directionToMove);
      return this[nextFieldPoint];
    }


    public bool PointWithinGrid(Point point) {
      if((point.Row >= 0 && point.Row < RowCount) &&
         (point.Column >= 0 && point.Column < ColumnCount)) {
        return true;
      }
      else {
        return false;
      }
    }

    // Update object of type snake. Only updates fields that has been changed 
    public void UpdateObjectPosition(Snake snake, bool bodyAdded) {
      // Place new snake head on grid
      this[snake.Head.Point] = snake.Head;
      
      // If snake has a body, replace previous position of snake head with snake body
      if(snake.HasBody) {
        this[previousSnakeHeadPosition] = new SnakeBodyPart(previousSnakeHeadPosition);
      }
      // If no body added in this round, delete tail 
      if(!bodyAdded) {
        if(this[previousSnakeTailPosition] is Wall) {
          Console.WriteLine("Attemp to overwrite wall. Snake tail out of game boundaries!"); // Should never happen.
        }
        this[previousSnakeTailPosition] = new Empty(previousSnakeTailPosition);
      }

      // Copy position of head and tail to use when updating next time
      previousSnakeHeadPosition = new Point(snake.Head.Point.Row, snake.Head.Point.Column);
      previousSnakeTailPosition = new Point(snake.Body.Last().Point.Row, snake.Body.Last().Point.Column);
    }

    public void UpdateObjectPosition(Food food) {
      this[food.Point] = food;
      // Not deleting food position as it has been replaced by snake head earlier
    }

    // Print grid to console // Skal fjernes i final ver.
    public void Display() {
      for(int i = 0; i < RowCount; i++) {
        for(int j = 0; j < ColumnCount; j++) {
          Console.Write(Fields[i, j].ToString());
        }
        Console.WriteLine();
      }
    }
  }
}

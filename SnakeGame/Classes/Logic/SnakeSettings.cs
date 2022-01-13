using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameNS {

  // Settings passed to snake game
  [Serializable]
  public class SnakeSettings {
    public readonly int rowCount;
    public readonly int columnCount;
    public readonly Random randomNumber;
    public readonly int maxScore;
    public readonly int pointsPerFoodEaten;
    public readonly int sideLength; // For GUI

    public SnakeSettings(int gridRows, int gridColumns, int sideLenght, int pointsPerFoodEaten, Random randomNumber) {
      this.rowCount = gridRows;
      this.columnCount = gridColumns;
      this.pointsPerFoodEaten = pointsPerFoodEaten;
      this.randomNumber = randomNumber;
      this.sideLength = sideLenght;
      // Max Points = Every field not walls or snake head
      this.maxScore = ((gridRows * gridColumns) - (gridRows * 2 + gridColumns * 2) + (2 + 2) - 1)*pointsPerFoodEaten;
    }
  }
}

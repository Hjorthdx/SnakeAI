using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameNS {
  /// <summary>
  /// A class that contains the settings of the snake game.
  /// </summary>
  public class SnakeSettingsGUI {

    /// <summary>
    /// The number of rows in the grid.
    /// </summary>
    public readonly int rowCount;
    /// <summary>
    /// The number of columns in the grid.
    /// </summary>
    public readonly int columnCount;
    /// The side length of each field in the grid.
    /// </summary>
    public readonly int sideLength;

    /// <summary>
    /// Initializes a new instance of the <see cref="SnakeSettingsGUI"/> class, with custom settings.
    /// </summary>
    /// <param name="numColumns">The number of columns.</param>
    /// <param name="numRows">The number of rows.</param>
    /// <param name="sideLength">The side length of a single field.</param>
    public SnakeSettingsGUI(int rowCount, int columnCount, int sideLength) {
      this.rowCount = rowCount;
      this.columnCount = columnCount;
      this.sideLength = sideLength; // skal bare i settings 
    }
  }
}

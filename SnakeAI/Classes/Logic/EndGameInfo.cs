using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithmNS;
using SnakeGameNS;

namespace SnakeAI {
  // Husk denne klasse er unik for vores løsning

  /// <summary>
  /// Defines properties which represent information about a fitness calculation of a snake game.
  /// </summary>
  [Serializable]
  public class EndGameInfo : IFitnessInfo {

    public double Fitness { get; private set; }
    public int foodEaten; 
    public double Score { get; private set; }
    public double averageMovesPerFood;
    public readonly double MovesTotal;
    public SnakeCauseOfDeath SnakeCauseOfDeath;

    public EndGameInfo(SnakeGame snakegame, double fitness, double averageMovesPerPoint, int movesTotal) {
      Fitness = fitness;
      this.foodEaten = snakegame.FoodEaten;
      this.Score = snakegame.Score;
      this.averageMovesPerFood = averageMovesPerPoint;
      MovesTotal = movesTotal;
      SnakeCauseOfDeath = GetCauseOfDeath(snakegame.Snake);
    }

    private SnakeCauseOfDeath GetCauseOfDeath(Snake snake) {
      if(!snake.IsAlive) {
        return SnakeCauseOfDeath.KilledByGame;
      } // If still alive, exceeding step limit must have killed the snake
      else {
        return SnakeCauseOfDeath.KilledBySteps;
      }
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithmNS;
using SnakeGameNS;

namespace SnakeAI {
  // Egen impl. af prints... bør jo ikke være en del af de 3 objekter, da skal kunne bruges som libraries

  /// <summary>
  /// Print methods used in main
  /// </summary>
  static class Print {

    private static double timeIntervalBetweenPrintsMs = ProgramSettings.PRINT_INTERVAL_SECONDS * 1000.00;
    private static double timeAtNextPrintMs = 0;
    private static List<string> TopTenAverageFitnessStrings = new List<string>();
    private static int maxFitness = CalculateMaxFitness();

    public static void GeneticSettings(GeneticSettings geneticSettings) {

      GeneticSettings gs = geneticSettings;
      const int width = -30;

      Console.WriteLine("GENETIC SETTINGS: \n");
      Console.WriteLine($"{"Population size",width}: {gs.PopulationSize}");
      Console.WriteLine($"{"Selection size", width}: {gs.SelectionSize}");
      Console.WriteLine($"{"Mutation probability, genes", width}: {gs.MutationProbabilityGenes}");
      Console.WriteLine($"{"Mutation probability, agents", width}: {gs.MutationProbabiltyAgents}");
      Console.WriteLine($"{"GeneCount size",width}: {gs.GeneCount} (weights in NN)");
      Console.WriteLine($"{"Selection method", width}: {gs.Selector.Title}");
      Console.WriteLine($"{"Mutation method:", width}: {gs.Mutator.Title}");
      Console.WriteLine($"{"CrossOverMethod", width}: {gs.Crossover.Title}");
      Console.WriteLine($"{"Child agents in population", width}: {gs.ChildAgentsInPopulationCount}");
      Console.WriteLine($"{"Random agents in population", width}: {gs.RandomAgentsInPopulationCount}");
      Console.WriteLine();
    }

    public static void PopulationAndReadKey(string message, GeneticAlgorithm geneticAlgo) {
      Console.WriteLine(message);
      geneticAlgo.CurrentPopulation.Print();
      Console.ReadKey();
    }

    public static void TopTenAverageFitness(List<string> topTenAverageFitnessStrings) {
      Console.WriteLine();
      foreach(var item in topTenAverageFitnessStrings) {
        Console.WriteLine(item);
      }
    }

    // Print top ten list // Skal i winform når implementeret
    public static void TopTenAgents(TopKAgents topKAgents) {
      for(int i = 0; i < topKAgents.BestAgents.Count; i++) {

        // Check highscore is new
        if(topKAgents[i].IsNewScore) { // ændre tilbage til generation...
          // Make sound and print
          System.Media.SystemSounds.Beep.Play();
          topKAgents[i].Agent.Print();
          Console.Write($"{$"-> Food {(topKAgents[i].Agent.FitnessInfo as EndGameInfo).foodEaten}",-10}");

          Console.Write($"{$"-> Avg. moves pr. food {(topKAgents[i].Agent.FitnessInfo as EndGameInfo).averageMovesPerFood:N2}", -30}");
          Console.Write($"{$"-> {(topKAgents[i].Agent.FitnessInfo as EndGameInfo).SnakeCauseOfDeath.ToString()}",-5:N2}");
          Console.Write(" | NEW HIGHSCORE!!!");
          topKAgents[i].IsNewScore = false;
        }
        else { // Else print without highscore print
          topKAgents[i].Agent.Print();
          Console.Write($"{$"-> Food {(topKAgents[i].Agent.FitnessInfo as EndGameInfo).foodEaten}",-10}");
          Console.Write($"{$"-> Avg. moves pr. food {(topKAgents[i].Agent.FitnessInfo as EndGameInfo).averageMovesPerFood:N2}", -30}");
          Console.Write($"{$"-> {(topKAgents[i].Agent.FitnessInfo as EndGameInfo).SnakeCauseOfDeath.ToString()}",-5:N2}");
        }
        Console.WriteLine();
      }
    }

    public static void Status(GeneticAlgorithm geneticAlgorithm) {
      TimeSpan time = geneticAlgorithm.RunTime.Elapsed;
      Console.Clear();
      Console.Write($"\n--SELECTED AGENTS FOUND, GEN {geneticAlgorithm.GenerationCount} | ");
      Console.WriteLine($"RUN TIME: {geneticAlgorithm.RunTime.Elapsed.ToString(@"hh\:mm\:ss")}--");

      Console.WriteLine($"\nTOTAL SNAKE GAME SIMULATIONS: {Agent.TotalFitnessCalculations}");
      Console.WriteLine("\nALL TIME TOP 10 FITTEST");
      Console.WriteLine($"(Max attainable points in game = {maxFitness})\n");

      Print.TopTenAgents(geneticAlgorithm.TopKAgents);

      if(geneticAlgorithm.RunTime.ElapsedMilliseconds > timeAtNextPrintMs) {

        TopTenAverageFitnessStrings.Add($"STAMP {timeAtNextPrintMs / 1000.00 / 60.00:N2} min. Actual: {time.ToString(@"hh\:mm\:ss")}  " +
          $"Avg. top10 fitness --> {geneticAlgorithm.TopKAgents.AverageFitness} | Avg. current pop: {geneticAlgorithm.CurrentPopulation.AverageFitness:N2}" +
          $" --> Average agent convergence percent: {geneticAlgorithm.ConvergencePercentLatest:N5} %"); 
        
        // Set next print time to nearest interval above
        timeAtNextPrintMs = (Math.Ceiling(time.TotalMilliseconds / timeIntervalBetweenPrintsMs) * timeIntervalBetweenPrintsMs);
      }

      Print.TopTenAverageFitness(TopTenAverageFitnessStrings);
    }

    public static int CalculateMaxFitness() {
      // Make instance of snakesettings to get max attainable fitness
      SnakeSettings snakeSettings = new SnakeSettings(ProgramSettings.GRID_ROWS, ProgramSettings.GRID_COLUMNS, ProgramSettings.SIDE_LENGTH,
                                                      ProgramSettings.POINTS_PER_FOOD_EATEN, new Random());
      return snakeSettings.maxScore;
    }
  }
}

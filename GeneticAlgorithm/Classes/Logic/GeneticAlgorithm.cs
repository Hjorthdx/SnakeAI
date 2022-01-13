using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneticAlgorithmNS {
  /// <summary>
  /// Contains and invokes methods on the current population of agents. 
  /// </summary>
  public class GeneticAlgorithm {

    public Population CurrentPopulation { get; private set; }
    public int GenerationCount { get; private set; }
    public Stopwatch RunTime { get; set; } // Remember to stop when not calculating! (calling menu etc.)
    public TopKAgents TopKAgents { get; private set; } // The all time top agents of this genetic algorithm. 
    /// <summary>
    /// The latest calculated convergence percent representing how many genes every agent shares 
    ///  with other agents on average. 
    /// </summary>
    public double ConvergencePercentLatest { get; private set; } // 

    private GeneticSettings geneticSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeneticAlgorithm"/> class containing
    /// a random population using the specified <see cref="GeneticSettings"/>. 
    /// </summary>
    /// <param name="geneticSettings">The settings that the algorithm should use.</param>
    public GeneticAlgorithm(GeneticSettings geneticSettings) {
      // Start timer
      RunTime = new Stopwatch();
      // Set up variables
      GenerationCount = 0;
      this.geneticSettings = geneticSettings;
      TopKAgents = new TopKAgents(geneticSettings.TopKAgentsCount);
      // Make random start population
      MakeRandomPopulation(geneticSettings);
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="GeneticAlgorithm"/> class using
    /// the specified <see cref="GeneticSettings"/> and sets the given population as the 
    /// <see cref="CurrentPopulation"/>.
    /// </summary>
    /// <param name="geneticSettings">The settings that the algorithm should use.</param>
    /// <param name="population">The population that should be set as the current population of the algorithm.</param>
    public GeneticAlgorithm(GeneticSettings geneticSettings, Population population) {
      // Start timer
      RunTime = new Stopwatch();
      // Set up variables
      GenerationCount = 0;
      this.geneticSettings = geneticSettings;
      TopKAgents = new TopKAgents(geneticSettings.TopKAgentsCount);
      CurrentPopulation = population;
    }


    /// <summary>
    /// Makes a new random population from the recieved settings.
    /// </summary>
    private void MakeRandomPopulation(GeneticSettings geneticSettings) {
      CurrentPopulation = new Population(geneticSettings.PopulationSize, geneticSettings.GeneCount, GenerationCount, geneticSettings.RandomNumberGenerator);
    }

    /// <summary>
    /// Updates the <see cref="ConvergencePercentLatest"/> based on the convergence 
    /// of the current population of the algorithm.
    /// </summary>
    public void UpdateConvergencePercent() {
      ConvergencePercentLatest = CurrentPopulation.GetConvergencePercent();
    }

    /// <summary>
    /// Invokes the calculate fitness method on the current population and 
    /// </summary>
    public void CalculateFitness() {
      CurrentPopulation.CalculateFitness(geneticSettings.FitnessCalculator); 
      TopKAgents.Update(CurrentPopulation.Agents);
    }

    public void MakeSelection() {
      CurrentPopulation.MakeSelection(geneticSettings.Selector, geneticSettings.SelectionSize);
    }

    // Makes a new generation by invoking the crossover method on the current population
    public void MakeCrossovers() {
      List<Agent> children = new List<Agent>();
      GenerationCount++;
      children = CurrentPopulation.MakeCrossovers(geneticSettings.Crossover, geneticSettings.PopulationSize, geneticSettings.RandomNumberGenerator);
      CurrentPopulation = new Population(children, GenerationCount);
    }

    public void MakeMutations() {
      CurrentPopulation.MakeMutations(geneticSettings.Mutator, geneticSettings.MutationProbabiltyAgents,
                                      geneticSettings.MutationProbabilityGenes, geneticSettings.RandomNumberGenerator);
    }

    public void ShowGUI() {
      Thread thread = new Thread(RunThreadGUI);
      thread.Start();
    }

    private void RunThreadGUI() {
      Application.Run(new GeneticGUI(this, geneticSettings));
    }
  }
}

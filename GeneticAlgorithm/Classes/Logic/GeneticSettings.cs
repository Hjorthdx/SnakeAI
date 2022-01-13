using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmNS {
  /// <summary>
  /// Contains the settings of the genetic algorithm. Is immutable. 
  /// </summary>
  public class GeneticSettings {

    public int GeneCount { get; }
    public int PopulationSize { get; private set; }
    public int SelectionSize { get; private set; }

    public readonly int ChildAgentsInPopulationCount; // not impl.
    public readonly int RandomAgentsInPopulationCount; // not impl.

    public double CrossoverProbalityAgents { get; private set; } // not impl.
    public double MutationProbabiltyAgents { get; private set; }
    public double MutationProbabilityGenes  {get; private set;}

    public IRandomNumberGenerator RandomNumberGenerator { get; }
    public IFitnessCalculator FitnessCalculator { get; }
    public IMutator Mutator { get; }
    public ISelector Selector { get; }
    public ICrossover Crossover { get; }

    public int TopKAgentsCount { get; }

    public GeneticSettings(int populationSize,
                           int geneCount,
                           int selectionSize,
                           double childAgentsInPopulationPercent,
                           double crossoverProbabilityAgents,
                           double mutationProbabiltyAgents,
                           double mutationProbabilityGenes,
                           IRandomNumberGenerator randomNumberGenerator,
                           IFitnessCalculator fitnessCalculator,
                           IMutator mutator,
                           ISelector selector,
                           ICrossover crossover,
                           int topKAgentsCount) {

      if(populationSize < selectionSize) {
        throw new Exception("The population size has to be greater than the selection size");
      }

      PopulationSize = populationSize;
      GeneCount      = geneCount;
      SelectionSize  = selectionSize;


      ChildAgentsInPopulationCount = (int)(childAgentsInPopulationPercent * populationSize);
      RandomAgentsInPopulationCount = populationSize - ChildAgentsInPopulationCount;

      CrossoverProbalityAgents = crossoverProbabilityAgents;
      MutationProbabiltyAgents = mutationProbabiltyAgents;
      MutationProbabilityGenes = mutationProbabilityGenes;

      RandomNumberGenerator = randomNumberGenerator;
      FitnessCalculator     = fitnessCalculator;
      Mutator               = mutator;
      Selector              = selector;
      Crossover             = crossover;

      TopKAgentsCount = topKAgentsCount;
    }
  }
}

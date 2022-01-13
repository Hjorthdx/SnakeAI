using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmNS {
  /// <summary>
  /// Represents a crossover method that keeps the parents.
  /// </summary>
  public class OnePointCombinePlusElitismCrossover : OnePointCombineCrossover, ICrossover {

    public string Title { get; } = "OnePointCombinePlusElitismCrossover";

    public List<Agent> MakeCrossovers(List<Agent> parentAgents, int populationSize, IRandomNumberGenerator random) { // Random int
      List<Agent> children = MakeOnePointCombineCrossover(parentAgents, populationSize - parentAgents.Count, random );
      children.AddRange(parentAgents); // Add parents
      return children;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmNS {
  /// <summary>
  /// Represents a crossover method that crosses passed agents by randomly selecting a point where the chromsomes
  /// of two randomly selected parents will be combined.
  /// </summary>
  public class OnePointCombineCrossoverRegular : OnePointCombineCrossover, ICrossover {

    public string Title { get; } = "OnePointCombineCrossover";

    public List<Agent> MakeCrossovers(List<Agent> parentAgents, int populationSize, IRandomNumberGenerator random) { // Random int
      List<Agent> children = MakeOnePointCombineCrossover(parentAgents, populationSize, random);
      return children;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmNS {
  /// <summary>
  /// Represents a mutation method where the gene is randomized if below threshold.
  /// </summary>
  public class RandomResettingMutator : IMutator {

    public string Title { get; } = "RandomResettingMutator";

    public Gene[] MakeMutation(Gene[] genes, double mutationProbabilityGene, IRandomNumberGenerator random) { // random generator passes? som arver fra Irandom num?
      for(int i = 0; i < genes.Length; i++) {
        // Check if this gene should be mutated
        double randomDouble = random.GetDouble(0, 1); // skal ikke være magisk værdi. modtag genetic geneRange? 
        if(randomDouble < mutationProbabilityGene) {
          // Overwrite with new random gene
          genes[i] = new Gene(random);
        }
      }
      return genes;
    }
  }
}

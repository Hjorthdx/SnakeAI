using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmNS {

  // BRUGES IKKE, men kommer måske til dig i main for at kunne switche mellem metoder
  public enum SelectionMethod {
    Elite, RouletteWheel
  }

  public enum CrossoverMethod {
    Combine, Swap, None
  }
  //
  public enum MutationMethod {
    Mutate, None
  }
}

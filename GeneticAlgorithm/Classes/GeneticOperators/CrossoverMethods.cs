using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmNS{
  public class CrossoverMethods { // Factory pattern???

    public OnePointCombineCrossoverRegular OnePointCombineCrossoverRegular {
      get { return new OnePointCombineCrossoverRegular(); }
    }

    public OnePointCombinePlusElitismCrossover OnePointCombinePlusElitismCrossover {
      get { return new OnePointCombinePlusElitismCrossover(); }
    }

  }
}

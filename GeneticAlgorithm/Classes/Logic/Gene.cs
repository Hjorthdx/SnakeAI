using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmNS {
  /// <summary>
  /// Contains a number which represent the smallest part of a chromosome.
  /// </summary>
  [Serializable]
  public class Gene {

    public double Number { get; }

    public Gene(IRandomNumberGenerator random) {                                               
      Number = random.GetDouble(-1, 1);
    }

    public Gene(double number) {
      Number = number;
    }

    public override string ToString(){
      return $"{Number:N2}";
    }                       
  }
}

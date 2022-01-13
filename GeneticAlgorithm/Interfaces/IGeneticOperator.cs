using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmNS {
  /// <summary>
  /// Defines the basic information of a genetic operator. 
  /// </summary>
  public interface IGeneticOperator {
    string Title { get; } // The title of the strategy
    //string Description { get;  } // The descriptin of the strategy // Skal den med? 
  }
}

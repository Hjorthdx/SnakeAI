using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmNS {
  /// <summary>
  /// Represents a selection method that selects the agents using the Roulette wheel selection method
  /// </summary>
  public class RouletteWheelSelector : ISelector {

    public string Title { get; } = "RouletteWheelSelector";


    public List<Agent> MakeSelection(List<Agent> agents, int selectionSize) {

      List<Agent> selectedAgents = new List<Agent>();


      return selectedAgents;
    }
  }
}


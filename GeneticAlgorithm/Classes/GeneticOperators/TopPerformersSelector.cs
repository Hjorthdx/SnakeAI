using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmNS {
  /// <summary>
  /// Represents a selection method that selects only the top performers.
  /// </summary>
  public class TopPerformersSelector : ISelector {

    public string Title { get; } = "TopPerformersSelector";

    public List<Agent> MakeSelection(List<Agent> agents, int selectionSize) {
      List<Agent> selectedAgents = new List<Agent>();
      // Sort list descending by fitness 
      agents.Sort(); // pass sorter??
      // Fill list with top performers
      for(int i = 0; i < selectionSize; i++) {
        selectedAgents.Add(agents[i]);
      }
      return selectedAgents;
    }
  }
}

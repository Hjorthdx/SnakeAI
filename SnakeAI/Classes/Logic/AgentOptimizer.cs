using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithmNS;

namespace SnakeAI {
  // Dette er not it could have...
  //  Optimizes a the best snake by allowing the user to customize all genetic operators every step.
  // Makes it possible to go back to earlier population and other features.
  public class SnakeOptimizer {
    public List<Agent> AgentsToOptimize;
    public Population PopulationOptmized;

    public GeneticAlgorithm GA;

    public SnakeOptimizer(List<Agent> agents) {
      AgentsToOptimize = agents;

      //GA = new GeneticAlgorithm()
    }

    //Trying to

    // Makes population of the agent
    public void MakePopulation() {
      PopulationOptmized = new Population(AgentsToOptimize, 0);

    }

    // 
  }
}

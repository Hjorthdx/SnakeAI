using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmNS {
  /// <summary>
  /// Contains a list of the K best agents, where K is the number of agents in the list. 
  /// </summary>
  public class TopKAgents {
    public TopKAgent Best { get; private set; }
    public List<TopKAgent> BestAgents { get; private set; }  
    public double AverageFitness { get; private set; }
    private int K; // The number of agents in the list. 

    // For accessing the object directly by index
    public TopKAgent this[int i] {
      get { return BestAgents[i]; }
      private set { BestAgents[i] = value; }
    }

    public TopKAgents(int k) {
      K = k;
      BestAgents = new List<TopKAgent>();
    }
    /// <summary>
    ///  Updates the current TopKAgents with any new highscores in the passed agents. 
    /// </summary>
    public void Update(List<Agent> agents) {
      AddAnyTopAgents(agents);
      BestAgents.Sort();
      // If highscore list is filled, remove lowest score
      if(BestAgents.Count > K) {
        BestAgents.RemoveRange(K, BestAgents.Count - K);
      }
      // Calculate average
      AverageFitness = BestAgents.Average(b => b.Agent.Fitness);
      // Assign best
      Best = BestAgents.First();
    }
    /// <summary>
    ///  Goes through the passed agents and add agents who's fitness value exceeds the current top agents.
    /// </summary>
    private void AddAnyTopAgents(List<Agent> agents) {
      if(BestAgents.Count == 0) { // Check if list is empty
        BestAgents.Add(new TopKAgent(agents.First()));
      }
      for(int i = 0; i < agents.Count; i++) {
        // Check if new highscore
        if(agents[i].Fitness > BestAgents.Last().Agent.Fitness) {
          BestAgents.Add(new TopKAgent(agents[i].GetCopy()));
        }
      }
    }
  }

  public class TopKAgent : IComparable<TopKAgent>{ 
    public Agent Agent { get; }
    public TimeSpan TimeStamp { get; } // The time that the highscore was achieved.
    public bool IsNewScore { get; set; } // Is true for all new instances. Can be set to false from outside. 
    
    public TopKAgent(Agent agent) {
      Agent = agent;
      IsNewScore = true;
    }

    public int CompareTo(TopKAgent other) {
      return this.Agent.CompareTo(other.Agent); // Uses CompareTo in Agent class. 
    }
  }
}

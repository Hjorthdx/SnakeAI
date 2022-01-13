using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmNS {

  // Calculates the similarity of the agents in the population
  public class ConvergenceCalculator { 
    private List<Agent> agents;

    public int counter;

    public ConvergenceCalculator(List<Agent> agents) {
      this.agents = agents;
      counter = 0;
    }

    /// <summary>
    /// Calculates the convergence percent in the population. 
    /// </summary>
    /// <returns></returns>
    public double CalculateConvergence() {
      //Console.WriteLine("PRESS ENTER TO START CONVERGENCE TEST");

      //Console.ReadKey();

      //Console.WriteLine("Calculating!");

      double totaluniformGenesPercentAll = 0;
      double averageUniformGenesPercentAll = 0;

      // Test convergance for all agents
      for(int i = 0; i < agents.Count; i++) {
        //Console.WriteLine("Checking agent" + i);
        // Add this agents uniform gene percent to total. 
        totaluniformGenesPercentAll += TestAgent(agents[i]);
        //Console.WriteLine("done checking agent " + i);
      }

      // Calculate the how many genes the agents share with eachother on average. 
      averageUniformGenesPercentAll = totaluniformGenesPercentAll / agents.Count;

      //Console.WriteLine("Done :)");
      //Console.WriteLine("Average uniform genes for all agents: " + averageUniformGenesPercentAll);
      //Console.WriteLine(counter);
      //Console.ReadKey();
      return averageUniformGenesPercentAll;
    }

    // Test agent against other agents
    private double TestAgent(Agent agent) {
      double averageUniformGenesPercent = 0;
      double totaluniformGenesPercent = 0; // Genematch? Holde det til heltal??
      int uniformGeneCount = 0;

      double uniformGenesPercent = 0;

      for(int i = 0; i < agents.Count; i++) {
        //  Console.WriteLine("Comparing to agent" + i);
        //Console.ReadKey();
        // If the agent is not itself, compare genes with agent. 
        if(agent != agents[i]) {
          //Console.WriteLine("Agents not the same");
          //Console.ReadKey();

          // Calculate how many genes the agent shares with this agent. 
          uniformGeneCount = GetUniformGenesCount(agent, agents[i], 0);
          uniformGenesPercent = ((double)uniformGeneCount / agent.Chromosome.Genes.Length) * 100; // Get percentage();
          //Console.WriteLine("the two agents share percent genes: " + uniformGenesPercent);
          totaluniformGenesPercent += uniformGenesPercent;
        }
      }
      // Calculate how many genes the agent shares with other agents on average. 
      averageUniformGenesPercent = totaluniformGenesPercent / (agents.Count-1); // -1 as should not unclude itself

      return averageUniformGenesPercent;
    }

    // Returns how many uniform genes the agents share
    private int GetUniformGenesCount(Agent agent1, Agent agent2, double range) { // Range skal implementeres
      int uniformGeneCount = 0;

      Gene[] genes1 = agent1.Chromosome.Genes;
      Gene[] genes2 = agent2.Chromosome.Genes;
      int geneCount = genes1.Length;

      for(int i = 0; i < geneCount; i++) {
        counter++;

        //Console.WriteLine("Comparing genes at index" + i);
        //Console.WriteLine(genes1[i].Number);
        //Console.WriteLine(genes2[i].Number);
        //Console.ReadKey();

        if(genes1[i].Number == genes2[i].Number) {
          //Console.WriteLine("Genes are NOT the same!");
          //Console.ReadKey();
          uniformGeneCount++;
        }
      }

      //Console.WriteLine("uniform gene count this comparing: " + uniformGeneCount);
      //Console.ReadKey();

      return uniformGeneCount;
    }
  }
}

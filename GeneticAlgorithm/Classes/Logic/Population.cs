﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmNS {
  /// <summary>
  /// Contains a population of agents and methods for for calculating fitness, selection, crossover and mutation 
  /// that can be invoked on the population.
  /// </summary>
  [Serializable]
  public class Population {

    public List<Agent> Agents { get; private set; } 
    public double AverageFitness { get; private set; } 
    private bool fitnessCalculated; // True when fitness has been calculated. 

    // Constructor makes a new random population
    public Population(int populationSize, int geneCount, int generationNumber, IRandomNumberGenerator random) {
      // Make population of agents
      MakeRandomAgents(populationSize, geneCount, generationNumber, random);
      fitnessCalculated = false;
    }

    // Constructor makes a population of provided agents
    public Population(List<Agent> agents, int generationNumber) {                
      this.Agents = agents; // assignes først til sidst? 

      // Make population of agents
      for(int i = 0; i < agents.Count; i++) {
        // If agent not from earlier generation, assign generationID
        if(Agents[i].GenerationNumber == -1) {
          Agents[i].GenerationNumber = generationNumber;
        }
        Agents[i].NumberID = i;
      }
    }

    private void MakeRandomAgents(int populationSize, int geneCount, int generationNumber, IRandomNumberGenerator random) {
      Agents = new List<Agent>();
      for(int i = 0; i < populationSize; i++) {
        Agents.Add(new Agent(i, geneCount, generationNumber, random));
      }
    }

    /// <summary>
    /// Invokes the calculate fitness method on all agents in the population.
    /// </summary>
    public void CalculateFitness(IFitnessCalculator fitnessCalculator) {
      double totalFitness = 0;
      
      for(int i = 0; i < Agents.Count; i++) {
        Agents[i].CalculateFitness(fitnessCalculator);
        totalFitness += Agents[i].Fitness;
      }

      fitnessCalculated = true;
      AverageFitness = CalculateAverageFitness(totalFitness);
    }

    private double CalculateAverageFitness(double totalFitness) {
      double averageFitness = 0;
      if(Agents.Count != 0) {
        averageFitness = totalFitness / Agents.Count();
      }
      return averageFitness;
    }

      /// <summary>
      /// Replaces the current agents in the population with the selected agents. 
      /// </summary>
      /// <param name="selector">The selection method that defines how the selection should be perfomed.</param>
      /// <param name="selectionSize">The desired size of the selected agents.</param>
    public void MakeSelection(ISelector selector, int selectionSize) {
      if(!fitnessCalculated) {
        throw new Exception("Fitness must be calculated selection can be performed.");
      }
      Agents = selector.MakeSelection(Agents, selectionSize); // lav til void
    }

    /// <summary>
    /// Invokes the mutation method on all agents in the population.
    /// </summary>
    /// <param name="mutator">The mutation method that defines how the population should be mutated.</param>
    /// <param name="mutationProbalityAgent">The probability for an agent to be mutated.</param>
    /// <param name="mutationProbilityGene">The probability for a gene to be mutated.</param>
    /// <param name="random">The random generator that produces random numbers used in the method to determine 
    /// if an agent or a gene should be mutated.</param>
    public void MakeMutations(IMutator mutator, double mutationProbalityAgent, double mutationProbilityGene, IRandomNumberGenerator random) {

      // Mutate all agents in population
      for(int i = 0; i < Agents.Count; i++) {
        // Check if agent should be mutated
        double randomDouble = random.GetDouble(0, 1);
        if(randomDouble < mutationProbalityAgent) {
          Agents[i].Mutate(mutator, mutationProbilityGene, random);
        }
      }
    }


    /// <summary>
    /// Crosses the agents in population and returns the children. 
    /// </summary>
    /// <param name="crossover"></param>
    /// <param name="populationSize"></param>
    /// <param name="random"></param>
    /// <returns></returns>
    public List<Agent> MakeCrossovers(ICrossover crossover, int populationSize, IRandomNumberGenerator random) {
      List<Agent> children = new List<Agent>();

      children = crossover.MakeCrossovers(Agents, populationSize, random);

      // Check if list is filled // Skal laves i unit test
      if(children.Count != populationSize) {
        throw new ArgumentException("Selected agents is not " + populationSize);
      }
      return children;
    }

    public double GetConvergencePercent() {
      ConvergenceCalculator convergenceCalculator = new ConvergenceCalculator(Agents);
      return convergenceCalculator.CalculateConvergence();
    }

    // Prints the popluation // Nødvendigt????? Skal ikke ske i console.
    public void Print() {
      foreach(var agent in Agents) {
        agent.Print();
        Console.Write("\n");
      }
    }
  }
}
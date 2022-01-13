using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkNS {

  // Settings passed to neural network
  [Serializable]
  public class NetworkSettings {

    public readonly int numberOfWeights;
    public readonly int numberOfInputNeurons;
    public readonly int[] hiddenLayerStructure; // Structure of the network, how many neurons in each hidden layer
    public readonly int numberOfOutputNeurons;
    public readonly int[] outputLabels;

    // Number of hidden layers?
    // Number of hidden neurons?

    public NetworkSettings(int numberOfInputNeurons, int[] hiddenLayerStructure, int[] outputLabels) {

      this.numberOfInputNeurons = numberOfInputNeurons;
      this.hiddenLayerStructure = hiddenLayerStructure;
      this.numberOfOutputNeurons = outputLabels.Length; 
      this.outputLabels = outputLabels; // skal hedde labels
      numberOfWeights = CalculateNumberOfWeights();
    }

    // Calculate number of weights based on size of network
    public int CalculateNumberOfWeights() {

      int totalNumberOfWeights = 0;

      // Make list of number of neurons in each layer
      List<int> totalNumberOfNeurons = new List<int>();
      totalNumberOfNeurons.Add(numberOfInputNeurons);
      totalNumberOfNeurons.AddRange(hiddenLayerStructure);
      totalNumberOfNeurons.Add(numberOfOutputNeurons);

      for (int i = 0; i < totalNumberOfNeurons.Count - 1; i++) {
        totalNumberOfNeurons[i]++;
      }

      int biasNeuronCount = 1;

      for(int i = 0; i < totalNumberOfNeurons.Count - 1; i++) { 
        // Check if 
        if(totalNumberOfNeurons[i + 1] == totalNumberOfNeurons.Last()) {
          biasNeuronCount = 0;
        }
        totalNumberOfWeights += totalNumberOfNeurons[i] * (totalNumberOfNeurons[i + 1] - biasNeuronCount);
      }
      return totalNumberOfWeights;
    }
  }
} 

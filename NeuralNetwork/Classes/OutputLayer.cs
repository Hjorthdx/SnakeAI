using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkNS {
  public class OutputLayer : Layer {

    public OutputLayer(int[] outputLabels) {
      Neurons = new List<Neuron>();
      
      for(int i = 0; i < outputLabels.Length; i++) {
        Neurons.Add(new OutputNeuron(outputLabels[i]));
      }
    }

    public OutputNeuron GetMaxOutputNeuron() {
      return Neurons.Max() as OutputNeuron;
    }
  }
}

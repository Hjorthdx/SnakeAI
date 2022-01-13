using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkNS {
  public class HiddenLayer : Layer {

    // Constructer, instantiates a hidden layer with a given number of neurons
    public HiddenLayer(int numberOfNeurons) {
      Neurons = new List<Neuron>();

      for(int i = 0; i < numberOfNeurons; i++) {
        Neurons.Add(new HiddenNeuron());
      }
      Neurons.Add(new BiasNeuron());
    }
  }
}

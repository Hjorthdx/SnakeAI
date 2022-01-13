using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkNS {
public class InputLayer : Layer {

    public InputLayer(int numberOfInputNeurons) {
      Neurons = new List<Neuron>();

      for (int i = 0; i < numberOfInputNeurons; i++) {
        Neurons.Add(new InputNeuron());
      }
      Neurons.Add(new BiasNeuron());
    }

    /// <summary>
    /// Assigns the given input values the input neurons of the layer. 
    /// </summary>
    /// <param name="inputValues"></param>
    public void Update(double[] inputValues) {

      for (int i = 0; i < inputValues.Length; i++) {
        Neurons[i].Output = inputValues[i];
      }
    }
  }
}

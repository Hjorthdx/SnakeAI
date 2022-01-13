using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkNS {
  public class Dendrite {
    private readonly double weight;
    public double Value { get; set; }

    // Constructer, instantiates a dendrite with a given weight
    public Dendrite (double weight) {
      this.weight = weight;
    }

    // Calculate dendrite value, given a neuron output
    public void CalculateValue(double neuronOutput){
      Value = weight * neuronOutput;
    }
  }
}

  using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkNS {
  public abstract class Neuron : IComparable<Neuron> {

    public List<Dendrite> Dendrites { get; set; }
    public double Output { get; set; }

    public Neuron() {
      this.Dendrites = new List<Dendrite>();
    }

    // Calculate output value of neuron
    public void CalculateOutput() {
      double dendriteSum = 0;
      double neuronValue;
      // Sum dendrite value for all dendrites in list
      foreach (Dendrite d in Dendrites) {
        dendriteSum += d.Value;
      }
      neuronValue = dendriteSum;
      this.Output = ActivationMethod(neuronValue);
    }

    // Activation method, ReLU function
    double ActivationMethod(double neuronValue) {

      if (neuronValue < 0){
        neuronValue = 0;
      }
      return neuronValue;
    }
    // CompareTo used for finding max output neuron
    public int CompareTo(Neuron other) {
      return this.Output.CompareTo(other.Output);
    }
  }
}

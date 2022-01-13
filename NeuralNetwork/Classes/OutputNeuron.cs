using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkNS {
  public class OutputNeuron : Neuron {
    public int Action { get; set; }
    public OutputNeuron(int action) : base() {
      this.Action = action; // Label??
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace GeneticAlgorithmNS {
  class GeneticButtonGUI : Button {
    public GeneticButtonGUI(Point location, Size size, Font font, EventHandler eventHandler) {
      Font = font;
      Location = location;
      Size = size;
      UseVisualStyleBackColor = true;
      Click += eventHandler;
    }
  }
}

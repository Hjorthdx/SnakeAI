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
  class GeneticLabelGUI : Label {
    public GeneticLabelGUI(Point location, Size size, Font font) {

      Location = location;
      Size = size;
      Font = font;

      AutoSize = true;
    }
  }
}

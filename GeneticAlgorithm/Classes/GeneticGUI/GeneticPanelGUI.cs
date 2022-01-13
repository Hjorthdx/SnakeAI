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
  class GeneticPanelGUI : Panel {

    public GeneticPanelGUI(Point location, Size size) {
      BackColor = Color.White;
      BorderStyle = BorderStyle.FixedSingle;
      Location = location;
      Size = size;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameNS {
  public class Food : Field {

    public Food(Point point) : base (point) {
    }

    public override string ToString() {
      //return "F";
      return $"{(char)184}";
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameNS {
  public class Empty : Field {
    public Empty(Point point) : base(point) { }

    public override string ToString() {
      return " ";
    }

  }
}

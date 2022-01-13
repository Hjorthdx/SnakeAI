using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameNS {
  public class Wall : Obstacle {

    public Wall(Point point) : base(point) {
    }

    public override string ToString() {
      return $"{(char)219}";
      //return "O";
    }
  }
}

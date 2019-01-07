using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Engine
{
    class ModulePosition
    {
        Module tempModule;
        float xShift;
        float yShift;
        Direction direction;
        bool available;

        public bool Available { get { return available; } set { available = value; } }
        public Direction Direction { get { return direction; } }
        public Module TempModule { get { return available?tempModule:null; } set { tempModule = value; } }
        public float XShift { get { return xShift; } }
        public float YShift { get { return yShift; } }

        public ModulePosition (float xShift, float yShift, Direction direction)
        {
            this.available = true;
            this.xShift = xShift;
            this.yShift = yShift;
            this.direction = direction;
        }
    }
}

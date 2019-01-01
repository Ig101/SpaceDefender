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

        public Direction Direction { get { return direction; } }
        public Module TempModule { get { return tempModule; } set { tempModule = value; } }
        public float XShift { get { return xShift; } }
        public float YShift { get { return yShift; } }

        public ModulePosition (float xShift, float yShift, Direction direction)
        {
            this.xShift = xShift;
            this.yShift = yShift;
            this.direction = direction;
        }
    }
}

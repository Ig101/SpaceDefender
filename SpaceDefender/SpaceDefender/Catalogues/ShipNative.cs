using Game.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Catalogues
{
    class ShipNative
    {
        Ship ship;
        int minStage;
        int stageRange;
        int shipClass;

        public Ship Ship { get { return ship; } }
        public int MinStage { get { return minStage; } }
        public int StageRange { get { return stageRange; } }
        public int ShipClass { get { return shipClass; } }

        public ShipNative(Ship ship, int minStage, int stageRange, int shipClass)
        {
            this.ship = ship;
            this.minStage = minStage;
            this.stageRange = stageRange;
            this.shipClass = shipClass;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Catalogues
{
    class Catalogue
    {
        List<ShipNative> shipNatives = new List<ShipNative>();

        public List<ShipNative> ShipNatives { get { return shipNatives; } }

        public Catalogue()
        {

        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Catalogues
{
    class Catalogue
    {
        Hashtable shipNatives = new Hashtable();
        Hashtable moduleNatives = new Hashtable();

        public Hashtable ShipNatives { get { return shipNatives; } }
        public Hashtable ModuleNatives { get { return moduleNatives; } }

        public Catalogue()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Engine
{
    class Scene
    {
        Ship playerShip;

        List<Ship> ships;
        List<Missle> missles;
        List<SpecEffect> effects;
        List<Star> stars;

        public Ship PlayerShip { get { return playerShip; } }
        public List<Ship> Ships { get { return ships; } }
        public List<Missle> Missles { get { return missles; } }
        public List<SpecEffect> Effects { get { return effects; } }
        public List<Star> Stars { get { return stars; } }

        public void Update(float milliseconds)
        {

        }
    }
}

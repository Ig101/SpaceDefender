using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Engine
{
    class Star: GameObject
    {
        public Star(Scene parent, float x, float y, Sprite sprite)
            :base (parent, x,y, sprite)
        {

        }

        public override void Update(float milliseconds)
        {
            this.Y += Parent.PlayerShip.Speed;
        }
    }
}

﻿using Microsoft.Xna.Framework;
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

        public void Update(float milliseconds)
        {
            this.Y += (Parent.PlayerShip.Speed)/2*milliseconds/1000;
            if (this.Y > 750)
            {
                this.Y -= 1500;
                this.X = (float)Parent.GlobalRandom.NextDouble() * 2000f - 1000f;
            }
        }
    }
}

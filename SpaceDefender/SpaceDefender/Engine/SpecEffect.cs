using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Engine
{
    class SpecEffect:GameObject
    {
        float duration;

        public float Duration { get { return duration; } }

        public SpecEffect(Scene parent, float x, float y, Sprite sprite, float duration)
            :base(parent, x, y, sprite)
        {
            this.duration = duration;
        }

        public override void Update(float milliseconds)
        {
            this.duration -= milliseconds / 1000;
            base.Update(milliseconds);
            if (duration <= 0) IsAlive = false;
        }
    }
}

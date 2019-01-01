using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Engine
{
    class Sprite
    {
        string spriteName;
        int maxFrame;
        float frame;
        float size;
        int maxAnimation;
        int animation;
        Color color;

        public Color Color { get { return color; } set { color = value; } }
        public int Animation { get { return animation; } }
        public int MaxAnimation { get { return maxAnimation; } }
        public float Size { get { return size; } set { size = value; } }
        public string SpriteName { get { return spriteName; } }
        public int MaxFrame { get { return maxFrame; } }
        public float Frame { get { return frame; } }

        public Sprite(string spriteName, int maxFrame, float size, int maxAnimation, Color color)
        {
            this.size = size;
            this.spriteName = spriteName;
            this.maxFrame = maxFrame;
            this.maxAnimation = maxAnimation;
            frame = 0;
            animation = 0;
            this.color = color;
        }

        public void Update(float milliseconds)
        {
            this.frame += milliseconds / 15;
            if (frame>=maxFrame)
            {
                frame -= maxFrame;
            }
        }
    }
}

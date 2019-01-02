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
        int width;
        int height;

        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }
        public Color Color { get { return color; } set { color = value; } }
        public int Animation { get { return animation; } set { animation = value; } }
        public int MaxAnimation { get { return maxAnimation; } }
        public float Size { get { return size; } set { size = value; } }
        public string SpriteName { get { return spriteName; } }
        public int MaxFrame { get { return maxFrame; } }
        public float Frame { get { return frame; } }

        public int AbsoluteWidth { get { return (int)(width * size); } }
        public int AbsoluteHeight { get { return (int)(height * size); } }

        public Sprite(string spriteName, int width, int height, int maxFrame, float size, int maxAnimation, Color color)
        {
            this.width = width;
            this.height = height;
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
            this.frame += milliseconds / 1000 * 30;
            if (frame>=maxFrame)
            {
                frame -= maxFrame;
            }
        }
    }
}

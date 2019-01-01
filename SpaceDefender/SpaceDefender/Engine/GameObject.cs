using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Engine
{
    abstract class GameObject
    {
        float x;
        float y;
        bool isAlive;
        Scene parent;
        Sprite sprite;

        public Sprite Sprite { get { return sprite; } }
        public Scene Parent { get { return parent; } }
        public bool IsAlive { get { return isAlive; } set { isAlive = value; } }
        public float X { get { return x; } set { x = value; } }
        public float Y { get { return y; } set { y = value; } }

        public GameObject (Scene parent, float x, float y, Sprite sprite)
        {
            this.parent = parent;
            this.isAlive = true;
            this.x = x;
            this.y = y;
            this.sprite = sprite;
        }

        public virtual void Update(float milliseconds)
        {
            sprite.Update(milliseconds);
        }
    }
}

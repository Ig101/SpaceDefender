using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Catalogues;

namespace Game.Engine
{
    class Scene
    {
        Ship playerShip;
        float score;

        List<Ship> ships = new List<Ship>();
        List<Missle> missles = new List<Missle>();
        List<SpecEffect> effects = new List<SpecEffect>();
        List<Star> stars = new List<Star>();

        Random globalRandom;
        int sideMinShift;
        int sideShiftRange;
        int topShiftRange;
        float stage;

        public Ship PlayerShip { get { return playerShip; } }
        public List<Ship> Ships { get { return ships; } }
        public List<Missle> Missles { get { return missles; } }
        public List<SpecEffect> Effects { get { return effects; } }
        public List<Star> Stars { get { return stars; } }

        Catalogue catalogue;

        public Scene (Catalogue catalogue)
        {
            this.catalogue = catalogue;
        }

        public void Update(float milliseconds)
        {
            for (int i = 0; i < missles.Count; i++)
            {
                if (missles[i].IsAlive)
                {
                    missles[i].Update(milliseconds);
                }
                if (missles[i].IsAlive)
                {
                    missles[i].Death?.Invoke(this, missles[i].X, missles[i].Y);
                    missles.RemoveAt(i);
                    i--;
                }
            }
            PlayerShip.TargetSpeed = 300;
            PlayerShip.Acceleration = 300;
            for (int i = 0; i<ships.Count;i++)
            {
                if (ships[i].IsAlive)
                {
                    ships[i].Update(milliseconds);
                }
                if (!ships[i].IsAlive || (ships[i].Speed == 0 && Math.Abs(playerShip.Y - ships[i].Y)>2000))
                {
                    ships[i].Death?.Invoke(this, ships[i].X, ships[i].Y);
                    ships.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < effects.Count; i++)
            {
                if (effects[i].IsAlive)
                {
                    effects[i].Update(milliseconds);
                }
                if (!effects[i].IsAlive)
                {
                    effects.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < stars.Count; i++)
            {
                if (stars[i].IsAlive)
                {
                    stars[i].Update(milliseconds);
                }
                if (!stars[i].IsAlive)
                {
                    stars.RemoveAt(i);
                    i--;
                }
            }
            score += playerShip.Speed*milliseconds/1000;
            StarsGeneration(milliseconds); 
            EnemyGeneration(milliseconds);
        }

        public void StarsGeneration(float milliseconds)
        {

        }

        public void EnemyGeneration(float milliseconds)
        {

        }

        public Ship CreateShip(string nativeName, int x, int y)
        {
            return null;
        }
    }
}

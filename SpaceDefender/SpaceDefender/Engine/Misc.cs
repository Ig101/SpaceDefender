using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Engine
{
    enum Direction { Left, Right };
    enum DamageType { Physical, Energy, Magic };

    delegate bool SceneAction(Scene scene, Module owner);
    delegate bool TargetSceneAction(Scene scene, Module target, Missle owner);
    delegate void MoveRule(Scene scene, Missle obj, float milliseconds);
    delegate void DeathEffect(Scene scene, float x, float y);
    delegate bool AssembleModule(Ship ship, int position);

    class Misc
    {

        public static float DistanceBetween(GameObject obj1, GameObject obj2)
        {
            return (float)Math.Sqrt((obj1.X - obj2.X) * (obj1.X - obj2.X) + (obj1.Y - obj2.Y) * (obj1.Y - obj2.Y));
        }
        public static float DistanceBetween(float x1, float y1, GameObject obj2)
        {
            return (float)Math.Sqrt((x1 - obj2.X) * (x1 - obj2.X) + (y1 - obj2.Y) * (y1 - obj2.Y));
        }
        public static float DistanceBetween(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }
        public static float DistanceBetween(int x1, int y1, int x2, int y2)
        {
            return (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        public static float AngleBetween(GameObject obj1, GameObject obj2)
        {
            return (float)Math.Atan2(obj2.Y - obj1.Y, obj2.X - obj1.X);
        }
        public static float AngleBetween(float x1, float y1, GameObject obj2)
        {
            return (float)Math.Atan2(obj2.Y - y1, obj2.X - x1);
        }
        public static float AngleBetween(GameObject obj1, float x2, float y2)
        {
            return (float)Math.Atan2(y2 - obj1.Y, x2 - obj1.X);
        }
        public static float AngleBetween(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Atan2(y2 - y1, x2 - x1);
        }
    }
}

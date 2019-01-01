using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Engine
{
    class Ship: GameObject
    {
        float speed;
        float acceleration;
        float targetSpeed;

        float height;
        int team;

        ModulePosition[] positions;
        Module engineModule;
        Module coreModule;
        DeathEffect death;

        float resources;

        public float Height { get { return height; } }
        public int Team { get { return team; } }
        public DeathEffect Death { get { return death; } }
        public float Resources { get { return resources; } set { resources = value; } }
        public Module EngineModule { get { return engineModule; } }
        public Module CoreModule { get { return coreModule; } }
        public float Speed { get { return speed; } }
        public float TargetSpeed { get { return targetSpeed; } set { targetSpeed = value; } }
        public float Acceleration { get { return acceleration; } set { acceleration = value; } }
        public ModulePosition[] Positions { get { return positions; } }

        public Ship(Scene parent, float x, float y, float speed, Sprite sprite, ModulePosition[] positions, Module engineModule, int enginePosition, 
            Module coreModule, int corePosition, DeathEffect death, float height, int team)
    :       base(parent, x, y, sprite)
        {
            this.height = height;
            this.team = team;
            this.death = death;
            this.speed = speed;
            this.positions = positions;
            this.coreModule = coreModule;
            this.engineModule = engineModule;
            AssembleModule(engineModule, enginePosition);
            AssembleModule(coreModule, corePosition);
        }

        public override void Update(float milliseconds)
        {
            if (!engineModule.Working)
            {
                targetSpeed = 0;
                acceleration = 40;
            }
            if (targetSpeed > speed)
            {
                speed += acceleration;
                if (speed > targetSpeed) speed = targetSpeed;
            }
            else if (targetSpeed < speed)
            {
                speed -= acceleration;
                if (speed < targetSpeed) speed = targetSpeed;
            }
            float absoluteSpeed = this.Speed - Parent.PlayerShip.Speed;
            this.Y -= absoluteSpeed * milliseconds / 1000;
            foreach(ModulePosition pos in positions)
            {
                if (pos.TempModule != null)
                {
                    pos.TempModule.Update(milliseconds);
                    if(!pos.TempModule.IsAlive)
                    {
                        pos.TempModule.Death?.Invoke(Parent, pos.TempModule.AbsoluteX, pos.TempModule.AbsoluteY);
                        pos.TempModule = null;
                    }
                }
            }
            if (!coreModule.IsAlive) this.IsAlive = false;
            base.Update(milliseconds);
        }

        public void AssembleModule (Module module, int position)
        {
            this.positions[position].TempModule = module;
            module.TempPosition = position;
        }
    }
}

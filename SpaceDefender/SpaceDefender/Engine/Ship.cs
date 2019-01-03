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
        float defaultSpeed;

        float height;
        int team;

        ModulePosition[] positions;
        int enginePosition;
        int corePosition;
        DeathEffect death;

        float resources;
        float resourceGeneration;
        Sprite engineFire;
        float engineX;
        float engineY;
        bool colorEngine;
        
        public bool ColorEngine { get { return colorEngine; } }
        public Sprite EngineFire { get { return engineFire; } }
        public float EngineX { get { return engineX; } }
        public float EngineY { get { return engineY; } }
        public float DefaultSpeed { get { return defaultSpeed; } }
        public float ResourceGeneration { get { return resourceGeneration; } }
        public int EnginePosition { get { return enginePosition; } }
        public int CorePosition { get { return corePosition; } }
        public float Height { get { return height; } }
        public int Team { get { return team; } }
        public DeathEffect Death { get { return death; } }
        public float Resources { get { return resources; } set { resources = value>99.9f?99.9f:value; } }
        public Module EngineModule { get { return positions[enginePosition].TempModule; } }
        public Module CoreModule { get { return positions[corePosition].TempModule; } }
        public float Speed { get { return speed; } }
        public float TargetSpeed { get { return targetSpeed; } set { targetSpeed = value; } }
        public float Acceleration { get { return acceleration; } set { acceleration = value; } }
        public ModulePosition[] Positions { get { return positions; } }

        public Ship(Scene parent, float x, float y, float speed, Sprite sprite, ModulePosition[] positions, int enginePosition, 
            int corePosition, DeathEffect death, float height, int team, int resources, float resourceGeneration, Sprite engineFire, float engineX, float engineY,
            bool colorEngine)
    :       base(parent, x, y, sprite)
        {
            this.colorEngine = colorEngine;
            this.engineFire = engineFire;
            this.engineX = engineX;
            this.engineY = engineY;
            this.resourceGeneration = resourceGeneration;
            this.resources = resources;
            this.height = height;
            this.team = team;
            this.death = death;
            this.speed = speed;
            this.defaultSpeed = speed;
            this.positions = positions;
            this.corePosition = corePosition;
            this.enginePosition = enginePosition;
        }

        public override void Update(float milliseconds)
        {
            if (EngineModule==null || !EngineModule.Working)
            {
                targetSpeed = 0;
                acceleration = 400;
            }
            else
            {
                targetSpeed = defaultSpeed;
                acceleration = 1000;
            }
            Resources += resourceGeneration * milliseconds/1000;
            if (targetSpeed > speed)
            {
                speed += acceleration * milliseconds/1000;
                if (speed > targetSpeed) speed = targetSpeed;
            }
            else if (targetSpeed < speed)
            {
                speed -= acceleration * milliseconds/1000;
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
            if (CoreModule == null || !CoreModule.Working) this.IsAlive = false;
            base.Update(milliseconds);
        }

        public void AssembleModule (Module module, int position)
        {
            this.positions[position].TempModule = module;
            module.TempPosition = position;
        }
    }
}

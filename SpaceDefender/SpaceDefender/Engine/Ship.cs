﻿using Microsoft.Xna.Framework;
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

        ModulePosition[] positions;
        Module engineModule;
        Module coreModule;

        float resources;

        public float Resources { get { return resources; } set { resources = value; } }
        public Module EngineModule { get { return engineModule; } }
        public Module CoreModule { get { return coreModule; } }
        public float Speed { get { return speed; } }
        public float TargetSpeed { get { return targetSpeed; } set { targetSpeed = value; } }
        public float Acceleration { get { return acceleration; } set { acceleration = value; } }
        public ModulePosition[] Positions { get { return positions; } }

        public Ship(Scene parent, float x, float y, float speed, Sprite sprite, ModulePosition[] positions, Module engineModule, int enginePosition, 
            Module coreModule, int corePosition)
    :       base(parent, x, y, sprite)
        {
            this.speed = speed;
            this.positions = positions;
            this.coreModule = coreModule;
            this.engineModule = engineModule;
            AssembleModule(engineModule, enginePosition);
            AssembleModule(coreModule, corePosition);
        }

        public override void Update(float milliseconds)
        {
            if(targetSpeed>speed)
            {
                speed += acceleration;
                if (speed > targetSpeed) speed = targetSpeed;
            }
            else if(targetSpeed<speed)
            {
                speed -= acceleration;
                if (speed < targetSpeed) speed = targetSpeed;
            }
            float absoluteSpeed = this.Speed - Parent.PlayerShip.Speed;
            this.Y -= absoluteSpeed * milliseconds / 1000;
            foreach(ModulePosition pos in positions)
            {
                if (pos.TempModule != null) pos.TempModule.Update(milliseconds);
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

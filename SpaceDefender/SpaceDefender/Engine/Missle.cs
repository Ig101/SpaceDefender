using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Engine
{
    class Missle : GameObject
    {
        DeathEffect death;
        Ship owner;
        float angle;
        MoveRule moveRule;
        TargetSceneAction actionOnCollision;
        float strength;
        float speed;
        float lifeTime;
        float speedOnStart;

        public DeathEffect Death { get { return death; } }
        public float SpeedOnStart { get { return speedOnStart; } }
        public Ship Owner { get { return owner; } set { owner= value; } }
        public float LifeTime { get { return lifeTime; } set { lifeTime = value; } }
        public float Speed { get { return speed; } set { speed = value; } }
        public float Angle { get { return angle; } set { angle = value; } }
        public MoveRule MoveRule { get { return moveRule; } }
        public TargetSceneAction ActionOnCollision {  get { return actionOnCollision; } }
        public float Strength
        {
            get { return strength; }
            set { strength = value; }
        }

        public Missle (Scene parent, float x, float y, Sprite sprite, float lifeTime, float speed, float angle, MoveRule rule, 
            TargetSceneAction action, float strength, Ship owner, DeathEffect death)
            :base (parent, x,y, sprite)
        {
            this.death = death;
            this.speedOnStart = owner.Speed;
            this.owner = owner;
            this.lifeTime = lifeTime;
            this.speed = speed;
            this.angle = angle;
            this.moveRule = rule;
            this.actionOnCollision = action;
            this.strength = strength;
        }

        public override void Update(float milliseconds)
        {
            MoveRule(Parent, this, milliseconds);
            Module target = CollisionCheck();
            if (target != null)
            {
                ActionOnCollision(Parent, target, this);
                this.IsAlive = false;
            }
            else
            {
                this.lifeTime -= milliseconds;
                if (lifeTime <= 0)
                {
                    ActionOnCollision(Parent, null, this);
                    this.IsAlive = false;
                }
            }
            base.Update(milliseconds);
        }

        public Module CollisionCheck ()
        {
            foreach (Ship ship in Parent.Ships)
            {
                if (ship.Team != owner.Team)
                {
                    foreach (ModulePosition position in ship.Positions)
                    {
                        if (position.TempModule != null 
                            && this.X >= (position.TempModule.AbsoluteX - position.TempModule.Width / 2) 
                            && this.X <= (position.TempModule.AbsoluteX + position.TempModule.Width / 2)
                            && this.Y >= (position.TempModule.AbsoluteY - position.TempModule.Height / 2)
                            && this.Y <= (position.TempModule.AbsoluteY + position.TempModule.Height / 2))
                        {
                            return position.TempModule;
                        }
                    }
                }
            }
            return null;
        }
    }
}

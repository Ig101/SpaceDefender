using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Engine
{
    class Module
    {
        Ship parent;
        int tempPosition;

        bool isAlive;
        bool repairs;
        bool working;

        Sprite sprite;
        float health;
        int maxHealth;
        int cost;
        float[] defence;

        SceneAction action;
        float cooldown;
        float actionCost;
        float cooldownTimer;

        float width;
        float height;

        public int TempPosition { get { return tempPosition; } set { tempPosition = value; } }
        public float CooldownTimer { get { return cooldownTimer; } set { cooldownTimer = value; } }
        public Ship Parent { get { return parent; } }
        public float Cooldown { get { return cooldown; } }
        public float ActionCost { get { return actionCost; } }
        public bool Working { get { return working; } set { working = value; } }
        public bool IsAlive { get { return isAlive; } set { isAlive = value; } }
        public bool Repairs { get { return repairs; } }
        public float Width { get { return width; } }
        public float Height { get { return height; } }
        public SceneAction Action { get { return action; } }
        public float[] Defence { get { return defence; } }
        public Sprite Sprite { get { return sprite; } }
        public float Health { get { return health; } set { health = value; } }
        public int MaxHealth { get { return maxHealth; } }
        public int Cost { get { return cost; } }

        public Direction direction
        {
            get
            {
                return parent.Positions[tempPosition].Direction;
            }
        }
        public float absoluteX
        {
            get
            {
                return parent.X + parent.Positions[tempPosition].XShift;
            }
        }
        public float absoluteY
        {
            get
            {
                return parent.Y + parent.Positions[tempPosition].YShift;
            }
        }

        public Module(Ship parent, float width, float height, float cooldown, float actionCost, SceneAction action, float[] defence, Sprite sprite,
            int maxHealth, int cost)
        {
            this.parent = parent;
            this.width = width;
            this.height = height;
            this.cooldown = cooldown;
            this.actionCost = actionCost;
            this.action = action;
            this.defence = defence;
            this.sprite = sprite;
            this.maxHealth = maxHealth;
            this.health = maxHealth;
            this.cost = cost;
            this.cooldownTimer = cooldown * 5;
        }

        public void Update (float milliseconds)
        {
            cooldownTimer -= milliseconds;
            if(cooldownTimer<=0)
            {
                if (cost > parent.Resources)
                {
                    cooldownTimer = 0;
                }
                else
                {
                    parent.Resources -= cost;
                    cooldownTimer = cooldown;
                    Action(parent.Parent, this);
                }
            }
            if(repairs && !working)
            {
                health += milliseconds / 30000 * maxHealth;
                if(health>= maxHealth)
                {
                    health = maxHealth;
                    working = true;
                }
            }
        }

        public bool Damage (float amount, DamageType type)
        {
            this.health -= amount * defence[(int)type];
            if(health <= 0)
            {
                if(!repairs)
                {
                    isAlive = false;
                }
                else
                {
                    working = false;
                }
            }
            return isAlive;
        }

    }
}

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

        DeathEffect death;
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

        float damageTimer;

        public float DamageTimer { get { return damageTimer; } }
        public DeathEffect Death { get { return death; } }
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

        public Direction Direction
        {
            get
            {
                return parent.Positions[tempPosition].Direction;
            }
        }
        public float AbsoluteX
        {
            get
            {
                return parent.X + parent.Positions[tempPosition].XShift * parent.Sprite.Size;
            }
        }
        public float AbsoluteY
        {
            get
            {
                return parent.Y + parent.Positions[tempPosition].YShift * parent.Sprite.Size;
            }
        }

        public Module(Ship parent, float width, float height, float cooldown, float actionCost, SceneAction action, float[] defence, Sprite sprite,
            int maxHealth, int cost, DeathEffect death, bool repairs)
        {
            this.working = true;
            this.isAlive = true;
            this.repairs = repairs;
            this.death = death;
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
            this.cooldownTimer = cooldown;
        }

        public void Update (float milliseconds)
        {
            if (Action != null)
            {
                cooldownTimer -= milliseconds/1000;
                if (cooldownTimer <= 0)
                {
                    if (actionCost > parent.Resources ||  Parent.Parent.TempLevelManager.NextLevel)
                    {
                        cooldownTimer = 0;
                    }
                    else
                    {
                        
                        parent.Resources -= actionCost;
                        cooldownTimer = cooldown;
                        Action(parent.Parent, this);
                    }
                }
            }
            if(repairs && parent.CoreModule!=null && !working)
            {
                health += milliseconds / 30000 * maxHealth;
                if(health>= maxHealth / sprite.MaxAnimation)
                {
                    working = true;
                }
            }
            if(IsAlive) sprite.Animation = (int)((1-health / maxHealth) * sprite.MaxAnimation);
            if (damageTimer > 0)
            {
                damageTimer -= milliseconds;
            }
        }

        public bool Damage (float amount, DamageType type)
        {
            float damage = amount * defence[(int)type];
            if (damage > 0)
            {
                damageTimer = 100;
                this.health -= damage;
            }
            if (!repairs)
            {
                if (health <= 0)
                {
                    isAlive = false;
                }
            }
            else
            {
                if (health <= maxHealth/sprite.MaxAnimation)
                {
                    isAlive = true;
                    working = false;
                    this.health = Math.Max(0.00001f,health);
                }
            }
            return isAlive;
        }

    }
}

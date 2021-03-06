﻿using Game.Engine;
using Game.Progress;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Catalogues
{
    static class Delegates
    {
        public static bool BlasterAttack(Scene scene, Module owner)
        {
            bool direction = owner.Parent.Positions[owner.TempPosition].Direction == Direction.Right;
            scene.Missles.Add(new Missle(scene, owner.AbsoluteX + (owner.Width / 2.5f * (direction ? 1 : -1)),
                owner.AbsoluteY, new Sprite("blasterMissle", 16, 16, 1, 1, 1, Color.White), 2, 1500, direction ? 0 : MathHelper.Pi, DefaultMoveRule, DefaultEnergyDamage,
                1, owner.Parent, null));
            return true;
        }

        public static bool RocketAttack(Scene scene, Module owner)
        {
            bool direction = owner.Parent.Positions[owner.TempPosition].Direction == Direction.Right;
            scene.Missles.Add(new Missle(scene, owner.AbsoluteX + (owner.Width / 2f * (direction ? 1 : -1)),
                owner.AbsoluteY, new Sprite("rocketMissle", 16, 16, 1, 1, 1, Color.White), 2, 1200, direction ? 0 : MathHelper.Pi, DefaultMoveRule, DefaultPhysicalDamage,
                1, owner.Parent, null));
            return true;
        }

        public static bool AnnihilatorAttack(Scene scene, Module owner)
        {
            bool direction = owner.Parent.Positions[owner.TempPosition].Direction == Direction.Right;
            scene.Missles.Add(new Missle(scene, owner.AbsoluteX + (owner.Width / 10f * (direction ? 1 : -1)),
                owner.AbsoluteY, new Sprite("anniMissle", 16, 16, 1, 1, 1, Color.White), 2, 500, direction ? 0 : MathHelper.Pi, DefaultMoveRule, DefaultMagicDamage,
                5, owner.Parent, null));
            return true;
        }

        public static bool GenerateResource(Scene scene, Module owner)
        {
            if(owner.Parent.EngineModule.Working)
                owner.Parent.Resources += 0.1f;
            scene.Effects.Add(new SpecEffect(scene, owner.AbsoluteX, owner.AbsoluteY, new Sprite("genEffect", 64, 64, 9, 1, 1, Color.White), 0.28f, scene.GlobalRandom));
            return true;
        }

        public static void DefaultMoveRule(Scene scene, Missle obj, float milliseconds)
        {
            obj.X += obj.Speed * milliseconds / 1000 * (float)Math.Cos(obj.Angle);
            obj.Y += obj.Speed * milliseconds / 1000 * (float)Math.Sin(obj.Angle);
        }

        public static bool DefaultEnergyDamage(Scene scene, Module target, Missle owner)
        {
            if (target != null)
            {
                return target.Damage(owner.Strength, DamageType.Energy);
            }
            else
            {
                return false;
            }
        }

        public static bool DefaultPhysicalDamage(Scene scene, Module target, Missle owner)
        {
            if (target != null)
            {
                return target.Damage(owner.Strength, DamageType.Physical);
            }
            else
            {
                return false;
            }
        }

        public static bool DefaultMagicDamage(Scene scene, Module target, Missle owner)
        {
            if (target != null)
            {
                return target.Damage(owner.Strength, DamageType.Magic);
            }
            else
            {
                return false;
            }
        }

        public static void DefaultDeath(Scene scene, float x, float y)
        {
            scene.Effects.Add(new SpecEffect(scene, x, y, new Sprite("explosion", 96, 96, 18, 1, 1, Color.White),0.55f, scene.GlobalRandom));
        }


        public static bool AssembleBlasterModule(Ship ship, int position)
        {
            if (ship.Positions.Length <= position) return false;
            ship.AssembleModule(new Module(ship, 64, 64, 1f, 0, Delegates.BlasterAttack, new float[] { 1, 1, 1 },
                new Sprite("blaster", 64, 64, 1, 1, 1, Color.White), 1, 1, Delegates.DefaultDeath, false), position);
            return true;
        }
        public static bool AssembleRocketModule(Ship ship, int position)
        {
            if (ship.Positions.Length <= position) return false;
            ship.AssembleModule(new Module(ship, 64, 64, 1f, 0, Delegates.RocketAttack, new float[] { 1, 1, 1 },
                new Sprite("rocket", 64, 64, 1, 1, 1, Color.White), 1, 1, Delegates.DefaultDeath, false), position);
            return true;
        }
        public static bool AssembleAnniModule(Ship ship, int position)
        {
            if (ship.Positions.Length <= position) return false;
            ship.AssembleModule(new Module(ship, 64, 64, 2f, 0.2f, Delegates.AnnihilatorAttack, new float[] { 1, 1, 1 },
                new Sprite("annihilator", 64, 64, 1, 1, 1, Color.White), 1, 1, Delegates.DefaultDeath, false), position);
            return true;
        }
        public static bool AssembleGeneratorModule(Ship ship, int position)
        {
            if (ship.Positions.Length <= position) return false;
            ship.AssembleModule(new Module(ship, 64, 64, 1f, 0, Delegates.GenerateResource, new float[] { 1, 1, 1 },
                new Sprite("generator", 64, 64, 1, 1, 1, Color.White), 2, 1, Delegates.DefaultDeath, false), position);
            return true;
        }
        public static bool AssembleArmorModule(Ship ship, int position)
        {
            if (ship.Positions.Length <= position) return false;
            ship.AssembleModule(new Module(ship, 64, 64, 1f, 0, null, new float[] { 0.15f, 1, 1 },
                new Sprite("armor", 64, 64, 1, 1, 5, Color.White), 2, 1, null, false), position);
            return true;
        }
        public static bool AssembleShieldModule(Ship ship, int position)
        {
            if (ship.Positions.Length <= position) return false;
            ship.AssembleModule(new Module(ship, 64, 64, 1f, 0, null, new float[] { 1, 0.15f, 1 },
                new Sprite("shield", 64, 64, 1, 1, 5, Color.White), 2, 1, null, false), position);
            return true;
        }
        public static bool DemolishModule(Ship ship, int position)
        {
            if (ship.Positions.Length <= position) return false;
            ship.Positions[position].TempModule = null;
            return true;
        }


        public static bool EnemyBlasterAttack(Scene scene, Module owner)
        {
            bool direction = owner.Parent.Positions[owner.TempPosition].Direction == Direction.Right;
            if (owner.Parent.LifeTime > 60)
            {
                scene.Effects.Add(new SpecEffect(scene, owner.AbsoluteX, owner.AbsoluteY, new Sprite("genEffect", 64, 64, 9, 1, 1, Color.Black), 0.28f, scene.GlobalRandom));
                scene.Missles.Add(new Missle(scene, owner.AbsoluteX + (owner.Width / 2.5f * (direction ? 1 : -1)),
                    owner.AbsoluteY, new Sprite("enemyBlasterMissleEnrage", 32, 32, 1, 1, 1, Color.White), 2, 750, direction ? 0 : MathHelper.Pi, DefaultMoveRule, DefaultEnergyDamage,
                    10, owner.Parent, null));
            }
            else
            {
                scene.Missles.Add(new Missle(scene, owner.AbsoluteX + (owner.Width / 2.5f * (direction ? 1 : -1)),
                owner.AbsoluteY, new Sprite("enemyBlasterMissle", 32, 32, 1, 1, 1, Color.White), 2, 750, direction ? 0 : MathHelper.Pi, DefaultMoveRule, DefaultEnergyDamage,
                1, owner.Parent, null));
            }
            return true;
        }

        public static bool EnemyRocketAttack(Scene scene, Module owner)
        {
            bool direction = owner.Parent.Positions[owner.TempPosition].Direction == Direction.Right;
            if (owner.Parent.LifeTime > 60)
            {
                scene.Effects.Add(new SpecEffect(scene, owner.AbsoluteX, owner.AbsoluteY, new Sprite("genEffect", 64, 64, 9, 1, 1, Color.Black), 0.28f, scene.GlobalRandom));
                scene.Missles.Add(new Missle(scene, owner.AbsoluteX + (owner.Width / 2f * (direction ? 1 : -1)),
                    owner.AbsoluteY, new Sprite("enemyRocketMissleEnrage", 32, 32, 1, 1, 1, Color.White), 2, 500, direction ? 0 : MathHelper.Pi, DefaultMoveRule, DefaultPhysicalDamage,
                    10, owner.Parent, null));
            }
            else
            {
                scene.Missles.Add(new Missle(scene, owner.AbsoluteX + (owner.Width / 2f * (direction ? 1 : -1)),
                    owner.AbsoluteY, new Sprite("enemyRocketMissle", 32, 32, 1, 1, 1, Color.White), 2, 500, direction ? 0 : MathHelper.Pi, DefaultMoveRule, DefaultPhysicalDamage,
                    1, owner.Parent, null));
            }
            return true;
        }

        public static void SpawnDeath(Scene scene, float x, float y)
        {
            scene.Effects.Add(new SpecEffect(scene, x, y, new Sprite("spawnDeath", 128, 128, 6, 1, 1, Color.White), 0.19f, scene.GlobalRandom));
        }

        public static bool WinConditionKillEmAll (Level level, Scene tempScene)
        {
            if (level.MaxScore < tempScene.Score) return true;
            foreach(LevelEnemySpawn spawn in level.Spawns)
            {
                if (spawn.Enemy == null || spawn.Enemy.IsAlive) return false;
            }
            return true;
        }
    }
}

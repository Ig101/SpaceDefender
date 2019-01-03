using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Catalogues;
using Microsoft.Xna.Framework;

namespace Game.Engine
{
    class Scene
    {
        bool defeat;

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

        public Random GlobalRandom { get { return globalRandom; } }
        public bool Defeat { get { return defeat; } }
        public float Score { get { return score; } }
        public Ship PlayerShip { get { return playerShip; } }
        public List<Ship> Ships { get { return ships; } }
        public List<Missle> Missles { get { return missles; } }
        public List<SpecEffect> Effects { get { return effects; } }
        public List<Star> Stars { get { return stars; } }

        Catalogue catalogue;

        public Scene (Catalogue catalogue, int sideMinShift, int sideShiftRange, int topShiftRange)
        {
            this.defeat = false;
            this.catalogue = catalogue;
            this.sideMinShift = sideMinShift;
            this.sideShiftRange = sideShiftRange;
            this.topShiftRange = topShiftRange;
            this.stage = 0;
            this.score = 0;
            globalRandom = new Random();
            StarsGenerationFirst();
            playerShip = CreateShip("mothership", 0, 30, 0, 0);
            CreateShip("spawnBlasterRight", -400, 32, 1, 1);
            CreateShip("spawnRocketRight", -350, -150, 1, 1);
            CreateShip("spawnBlasterRight", -500, 200, 1, 1);
            CreateShip("spawnRocketRight", -600, -32, 1, 1);
        }

        public void Update(float milliseconds)
        {
            if (PlayerShip != null)
            {
                for (int i = 0; i < missles.Count; i++)
                {
                    if (missles[i].IsAlive)
                    {
                        missles[i].Update(milliseconds);
                    }
                    if (!missles[i].IsAlive)
                    {
                        missles[i].Death?.Invoke(this, missles[i].X, missles[i].Y);
                        missles.RemoveAt(i);
                        i--;
                    }
                }
                for (int i = 0; i < ships.Count; i++)
                {
                    if (ships[i].IsAlive)
                    {
                        ships[i].Update(milliseconds);
                        ships[i].EngineFire.Update(milliseconds);
                    }
                    if (!ships[i].IsAlive || (ships[i].Speed == 0 && Math.Abs(playerShip.Y - ships[i].Y) > 2000))
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
                score += playerShip.Speed * milliseconds / 1000;
                EnemyGeneration(milliseconds);
            }
        }

        public void StarsGenerationFirst()
        {
            for(int i = 0; i<globalRandom.Next(100,150);i++)
            {
                int variation = globalRandom.Next(100);
                Color color = variation <= 60 ? Color.White : variation <= 80 ? Color.Yellow : Color.AliceBlue;
                stars.Add(new Star(this, (float)globalRandom.NextDouble() * 2000f - 1000f, (float)globalRandom.NextDouble() * 1500f - 750f,
                    new Sprite("star", 8, 8, 1, (float)globalRandom.NextDouble()/2 + 0.5f, 1, color)));
            }
        }

        public void EnemyGeneration(float milliseconds)
        {

        }

        public Ship CreateShip(string nativeName, int x, int y, float height, int team)
        {
            ShipNative native = (ShipNative)catalogue.ShipNatives[nativeName];
            return CreateShip(native.Ship, x, y, height,team);
        }

        public Ship CreateShip(Ship native, int x, int y, float height, int team)
        {
            ModulePosition[] modules = new ModulePosition[native.Positions.Length];
            for(int i = 0; i<modules.Length;i++)
            {
                modules[i] = new ModulePosition(native.Positions[i].XShift, native.Positions[i].YShift, native.Positions[i].Direction);
            }
            Ship ship = new Ship(this, x, y, native.Speed,
                new Sprite(native.Sprite.SpriteName, native.Sprite.Width, native.Sprite.Height,
                native.Sprite.MaxFrame, native.Sprite.Size, native.Sprite.MaxAnimation, native.Sprite.Color),
                modules, native.EnginePosition, native.CorePosition, native.Death, height, team,(int)native.Resources, native.ResourceGeneration, native.EngineFire, native.EngineX, native.EngineY, native.ColorEngine);
            for (int i = 0; i < modules.Length; i++)
            {
                if (native.Positions[i].TempModule != null)
                {
                    modules[i].TempModule = new Module(ship, native.Positions[i].TempModule.Width, native.Positions[i].TempModule.Height,
                        native.Positions[i].TempModule.Cooldown, native.Positions[i].TempModule.ActionCost, native.Positions[i].TempModule.Action,
                        native.Positions[i].TempModule.Defence, native.Positions[i].TempModule.Sprite, native.Positions[i].TempModule.MaxHealth,
                        native.Positions[i].TempModule.Cost, native.Positions[i].TempModule.Death, native.Positions[i].TempModule.Repairs);
                    modules[i].TempModule.TempPosition = i;
                }
            }
            ships.Add(ship);
            return ship;
        }

        public bool AssembleBlasterModule(Ship ship, int position)
        {
            if (ship.Positions.Length <= position) return false;
            ship.AssembleModule(new Module(ship, 64, 64, 1f, 0, Delegates.BlasterAttack, new float[] { 1, 1, 1 },
                new Sprite("blaster", 64, 64, 1, 1, 1, Color.White), 1, 1, Delegates.DefaultDeath, false), position);
            return true;
        }
        public bool AssembleRocketModule(Ship ship, int position)
        {
            if (ship.Positions.Length <= position) return false;
            ship.AssembleModule(new Module(ship, 64, 64, 1f, 0, Delegates.RocketAttack, new float[] { 1, 1, 1 },
                new Sprite("rocket", 64, 64, 1, 1, 1, Color.White), 1, 1, Delegates.DefaultDeath, false), position);
            return true;
        }
        public bool AssembleAnniModule(Ship ship, int position)
        {
            if (ship.Positions.Length <= position) return false;
            ship.AssembleModule(new Module(ship, 64, 64, 2f, 0.2f, Delegates.AnnihilatorAttack, new float[] { 1, 1, 1 },
                new Sprite("annihilator", 64, 64, 1, 1, 1, Color.White), 1, 1, Delegates.DefaultDeath, false), position);
            return true;
        }
        public bool AssembleGeneratorModule(Ship ship, int position)
        {
            if (ship.Positions.Length <= position) return false;
            ship.AssembleModule(new Module(ship, 64, 64, 1f, 0, Delegates.GenerateResource, new float[] { 1, 1, 1 },
                new Sprite("generator", 64, 64, 1, 1, 1, Color.White), 2, 1, Delegates.DefaultDeath, false), position);
            return true;
        }
        public bool AssembleArmorModule(Ship ship, int position)
        {
            if (ship.Positions.Length <= position) return false;
            ship.AssembleModule(new Module(ship, 64, 64, 1f, 0, null, new float[] { 0.1f, 1, 1 },
                new Sprite("armor", 64, 64, 1, 1, 5, Color.White), 2, 1, null, false), position);
            return true;
        }
        public bool AssembleShieldModule(Ship ship, int position)
        {
            if (ship.Positions.Length <= position) return false;
            ship.AssembleModule(new Module(ship, 64, 64, 1f, 0, null, new float[] { 1, 0.1f, 1 },
                new Sprite("shield", 64, 64, 1, 1, 5, Color.White), 2, 1, null, false), position);
            return true;
        }
    }
}

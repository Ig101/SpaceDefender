using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Catalogues;
using Game.Progress;
using Microsoft.Xna.Framework;

namespace Game.Engine
{
    class Scene
    {
        bool defeat;
        float defeatTimer;
        float defeatCd;

        Ship playerShip;
        float score;

        float timerToEnd;

        List<Ship> ships = new List<Ship>();
        List<Missle> missles = new List<Missle>();
        List<SpecEffect> effects = new List<SpecEffect>();
        List<Star> stars = new List<Star>();

        Random globalRandom;
        float stage;

        public float TimerToEnd { get { return timerToEnd; } }
        public float DefeatTimer { get { return defeatTimer; } }
        public Random GlobalRandom { get { return globalRandom; } }
        public bool Defeat { get { return defeat; } }
        public float Score { get { return score; } set { score = value; } }
        public Ship PlayerShip { get { return playerShip; } }
        public List<Ship> Ships { get { return ships; } }
        public List<Missle> Missles { get { return missles; } }
        public List<SpecEffect> Effects { get { return effects; } }
        public List<Star> Stars { get { return stars; } }
        public Catalogue Catalogue { get { return catalogue; } }
        public float Stage { get { return stage; } set { stage = value; } }
        public LevelManager TempLevelManager { get { return tempLevelManager; } set { tempLevelManager = value; } }

        Catalogue catalogue;
        LevelManager tempLevelManager;

        public Scene (Catalogue catalogue, LevelManager manager)
        {
            manager.TempScene = this;
            this.tempLevelManager = manager;
            this.defeat = false;
            this.catalogue = catalogue;
            this.stage = 0;
            this.score = 0;
            globalRandom = new Random();
            StarsGenerationFirst();
            playerShip = manager.Levels[manager.TempLevelNumber+1].CreateMotherShip(this);
        }

        public void Update(float milliseconds)
        {
            if (PlayerShip != null)
            {
                tempLevelManager.Update(milliseconds);
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
                if (playerShip.EngineModule.Working)
                {
                    score += playerShip.Speed * milliseconds / 1000;
                }
                stage += milliseconds / 1000;
                if (timerToEnd > 0) timerToEnd -= milliseconds / 1000;
                if (defeat && defeatTimer > 0)
                {
                    playerShip.Sprite.Size = (Math.Min(10,defeatTimer))  / 10f;
                    if (playerShip.Sprite.Size < 1f)
                    {
                        playerShip.TargetSpeed = playerShip.Speed;
                    }
                    else
                    {
                        playerShip.TargetSpeed = 0;
                        playerShip.Acceleration = 600;
                    }
                    playerShip.EngineModule.Sprite.Size = playerShip.Sprite.Size;
                    defeatTimer -= milliseconds / 1000;
                    if (defeatCd > 0)
                    {
                        defeatCd -= milliseconds / 1000;
                    }
                    if (defeat && defeatCd <= 0)
                    {
                        for (int i = 0; i < 3 + (int)(GlobalRandom.NextDouble() * 7); i++)
                        {
                            ModulePosition position = playerShip.Positions[(int)(globalRandom.NextDouble() * playerShip.Positions.Length * 0.999f)];
                            Effects.Add(new SpecEffect(this, (position.XShift + playerShip.X) * playerShip.Sprite.Size, 
                                (position.YShift + playerShip.Y)*playerShip.Sprite.Size,
                                new Sprite("explosion", 96, 96, 18, playerShip.Sprite.Size, 1, Color.White), 0.55f, GlobalRandom));
                        }
                        defeatCd = 0.2f + (float)GlobalRandom.NextDouble() * 0.2f;
                    }
                    if(defeatTimer <=0)
                    {
                        playerShip.IsAlive = false;
                    }
                }
            }
        }

        public void IAdmitDefeat()
        {
            this.defeat = true;
            this.defeatTimer = 12;
            this.timerToEnd = 1;
        }

        public void Victory()
        {
            this.timerToEnd = 1;
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
                modules, native.EnginePosition, native.CorePosition, native.Death, height, team,(int)native.Resources, native.ResourceGeneration, 
                new Sprite(native.EngineFire.SpriteName, native.EngineFire.Width, native.EngineFire.Height,native.EngineFire.MaxFrame,
                native.EngineFire.Size,native.EngineFire.MaxAnimation,native.EngineFire.Color),
                native.EngineX, native.EngineY, native.ColorEngine);
            for (int i = 0; i < modules.Length; i++)
            {
                if (native.Positions[i].TempModule != null)
                {
                    modules[i].TempModule = new Module(ship, native.Positions[i].TempModule.Width, native.Positions[i].TempModule.Height,
                        native.Positions[i].TempModule.Cooldown, native.Positions[i].TempModule.ActionCost, native.Positions[i].TempModule.Action,
                        native.Positions[i].TempModule.Defence, 
                        new Sprite(native.Positions[i].TempModule.Sprite.SpriteName, native.Positions[i].TempModule.Sprite.Width,
                        native.Positions[i].TempModule.Sprite.Height,native.Positions[i].TempModule.Sprite.MaxFrame,
                        native.Positions[i].TempModule.Sprite.Size, native.Positions[i].TempModule.Sprite.MaxAnimation,
                        native.Positions[i].TempModule.Sprite.Color), native.Positions[i].TempModule.MaxHealth,
                        native.Positions[i].TempModule.Cost, native.Positions[i].TempModule.Death, native.Positions[i].TempModule.Repairs)
                    {
                        TempPosition = i
                    };
                }
            }
            ships.Add(ship);
            return ship;
        }
    }
}

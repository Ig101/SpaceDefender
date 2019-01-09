using Game.Catalogues;
using Game.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Progress
{
    class Level
    {
        /// <summary>
        /// 600 - maxY
        /// 350 - minY
        /// Sector starts from shipPosition +/- 350
        /// 350 is Sector
        /// 475 is Sector
        /// 600 is Sector
        /// </summary>
        /// Vertical has 0-6 sectors
        public const int sectorLength = 125;
        public const int sectorHeight = 66;
        public const int sectorBound = 600;
        public const int sectorMin = 350;
        public const int sectorVerticalStart = -295;
        public const int maxPositions = 6;

        bool[] availableModules;
        int levelPreference;

        LevelManager manager;
        float maxScore;

        int shipPosition;
        bool firstRow;

        List<LevelEnemySpawn> spawns = new List<LevelEnemySpawn>();
        List<LevelMissleSpawn> missles = new List<LevelMissleSpawn>();
        WinCondition winCondition;

        public bool FirstRow { get { return firstRow; } }
        public bool[] AvailableModules { get { return availableModules; } }
        public float MaxScore { get { return maxScore; } }
        public int ShipPosition { get { return shipPosition; } }
        public List<LevelEnemySpawn> Spawns { get { return spawns; } }
        public List<LevelMissleSpawn> Missles { get { return missles; } }
        public int LevelPreference { get { return levelPreference; } }
        public WinCondition WinCondition { get { return winCondition; } }

        public int SectorsCountLeft { get { return (Level.sectorBound - (Level.sectorMin - ShipPosition)) / Level.sectorLength + 1; } }
        public int SectorsCountRight { get { return (Level.sectorBound - (Level.sectorMin + ShipPosition)) / Level.sectorLength + 1; } }

        public Level (LevelManager manager, WinCondition winCondition, float maxScore, int shipPosition, int levelPreference, bool firstRow, 
            bool[] availableModules)
        {
            this.availableModules = availableModules;
            this.levelPreference = levelPreference;
            this.shipPosition = shipPosition;
            this.maxScore = maxScore;
            this.manager = manager;
            this.winCondition = winCondition;
            this.firstRow = firstRow;
        }

        public Level (LevelManager manager, Level native)
        {
            this.levelPreference = native.LevelPreference;
            this.shipPosition = native.ShipPosition;
            this.maxScore = native.MaxScore;
            this.manager = manager;
            this.winCondition = native.WinCondition;
            this.availableModules = native.AvailableModules;
            for(int i =0; i<native.Spawns.Count;i++)
            {
                this.Spawns.Add(new LevelEnemySpawn(this, native.Spawns[i].Stage, native.Spawns[i].ShipClass, native.Spawns[i].EnemyName,
                    native.Spawns[i].Direction, native.Spawns[i].Sector, native.Spawns[i].TargetPosition,native.Spawns[i].Dependencies));
            }
        }

        public Ship CreateMotherShip(Scene scene, float resources, string[] positions)
        {
            Ship ship = scene.CreateShip("mothership", shipPosition, 30, 0, 0);
            ship.Resources = resources;
            if (positions != null)
            {
                for (int i = 0; i < positions.Length; i++)
                {
                    string[] strs = positions[i].Split(new char[] { ':' });
                    switch (strs[0])
                    {
                        case "blaster":
                            Delegates.AssembleBlasterModule(ship, i);
                            break;
                        case "rocket":
                            Delegates.AssembleRocketModule(ship, i);
                            break;
                        case "annihilator":
                            Delegates.AssembleAnniModule(ship, i);
                            break;
                        case "generator":
                            Delegates.AssembleGeneratorModule(ship, i);
                            break;
                        case "armor":
                            Delegates.AssembleArmorModule(ship, i);
                            break;
                        case "shield":
                            Delegates.AssembleShieldModule(ship, i);
                            break;
                    }
                    if (ship.Positions[i].TempModule != null)
                    {
                        ship.Positions[i].TempModule.Health = float.Parse(strs[1]);
                        ship.Positions[i].TempModule.Update(1);
                    }
                }
            }
            return ship;
        }

        public void ChangeMothershipModules (Scene tempScene, Ship ship, bool effect)
        {
            for(int i  =0; i<availableModules.Length;i++)
            {
                if (effect)
                {
                    if (!ship.Positions[i].Available && availableModules[i])
                    {
                        tempScene.Effects.Add(new SpecEffect(tempScene, ship.Positions[i].XShift + ship.X, ship.Positions[i].YShift + ship.Y,
                            new Sprite("genEffect", 64, 64, 9, 1, 1, Color.White), 0.28f, tempScene.GlobalRandom));
                    }
                    if (ship.Positions[i].Available && !availableModules[i])
                    {
                        tempScene.Effects.Add(new SpecEffect(tempScene, ship.Positions[i].XShift + ship.X, ship.Positions[i].YShift + ship.Y,
                            new Sprite("explosion", 96, 96, 18, 1, 1, Color.White), 0.55f, tempScene.GlobalRandom));
                    }
                }
                ship.Positions[i].Available = availableModules[i];
            }
        }

        public void Update(float milliseconds, Scene scene)
        {
            foreach (LevelEnemySpawn spawn in spawns)
            {
                spawn.GenerateEnemy(scene);
                spawn.Update(milliseconds, scene);
            }
            if (manager.NextLevel == false && winCondition(this, scene))
            {
                scene.Victory();
                manager.NextLevel = true;
            }
        }
    }
}

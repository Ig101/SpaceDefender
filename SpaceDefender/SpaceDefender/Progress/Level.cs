using Game.Engine;
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

        int levelPreference;

        LevelManager manager;
        float maxScore;

        int shipPosition;

        List<LevelEnemySpawn> spawns = new List<LevelEnemySpawn>();
        List<LevelMissleSpawn> missles = new List<LevelMissleSpawn>();
        WinCondition winCondition;

        public float MaxScore { get { return maxScore; } }
        public int ShipPosition { get { return shipPosition; } }
        public List<LevelEnemySpawn> Spawns { get { return spawns; } }
        public List<LevelMissleSpawn> Missles { get { return missles; } }
        public int LevelPreference { get { return levelPreference; } }
        public WinCondition WinCondition { get { return winCondition; } }

        public int SectorsCountLeft { get { return (Level.sectorBound - (Level.sectorMin - ShipPosition)) / Level.sectorLength + 1; } }
        public int SectorsCountRight { get { return (Level.sectorBound - (Level.sectorMin + ShipPosition)) / Level.sectorLength + 1; } }

        public Level (LevelManager manager, WinCondition winCondition, float maxScore, int shipPosition, int levelPreference)
        {
            this.levelPreference = levelPreference;
            this.shipPosition = shipPosition;
            this.maxScore = maxScore;
            this.manager = manager;
            this.winCondition = winCondition;
        }

        public Level (LevelManager manager, Level native)
        {
            this.levelPreference = native.LevelPreference;
            this.shipPosition = native.ShipPosition;
            this.maxScore = native.MaxScore;
            this.manager = manager;
            this.winCondition = native.WinCondition;
            for(int i =0; i<native.Spawns.Count;i++)
            {
                this.Spawns.Add(new LevelEnemySpawn(this, native.Spawns[i].Stage, native.Spawns[i].ShipClass, native.Spawns[i].EnemyName,
                    native.Spawns[i].Direction, native.Spawns[i].Sector, native.Spawns[i].TargetPosition));
            }
        }

        public Ship CreateMotherShip (Scene scene)
        {
            return scene.CreateShip("mothership", shipPosition, 30, 0, 0);
        }

        public void Update(float milliseconds, Scene scene)
        {
            foreach (LevelEnemySpawn spawn in spawns)
            {
                spawn.GenerateEnemy(scene);
                spawn.Update(milliseconds, scene);
            }
            scene.Victory();
            manager.NextLevel = (!manager.NextLevel) ? winCondition(this, scene) : true;
        }
    }
}

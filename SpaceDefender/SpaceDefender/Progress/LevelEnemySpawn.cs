using Game.Catalogues;
using Game.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Progress
{
    class LevelEnemySpawn
    {

        Level level;
        float stage;
        string enemyName; //random if null
        Direction direction;
        int shipClass;

        int height;
        int width;
        int sector;
        int targetPosition;

        Ship enemy;
        int shift;
        bool billed;

        int[] dependencies;

        public int[] Dependencies { get { return dependencies; } }
        public bool Billed { get { return billed; } set { billed = true; } }
        public Ship Enemy { get { return enemy; } }
        public int TargetY {get{ return Level.sectorVerticalStart + Level.sectorHeight * (targetPosition) + shift; } }
        public int Shift { get { return shift; } }
        public int TargetPosition { get { return targetPosition; } }
        public int Height { get { return height; } }
        public int Width { get { return width; } }
        public int ShipClass { get { return shipClass; } }
        public Direction Direction { get { return direction; } }
        public string EnemyName { get { return enemyName; } }
        public float Stage { get { return stage; } }
        public int Sector { get { return sector; } }

        public LevelEnemySpawn (Level level, float stage, int shipClass, string enemyName, Direction direction, int sector, int targetPosition,
            int[] dependencies)
        {
            this.level = level;
            this.stage = stage;
            this.shipClass = shipClass;
            this.enemyName = enemyName;
            this.direction = direction;
            this.sector = sector;
            this.targetPosition = targetPosition;
            this.dependencies = dependencies;
        }

        public bool GenerateEnemy(Scene scene)
        {
            if (scene.Stage >= stage && enemy==null)
            {
                if(dependencies!=null)
                    foreach(int spawn in dependencies)
                    {
                        if (level.Spawns[spawn].Enemy == null || level.Spawns[spawn].Enemy.IsAlive) return false;
                    }
                ShipNative targetShip;
                if (enemyName != null)
                {
                    targetShip = (ShipNative)scene.Catalogue.ShipNatives[enemyName + (direction == Direction.Right ? "Right" : "Left")];
                }
                else
                {
                    List<ShipNative> suitableShips = new List<ShipNative>();
                    foreach (object obj in scene.Catalogue.ShipNatives.Values)
                    {
                        ShipNative native = (ShipNative)obj;
                        if (native.MinStage <= level.LevelPreference && native.MinStage + native.StageRange >= level.LevelPreference &&
                            native.ShipClass == shipClass && native.Dir == direction)
                        {
                            suitableShips.Add(native);
                        }
                    }
                    targetShip = suitableShips[(int)(scene.GlobalRandom.NextDouble() * suitableShips.Count)];
                }
                this.width = targetShip.SectorLength;
                this.height = targetShip.SectorWidth;
                int x;
                int y;
                int[] sectorsMinY = new int[direction == Direction.Left ? level.SectorsCountLeft : level.SectorsCountRight];
                bool[] holdedPositions = new bool[Level.maxPositions + 1];
                for(int i = 0; i<sectorsMinY.Length;i++)
                {
                    sectorsMinY[i] = level.FirstRow?-2:-1;
                }
                foreach(LevelEnemySpawn spawn in level.Spawns)
                {
                    if(spawn.enemy!=null && spawn.enemy.IsAlive && spawn.direction == direction )
                    {
                        if (spawn.shipClass > 2 || this.shipClass>2) return false;
                        if (sectorsMinY[spawn.sector] < spawn.targetPosition + spawn.height)
                        {
                            sectorsMinY[spawn.sector] = spawn.targetPosition + spawn.height;
                            holdedPositions[spawn.targetPosition] = true;
                        }
                    }
                }
                if (targetPosition > Level.maxPositions)
                    return false;
                if (sectorsMinY[sector] >= targetPosition - height)
                {
                    if(sectorsMinY[sector] >= Level.maxPositions - height)
                    {
                        int minValue = sectorsMinY[0];
                        int minI = 0;
                        for(int i = 1; i<sectorsMinY.Length;i++)
                        {
                            if(minValue>sectorsMinY[i])
                            {
                                minI = i;
                                minValue = sectorsMinY[i];
                            }
                        }
                        if(minValue >= Level.maxPositions - height)
                        {
                            return false;
                        }
                        else
                        {
                            sector = minI;
                            targetPosition = minValue + 1 + height;
                        }
                    }
                    else
                    {
                        targetPosition = sectorsMinY[sector] + 1 + height;
                    }
                }
                while (holdedPositions[targetPosition])
                {
                    targetPosition++;
                    if (targetPosition > Level.maxPositions)
                    {
                        targetPosition = 0;
                        return false;
                    }
                }
                x = level.ShipPosition + (Level.sectorMin + Level.sectorLength * sector)*(direction == Direction.Right?1:-1) + 
                    (int)(40*scene.GlobalRandom.NextDouble()-20);
                y = Level.sectorVerticalStart + Level.sectorHeight * targetPosition + 2000 + (int)(scene.GlobalRandom.NextDouble()*500);
                shift = (int)(16 * scene.GlobalRandom.NextDouble() - 8);
                enemy = scene.CreateShip(targetShip.Ship, x, y, (float)(sector + 1 + scene.GlobalRandom.NextDouble())*0.1f, 1);
                return true;
            }
            return false;
        }
        
        public void Update(float milliseconds, Scene scene)
        {
            if (this.enemy != null && this.enemy.IsAlive)
            {
                if (scene.PlayerShip.CoreModule == null)
                {
                    enemy.TargetSpeed = enemy.DefaultSpeed;
                    enemy.Acceleration = 1000;
                }
                else
                {
                    if (enemy.Y - TargetY <= 200)
                    {
                        enemy.TargetSpeed = scene.PlayerShip.Speed;
                        float s = enemy.Y - TargetY;
                        float dv = enemy.Speed - scene.PlayerShip.Speed;
                        float t = s / (enemy.Speed - dv / 2);
                        enemy.Acceleration = s / t;
                    }
                }
            }
        }
    }
}

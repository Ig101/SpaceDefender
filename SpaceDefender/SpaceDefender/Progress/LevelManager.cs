using Game.Catalogues;
using Game.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Progress
{
    delegate bool WinCondition(Level level, Scene tempScene);

    class LevelManager
    {
        Level[] levels;
        int tempLevel;
        Scene tempScene;
        bool nextLevel;
        float nextLevelTimer;
        float killedCount;

        public float NextLevelTimer { get { return nextLevelTimer; } set { nextLevelTimer = value; } }
        public float KilledCount { get { return killedCount; } set { killedCount = value; } }
        public Level TempLevel { get { return tempLevel>=0?levels[tempLevel]:null; } }
        public Level NextLevelEntity { get { return tempLevel+1 < levels.Length ? levels[tempLevel+1] : null; } }
        public Scene TempScene { get { return tempScene; } set { tempScene = value; } }
        public bool NextLevel { get { return nextLevel; } set { nextLevel = value; } }
        public Level[] Levels { get { return levels; } }
        public int TempLevelNumber { get { return tempLevel; } }

        public LevelManager (Catalogue catalogue)
        {
            GenerateLevels(catalogue);
            this.nextLevel = true;
            this.tempLevel = -1;
            this.nextLevelTimer = 0.0001f;
        }

        public void GenerateLevels (Catalogue catalogue)
        {
            tempLevel = 0;
            levels = new Level[10];
            levels[0] = new Level(this, Delegates.WinConditionKillEmAll,1500000,0,0);
            levels[0].Spawns.Add(new LevelEnemySpawn(levels[0], 0, 1, null, Direction.Left, 1, 1));
            levels[0].Spawns.Add(new LevelEnemySpawn(levels[0], 0, 1, null, Direction.Left, 0, 6));
            levels[0].Spawns.Add(new LevelEnemySpawn(levels[0], 0, 1, null, Direction.Left, 2, 4));
            levels[1] = new Level(this, Delegates.WinConditionKillEmAll,1500000,0,1);
            levels[1].Spawns.Add(new LevelEnemySpawn(levels[0], 0, 1, null, Direction.Right, 1, 1));
            levels[1].Spawns.Add(new LevelEnemySpawn(levels[0], 0, 1, null, Direction.Right, 0, 6));
            levels[1].Spawns.Add(new LevelEnemySpawn(levels[0], 0, 1, null, Direction.Right, 2, 4));
        }

        public void NextLevelStart ()
        {
            if(this.nextLevel)
            {
                nextLevel = false;
                this.tempLevel++;
                tempScene.Stage = 0;
                tempScene.Score = 0;
                tempScene.PlayerShip.Acceleration = 800;
                tempScene.PlayerShip.TargetSpeed = tempScene.PlayerShip.DefaultSpeed;
                this.nextLevelTimer = 0;
            }
        }

        public void Update(float milliseconds)
        {
            if (tempScene != null && !nextLevel)
            {
                levels[tempLevel].Update(milliseconds,tempScene);
            }
            if(tempScene!=null && nextLevel && nextLevelTimer>0)
            {
                nextLevelTimer -= milliseconds / 1000;
                if(nextLevelTimer<=0)
                {
                    NextLevelStart();
                }
            }
        }
    }
}

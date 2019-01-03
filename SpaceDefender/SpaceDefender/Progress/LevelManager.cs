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
        float killedCount;

        public float KilledCount { get { return killedCount; } set { killedCount = value; } }
        public Level TempLevel { get { return levels[tempLevel]; } }
        public Scene TempScene { get { return tempScene; } set { tempScene = value; } }
        public bool NextLevel { get { return nextLevel; } set { nextLevel = value; } }
        public Level[] Levels { get { return levels; } }
        public int TempLevelNumber { get { return tempLevel; } }

        public LevelManager (Catalogue catalogue)
        {
            GenerateLevels(catalogue);
        }

        public void GenerateLevels (Catalogue catalogue)
        {
            tempLevel = 0;
            levels = new Level[5];
            levels[0] = new Level(this, Delegates.WinConditionKillEmAll);
            levels[0].Spawns.Add(new LevelEnemySpawn(levels[0], 0, 1, null, Direction.Left, 1, 1));
            levels[0].Spawns.Add(new LevelEnemySpawn(levels[0], 0, 1, null, Direction.Left, 0, 6));
            levels[0].Spawns.Add(new LevelEnemySpawn(levels[0], 0, 1, null, Direction.Left, 2, 4));
            levels[1] = new Level(this, Delegates.WinConditionKillEmAll);
            levels[1].Spawns.Add(new LevelEnemySpawn(levels[0], 0, 1, null, Direction.Left, 1, 1));
            levels[1].Spawns.Add(new LevelEnemySpawn(levels[0], 0, 1, null, Direction.Left, 0, 6));
            levels[1].Spawns.Add(new LevelEnemySpawn(levels[0], 0, 1, null, Direction.Left, 2, 4));
        }

        public void Update(float milliseconds)
        {
            if (tempScene != null)
            {
                levels[tempLevel].Update(milliseconds,tempScene);
            }
        }
    }
}

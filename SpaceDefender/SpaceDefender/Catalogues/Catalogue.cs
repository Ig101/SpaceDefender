using Game.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Catalogues
{
    class Catalogue
    {
        Hashtable shipNatives = new Hashtable();
        Hashtable moduleNatives = new Hashtable();
        Hashtable levelNatives = new Hashtable();

        public Hashtable ShipNatives { get { return shipNatives; } }
        public Hashtable ModuleNatives { get { return moduleNatives; } }
        public Hashtable LevelNatives { get { return levelNatives; } }

        public Catalogue()
        {
            /////////////////////Levels
            
            /////////////////////Modules
            ModuleNatives.Add("motherCore", new Module(null, 36, 330, 1, 0, null, new float[] { 1, 1, 0 },
                new Engine.Sprite("motherCore", 64, 512, 1, 1, 5, Color.White), 100, 0, null, false));
            ModuleNatives.Add("motherEngine", new Module(null, 64, 64, 1, 0, null, new float[] { 1, 1, 1 },
                new Engine.Sprite("motherEngine", 64, 64, 1, 1, 5, Color.White), 10, 0, null, true));
            ModuleNatives.Add("enemyBlasterCore", new Module(null, 64, 64, 1, 0, Delegates.EnemyBlasterAttack, new float[] { 1, 1, 0 },
                new Engine.Sprite("enemyBlasterCore", 64, 64, 1, 1, 3, Color.White), 10, 0, null, false));
            ModuleNatives.Add("enemyRocketCore", new Module(null, 64, 64, 1, 0, Delegates.EnemyRocketAttack, new float[] { 1, 1, 0 },
                new Engine.Sprite("enemyRocketCore", 64, 64, 1, 1, 3, Color.White), 10, 0, null, false));
            ModuleNatives.Add("enemyCore", new Module(null, 64, 64, 1, 0, null, new float[] { 1, 1, 0 },
                new Engine.Sprite("enemyCore", 64, 64, 1, 1, 3, Color.White), 20, 0, null, false));
            /////////////////////Ships
            Ship ship = new Ship(null, 0, 0, 3000 / 2, new Engine.Sprite("mothership", 512, 512, 1, 1, 1, Color.White), new Engine.ModulePosition[36], 1, 0, null, 0, 0, 10, 0.2f,
                new Engine.Sprite("motherFire", 64, 64, 4, 1, 1, Color.White), 0, 259, false);
            ship.Positions[0] = new Engine.ModulePosition(0, 0, Direction.Right)
            {
                TempModule = (Module)ModuleNatives["motherCore"]
            };
            ship.Positions[1] = new Engine.ModulePosition(0, 404 / 2, Direction.Right)
            {
                TempModule = (Module)ModuleNatives["motherEngine"]
            };
            ship.Positions[2] = new Engine.ModulePosition(66 / 2, -386 / 2, Direction.Right);
            ship.Positions[3] = new Engine.ModulePosition(198 / 2, -386 / 2, Direction.Right);
            ship.Positions[4] = new Engine.ModulePosition(-66 / 2, -386 / 2, Direction.Left);
            ship.Positions[5] = new Engine.ModulePosition(-198 / 2, -386 / 2, Direction.Left);
            ship.Positions[6] = new Engine.ModulePosition(102 / 2, -254 / 2, Direction.Right);
            ship.Positions[7] = new Engine.ModulePosition(234 / 2, -254 / 2, Direction.Right);
            ship.Positions[7] = new Engine.ModulePosition(234 / 2, -254 / 2, Direction.Right);
            ship.Positions[8] = new Engine.ModulePosition(-102 / 2, -254 / 2, Direction.Left);
            ship.Positions[9] = new Engine.ModulePosition(-234 / 2, -254 / 2, Direction.Left);
            ship.Positions[10] = new Engine.ModulePosition(102 / 2, -122 / 2, Direction.Right);
            ship.Positions[11] = new Engine.ModulePosition(234 / 2, -122 / 2, Direction.Right);
            ship.Positions[12] = new Engine.ModulePosition(-102 / 2, -122 / 2, Direction.Left);
            ship.Positions[13] = new Engine.ModulePosition(-234 / 2, -122 / 2, Direction.Left);
            ship.Positions[14] = new Engine.ModulePosition(102 / 2, 10 / 2, Direction.Right);
            ship.Positions[15] = new Engine.ModulePosition(234 / 2, 10 / 2, Direction.Right);
            ship.Positions[16] = new Engine.ModulePosition(-102 / 2, 10 / 2, Direction.Left);
            ship.Positions[17] = new Engine.ModulePosition(-234 / 2, 10 / 2, Direction.Left);
            ship.Positions[18] = new Engine.ModulePosition(102 / 2, 142 / 2, Direction.Right);
            ship.Positions[19] = new Engine.ModulePosition(234 / 2, 142 / 2, Direction.Right);
            ship.Positions[20] = new Engine.ModulePosition(-102 / 2, 142 / 2, Direction.Left);
            ship.Positions[21] = new Engine.ModulePosition(-234 / 2, 142 / 2, Direction.Left);
            ship.Positions[22] = new Engine.ModulePosition(102 / 2, 274 / 2, Direction.Right);
            ship.Positions[23] = new Engine.ModulePosition(234 / 2, 274 / 2, Direction.Right);
            ship.Positions[24] = new Engine.ModulePosition(-102 / 2, 274 / 2, Direction.Left);
            ship.Positions[25] = new Engine.ModulePosition(-234 / 2, 274 / 2, Direction.Left);
            ship.Positions[26] = new Engine.ModulePosition(130 / 2, 406 / 2, Direction.Right);
            ship.Positions[27] = new Engine.ModulePosition(262 / 2, 406 / 2, Direction.Right);
            ship.Positions[28] = new Engine.ModulePosition(-130 / 2, 406 / 2, Direction.Left);
            ship.Positions[29] = new Engine.ModulePosition(-262 / 2, 406 / 2, Direction.Left);
            ship.Positions[30] = new Engine.ModulePosition(366 / 2, 10 / 2, Direction.Right);
            ship.Positions[31] = new Engine.ModulePosition(-366 / 2, 10 / 2, Direction.Left);
            ship.Positions[32] = new Engine.ModulePosition(366 / 2, 142 / 2, Direction.Right);
            ship.Positions[33] = new Engine.ModulePosition(-366 / 2, 142 / 2, Direction.Left);
            ship.Positions[34] = new Engine.ModulePosition(366 / 2, 274 / 2, Direction.Right);
            ship.Positions[35] = new Engine.ModulePosition(-366 / 2, 274 / 2, Direction.Left);
            ShipNatives.Add("mothership", new ShipNative(ship, 0, 0, 0,Direction.Left,0,0));

            ship = new Ship(null, 0, 0, 2000, new Engine.Sprite("spawn", 128, 128, 1, 1, 1, Color.White), new ModulePosition[]
            {
                new ModulePosition(0,0,Direction.Right)
            }, 0, 0, Delegates.SpawnDeath, 0, 0, 0, 0, new Engine.Sprite("spawnFire", 128, 64, 8, 1, 1, Color.White), -3, 67, true);
            ship.Positions[0].TempModule = (Module)ModuleNatives["enemyBlasterCore"];
            ShipNatives.Add("spawnBlasterLeft", new ShipNative(ship, 0, 3, 1,Direction.Left,0,1));
            ship = new Ship(null, 0, 0, 2000, new Engine.Sprite("spawn", 128, 128, 1, 1, 1, Color.White), new ModulePosition[]
                {
                new ModulePosition(0,0,Direction.Left)
                }, 0, 0, Delegates.SpawnDeath, 0, 0, 0, 0, new Engine.Sprite("spawnFire", 128, 64, 8, 1, 1, Color.White), -3, 67, true);
            ship.Positions[0].TempModule = (Module)ModuleNatives["enemyBlasterCore"];
            ShipNatives.Add("spawnBlasterRight", new ShipNative(ship, 0, 3, 1,Direction.Right,0,1));
            ship = new Ship(null, 0, 0, 2000, new Engine.Sprite("spawn", 128, 128, 1, 1, 1, Color.White), new ModulePosition[]
            {
                new ModulePosition(0,0,Direction.Right)
            }, 0, 0, Delegates.SpawnDeath, 0, 0, 0, 0, new Engine.Sprite("spawnFire", 128, 64, 8, 1, 1, Color.White), -3, 67, true);
            ship.Positions[0].TempModule = (Module)ModuleNatives["enemyRocketCore"];
            ShipNatives.Add("spawnRocketLeft", new ShipNative(ship, 0, 3, 1,Direction.Left,0,1));
            ship = new Ship(null, 0, 0, 2000, new Engine.Sprite("spawn", 128, 128, 1, 1, 1, Color.White), new ModulePosition[]
            {
                new ModulePosition(0,0,Direction.Left)
            }, 0, 0, Delegates.SpawnDeath, 0, 0, 0, 0, new Engine.Sprite("spawnFire", 128, 64, 8, 1, 1, Color.White), -3, 67, true);
            ship.Positions[0].TempModule = (Module)ModuleNatives["enemyRocketCore"];
            ShipNatives.Add("spawnRocketRight", new ShipNative(ship, 0, 3, 1,Direction.Right,0,1));
        }
    }
}

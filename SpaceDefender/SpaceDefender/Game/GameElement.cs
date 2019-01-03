using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMaker;
using Ignitus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Engine;
using Game.Catalogues;

namespace Game
{
    class GameElement:GameElementShell
    {
        Scene tempScene;
        Catalogue catalogue;
        GameOverlayElement overlay;

        public GameOverlayElement Overlay
        {
            set { overlay = value; }
            get { return overlay; }
        }
        public Scene TempScene { get { return tempScene; } }
                
        public GameElement(string fon)
            :base(fon)
        {
            catalogue = new Catalogue();
            /////////////////////Modules
            catalogue.ModuleNatives.Add("motherCore",new Module(null, 36, 330, 1, 0, null, new float[] { 1, 1, 0 },
                new Engine.Sprite("motherCore", 64, 512, 1, 1, 5, Color.White), 100, 0, null, true));
            catalogue.ModuleNatives.Add("motherEngine", new Module(null, 64, 64, 1, 0, null, new float[] { 1, 1, 1 },
                new Engine.Sprite("motherEngine", 64, 64, 1, 1, 5, Color.White), 50, 0, null, true));
            catalogue.ModuleNatives.Add("enemyBlasterCore", new Module(null, 64, 64, 1, 0, Delegates.EnemyBlasterAttack, new float[] { 1, 1, 0 },
                new Engine.Sprite("enemyBlasterCore", 64, 64, 1, 1, 3, Color.White), 10, 0, null, false));
            catalogue.ModuleNatives.Add("enemyRocketCore", new Module(null, 64, 64, 1, 0, Delegates.EnemyRocketAttack, new float[] { 1, 1, 0 },
                new Engine.Sprite("enemyRocketCore", 64, 64, 1, 1, 3, Color.White), 10, 0, null, false));
            catalogue.ModuleNatives.Add("enemyCore", new Module(null, 64, 64, 1, 0, null, new float[] { 1, 1, 0 },
                new Engine.Sprite("enemyCore", 64, 64, 1, 1, 3, Color.White), 20, 0, null, false));
            /////////////////////Ships
            Ship ship = new Ship(null, 0, 0, 3000/2, new Engine.Sprite("mothership",512,512,1,1,1, Color.White), new Engine.ModulePosition[36], 1, 0, null, 0, 0,10,0.2f,
                new Engine.Sprite("motherFire",64,64,4,1,1,Color.White),0,259,false);
            ship.Positions[0] = new Engine.ModulePosition(0, 0, Direction.Right);
            ship.Positions[0].TempModule = (Module)catalogue.ModuleNatives["motherCore"];
            ship.Positions[1] = new Engine.ModulePosition(0, 404/2, Direction.Right);
            ship.Positions[1].TempModule = (Module)catalogue.ModuleNatives["motherEngine"];
            ship.Positions[2] = new Engine.ModulePosition(66 / 2, -386/2, Direction.Right);
            ship.Positions[3] = new Engine.ModulePosition(198 / 2, -386/2, Direction.Right);
            ship.Positions[4] = new Engine.ModulePosition(-66 / 2, -386/2, Direction.Left);
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
            catalogue.ShipNatives.Add("mothership", new ShipNative(ship,0,0,0));

            ship = new Ship(null, 0, 0, 1500, new Engine.Sprite("spawn", 128, 128, 1, 1, 1, Color.White), new ModulePosition[]
            {
                new ModulePosition(0,0,Direction.Right)
            }, 0, 0, Delegates.SpawnDeath, 0, 0, 0, 0, new Engine.Sprite("spawnFire", 128, 64, 8, 1, 1, Color.White), -3, 67,true);
            ship.Positions[0].TempModule = (Module)catalogue.ModuleNatives["enemyBlasterCore"];
            catalogue.ShipNatives.Add("spawnBlasterRight", new ShipNative(ship,0,0,0));
            ship = new Ship(null, 0, 0, 1500, new Engine.Sprite("spawn", 128, 128, 1, 1, 1, Color.White), new ModulePosition[]
                {
                new ModulePosition(0,0,Direction.Left)
                }, 0, 0, Delegates.SpawnDeath, 0, 0, 0, 0, new Engine.Sprite("spawnFire", 128, 64, 8, 1, 1, Color.White), -3, 67,true);
            ship.Positions[0].TempModule = (Module)catalogue.ModuleNatives["enemyBlasterCore"];
            catalogue.ShipNatives.Add("spawnBlasterLeft", new ShipNative(ship, 0, 0, 0));
            ship = new Ship(null, 0, 0, 1500, new Engine.Sprite("spawn", 128, 128, 1, 1, 1, Color.White), new ModulePosition[]
            {
                new ModulePosition(0,0,Direction.Right)
            }, 0, 0, Delegates.SpawnDeath, 0, 0, 0, 0, new Engine.Sprite("spawnFire", 128, 64, 8, 1, 1, Color.White), -3, 67,true);
            ship.Positions[0].TempModule = (Module)catalogue.ModuleNatives["enemyRocketCore"];
            catalogue.ShipNatives.Add("spawnRocketRight", new ShipNative(ship, 0, 0, 0));
            ship = new Ship(null, 0, 0, 1500, new Engine.Sprite("spawn", 128, 128, 1, 1, 1, Color.White), new ModulePosition[]
            {
                new ModulePosition(0,0,Direction.Left)
            }, 0, 0, Delegates.SpawnDeath, 0, 0, 0, 0, new Engine.Sprite("spawnFire", 128, 64, 8, 1, 1, Color.White), -3, 67,true);
            ship.Positions[0].TempModule = (Module)catalogue.ModuleNatives["enemyRocketCore"];
            catalogue.ShipNatives.Add("spawnRocketLeft", new ShipNative(ship, 0, 0, 0));
        }

        protected override void DrawAll(IgnitusGame game, Color fonColor)
        {
            game.BeginDrawing(SpriteSortMode.FrontToBack, null);
            if (tempScene != null)
            {
                foreach (Ship ship in tempScene.Ships)
                {
                    if (ship.IsAlive)
                    {
                        game.DrawSprite(ship.Sprite.SpriteName,
                            new Rectangle((int)ship.X + 640, (int)ship.Y + 400, ship.Sprite.AbsoluteWidth, ship.Sprite.AbsoluteHeight),
                            new Rectangle(ship.Sprite.Width * (int)ship.Sprite.Frame, ship.Sprite.Height * ship.Sprite.Animation, ship.Sprite.Width, ship.Sprite.Height),
                            ship.CoreModule==null || ship.CoreModule.DamageTimer > 0 ? Color.Red: ship.Sprite.Color, 0, new Vector2(ship.Sprite.Width / 2, ship.Sprite.Height / 2), SpriteEffects.None, ship.Height);
                        if (ship.EngineModule != null && ship.EngineModule.Working)
                        {
                            game.DrawSprite(ship.EngineFire.SpriteName,
                                new Rectangle((int)(ship.X + ship.EngineX) + 640, (int)(ship.Y + ship.EngineY) + 400, ship.EngineFire.AbsoluteWidth, ship.EngineFire.AbsoluteHeight),
                                new Rectangle(ship.EngineFire.Width * (int)ship.EngineFire.Frame, ship.EngineFire.Height * ship.EngineFire.Animation, 
                                ship.EngineFire.Width, ship.EngineFire.Height),
                                (ship.CoreModule == null || ship.CoreModule.DamageTimer > 0) && ship.ColorEngine?Color.Red:ship.EngineFire.Color,
                                0, new Vector2(ship.EngineFire.Width / 2, ship.EngineFire.Height / 2), SpriteEffects.None, ship.Height-0.1f);
                        }
                        foreach (Engine.ModulePosition pos in ship.Positions)
                        {
                            if (pos.TempModule != null && pos.TempModule.IsAlive)
                            {
                                game.DrawSprite(pos.TempModule.Sprite.SpriteName,
                                    new Rectangle((int)pos.TempModule.AbsoluteX+640, (int)pos.TempModule.AbsoluteY+400,
                                    pos.TempModule.Sprite.AbsoluteWidth, pos.TempModule.Sprite.AbsoluteHeight),
                                    new Rectangle(pos.TempModule.Sprite.Width * (int)pos.TempModule.Sprite.Frame, pos.TempModule.Sprite.Height *
                                    pos.TempModule.Sprite.Animation, pos.TempModule.Sprite.Width, pos.TempModule.Sprite.Height),
                                    pos.TempModule.DamageTimer>0? Color.Red : pos.TempModule.Sprite.Color, 
                                    0, new Vector2(pos.TempModule.Sprite.Width / 2, pos.TempModule.Sprite.Height / 2),
                                    pos.Direction == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally, ship.Height + 2f);
                            }
                            if(pos.TempModule == null && ship == tempScene.PlayerShip)
                            {
                                game.DrawSprite("emptyModule",
                                    new Rectangle((int)(pos.XShift + ship.X) + 640, (int)(pos.YShift + ship.Y) + 400,
                                    64, 64),
                                    new Rectangle(0,0,64,64),
                                    Color.White,
                                    0, new Vector2(32, 32),
                                    SpriteEffects.None, ship.Height + 2f);
                            }
                        }
                    }
                }
                foreach (Missle missle in tempScene.Missles)
                {
                    if (missle.IsAlive)
                    {
                        game.DrawSprite(missle.Sprite.SpriteName,
                            new Rectangle((int)missle.X+640, (int)missle.Y+400, missle.Sprite.AbsoluteWidth, missle.Sprite.AbsoluteHeight),
                            new Rectangle(missle.Sprite.Width * (int)missle.Sprite.Frame, missle.Sprite.Height * missle.Sprite.Animation,
                            missle.Sprite.Width, missle.Sprite.Height),
                            missle.Sprite.Color, missle.Angle, new Vector2(missle.Sprite.Width / 2, missle.Sprite.Height / 2),
                            SpriteEffects.None, missle.Owner.Height + 4f);
                    }
                }
                foreach (SpecEffect effect in tempScene.Effects)
                {
                    if (effect.IsAlive)
                    {
                        game.DrawSprite(effect.Sprite.SpriteName,
                            new Rectangle((int)effect.X+640, (int)effect.Y+400, effect.Sprite.AbsoluteWidth, effect.Sprite.AbsoluteHeight),
                            new Rectangle(effect.Sprite.Width * (int)effect.Sprite.Frame, effect.Sprite.Height * effect.Sprite.Animation,
                            effect.Sprite.Width, effect.Sprite.Height),
                            effect.Sprite.Color, 0, new Vector2(effect.Sprite.Width / 2, effect.Sprite.Height / 2),
                            SpriteEffects.None, 10f+effect.Depth);
                    }
                }
                foreach (Star star in tempScene.Stars)
                {
                    if (star.IsAlive)
                    {
                        game.DrawSprite(star.Sprite.SpriteName,
                            new Rectangle((int)star.X+640, (int)star.Y+400, star.Sprite.AbsoluteWidth, star.Sprite.AbsoluteHeight),
                            new Rectangle(star.Sprite.Width * (int)star.Sprite.Frame, star.Sprite.Height * star.Sprite.Animation,
                            star.Sprite.Width, star.Sprite.Height),
                            star.Sprite.Color, 0, new Vector2(star.Sprite.Width / 2, star.Sprite.Height / 2),
                            SpriteEffects.None, -1f);
                    }
                }
            }
            game.EndDrawing();
            }

        protected override void UpdateAll(GameElementShell gameElement, ControlsState state, ControlsState prevState, float milliseconds)
        {
            if(tempScene!=null)
            {
                tempScene.Update(milliseconds);
            }
        }

        protected override void UpdateButtons(GameElementShell gameElement, ControlsState state, ControlsState prevState, float milliseconds)
        {
            
        }

        protected override void FirstTimeUpdate(GameElementShell gameElement)
        {
            tempScene = new Scene(catalogue,200,400,300);
        }

        void PlayerCommand (int keyIndex, bool state)
        {

        }
    }
}

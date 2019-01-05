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
using Game.Progress;

namespace Game
{
    class GameElement: GameElementShell
    {
        Scene tempScene;
        Catalogue catalogue;
        GameOverlayElement overlay;
        LevelManager tempLevelManager;

        public LevelManager TempManager { get { return tempLevelManager; } set { tempLevelManager = value; } }
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
                        if (ship.EngineModule != null && ship.CoreModule!=null)
                        {
                            game.DrawSprite(ship.EngineFire.SpriteName,
                                new Rectangle((int)(ship.X + ship.EngineX) + 640, (int)(ship.Y + ship.EngineY) + 
                                400, ship.EngineFire.AbsoluteWidth, ship.EngineFire.AbsoluteHeight),
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
                                    pos.TempModule.DamageTimer>0 || ship.CoreModule==null? Color.Red : pos.TempModule.Sprite.Color, 
                                    0, new Vector2(pos.TempModule.Sprite.Width / 2, pos.TempModule.Sprite.Height / 2),
                                    pos.Direction == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally, ship.Height + (ship==tempScene.PlayerShip?8f:2f));
                            }
                            if(pos.TempModule == null && ship == tempScene.PlayerShip && ship.CoreModule!= null && ship.CoreModule.IsAlive)
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
                            SpriteEffects.None, missle.Owner!=null?missle.Owner.Height + (missle.Owner == tempScene.PlayerShip ? 9f:4f):4f);
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

        protected override void UpdateAll(IgnitusGame game, GameElementShell gameElement, ControlsState state, ControlsState prevState, float milliseconds)
        {
            if(tempScene!=null)
            {
                tempScene.Update(milliseconds);
                if(tempScene.Defeat && tempScene.TimerToEnd<=0 && tempLevelManager!=null && tempLevelManager.TempLevelNumber>=0)
                {
                    foreach (LevelEnemySpawn spawn in tempLevelManager.TempLevel.Spawns)
                    {
                        if (spawn.Enemy != null && !spawn.Enemy.IsAlive && !spawn.Billed)
                        {
                            tempLevelManager.KilledCount++;
                            spawn.Billed = true;
                        }
                    }
                    ((LabelElement)((Mode)game.Modes["game_mode_result"]).Elements[2]).Text = game.Id2Str("killed") + " " + tempLevelManager.KilledCount;
                    ((LevelScaleElement)((Mode)game.Modes["game_mode_result"]).Elements[3]).Manager = tempLevelManager;
                    PrepareFrom(game, "game_mode_result");
                }
            }
            if(tempLevelManager!=null && tempLevelManager.NextLevel)
            {
                foreach(Ship ship in tempScene.Ships)
                {
                    if (ship == tempScene.PlayerShip)
                    {
                        ship.TargetSpeed = 900;
                        ship.Acceleration = 400;
                    }
                    else
                    {
                        ship.TargetSpeed = 0;
                        ship.Acceleration = 1000;
                    }
                }
                if (tempScene.PlayerShip.CoreModule.Health < tempScene.PlayerShip.CoreModule.MaxHealth)
                {
                    tempScene.PlayerShip.CoreModule.Health += tempScene.PlayerShip.CoreModule.MaxHealth * milliseconds / 5000;
                }
                else
                {
                    tempScene.PlayerShip.CoreModule.Health = tempScene.PlayerShip.CoreModule.MaxHealth;
                }
                if (tempScene.PlayerShip.EngineModule.Health < tempScene.PlayerShip.EngineModule.MaxHealth)
                {
                    tempScene.PlayerShip.EngineModule.Health += tempScene.PlayerShip.EngineModule.MaxHealth * milliseconds / 5000;
                }
                else
                {
                    tempScene.PlayerShip.EngineModule.Health = tempScene.PlayerShip.EngineModule.MaxHealth;
                }
                if(tempLevelManager.NextLevelEntity!=null)
                {
                    if(tempScene.PlayerShip.X > tempLevelManager.NextLevelEntity.ShipPosition)
                    {
                        tempScene.PlayerShip.X -= milliseconds;
                        if(tempScene.PlayerShip.X >= tempLevelManager.NextLevelEntity.ShipPosition)
                        {
                            tempScene.PlayerShip.X = tempLevelManager.NextLevelEntity.ShipPosition;
                        }
                    }
                    else if (tempScene.PlayerShip.X < tempLevelManager.NextLevelEntity.ShipPosition)
                    {
                        tempScene.PlayerShip.X += milliseconds;
                        if (tempScene.PlayerShip.X <= tempLevelManager.NextLevelEntity.ShipPosition)
                        {
                            tempScene.PlayerShip.X = tempLevelManager.NextLevelEntity.ShipPosition;
                        }
                    }
                }
                if (tempScene.TimerToEnd <=0 && tempLevelManager.TempLevelNumber>=0)
                {
                    foreach (LevelEnemySpawn spawn in tempLevelManager.TempLevel.Spawns)
                    {
                        if (spawn.Enemy != null && !spawn.Enemy.IsAlive && !spawn.Billed)
                        {
                            tempLevelManager.KilledCount++;
                            spawn.Billed = true;
                        }
                    }
                    if (tempLevelManager.NextLevelEntity==null)
                    {
                        tempScene.PlayerShip.Speed = 0;
                        tempScene.PlayerShip.TargetSpeed = 0;
                        tempScene.PlayerShip.Y -= (milliseconds / 1000) * tempScene.PlayerShip.DefaultSpeed;
                        ((LabelElement)((Mode)game.Modes["game_mode_victory"]).Elements[2]).Text = game.Id2Str("killed") + " " + tempLevelManager.KilledCount;
                        PrepareFrom(game, "game_mode_victory");
                    }
                }
            }
        }

        protected override void UpdateButtons(GameElementShell gameElement, ControlsState state, ControlsState prevState, float milliseconds)
        {
            
        }

        protected override void FirstTimeUpdate(IgnitusGame game, GameElementShell gameElement)
        {
            if(tempLevelManager==null)
            {
                tempLevelManager = new LevelManager(catalogue);
            }
            ((LevelScaleElement)((Mode)game.Modes["game_mode_context"]).Elements[1]).Manager = tempLevelManager;
            
            tempScene = new Scene(catalogue, tempLevelManager);
        }

        void PlayerCommand (int keyIndex, bool state)
        {

        }
    }
}

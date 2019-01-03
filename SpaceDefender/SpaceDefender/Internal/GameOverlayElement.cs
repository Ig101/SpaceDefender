using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using Game.Engine;
using Ignitus;
using Microsoft.Xna.Framework;

namespace GameMaker
{
    class GameOverlayElement: SpriteElement
    {
        GameElement gameElement;
        GameSkillButtonElement tempSkillButton;
        Color tempColor;
        bool shown;
        bool right;
        int? tempPosition;

        public GameSkillButtonElement TempSkillButton { get { return tempSkillButton; } set { tempSkillButton = value; } }

        public GameOverlayElement(string name, int width, int height, Rectangle source, GameElement gameElement)
            :base(name,0,0,width,height,"",Color.White,source,0,Vector2.Zero,Microsoft.Xna.Framework.Graphics.SpriteEffects.None,false,false)
        {
            this.shown = false;
            this.gameElement = gameElement;
            ((GameElement)gameElement).Overlay = this;
        }

        public override void PassiveUpdate(IgnitusGame game, Mode mode, ControlsState state, ControlsState prevState, float milliseconds)
        {
            if (game.GetTempMode().Name == "game" && tempSkillButton != null &&
                gameElement.TempScene != null && gameElement.TempScene.PlayerShip != null)
            {
                if (state.LeftButtonState)
                {
                    shown = true;
                    tempPosition = null;
                    if (tempSkillButton.Cost > gameElement.TempScene.PlayerShip.Resources)
                    {
                        this.X = state.MousePosition.X - Width / 2;
                        this.Y = state.MousePosition.Y - Height / 2;
                        this.Color = Color.Red;
                        right = true;
                    }
                    else
                    {
                        for (int i = 0; i < gameElement.TempScene.PlayerShip.Positions.Length; i++)
                        {
                            if (gameElement.TempScene.PlayerShip.Positions[i].TempModule != gameElement.TempScene.PlayerShip.EngineModule &&
                                gameElement.TempScene.PlayerShip.Positions[i].TempModule != gameElement.TempScene.PlayerShip.CoreModule)
                            {
                                float realX = gameElement.TempScene.PlayerShip.Positions[i].XShift + gameElement.TempScene.PlayerShip.X + 640;
                                float realY = gameElement.TempScene.PlayerShip.Positions[i].YShift + gameElement.TempScene.PlayerShip.Y + 400;
                                if (realX >= state.MousePosition.X - Width / 2 - 1 &&
                                    realX <= state.MousePosition.X + Width / 2 + 1 &&
                                    realY >= state.MousePosition.Y - Height / 2 - 1 &&
                                    realY <= state.MousePosition.Y + Height / 2 + 1)
                                {
                                    tempPosition = i;
                                    continue;
                                }
                            }
                        }
                        this.SpriteName = tempSkillButton.PressedSpriteName;
                        if (tempPosition != null)
                        {
                            float realX = gameElement.TempScene.PlayerShip.Positions[tempPosition.Value].XShift + gameElement.TempScene.PlayerShip.X + 640;
                            float realY = gameElement.TempScene.PlayerShip.Positions[tempPosition.Value].YShift + gameElement.TempScene.PlayerShip.Y + 400;
                            this.X = (int)realX - Width / 2;
                            this.Y = (int)realY - Width / 2;
                            this.Color = Color.Green;
                            right = gameElement.TempScene.PlayerShip.Positions[tempPosition.Value].Direction == Direction.Right;
                        }
                        else
                        {
                            this.X = state.MousePosition.X - Width / 2;
                            this.Y = state.MousePosition.Y - Height / 2;
                            this.Color = Color.Yellow;
                            right = true;
                        }
                    }
                }
                else if (prevState.LeftButtonState)
                {
                    if (tempPosition != null && tempSkillButton.SkillMethod != null)
                    {
                        gameElement.TempScene.PlayerShip.Resources -= tempSkillButton.Cost;
                        tempSkillButton.SkillMethod(gameElement.TempScene.PlayerShip, tempPosition.Value);
                        shown = false;
                        tempSkillButton.Pressed = false;
                        tempSkillButton.Selected = false;
                        tempSkillButton = null;
                    }
                }
                else
                {
                    shown = false;
                    tempSkillButton.Pressed = false;
                    tempSkillButton.Selected = false;
                    tempSkillButton = null;
                }
            }
            else
            {
                shown = false;
                if (tempSkillButton != null)
                {
                    tempSkillButton.Pressed = false;
                    tempSkillButton.Selected = false;
                    tempSkillButton = null;
                }
            }
        }

        public override void Update(IgnitusGame game, Mode mode, ControlsState state, ControlsState prevState, float milliseconds)
        {

        }

        public override void Draw(IgnitusGame game, Matrix animation, Color fonColor, float milliseconds)
        {
            if (shown)
            {
                game.DrawSprite(SpriteName, new Rectangle(X, Y, Width, Height), Source, new Color(Color.R * fonColor.R / 255,
                    Color.G * fonColor.G / 255, Color.B * fonColor.B / 255, Color.A * fonColor.A / 255), 0, Vector2.Zero, 
                    right?Microsoft.Xna.Framework.Graphics.SpriteEffects.None:Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally, 0);
            }
        }
    }
}

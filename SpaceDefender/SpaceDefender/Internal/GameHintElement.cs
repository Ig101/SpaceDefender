using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using Ignitus;
using Microsoft.Xna.Framework;

namespace GameMaker
{
    class GameHintElement: HudElement
    {
        GameElement gameElement;
        float shown;
        Color color;
        Color textColor;
        string text;
        bool align;
        string baseText;
        GameSkillButtonElement[] skillButtonsToHint;
        int levelToHide;
        bool nextGame;

        public GameHintElement(string name, int x, int y, int width, int height, Color color,
            Color textColor, string text, GameSkillButtonElement[] skillButtonsToHint, GameElement gameElement, bool align, int levelToHide,
            bool nextGame)
            :base(name,x,y,width,height,true,false,false)
        {
            this.nextGame = nextGame;
            this.levelToHide = levelToHide;
            this.baseText = text;
            this.align = align;
            this.skillButtonsToHint = skillButtonsToHint;
            this.gameElement = gameElement;
            this.color = color;
            this.textColor = textColor;
            this.text = text;
        }

        public override void Update(IgnitusGame game, Mode mode, ControlsState state, ControlsState prevState, float milliseconds)
        {
            float coff = 0;
            if (gameElement == null && game.GetTempMode().Name == "game")
            {
                coff = 1;
            }
            if (gameElement != null)
            {
                coff = gameElement.UpdateButtonCoff();
            }
        }

        public override void Draw(IgnitusGame game, Matrix animation, Color fonColor, float milliseconds)
        {
            if (shown > 0)
            {
                float coff = 0;
                if (gameElement == null && game.GetTempMode().Name == "game")
                {
                    coff = 1;
                }
                if (gameElement != null)
                {
                    coff = gameElement.UpdateButtonCoff();
                }
                if (coff > 0)
                {
                    Color c = new Color((byte)(color.R * fonColor.R / 255 * coff * shown),
                        (byte)(color.G * fonColor.G / 255 * coff * shown), (byte)(color.B * fonColor.B / 255 * coff * shown),
                        (byte)(color.A * fonColor.A / 255 * coff * shown));
                    float size = 2f*Height / Width;
                    game.DrawSprite("context_shade", new Rectangle(X, Y, Width, Height/2), new Rectangle(0,0,1024,(int)(256*size)), 
                        c, 0, Vector2.Zero,
                        Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                    game.DrawSprite("context_shade", new Rectangle(X, Y + Height/2, Width, Height/2), new Rectangle(0, 512-(int)(256*size), 1024, (int)(256 * size)),
                        c, 0, Vector2.Zero,
                        Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                    game.DrawString("smallFont", game.Id2Str(text), align, new Point(X + 150, Y + 20), Width - 300,
                        new Color((byte)(textColor.R * fonColor.R / 255 * coff * shown),
                        (byte)(textColor.G * fonColor.G / 255 * coff * shown), (byte)(textColor.B * fonColor.B / 255 * coff * shown),
                        (byte)(textColor.A * fonColor.A / 255 * coff * shown))
                        );
                }
            }
        }

        public override void PassiveUpdate(IgnitusGame game, Mode mode, ControlsState state, ControlsState prevState, float milliseconds)
        {
            if (gameElement.TempManager != null)
            {
                if (gameElement.TempManager.TempLevelNumber < levelToHide)
                {
                    this.shown = 1;
                }
                else
                {
                    if (nextGame)
                    {
                        this.shown = gameElement.TempManager.TempLevelNumber == levelToHide ? (gameElement.TempManager.NextLevelTimer > 0 ? gameElement.TempManager.NextLevelTimer : 1) : 0;
                    }
                    else
                    {
                        this.shown = gameElement.TempManager.TempLevelNumber == levelToHide ? (gameElement.TempScene.TimerToEnd > 0 ? gameElement.TempScene.TimerToEnd : gameElement.TempManager.NextLevel? 0 : 1) : 0;
                    }
                }
                if (skillButtonsToHint != null)
                {
                    foreach (GameSkillButtonElement button in skillButtonsToHint)
                    {
                        if (button.Selected || button.Pressed)
                        {
                            this.text = button.Name + "_hint";
                            return;
                        }
                    }
                    this.shown = 0;
                    this.text = baseText;
                }
            }
            else shown = 0;
        }

        public override void DrawPreActionsUpdate(IgnitusGame game, Color fonColor)
        {

        }
    }
}

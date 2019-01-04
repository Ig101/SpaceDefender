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
    class GameNextLevelElement: SpriteButtonElement
    {
        GameElementShell gameElement;

        public void NextLevel ()
        {
            ((GameElement)gameElement).TempManager.NextLevelTimer = 1;
        }

        public GameNextLevelElement(string name, int x, int y, int width, int height, Color color, Color selectedColor, Color pressedColor,
            string spriteName, string selectedSpriteName, string pressedSpriteName, Rectangle source, PressButtonAction action, GameElementShell gameElement)
            :base(name,x,y,width,height,"","",color,selectedColor,pressedColor,color,spriteName,selectedSpriteName,pressedSpriteName,
                 source,action,false,false)
        {
            this.gameElement = gameElement;
        }

        public override void PassiveUpdate(IgnitusGame game, Mode mode, ControlsState state, ControlsState prevState, float milliseconds)
        {
            if (((GameElement)gameElement).TempManager.NextLevel && ((GameElement)gameElement).TempManager.NextLevelTimer == 0)
            {
                this.Y = 25 - (int)(((GameElement)gameElement).TempScene.TimerToEnd * 1200);
            }
            else
            {
                this.Y = -1175 + (int)(((GameElement)gameElement).TempManager.NextLevelTimer * 1200);
            }
            base.PassiveUpdate(game, mode, state, prevState, milliseconds);
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
            if (coff>=0.2f && ((GameElement)gameElement).TempScene.TimerToEnd <= 0.2f && (((GameElement)gameElement).TempManager.NextLevelTimer>0.2f
                || ((GameElement)gameElement).TempManager.NextLevelTimer<=0) && Y>0)
                base.Update(game, mode, state, prevState, milliseconds);
        }

        public override void Draw(IgnitusGame game, Matrix animation, Color fonColor, float milliseconds)
        {
            float coff = 0;
            if(gameElement == null && game.GetTempMode().Name == "game")
            {
                coff = 1;
            }
            if(gameElement!=null)
            { 
                coff = gameElement.UpdateButtonCoff();
            }
            if (coff > 0)
            {
                base.Draw(game, animation, new Color((byte)(fonColor.R * coff), (byte)(fonColor.G * coff), (byte)(fonColor.B * coff), (byte)(fonColor.A * coff)),milliseconds);
            }
        }
    }
}

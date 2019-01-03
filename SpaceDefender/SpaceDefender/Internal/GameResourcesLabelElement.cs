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
    class GameResourcesLabelElement: LabelElement
    {
        GameElementShell gameElement;

        public GameResourcesLabelElement(string name, int x, int y, int width, string text, bool convert, bool align, Color color,
            string font, GameElementShell gameElement)
            :base(name,x,y,width,text,convert,align,color,font,false,false)
        {
            this.gameElement = gameElement;
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
            if (coff>=0.2f)
                base.Update(game, mode, state, prevState, milliseconds);
        }

        public override void Draw(IgnitusGame game, Matrix animation, Color fonColor, float milliseconds)
        {
            if (((GameElement)gameElement).TempScene != null && ((GameElement)gameElement).TempScene.PlayerShip != null)
            {
                this.Text = ((int)((GameElement)gameElement).TempScene.PlayerShip.Resources).ToString();
            }
            else
            {
                this.Text = "";
            }
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

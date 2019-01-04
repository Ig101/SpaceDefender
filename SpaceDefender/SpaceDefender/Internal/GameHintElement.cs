using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ignitus;
using Microsoft.Xna.Framework;

namespace GameMaker
{
    class GameHintElement: HudElement
    {
        GameElement gameElement;
        float shown;

        public GameHintElement(string name, int x, int y, int width, int height, Color color,
            string spriteName, Rectangle source, GameElement gameElement)
            :base(name,x,y,width,height,true,false,false)
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
            if (coff > 0 && shown>0)
            {
                
            }
        }

        public override void PassiveUpdate(IgnitusGame game, Mode mode, ControlsState state, ControlsState prevState, float milliseconds)
        {
            this.shown = gameElement
        }

        public override void DrawPreActionsUpdate(IgnitusGame game, Color fonColor)
        {

        }
    }
}

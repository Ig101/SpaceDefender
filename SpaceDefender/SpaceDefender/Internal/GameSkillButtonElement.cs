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
    class GameSkillButtonElement: SpriteButtonElement
    {
        GameElement gameElement;
        bool selectedChange;
        float cost;
        bool allowed;
        AssembleModule skillMethod;

        public AssembleModule SkillMethod { get { return skillMethod; } }
        public float Cost { get { return cost; } }

        public GameSkillButtonElement(string name, int x, int y, int width, int height, Color color, Color selectedColor, Color pressedColor,
            string spriteName, string selectedSpriteName, string pressedSpriteName, Rectangle source, GameElement gameElement, float cost, AssembleModule skillMethod)
            :base(name,x,y,width,height,"","",color,selectedColor,pressedColor,color,spriteName,selectedSpriteName,pressedSpriteName,
                 source,null,false,false)
        {
            this.cost = cost;
            this.skillMethod = skillMethod;
            this.gameElement = gameElement;
        }

        public override void PassiveUpdate(IgnitusGame game, Mode mode, ControlsState state, ControlsState prevState, float milliseconds)
        {
            if (gameElement.TempScene != null && gameElement.TempScene.PlayerShip != null && gameElement.TempScene.PlayerShip.Resources >= cost)
            {
                allowed = true;
                if (selectedChange == false)
                {
                    Selected = false;
                }
                selectedChange = false;
                if (!state.LeftButtonState && !state.KeysState[0] &&
                    !prevState.LeftButtonState && !prevState.KeysState[0])
                {
                    Pressed = false;
                }
            }
            else
            {
                allowed = false;
                Selected = false;
                Pressed = false;
            }
        }

        public override void Update(IgnitusGame game, Mode mode, ControlsState state, ControlsState prevState, float milliseconds)
        {
            if (gameElement.TempScene != null && gameElement.TempScene.PlayerShip != null && gameElement.TempScene.PlayerShip.Resources >= cost
                && !gameElement.TempScene.Defeat)
            {
                allowed = true;
                float coff = 0;
                if (gameElement == null && game.GetTempMode().Name == "game")
                {
                    coff = 1;
                }
                if (gameElement != null)
                {
                    coff = gameElement.UpdateButtonCoff();
                }
                if (coff >= 0.2f)
                {
                    if (Selected == false && !Pressed)
                    {
                        selectedChange = true;
                    }
                    Selected = true;
                    selectedChange = true;
                    Point correctedMousePos = TransformPointToElementCoords(state.MousePosition);
                    if (CheckMousePositionInElement(state.MousePosition) &&
                        state.LeftButtonState && !prevState.LeftButtonState ||
                        state.KeysState[0] && !prevState.KeysState[0] && mode.KeyboardUse)
                    {
                        Pressed = true;
                        gameElement.Overlay.TempSkillButton = this;
                    }
                }
            }
            else
            {
                allowed = false;
                Selected = false;
                Pressed = false;
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
            if (coff > 0)
            {
                if(!allowed)
                {
                    fonColor = new Color((byte)(fonColor.R * 0.5f), (byte)(fonColor.G * 0.5f), (byte)(fonColor.B * 0.5f), fonColor.A);
                }
             /**   if (!Pressed)
                {*/
                    base.Draw(game, animation, new Color((byte)(fonColor.R * coff), (byte)(fonColor.G * coff), (byte)(fonColor.B * coff), (byte)(fonColor.A * coff)), milliseconds);
                //}
            }
        }
    }
}

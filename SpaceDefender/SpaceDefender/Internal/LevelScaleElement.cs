using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Progress;
using Ignitus;
using Microsoft.Xna.Framework;

namespace GameMaker
{
    class LevelScaleElement: SpriteElement
    {
        LevelManager manager;
        float migalkaTime;
        bool shown;

        public LevelManager Manager { get { return manager; } set { manager = value; } }

        public LevelScaleElement(string name, int x, int y, int width, int height, Color color, LevelManager manager)
            :base(name,x,y,width,height,"scale",color,Rectangle.Empty,0,Vector2.Zero,Microsoft.Xna.Framework.Graphics.SpriteEffects.None,false,false)
        {
            this.manager = manager;
        }

        public override void PassiveUpdate(IgnitusGame game, Mode mode, ControlsState state, ControlsState prevState, float milliseconds)
        {
            migalkaTime -= milliseconds / 1000;
            if(migalkaTime <=0)
            {
                shown = !shown;
                migalkaTime = 0.6f;
            }
            base.PassiveUpdate(game, mode, state, prevState, milliseconds);
        }

        public override void Draw(IgnitusGame game, Matrix animation, Color fonColor, float milliseconds)
        {
            int width = this.Width / manager.Levels.Length;
            for(int i = 0; i<manager.Levels.Length;i++)
            {
                game.DrawSprite("scale", new Rectangle(X + i*width, Y, width, Height), new Rectangle(0,0,width/(Height/16),16), new Color(Color.R * fonColor.R / 255,
                    Color.G * fonColor.G / 255, Color.B * fonColor.B / 255, Color.A * fonColor.A / 255), 0, Vector2.Zero, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            }
            game.DrawSprite("scale", new Rectangle(X + (manager.Levels.Length-1) * width, Y, width, Height), new Rectangle(0, 0, width / (Height / 16), 16), new Color(Color.R * fonColor.R / 255,
                Color.G * fonColor.G / 255, Color.B * fonColor.B / 255, Color.A * fonColor.A / 255), 0, Vector2.Zero, Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally, 0);
            if(shown) game.DrawSprite("scale_marker", new Rectangle(X + width/2 + manager.TempLevelNumber * width, Y, Height, Height), new Rectangle(0, 0, 16, 16), new Color(Color.R * fonColor.R / 255,
                Color.G * fonColor.G / 255, Color.B * fonColor.B / 255, Color.A * fonColor.A / 255), 0, new Vector2(8,0), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
        }
    }
}

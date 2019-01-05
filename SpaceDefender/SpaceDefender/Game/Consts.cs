using Game.Engine;
using GameMaker;
using Ignitus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public static class Consts
    {
        //Time before moving to the main menu at the start of application
        public static int loadingTime = 0;

        //Name of texture that used for the base menu fon
        public static string fonName = "";

        //All textures that should be loaded before loading (for loading screen)
        public static string[] texturesOnStart = new string[]
        {

        };

        //All extra loading screen elements (labels, sprites, etc.)
        public static HudElement[] loadingScreenElements = new HudElement[]
        {

        };

        //All extra main menu elements
        public static HudElement[] mainElements = new HudElement[]
        {

        };

        //Path of the profile directory (file profile.mrc will be held here)
        public static string profileSavePath = Environment.CurrentDirectory;

        //Method that brings profile information and analyse it
        public static void LoadProfile(GameElementShell gameElementShell, string profile)
        {
            GameElement gameElement = (GameElement)gameElementShell;
            string[] strs = profile.Split(new char[] { ';' });
            if(strs.Length>1)
            {
                gameElement.TempManager = new Game.Progress.LevelManager(gameElement.Catalogue);
                gameElement.TempManager.TempLevelNumber = int.Parse(strs[0]);
                string[] positions = new string[strs.Length - 3];
                for(int i = 2; i<strs.Length-1;i++)
                {
                    positions[i - 2] = strs[i];
                }
                gameElement.SetShipStartCharacteristics(float.Parse(strs[1]),positions);
            }
            //Put your logic here
        }

        //Method that creates profile information string
        public static string SaveProfile(GameElementShell gameElementShell)
        {
            GameElement gameElement = (GameElement)gameElementShell;
            if(gameElement.TempManager.NextLevel && gameElement.TempManager.NextLevelEntity!=null && gameElement.TempManager.TempLevelNumber>=0 && gameElement.TempManager.NextLevelTimer<=0)
            {
                string str = (gameElement.TempManager.TempLevelNumber).ToString() + ";" + gameElement.TempScene.PlayerShip.Resources+";";
                foreach(ModulePosition pos in gameElement.TempScene.PlayerShip.Positions)
                {
                    str += (pos.TempModule == null ? "null" : pos.TempModule.Sprite.SpriteName)+":" + (pos.TempModule == null ? "null" : pos.TempModule.Health.ToString())+ ";";
                }
                return str;
            }
            return null;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using Game.Progress;
using Ignitus;

namespace GameMaker
{
    public static partial class MenuActions
    {
        public static void PauseGame(IgnitusGame game, Mode mode, HudElement element)
        {
            Mode gameMode = (Mode)game.Modes["game_mode"];
            if (gameMode.Elements.Length > 1)
            {
                ((GameElement)(gameMode).Elements[0]).PrepareFrom(game,"game_mode_context");
            }
            else
            {
                game.GoToMode("game_mode_context");
            }
        }

        public static void EndGame(IgnitusGame game, Mode mode, HudElement element)
        {
            GameElement elem = ((GameElement)((Mode)game.Modes["game_mode"]).Elements[0]);
            if(elem.TempManager.NextLevelEntity == null)
            {
                elem.TempManager = null;
                ((LevelScaleElement)((Mode)game.Modes["main"]).Elements[0]).Manager = null;
                ((Game1Shell)game).SetScore(((Game1Shell)game).Score+1);
            }
            if (!elem.TempManager.NextLevel || elem.TempManager.NextLevelTimer > 0)
            {
                elem.TempManager.TempLevelNumber--;
                elem.TempManager.NextLevel = true;
                elem.TempManager.NextLevelTimer = 0;
                elem.TempManager.KilledCount = 0;
            }
            game.GoToMode("main");
        }

        public static void NextLevel(IgnitusGame game, Mode mode, HudElement element)
        {
            ((GameNextLevelElement)element).NextLevel();
            //((Game1Shell)game).SaveProfilePublic();
            //((ButtonElement)((Mode)game.Modes["game_mode_context"]).Elements[3]).Text = game.Id2Str("concede");
        }

        public static void StartGame(IgnitusGame game, Mode mode, HudElement element)
        {
            Mode gameMode = (Mode)game.Modes["game_mode"];
            if (gameMode.Elements.Length > 1)
            {
                GameElement gameElement = (GameElement)(gameMode).Elements[0];
                gameElement.PrepareForFirstUse(game);
            }
            //TODO
            game.GoToMode("game_mode");
        }

        public static void ContinueGame(IgnitusGame game, Mode mode, HudElement element)
        {
            Mode gameMode = (Mode)game.Modes["game_mode"];
            if (gameMode.Elements.Length > 1)
            {
                ((GameElement)(gameMode).Elements[0]).PrepareTo();
            }
            game.GoToMode("game_mode");
        }

        public static void Exit (IgnitusGame game, Mode mode, HudElement element)
        {
            game.SaveConfig();
            ((Game1Shell)game).EndGameActivities();
            game.Exit();
        }

        public static void SoundVolume (IgnitusGame game, Mode mode, HudElement element)
        {
            float pos = ((SlideElement)element).Position;
            game.SoundVolume = (int)(pos*100);
            game.Volume = (int)(pos * 100);
            
        }
    }
}

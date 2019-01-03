﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ignitus;
using System.IO;
using System;
using System.Text;
using Game;
using Game.Catalogues;

namespace GameMaker
{

    public class Game1Shell : IgnitusGame
    {
        Color c_color = new Color(200, 200, 200, 200);
        Color c_selected_color = new Color(240, 240, 240, 255);
        Color c_pressed_color = new Color(120, 120, 120, 255);

        public delegate string SetProfileString(GameElementShell gameElement);
        public delegate void GetProfileString(GameElementShell gameElement, string str);

        SetProfileString methodForSaveProfile;
        GetProfileString methodForLoadProfile;
        int loadingTime;
        string[] texturesOnStart;
        HudElement[] loadingScreenElements;
        HudElement[] mainElements;
        GameElementShell gameElement;

        int score;
        int maxScore;
        string profileFilePath;

        public GameElementShell GameElement { get { return gameElement; } }


        public Game1Shell(int loadingTime, string[] texturesOnStart, HudElement[] loadingScreenElements, HudElement[] mainElements,
            string profileFilePath,
            SetProfileString methodForSaveProfile, GetProfileString methodForLoadProfile, GameElementShell gameElement)
            :base(new Vector2(-16, -16), 32, MathHelper.Pi + MathHelper.PiOver4, new Point(1280, 800))
        {
            this.gameElement = gameElement;
            this.loadingTime = loadingTime;
            this.texturesOnStart = texturesOnStart;
            this.loadingScreenElements = loadingScreenElements;
            this.mainElements = mainElements;
            this.methodForSaveProfile = methodForSaveProfile;
            this.methodForLoadProfile = methodForLoadProfile;
            this.profileFilePath = profileFilePath;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            HudElement[] tempLoadingElements;
            if (loadingScreenElements != null)
            {
                tempLoadingElements = new HudElement[loadingScreenElements.Length + 1];
                for (int i = 0; i < loadingScreenElements.Length; i++)
                {
                    tempLoadingElements[i] = loadingScreenElements[i];
                }
            }
            else
            {
                tempLoadingElements = new HudElement[1];
            }
            tempLoadingElements[tempLoadingElements.Length - 1] = 
                new LoadingWheelElement("loading", 1200, 730, 100, 100, "loadingWheel", loadingTime, true, Color.White, 1,
                    Game1Shell.PreLoadingMethodBeforeStart, Game1Shell.PreLoadingMethodBeforeStart, false, false);
            modes.Add("loadingScreen", new Mode(null, tempLoadingElements, 5, "loadingScreen", Mode.BlackGlow, null, false));
            GoToLoadingMode(new object[] { this }, PreLoadingMethodBeforeStart, LoadingMethodBeforeStart, "main");
            //fullScreen = true;
            base.Initialize();
            keys = new Keys[7];
            keys[0] = Keys.Enter;
            keys[1] = Keys.Escape;
            keys[2] = Keys.Left;
            keys[3] = Keys.Right;
            keys[4] = Keys.Up;
            keys[5] = Keys.Down;
            keys[6] = Keys.F4;
        }


        protected override void Update(GameTime gameTime)
        {
            
            if(controlsPrevState.KeysState.Length>6 && controlsPrevState.KeysState[6] && !Keyboard.GetState().IsKeyDown(keys[6]))
            {
                ToggleFullScreen();
                SaveConfig();
                gameElement.PickSampleFon();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        protected override void LoadConfigFromFile(StreamReader reader)
        {
            if (reader.EndOfStream)
            {
                return;
            }
            string[] line = reader.ReadLine().Split(new char[] { '=' });
            if (line.Length > 1 && int.TryParse(line[1], out int result))
            {
                resolution = (Resolution)result;
            }
            if (reader.EndOfStream)
            {
                return;
            }
            line = reader.ReadLine().Split(new char[] { '=' });
            if (line.Length > 1 && int.TryParse(line[1], out result))
            {
                winResolution = (Resolution)result;
            }
            if (reader.EndOfStream)
            {
                return;
            }
            line = reader.ReadLine().Split(new char[] { '=' });
            if (line.Length > 1 && int.TryParse(line[1], out result))
            {
                fullScreen = result == 1;
            }
            if (reader.EndOfStream)
            {
                return;
            }
            line = reader.ReadLine().Split(new char[] { '=' });
            if (line.Length > 1 && int.TryParse(line[1], out result))
            {
                volume = result;
                soundVolume = result;
            }
            if (reader.EndOfStream)
            {
                return;
            }
            line = reader.ReadLine().Split(new char[] { '=' });
            if (line.Length > 1)
            {
                languageName = line[1];
            }
        }

        protected override HudElement LoadModeElementParser(string type, string name, string[] parametres)
        {
            return base.LoadModeElementParser(type, name, parametres);
        }

        protected override bool SaveConfigCore(StreamWriter writer)
        {
            writer.WriteLine("resolution=" + ((int)resolution).ToString());
            writer.WriteLine("winResolution=" + ((int)winResolution).ToString());
            writer.WriteLine("fullscreen=" + (fullScreen?"1":"0"));
            writer.WriteLine("volume=" + volume.ToString());
            writer.WriteLine("language=" + languageName);
            return true;
        }

        protected override void LoadStartConfig()
        {
            winResolution = Resolution.R800x480;
            resolution = Resolution.RYours;
            volume = 50;
            soundVolume = 50;
            languageName = "eng";
            fullScreen = true;
        }

        protected override object LoadNativeParser(string type, string name, string[] parametres)
        {
            return null;
        }

        protected override void LoadProfile()
        {
            HudElement[] elements = new HudElement[13];
            elements[0] = gameElement;
            elements[1] = new GameButtonElement("pause", 25, 25, 75, 75, c_color, c_selected_color, c_pressed_color,
                "pause_button", null, null, new Rectangle(0, 0, 512, 512), MenuActions.PauseGame, gameElement);
            elements[2] = new GameSpriteElement("resources", 1105, 25, 150, 75,c_color, "resources", new Rectangle(0, 0, 1024, 512), gameElement);
            elements[3] = new GameSpriteElement("actionPanel", 365, 713, 550, 85, c_color, "actionPanel", new Rectangle(0, 0, 3072, 512), gameElement);
            elements[4] = new GameResourcesLabelElement("resources", 1153, 38, 100, "99", false, false, Color.White, "largeFont", gameElement);
            elements[5] = new GameSkillButtonElement("blaster", 383, 1446/2, 128/2, 128/2, Color.White, new Color(230,230,230), new Color(0, 0, 0, 255),
                "blaster", "blaster", "blaster", new Rectangle(0, 0, 64, 64), (GameElement)gameElement,1, Delegates.AssembleBlasterModule);
            elements[6] = new GameSkillButtonElement("actionPanel", 916/2, 1446/2, 128/2, 128/2, Color.White, new Color(230, 230, 230), new Color(0, 0, 0, 255),
                "rocket", "rocket", "rocket", new Rectangle(0, 0, 64, 64), (GameElement)gameElement,1, Delegates.AssembleRocketModule);
            elements[7] = new GameSkillButtonElement("actionPanel", 1066/2, 1446/2, 128/2, 128/2, Color.White, new Color(230, 230, 230), new Color(0, 0, 0, 255),
                "annihilator", "annihilator", "annihilator", new Rectangle(0, 0, 64, 64), (GameElement)gameElement,1, Delegates.AssembleAnniModule);
            elements[8] = new GameSkillButtonElement("actionPanel", 1216/2, 1446/2, 128/2, 128/2, Color.White, new Color(230, 230, 230), new Color(0, 0, 0, 255),
                "generator", "generator", "generator", new Rectangle(0, 0, 64, 64), (GameElement)gameElement,1, Delegates.AssembleGeneratorModule);
            elements[9] = new GameSkillButtonElement("actionPanel", 1366/2, 1446/2, 128/2, 128/2, Color.White, new Color(230, 230, 230), new Color(0, 0, 0, 255),
                "armor", "armor", "armor", new Rectangle(0, 0, 64, 64), (GameElement)gameElement,1, Delegates.AssembleArmorModule);
            elements[10] = new GameSkillButtonElement("actionPanel", 1516/2, 1446/2, 128/2, 128/2, Color.White, new Color(230, 230, 230), new Color(0, 0, 0, 255),
                "shield", "shield", "shield", new Rectangle(0, 0, 64, 64), (GameElement)gameElement,1, Delegates.AssembleShieldModule);
            elements[11] = new GameSkillButtonElement("actionPanel", 1666/2, 1446/2, 128/2, 128/2, Color.White, new Color(230, 230, 230), new Color(0, 0, 0, 255),
                "demolish", "demolish", "emptyModule", new Rectangle(0, 0, 64, 64), (GameElement)gameElement,0, Delegates.DemolishModule);
            elements[12] = new GameOverlayElement("skill_overlay", 128 / 2, 128 / 2, new Rectangle(0, 0, 64, 64), (GameElement)gameElement);
            modes.Add("game_mode", new Mode(null, elements, 5, "game", Mode.BlackGlow, null, false));
            mainElements = new HudElement[1];
            mainElements[0] = new SpriteElement("shade", 490, 150, 300, 600, "play_shade", new Color(0, 0, 0, 150), new Rectangle(0, 0, 512, 1024), false, false);
            if (mainElements != null)
            {
                elements = new HudElement[mainElements.Length + 7];
                for (int i = 0; i < mainElements.Length; i++)
                {
                    elements[i] = mainElements[i];
                }
            }
            else
            {
                elements = new HudElement[7];
            }
            //DownloadAdditiveElements
            elements[elements.Length - 1] = new SpriteButtonElement("play", ingameScreenSize.X / 2 - 200 / 2, ingameScreenSize.Y / 2, 400 / 2, 400 / 2, "", "", c_color,
                c_selected_color,c_pressed_color,c_color, "play_button", null, null, new Rectangle(0, 0, 512, 512),
                MenuActions.StartGame, false, false);
            elements[elements.Length - 2] = new SpriteButtonElement("exit", 50 / 2, 50 / 2, 150 / 2, 150 / 2, null, null, c_color,
                c_selected_color, c_pressed_color, c_color, "exit_button", null, null, new Rectangle(0, 0, 512, 512),
                MenuActions.Exit, false, false);
            elements[elements.Length - 3] = new SlideElement("soundReg", 550 / 2, 75 / 2, 400 / 2, 50 / 2, "slider", c_color, c_selected_color, c_pressed_color,
                MenuActions.SoundVolume, 130, false, true, 0.13f, false, false);
            ((SlideElement)elements[elements.Length - 3]).Position = volume / 100f;
            elements[elements.Length - 4] = new LabelElement("fullscreen", 205 / 2, 1450 / 2, ingameScreenSize.X - 400 / 2, "fullscreen_message", true, false, c_color, "smallFont", false, false);
            elements[elements.Length - 5] = new LabelElement("score", 195 / 2, 570 / 2, ingameScreenSize.X - 400 / 2, Id2Str("score") + " " + score.ToString(), false, false, c_color, "mediumFont", false, false);
            elements[elements.Length - 6] = new LabelElement("max_score", 195 / 2, 670 / 2, ingameScreenSize.X - 400 / 2, Id2Str("top_result") + " " + maxScore.ToString(), false, false, c_color, "mediumFont", false, false);
            elements[elements.Length - 7] = new LabelElement("volume", 320 / 2, 80 / 2, 1000 / 2, "volume", true, true, c_color, "smallFont", false, false);
            //
            modes.Add("main", new Mode((Mode)modes["game_mode"], elements, 3.5f, "main", FromAbove, null, false));

            modes.Add("game_mode_context", new Mode((Mode)modes["game_mode"], new HudElement[]
            {
                new SpriteElement("shade",340,250,600,300,"context_shade",new Color(0,0,0,150),new Rectangle(0,0,1024,512),false,false),
                new LevelScaleElement("scale",490,480,300,16,Color.White,null),
                new ButtonElement("continue",600/2,610/2,ingameScreenSize.X-1200/2,100/2,Id2Str("continue"),"largeFont",false,
                c_color,c_selected_color,c_pressed_color,MenuActions.ContinueGame,false,false),
                new ButtonElement("end",600/2,770/2,ingameScreenSize.X-1200/2,100/2,Id2Str("end"),"largeFont",false,
                c_color,c_selected_color,c_pressed_color,MenuActions.EndGame,false,false)
            }, 3.5f, "context", FromAbove, null, false));

            modes.Add("game_mode_result", new Mode((Mode)modes["game_mode"], new HudElement[]
            {
               new SpriteElement("shade",340,250,600,300,"context_shade",new Color(0,0,0,150),new Rectangle(0,0,1024,512),false,false),
                new LabelElement("defeat", 200/2, 600/2, ingameScreenSize.X - 400/2, Id2Str("defeat"), false, false, c_color, "largeFont", false, false),
                new LabelElement("killed", 200/2, 840/2, ingameScreenSize.X - 400/2, Id2Str("killed") + " " + maxScore.ToString(), false, false, c_color, "mediumFont", false, false),
                new LevelScaleElement("scale",490,480,300,16,Color.White,null),
                new LabelElement("any_key", 200/2, 1400/2, ingameScreenSize.X - 400/2, "any_key_message", true, false, c_color, "mediumFont", false, false),
                new AnyKeyElement("result",MenuActions.EndGame)
            }, 3.5f, "result", FromAbove, null, false));
            modes.Add("game_mode_victory", new Mode((Mode)modes["game_mode"], new HudElement[]
            {
               new SpriteElement("shade",340,250,600,300,"context_shade",new Color(0,0,0,150),new Rectangle(0,0,1024,512),false,false),
                new LabelElement("defeat", 200/2, 600/2, ingameScreenSize.X - 400/2, Id2Str("victory"), false, false, c_color, "largeFont", false, false),
                new LabelElement("killed", 200/2, 840/2, ingameScreenSize.X - 400/2, Id2Str("killed") + " " + maxScore.ToString(), false, false, c_color, "mediumFont", false, false),
                new LabelElement("any_key", 200/2, 1400/2, ingameScreenSize.X - 400/2, "any_key_message", true, false, c_color, "mediumFont", false, false),
                new AnyKeyElement("result",MenuActions.EndGame)
            }, 3.5f, "result", FromAbove, null, false));
            modes.Add("game_mode_nextlevel", new Mode((Mode)modes["game_mode"], new HudElement[]
            {
               new SpriteElement("shade",340,250,600,300,"context_shade",new Color(0,0,0,150),new Rectangle(0,0,1024,512),false,false),
                new LabelElement("defeat", 200/2, 600/2, ingameScreenSize.X - 400/2, Id2Str("success"), false, false, c_color, "largeFont", false, false),
                new LabelElement("killed", 200/2, 840/2, ingameScreenSize.X - 400/2, Id2Str("killed") + " " + maxScore.ToString(), false, false, c_color, "mediumFont", false, false),
                new LevelScaleElement("scale",490,480,300,16,Color.White,null),
                new LabelElement("any_key", 200/2, 1400/2, ingameScreenSize.X - 400/2, "any_key_message", true, false, c_color, "mediumFont", false, false),
                new AnyKeyElement("result",MenuActions.EndGame)
            }, 3.5f, "result", FromAbove, null, false));
            //LoadFile
            if (File.Exists(profileFilePath+@"\profile.mrc"))
            {
                byte[] bytes = Magic.Restore(profileFilePath + @"\profile.mrc");
                string str = Encoding.UTF8.GetString(bytes);
                string[] strs = str.Split(new char[] { '\n' });
                string masterString = strs[0];
                //Master
                if (int.TryParse(masterString, out int result))
                {
                    maxScore = result;
                }
                string otherString = str.Remove(0, masterString.Length + 1);
                //Other
                methodForLoadProfile?.Invoke(gameElement, otherString);
            }
        }

        protected override void SaveProfile()
        {
            //SaveFile
            string str = maxScore.ToString() + "\n" + (methodForSaveProfile!=null?methodForSaveProfile(gameElement):"");
            Magic.Act(profileFilePath + @"\profile.mrc",Encoding.UTF8.GetBytes(str));
        }

        public bool SetScore (int score)
        {
            this.score = score;
            if(maxScore<score)
            {
                maxScore = score;
            }
            Mode mode = (Mode)modes["main"];
            ((LabelElement)mode.Elements[mode.Elements.Length - 5]).Text = Id2Str("score") + " " + score.ToString();
            ((LabelElement)mode.Elements[mode.Elements.Length - 6]).Text = Id2Str("top_result") + " " + maxScore.ToString();
            mode = (Mode)modes["game_mode_result"];
            ((LabelElement)mode.Elements[0]).Text = Id2Str("score") + " " + score.ToString();
            ((LabelElement)mode.Elements[1]).Text = Id2Str("top_result") + " " + maxScore.ToString();
            return maxScore < score;
        }

        public void LoadEngineContent()
        {
            content.Add("play_button", Content.Load<Texture2D>("menu//Play_button"));
            content.Add("exit_button", Content.Load<Texture2D>("menu//Exit_button"));
            content.Add("pause_button", Content.Load<Texture2D>("menu//Pause_button"));
            content.Add("slider", Content.Load<Texture2D>("menu//Slider"));
            content.Add("slider_slide", Content.Load<Texture2D>("menu//Slider_slide"));
            content.Add("slider_arrow", Content.Load<Texture2D>("menu//Slider_arrow"));
            content.Add("context_shade", Content.Load<Texture2D>("menu//ContextShade"));
            content.Add("play_shade", Content.Load<Texture2D>("menu//PlayShade"));
            content.Add("scale", Content.Load<Texture2D>("menu//scale"));
            content.Add("scale_marker", Content.Load<Texture2D>("menu//scale_marker"));
        }

        #region loading
        void LoadLoadingContent()
        {
            if (File.Exists(Environment.CurrentDirectory + "\\Content\\" + "NewSpriteFont" + ".xnb"))
                content.Add("largeFont", Content.Load<SpriteFont>("NewSpriteFont"));
            if (File.Exists(Environment.CurrentDirectory + "\\Content\\" + "lesser" + ".xnb"))
                content.Add("mediumFont", Content.Load<SpriteFont>("lesser"));
            if (File.Exists(Environment.CurrentDirectory + "\\Content\\" + "small" + ".xnb"))
                content.Add("smallFont", Content.Load<SpriteFont>("small"));
            if (File.Exists(Environment.CurrentDirectory + "\\Content\\" + "smallest" + ".xnb"))
                content.Add("systemFont", Content.Load<SpriteFont>("smallest"));
            for (int i = 0; i < texturesLoadingList.Count; i++)
            {
                if (File.Exists(Environment.CurrentDirectory + "\\Content\\"+texturesOnStart[i]+".xnb"))
                {
                    content.Add(texturesOnStart[i], Content.Load<Texture2D>("loadInfo\\" + texturesOnStart[i]));
                }
            }
            if (File.Exists(Environment.CurrentDirectory + "\\Content\\main_cursor.xnb"))
            {
                content.Add("cursor", Content.Load<Texture2D>("main_cursor"));
            }
        }

        public static void PreLoadingMethodBeforeStart(object[] objs)
        {
            Game1Shell game = (Game1Shell)objs[0];
            game.Content.Unload();
            game.content.Clear();
            game.LoadLoadingContent();
        }

        public static void LoadingMethodBeforeStart(object[] objs)
        {
            Game1Shell game = (Game1Shell)objs[0];
            game.LoadEngineContent();
            game.LoadMainInformation();
            game.LoadMainContent();
            game.LoadTexturePack("game");
            game.GameElement.SetFon((Texture2D)game.ContentTex[game.GameElement.FonName]);
        }
        #endregion

        public static MatrixColorCombo FromAbove(IgnitusGame game, float animationProgress, Color color)
        {
            return new MatrixColorCombo(Matrix.CreateTranslation(0, -game.GraphicsDevice.Viewport.Height* (1-animationProgress), 0), color);
        }

        public void EndGameActivities ()
        {
            SaveProfile();
        }
    }
}

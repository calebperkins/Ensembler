﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Ensembler
{
    /// <summary>
    /// Provides getters and setters for "global" game properties, such as the current resolution and screen.
    /// 
    /// This class should provide high-level objects, not primitives, when possible.
    /// </summary>
    public class GameState
    {

        public GameState()
        {
            // todo: remove these!!!
            SelectedLevel = "Levels/Default";
            //SaveRequested = true;
        }

        private DataTypes.Screens _current;

        /// <summary>
        /// Saves the current screen to LastScreen and updates the screen with the provided value.
        /// </summary>
        public DataTypes.Screens CurrentScreen
        {
            get { return _current; }
            set {PreviousScreen = _current; _current = value;}
        }

        /// <summary>
        /// Stores the previous screen the player was on.
        /// </summary>
        public DataTypes.Screens PreviousScreen
        {
            get;
            private set; // use CurrentScreen, see above.
        }

        public DataTypes.LevelSummary[] Levels
        {
            get;
            set;
        }

        public string SelectedLevel
        {
            get;
            set;
        }

        // the score of the last played game
        public int Score
        {
            get;
            set;
        }

        // the combo of the last played game
        public int Combo
        {
            get;
            set;
        }

        public WorldMapController world;

        public void LoadContent(ContentManager cm)
        {
            DataTypes.GameData data = cm.Load<DataTypes.GameData>("Levels\\Index");
            CurrentScreen = data.Screen;
            PreviousScreen = data.Screen;
            if (Levels == null) // hack!
                Levels = data.Levels;
            ViewPort = new Viewport(0, 0, data.Width, data.Height);
        }

        public bool SaveRequested
        {
            get;
            set;
        }

        public InputState Input
        {
            get;
            set;
        }

        /// <summary>
        /// The screen viewport (height, width, origin)
        /// </summary>
        public Viewport ViewPort
        {
            get;
            set;
        }

        public Vector2 ScreenCenter()
        {
            return new Vector2(ViewPort.Width, ViewPort.Height)/2;
        }

        public DataTypes.GameData Serialized()
        {
            DataTypes.GameData data = new DataTypes.GameData();
            data.Screen = DataTypes.Screens.Title;
            data.Height = ViewPort.Height;
            data.Width = ViewPort.Width;
            data.Levels = Levels;
            return data;
        }
        

        public DataTypes.WorldData Serialized2()
        {
            DataTypes.WorldData data = new DataTypes.WorldData();
            data.Cities = new DataTypes.WorldData.City[world.Cities.Count];
            Models.City[] mCities = new Models.City[world.Cities.Count];
            world.Cities.CopyTo(mCities);
            for (int i = 0; i < world.Cities.Count; i++)
            {
                data.Cities[i] = mCities[i].Data;
                data.Cities[i].State = mCities[i].State;
            }
            return data;
        }

        /// <summary>
        /// Updates high score and high combo of a level. TODO: move this somewhere better
        /// </summary>
        /// <returns></returns>
        public void UpdateStats()
        {
            int index;
            // Finds index of city with SelectedLevel in Levels
            for (index = 0; index < Levels.Length; index++)
            {
                if (Levels[index].AssetName == SelectedLevel)
                {
                    break;
                }
            }

            Levels[index].HighScore = Math.Max(Levels[index].HighScore, Score);
            Levels[index].HighCombo = Math.Max(Levels[index].HighCombo, Combo);

            // value changed to true in update of WorldMapController instead, after city unlock are updated too
            //SaveRequested = true;  
        }
    }
}

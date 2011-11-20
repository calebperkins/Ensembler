using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using System;

namespace EnsemPro
{
    /// <summary>
    /// Provides getters and setters for "global" game properties, such as the current resolution and screen.
    /// 
    /// This class should provide high-level objects, not primitives, when possible.
    /// </summary>
    public class GameState
    {

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

        public void LoadContent(ContentManager cm)
        {
            DataTypes.GameData data = cm.Load<DataTypes.GameData>("Levels//Index");
            CurrentScreen = data.Screen;
            PreviousScreen = data.Screen;
            Levels = data.Levels;
            ViewPort = new Viewport(0, 0, data.Width, data.Height);
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

        /// <summary>
        /// Updates high score and high combo of a level
        /// </summary>
        /// <returns></returns>
        public void UpdateStats()
        {
            int index = 0;
            Console.WriteLine("SELECTED IS " + SelectedLevel);
            // Finds index of city with SelectedLevel in Levels
            for (int i = 0; i < Levels.Length; i++)
            {
                Console.WriteLine(i + " " + Levels[i].AssetName);
                if (Levels[i].AssetName == SelectedLevel)
                {
                    index = i;
                    break;
                }
            }

            Levels[index].HighScore = Math.Max(Levels[index].HighScore, Score);
            Levels[index].HighCombo = Math.Max(Levels[index].HighCombo, Combo);                            
        }
    }
}

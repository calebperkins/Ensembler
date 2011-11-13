using Microsoft.Xna.Framework.Content;

namespace EnsemPro
{
    public class GameModel
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

        public bool ConfirmChanged
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
        }
    }
}

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace EnsemPro
{
    public class GameModel
    {
        public DataTypes.Screens CurrentScreen
        {
            get;
            set;
        }

        public DataTypes.Screens LastScreen;

        public DataTypes.LevelSummary[] Levels
        {
            get;
            set;
        }

        public void LoadContent(ContentManager cm)
        {
            DataTypes.GameData data = cm.Load<DataTypes.GameData>("Levels//Index");
            CurrentScreen = data.Screen;
            LastScreen = data.Screen;
            Levels = data.Levels;
        }
    }
}

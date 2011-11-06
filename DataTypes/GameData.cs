using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;

namespace DataTypes
{
    public enum Screens
    {
        Title,
        SelectLevel,
        Pause,
        PlayLevel,
        FinishLevel,
        Initial,
        Dialog
    }

    public struct LevelSummary
    {
        public string Title;
        public string AssetName;
    }

    public class GameData
    {
        public Screens Screen;
        public LevelSummary[] Levels;
    }
}

using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;

namespace DataTypes
{
    public enum Screens
    {
        Title,
        LevelSelect,
        Pause,
        PlayLevel,
        Initial
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

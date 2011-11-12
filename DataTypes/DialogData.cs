using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;

namespace DataTypes
{
    public enum Locations
    {
        TestCity
    }

    public struct NewlyUnlockedSummary
    {
        public string Character;
        public string Line;
    }

    public struct UnlockedSummary
    {
        public string Character;
        public string Line;
    }

    public struct ClearedSummary
    {
        public string Character;
        public string Line;
    }

    public class DialogData
    {
        public Locations Location;
        public NewlyUnlockedSummary[] NewlyUnlocked;
        public UnlockedSummary[] Unlocked;
        public ClearedSummary[] Cleared;
    }
}

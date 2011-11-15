using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DataTypes
{
    public class WorldData
    {
        public enum CityState
        {
            Locked,
            NewlyUnlocked,
            Unlocked,
            Cleared
        }

        public struct City
        {
            public string Name;
            public Vector2 Position;
            public CityState State;
            public string ClearedDialogAsset;
            public string UnlockedDialogAsset;
            public string NewlyUnlockedDialogAsset;
        }

        public City[] Cities;
    }
}

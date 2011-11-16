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
            public uint ID;
            public uint Left;
            public uint Right;
            public uint Up;
            public uint Down;
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

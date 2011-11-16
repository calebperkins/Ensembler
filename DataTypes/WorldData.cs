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
            // newly added ones
            public string playLevel;
            public int combo_req;
            public int score_req;
            // the nodes that gets unlocked after this level is succeeded
            // if not applicable, then input 0;
            public int unlock1;
            public int unlock2;
            public int unlock3;
            // whether this city is one of the big six city (i.e. whether or not unlocking this city will help advance
            // its progress in unlocking the crazy final level + unlocking the path to success
            public bool big_six;
        }

        public City[] Cities;
    }
}

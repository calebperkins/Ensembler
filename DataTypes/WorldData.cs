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
            public string SuccessAsset;

            public string PlayLevel;
            public int ComboReq;
            public int ScoreReq;
            // the nodes that gets unlocked after this level is succeeded
            // if not applicable, then input 0;
            public uint Unlock1;
            public uint Unlock2;
            public uint Unlock3;
            // whether this city is one of the big six city (i.e. whether or not unlocking this city will help advance
            // its progress in unlocking the crazy final level + unlocking the path to success
            public bool BigSix;
        }

        public City[] Cities;
    }
}

using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace DataTypes
{
    public struct MovementData
    {
        public string Kind;
        public int ShowBeat;
        public int StartBeat;
        public int EndBeat;
        public int FadeBeat;

        [ContentSerializer(Optional = true)]
        public int StartX;

        [ContentSerializer(Optional = true)]
        public int StartY;

        [ContentSerializer(Optional = true)]
        public int EndX;

        [ContentSerializer(Optional = true)]
        public int EndY;

        [ContentSerializer(Optional = true)]
        public float A;
    }

    public class LevelData
    {
        public string Title;
        public int BPM;
        public string SongAssetName;
        public string Artist;
        public string Background;
        public List<MovementData> Movements;
    }
}

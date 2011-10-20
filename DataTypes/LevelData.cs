using System.Collections.Generic;

namespace DataTypes
{
    public struct MovementData
    {
        public string Kind;
        public int StartBeat;
        public int EndBeat;
        public int ShowBeat;
        public int FadeBeat;
        public int StartX;
        public int StartY;
        public int EndX;
        public int EndY;
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

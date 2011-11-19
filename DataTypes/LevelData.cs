using System.Collections.Generic;
using System.Diagnostics;
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

        [ContentSerializer(Optional = true)]
        public int NewBPM;

        /// <summary>
        /// Run sanity checks to make sure this is a valid movement.
        /// </summary>
        public void AssertValid()
        {
            Debug.Assert(ShowBeat > 0, ToString(), "ShowBeat must > 0");
            Debug.Assert(StartBeat > 0, ToString(), "StartBeat must > 0");
            Debug.Assert(EndBeat > 0, ToString(), "EndBeat must > 0");
            Debug.Assert(FadeBeat > 0, ToString(), "FadeBeat must > 0");
            Debug.Assert(ShowBeat <= StartBeat, ToString(), "ShowBeat <= StartBeat");
            Debug.Assert((StartBeat < EndBeat || Kind == "Control"), ToString(), "StartBeat must < EndBeat if not a control type");
            // Doesn't seem to be working
            //Debug.Assert(EndBeat <= FadeBeat, ToString(), "EndBeat must <= FadeBeat");
        }

        public override string ToString()
        {
            return Kind + "/" + StartBeat;
        }
    }

    public class LevelData
    {
        public string Title;
        public int BPM;
        public string SongAssetName;
        public string SatisfactionAssetName;
        public string Artist;
        public string Background;
        public List<MovementData> Movements;
    }
}

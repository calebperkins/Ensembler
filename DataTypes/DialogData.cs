using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;

namespace DataTypes
{
    public struct ContentSummary
    {
        public string Character;
        public string Line;
    }

    public struct ColorSummary
    {
        public string Character;
        public string Color;
    }

    public struct FaceTexturePath
    {
        public string FaceAssetName;
    }

    public struct CutSceneSummary
    {
        public string Picture;
        public string StartCue;
        public string StopCue;
    }
    
    public class DialogData
    {
        public ContentSummary[] Content;
        public ColorSummary[] Colors;
        public FaceTexturePath[] Faces;
        public CutSceneSummary[] CutScenes;
    }
}

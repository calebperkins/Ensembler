using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;

namespace DataTypes
{
    public struct ContentSummary
    {
        public string Character;
        public string Line;
       // public string Color;
    }
    
    public class DialogData
    {
        public ContentSummary[] Content;
    }
}

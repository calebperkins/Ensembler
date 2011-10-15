using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace EnsemPro
{
    public class LevelParser
    {
        // http://support.microsoft.com/kb/307548
        /*
         * XmlDocument represents the contents of an xml file. When loading it 
         * from a file, you read the entire file into memory.
         * 
         * XmlReader is an abstract class able to read from xml data. It can read 
         * from a file, from an internet location, or from any other stream of data. 
         * When reading from a file, you don't load the entire document at once.
         * 
         * XmlTextReader is an implementation of XmlReader able to read xml from a file. 
         * It's recommended to use XmlReader.Create(file) instead of instantiating an XmlTextReader.
         */
        public static PlayLevel getLevel(ContentManager content, String path)
        {
            return new PlayLevel();
        }
        



    }
}

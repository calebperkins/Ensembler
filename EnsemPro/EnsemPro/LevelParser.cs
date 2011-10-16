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
        public static void getLevel(ContentManager content, String path)
        {
            LinkedList<Movement> moves = new LinkedList<Movement>();
            Texture2D background;
            Song song;
            int bpm;

            XmlTextReader reader = new XmlTextReader(path);
            
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        if (reader.Name == "background")
                        {
                            reader.Read();
                            background = content.Load<Texture2D>(reader.Value);
                            Console.WriteLine(reader.Value);
                        }
                        else if (reader.Name == "song")
                        {
                            reader.Read();
                            song = content.Load<Song>(reader.Value);
                            Console.WriteLine(reader.Value);
                        }
                        else if (reader.Name == "bpm")
                        {
                            reader.Read();
                            bpm = Convert.ToInt32 (reader.Value);
                            Console.WriteLine("bpm " + bpm);
                        }
                        else if (reader.Name == "Movement")
                        {

                        }

                        //while (reader.MoveToNextAttribute()) // Read the attributes.
                            //Console.Write(" " + reader.Name + "='" + reader.Value + "'");
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        //Console.WriteLine(reader.Value);
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        //Console.Write("</" + reader.Name);
                        //Console.WriteLine(">");
                        break;
                }
            }

        }


    }
}

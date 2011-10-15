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
            PlayLevel level = new PlayLevel();
            //XmlReader reader = XmlReader.Create("first.xml");
            //XmlDocument doc = new XmlDocument();
            //doc.Load("first.xml");

            XmlTextReader reader = new XmlTextReader("first.xml");

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        Console.Write("<" + reader.Name);

                        //while (reader.MoveToNextAttribute()) // Read the attributes.
                        //    Console.Write(" " + reader.Name + "='" + reader.Value + "'");
                        Console.WriteLine(">");
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        Console.WriteLine(reader.Value);
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        Console.Write("</" + reader.Name);
                        Console.WriteLine(">");
                        break;
                }
            }
            //Console.ReadLine();

            return level;
        }

        public static void lol()
        {
            //LinkedList<Movement> moves = new LinkedList<Movement>();
            Function f1 = null;
            Movement move1 = new Movement(Movement.Type.Wave, 1, 4, 1, 4, new Point(0, 0), new Point(4, 4), f1);
            f1 = new Function(Function.Type.Parabola, move1, 60, 1, new Point (0,0));
        }


    }
}

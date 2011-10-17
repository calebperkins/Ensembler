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
        public static LinkedList<Movement> getLevel(ContentManager content, String path)
        {
            LinkedList<Movement> moves = new LinkedList<Movement>();
            Texture2D background = null;
            Song song = null;
            int bpm = 0;

            Movement.Type type = Movement.Type.Nonsense;
            int startBeat = 0;
            int endBeat = 0;
            int showBeat = 0;
            int fadeBeat = 0;
            int startCoordinateX = 0;
            int startCoordinateY = 0;
            int endCoordinateX = 0;
            int endCoordinateY = 0;
            float amplitude = 0;

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
                            //Console.WriteLine(reader.Value);
                        }
                        else if (reader.Name == "song")
                        {
                            reader.Read();
                            song = content.Load<Song>(reader.Value);
                            //Console.WriteLine(reader.Value);
                        }
                        else if (reader.Name == "bpm")
                        {
                            reader.Read();
                            bpm = Convert.ToInt32 (reader.Value);
                            //Console.WriteLine("bpm " + bpm);
                        }
                        else if (reader.Name == "Movement")
                        {
                            reader.MoveToNextAttribute();
                            if (reader.Value == "wave") type = Movement.Type.Wave;
                            else if (reader.Value == "shake") type = Movement.Type.Shake;
                            else if (reader.Value == "noop") type = Movement.Type.Noop;
                            //Console.WriteLine(reader.Value);
                        }
                        else if (reader.Name == "showBeat")
                        {
                            reader.Read();
                            showBeat = Convert.ToInt32(reader.Value);
                            //Console.WriteLine(reader.Value);
                        }
                        else if (reader.Name == "startBeat")
                        {
                            reader.Read();
                            startBeat = Convert.ToInt32(reader.Value);
                            //Console.WriteLine(reader.Value);
                        }
                        else if (reader.Name == "endBeat")
                        {
                            reader.Read();
                            endBeat = Convert.ToInt32(reader.Value);
                            //Console.WriteLine(reader.Value);
                        }
                        else if (reader.Name == "fadeBeat")
                        {
                            reader.Read();
                            fadeBeat = Convert.ToInt32(reader.Value);
                            //Console.WriteLine(reader.Value);
                        }
                        else if (reader.Name == "startCoordinateX")
                        {
                            reader.Read();
                            startCoordinateX = Convert.ToInt32(reader.Value);
                            //Console.WriteLine(reader.Value);
                        }
                        else if (reader.Name == "startCoordinateY")
                        {
                            reader.Read();
                            startCoordinateY = Convert.ToInt32(reader.Value);
                            //Console.WriteLine(reader.Value);
                        }
                        else if (reader.Name == "endCoordinateX")
                        {
                            reader.Read();
                            endCoordinateX = Convert.ToInt32(reader.Value);
                            //Console.WriteLine(reader.Value);
                        }
                        else if (reader.Name == "endCoordinateY")
                        {
                            reader.Read();
                            endCoordinateY = Convert.ToInt32(reader.Value);
                            //Console.WriteLine(reader.Value);
                        }
                        else if (reader.Name == "amplitude")
                        {
                            reader.Read();
                            amplitude = (float) Convert.ToDouble(reader.Value);
                            //Console.WriteLine(reader.Value);
                        }

                        break;
                    case XmlNodeType.Text:
                        break;
                    case XmlNodeType.EndElement:
                        if (reader.Name == "Movement")
                        {
                            
                            Movement move = null;
                            if (type == Movement.Type.Wave)
                            {
                                Function function = new Function();
                                move = new Movement(type, showBeat, startBeat, endBeat, fadeBeat,
                                    new Point(startCoordinateX, startCoordinateY),
                                    new Point(endCoordinateX, endCoordinateY), function);
                                function.InitializeCurve(Function.Type.Curve, move, bpm, amplitude);

                            }
                            else 
                            { 
                                move = new Movement(type, showBeat, startBeat, endBeat, fadeBeat); 
                            }

                            
                            moves.AddLast(move);

                            if (type == Movement.Type.Nonsense) Console.WriteLine("NONSENSE");
                            if (type == Movement.Type.Noop) Console.WriteLine(type + " " + showBeat + " " + startBeat + " " + endBeat + " " + fadeBeat);
                            if (type == Movement.Type.Shake) Console.WriteLine(type + " " + showBeat + " " + startBeat + " " + endBeat + " " + fadeBeat);
                            if (type == Movement.Type.Wave) Console.WriteLine(type + " " + showBeat + " " + startBeat + " " + endBeat + " " + fadeBeat + " / " + startCoordinateX + " " + startCoordinateY + " " + endCoordinateX + " " + endCoordinateY + " " + amplitude);
                        }
                        else if (reader.Name == "root")
                        {
                            Console.WriteLine(background);
                            Console.WriteLine(song);
                            Console.WriteLine(bpm);
                        }
                        break;
                }
            }
            Console.WriteLine(moves.Count);
            return moves;

        }


    }
}

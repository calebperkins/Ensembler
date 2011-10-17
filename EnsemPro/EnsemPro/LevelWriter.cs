using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace EnsemPro
{
    public class LevelWriter
    {
        static String background;
        static String song;
        static String bpm;

        static String movementType; // {shake, wave, noop}
        static String startBeat;    // the actual time of beat
        static String endBeat;
        static String showBeat;     // when to show warning
        static String fadeBeat;
        static String startCoordinateX; // assume origin is bottom left
        static String startCoordinateY;
        static String endCoordinateX;
        static String endCoordinateY;
        static String amplitude;
        static String fileName = "b5.xml";

        public static void writeLevel()
        {
            start();

            background = "images\\background";
            song = "images\\b5complete";
            bpm = "100"; // forgot the bpm of b5. please fill it in for me
            writeAtributes();

            movementType = "shake";
            startBeat = "1";
            endBeat = "7";
            showBeat = "1";
            fadeBeat = "7";
            writeMovement();
            /*
            movementType = "noop";
            startBeat = "8";
            endBeat = "9";
            showBeat = "8";
            fadeBeat = "9";
            writeMovement();
            */
            movementType = "shake";
            startBeat = "9";
            endBeat = "15";
            showBeat = "9";
            fadeBeat = "15";
            writeMovement();
            /*
            movementType = "noop";
            startBeat = "16";
            endBeat = "18";
            showBeat = "16";
            fadeBeat = "17";
            writeMovement();
            */
            movementType = "wave";
            showBeat = "17";
            startBeat = "18";
            endBeat = "19";
            fadeBeat = "20";
            startCoordinateX = "400"; // only fill this out if movement is wave
            startCoordinateY = "400"; // only fill this out if movement is wave
            endCoordinateX = "400"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "18";
            startBeat = "19";
            endBeat = "20";
            fadeBeat = "21";
            startCoordinateX = "400"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "19";
            startBeat = "20";
            endBeat = "21";
            fadeBeat = "22";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "20";
            startBeat = "21";
            endBeat = "22";
            fadeBeat = "23";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "400"; // only fill this out if movement is wave
            endCoordinateY = "400"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            //ADD MORE MOVEMENTS HERE

            end();
        }

        public static void start()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
                file.WriteLine("<?xml version='1.0'?>");
                file.WriteLine("<root>");
            }
        }

        public static void end()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true))
            {
                file.WriteLine("</root>");
            }
        }

        public static void writeAtributes()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true))
            {
                file.WriteLine("\t<background>" + background + "</background>");
                file.WriteLine("\t<song>" + song + "</song>");
                file.WriteLine("\t<bpm>" + bpm + "</bpm>");
                file.WriteLine();
            }
        }


        public static void writeMovement()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true))
            {
                file.WriteLine("\t<Movement type=\"" + movementType + "\">");
                file.WriteLine("\t\t<showBeat>" + showBeat + "</showBeat>");
                file.WriteLine("\t\t<startBeat>" + startBeat + "</startBeat>");
                file.WriteLine("\t\t<endBeat>" + endBeat + "</endBeat>");
                file.WriteLine("\t\t<fadeBeat>" + fadeBeat + "</fadeBeat>");
                if (movementType == "wave")
                {
                    file.WriteLine("\t\t<startCoordinateX>" + startCoordinateX + "</startCoordinateX>");
                    file.WriteLine("\t\t<startCoordinateY>" + startCoordinateY + "</startCoordinateY>");
                    file.WriteLine("\t\t<endCoordinateX>" + endCoordinateX + "</endCoordinateX>");
                    file.WriteLine("\t\t<endCoordinateY>" + endCoordinateY + "</endCoordinateY>");
                    file.WriteLine("\t\t<amplitude>" + amplitude + "</amplitude>");
                }
                file.WriteLine("\t</Movement>");
            }
        }
    }
}

/* TEMPLATE TO COPY FOR MOVEMENT
movementType = "";
showBeat = "";
startBeat = "";
endBeat = "";
fadeBeat = "";
startCoordinateX = ""; // only fill this out if movement is wave
startCoordinateY = ""; // only fill this out if movement is wave
endCoordinateX = ""; // only fill this out if movement is wave
endCoordinateY = ""; // only fill this out if movement is wave
amplitude = ""; // only fill this out if movement is wave
writeMovement();
*/

/* EXAMPLE OF A WAVE MOVEMENT
movementType = "wave";
startBeat = "1";
endBeat = "3";
showBeat = "2";
fadeBeat = "4";
startCoordinateX = "12"; // only fill this out if movement is wave
startCoordinateY = "12"; // only fill this out if movement is wave
endCoordinateX = "23"; // only fill this out if movement is wave
endCoordinateY = "23"; // only fill this out if movement is wave
amplitude = "6"; // only fill this out if movement is wave
writeMovement();
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public static void writeLevel()
        {
            start();

            background = "images\\background";
            song = "images\\b5complete";
            bpm = "100"; // forgot the bpm of b5. please fill it in for me
            writeAtributes();

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

            //ADD MORE MOVEMENTS HERE

            end();
        }

        public static void start()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"result.txt"))
            {
                file.WriteLine("<?xml version='1.0'?>");
                file.WriteLine("<root>");
            }
        }

        public static void end()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"result.txt", true))
            {
                file.WriteLine("</root>");
            }
        }

        public static void writeAtributes()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"result.txt", true))
            {
                file.WriteLine("\t<background>" + background + "</background>");
                file.WriteLine("\t<song>" + song + "</song>");
                file.WriteLine("\t<bpm>" + bpm + "</bpm>");
                file.WriteLine();
            }
        }


        public static void writeMovement()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"result.txt", true))
            {
                file.WriteLine("\t<Movement type=\"" + movementType + "\">");
                file.WriteLine("\t\t<startBeat>" + startBeat + "</startBeat>");
                file.WriteLine("\t\t<endBeat>" + endBeat + "</endBeat>");
                file.WriteLine("\t\t<showBeat>" + showBeat + "</showBeat>");
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
startBeat = "";
endBeat = "";
showBeat = "";
fadeBeat = "4";
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


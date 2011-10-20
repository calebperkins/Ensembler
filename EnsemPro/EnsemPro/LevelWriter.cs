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
        static String fileName = "b5.xml";

        public static void writeLevel()
        {
            start();

            background = "images\\background";
            song = "images\\b5complete";
            bpm = "100"; // forgot the bpm of b5. please fill it in for me
            writeAtributes();

            /*
            movementType = "wave";
            showBeat = "1";
            startBeat = "1";
            endBeat = "30";
            fadeBeat = "30";
            startCoordinateX = ""; // only fill this out if movement is wave
            startCoordinateY = ""; // only fill this out if movement is wave
            endCoordinateX = ""; // only fill this out if movement is wave
            endCoordinateY = ""; // only fill this out if movement is wave
            amplitude = ""; // only fill this out if movement is wave
            writeMovement();
            */

            movementType = "shake";
            showBeat = "1";
            startBeat = "1";
            endBeat = "7";
            fadeBeat = "7";
            writeMovement();

            movementType = "noop";
            showBeat = "8";
            startBeat = "8";
            endBeat = "9";
            fadeBeat = "9";
            writeMovement();

            movementType = "shake";
            showBeat = "9";
            startBeat = "9";
            endBeat = "15";
            fadeBeat = "15";
            writeMovement();

            movementType = "noop";
            showBeat = "16";
            startBeat = "16";
            endBeat = "18";
            fadeBeat = "17";
            writeMovement();

            // When first wave starts
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

            movementType = "wave";
            showBeat = "21";
            startBeat = "22";
            endBeat = "23";
            fadeBeat = "24";
            startCoordinateX = "400"; // only fill this out if movement is wave
            startCoordinateY = "400"; // only fill this out if movement is wave
            endCoordinateX = "400"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "22";
            startBeat = "23";
            endBeat = "24";
            fadeBeat = "25";
            startCoordinateX = "400"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "23";
            startBeat = "24";
            endBeat = "25";
            fadeBeat = "26";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "24";
            startBeat = "25";
            endBeat = "26";
            fadeBeat = "27";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "400"; // only fill this out if movement is wave
            endCoordinateY = "400"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();


            movementType = "wave";
            showBeat = "25";
            startBeat = "26";
            endBeat = "27";
            fadeBeat = "28";
            startCoordinateX = "400"; // only fill this out if movement is wave
            startCoordinateY = "400"; // only fill this out if movement is wave
            endCoordinateX = "400"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "26";
            startBeat = "27";
            endBeat = "28";
            fadeBeat = "29";
            startCoordinateX = "400"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "27";
            startBeat = "28";
            endBeat = "29";
            fadeBeat = "30";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "28";
            startBeat = "29";
            endBeat = "30";
            fadeBeat = "31";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "400"; // only fill this out if movement is wave
            endCoordinateY = "400"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "29";
            startBeat = "30";
            endBeat = "31";
            fadeBeat = "32";
            startCoordinateX = "400"; // only fill this out if movement is wave
            startCoordinateY = "400"; // only fill this out if movement is wave
            endCoordinateX = "400"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "30";
            startBeat = "31";
            endBeat = "32";
            fadeBeat = "33";
            startCoordinateX = "400"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "31";
            startBeat = "32";
            endBeat = "33";
            fadeBeat = "34";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "noop";
            showBeat = "34";
            startBeat = "34";
            endBeat = "35";
            fadeBeat = "35";
            writeMovement();

            movementType = "shake";
            showBeat = "36";
            startBeat = "36";
            endBeat = "41";
            fadeBeat = "41";
            writeMovement();

            movementType = "wave";
            showBeat = "41";
            startBeat = "42";
            endBeat = "43";
            fadeBeat = "44";
            startCoordinateX = "400"; // only fill this out if movement is wave
            startCoordinateY = "400"; // only fill this out if movement is wave
            endCoordinateX = "400"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "42";
            startBeat = "43";
            endBeat = "44";
            fadeBeat = "45";
            startCoordinateX = "400"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "43";
            startBeat = "44";
            endBeat = "45";
            fadeBeat = "46";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "44";
            startBeat = "45";
            endBeat = "46";
            fadeBeat = "47";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "400"; // only fill this out if movement is wave
            endCoordinateY = "400"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "45";
            startBeat = "46";
            endBeat = "47";
            fadeBeat = "48";
            startCoordinateX = "400"; // only fill this out if movement is wave
            startCoordinateY = "400"; // only fill this out if movement is wave
            endCoordinateX = "400"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "46";
            startBeat = "47";
            endBeat = "48";
            fadeBeat = "49";
            startCoordinateX = "400"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();


            movementType = "wave";
            showBeat = "47";
            startBeat = "48";
            endBeat = "49";
            fadeBeat = "50";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "48";
            startBeat = "49";
            endBeat = "50";
            fadeBeat = "51";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "400"; // only fill this out if movement is wave
            endCoordinateY = "400"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "48";
            startBeat = "50";
            endBeat = "51";
            fadeBeat = "52";
            startCoordinateX = "400"; // only fill this out if movement is wave
            startCoordinateY = "400"; // only fill this out if movement is wave
            endCoordinateX = "100"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "50";
            startBeat = "51";
            endBeat = "52";
            fadeBeat = "53";
            startCoordinateX = "100"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "700"; // only fill this out if movement is wave
            endCoordinateY = "400"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "51";
            startBeat = "52";
            endBeat = "53";
            fadeBeat = "54";
            startCoordinateX = "700"; // only fill this out if movement is wave
            startCoordinateY = "400"; // only fill this out if movement is wave
            endCoordinateX = "100"; // only fill this out if movement is wave
            endCoordinateY = "300"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "52";
            startBeat = "53";
            endBeat = "54";
            fadeBeat = "54";
            startCoordinateX = "100"; // only fill this out if movement is wave
            startCoordinateY = "300"; // only fill this out if movement is wave
            endCoordinateX = "700"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "53";
            startBeat = "54";
            endBeat = "55";
            fadeBeat = "56";
            startCoordinateX = "700"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "400"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "54";
            startBeat = "55";
            endBeat = "56";
            fadeBeat = "57";
            startCoordinateX = "400"; // only fill this out if movement is wave
            startCoordinateY = "100"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "55";
            startBeat = "56";
            endBeat = "57";
            fadeBeat = "58";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "300"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "56";
            startBeat = "57";
            endBeat = "58";
            fadeBeat = "59";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "300"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "400"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "57";
            startBeat = "58";
            endBeat = "59";
            fadeBeat = "60";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "400"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "58";
            startBeat = "59";
            endBeat = "60";
            fadeBeat = "61";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "600"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "shake";
            showBeat = "60";
            startBeat = "60";
            endBeat = "62";
            fadeBeat = "62";
            writeMovement();

            movementType = "noop";
            showBeat = "63";
            startBeat = "63";
            endBeat = "64";
            fadeBeat = "64";
            writeMovement();

            movementType = "shake";
            showBeat = "64";
            startBeat = "64";
            endBeat = "66";
            fadeBeat = "66";
            writeMovement();

            movementType = "noop";
            showBeat = "66";
            startBeat = "67";
            endBeat = "68";
            fadeBeat = "68";
            writeMovement();


            movementType = "shake";
            showBeat = "68";
            startBeat = "68";
            endBeat = "70";
            fadeBeat = "70";
            writeMovement();

            movementType = "wave";
            showBeat = "70";
            startBeat = "72";
            endBeat = "74";
            fadeBeat = "75";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "400"; // only fill this out if movement is wave
            endCoordinateY = "600"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "73";
            startBeat = "74";
            endBeat = "76";
            fadeBeat = "76";
            startCoordinateX = "400"; // only fill this out if movement is wave
            startCoordinateY = "600"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "76";
            startBeat = "79";
            endBeat = "80";
            fadeBeat = "81";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "70"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "78";
            startBeat = "80";
            endBeat = "81";
            fadeBeat = "82";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "100"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "50"; // only fill this out if movement is wave
            writeMovement();


            movementType = "wave";
            showBeat = "79";
            startBeat = "81";
            endBeat = "82";
            fadeBeat = "83";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "-70"; // only fill this out if movement is wave
            writeMovement();


            movementType = "wave";
            showBeat = "80";
            startBeat = "82";
            endBeat = "83";
            fadeBeat = "84";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "100"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "-50"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "82";
            startBeat = "83";
            endBeat = "84";
            fadeBeat = "85";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "70"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "83";
            startBeat = "84";
            endBeat = "85";
            fadeBeat = "86";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "100"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "50"; // only fill this out if movement is wave
            writeMovement();


            movementType = "wave";
            showBeat = "84";
            startBeat = "85";
            endBeat = "86";
            fadeBeat = "87";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "-70"; // only fill this out if movement is wave
            writeMovement();


            movementType = "wave";
            showBeat = "85";
            startBeat = "86";
            endBeat = "87";
            fadeBeat = "88";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "100"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "-50"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "86";
            startBeat = "87";
            endBeat = "88";
            fadeBeat = "89";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "70"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "87";
            startBeat = "88";
            endBeat = "89";
            fadeBeat = "90";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "100"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "50"; // only fill this out if movement is wave
            writeMovement();


            movementType = "wave";
            showBeat = "88";
            startBeat = "89";
            endBeat = "90";
            fadeBeat = "91";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "-70"; // only fill this out if movement is wave
            writeMovement();


            movementType = "wave";
            showBeat = "89";
            startBeat = "90";
            endBeat = "91";
            fadeBeat = "92";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "100"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "-50"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "90";
            startBeat = "91";
            endBeat = "92";
            fadeBeat = "95";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "91";
            startBeat = "91";
            endBeat = "93";
            fadeBeat = "96";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "500"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "92";
            startBeat = "94";
            endBeat = "95";
            fadeBeat = "97";
            startCoordinateX = "500"; // only fill this out if movement is wave
            startCoordinateY = "100"; // only fill this out if movement is wave
            endCoordinateX = "400"; // only fill this out if movement is wave
            endCoordinateY = "600"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "93";
            startBeat = "95";
            endBeat = "96";
            fadeBeat = "98";
            startCoordinateX = "400"; // only fill this out if movement is wave
            startCoordinateY = "600"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "94";
            startBeat = "96";
            endBeat = "98";
            fadeBeat = "100";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "100"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "97";
            startBeat = "99";
            endBeat = "100";
            fadeBeat = "101";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "99";
            startBeat = "100";
            endBeat = "101";
            fadeBeat = "101";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "100";
            startBeat = "101";
            endBeat = "102";
            fadeBeat = "104";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "100"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "101";
            startBeat = "102";
            endBeat = "103";
            fadeBeat = "104";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "100"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "102";
            startBeat = "103";
            endBeat = "104";
            fadeBeat = "105";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "200"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "103";
            startBeat = "104";
            endBeat = "105";
            fadeBeat = "106";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "200"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "300"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "104";
            startBeat = "105";
            endBeat = "106";
            fadeBeat = "107";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "300"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "300"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "105";
            startBeat = "106";
            endBeat = "107";
            fadeBeat = "108";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "300"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "400"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "106";
            startBeat = "107";
            endBeat = "108";
            fadeBeat = "109";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "400"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "400"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "107";
            startBeat = "108";
            endBeat = "109";
            fadeBeat = "110";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "400"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "108";
            startBeat = "109";
            endBeat = "110";
            fadeBeat = "111";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "109";
            startBeat = "110";
            endBeat = "111";
            fadeBeat = "112";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "100"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "70"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "110";
            startBeat = "111";
            endBeat = "112";
            fadeBeat = "113";
            startCoordinateX = "100"; // only fill this out if movement is wave
            startCoordinateY = "100"; // only fill this out if movement is wave
            endCoordinateX = "700"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "111";
            startBeat = "112";
            endBeat = "113";
            fadeBeat = "114";
            startCoordinateX = "700"; // only fill this out if movement is wave
            startCoordinateY = "100"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "-70"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "112";
            startBeat = "113";
            endBeat = "114";
            fadeBeat = "115";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "113";
            startBeat = "114";
            endBeat = "115";
            fadeBeat = "116";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "100"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "70"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "114";
            startBeat = "115";
            endBeat = "116";
            fadeBeat = "117";
            startCoordinateX = "100"; // only fill this out if movement is wave
            startCoordinateY = "100"; // only fill this out if movement is wave
            endCoordinateX = "700"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "115";
            startBeat = "116";
            endBeat = "117";
            fadeBeat = "118";
            startCoordinateX = "700"; // only fill this out if movement is wave
            startCoordinateY = "100"; // only fill this out if movement is wave
            endCoordinateX = "200"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "-70"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "116";
            startBeat = "117";
            endBeat = "118";
            fadeBeat = "119";
            startCoordinateX = "200"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "600"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "117";
            startBeat = "118";
            endBeat = "119";
            fadeBeat = "120";
            startCoordinateX = "600"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "100"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "70"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "118";
            startBeat = "119";
            endBeat = "120";
            fadeBeat = "121";
            startCoordinateX = "100"; // only fill this out if movement is wave
            startCoordinateY = "100"; // only fill this out if movement is wave
            endCoordinateX = "700"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "119";
            startBeat = "120";
            endBeat = "121";
            fadeBeat = "122";
            startCoordinateX = "700"; // only fill this out if movement is wave
            startCoordinateY = "100"; // only fill this out if movement is wave
            endCoordinateX = "700"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "120";
            startBeat = "121";
            endBeat = "122";
            fadeBeat = "123";
            startCoordinateX = "700"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "100"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "121";
            startBeat = "122";
            endBeat = "123";
            fadeBeat = "124";
            startCoordinateX = "100"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "100"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "122";
            startBeat = "123";
            endBeat = "124";
            fadeBeat = "125";
            startCoordinateX = "100"; // only fill this out if movement is wave
            startCoordinateY = "100"; // only fill this out if movement is wave
            endCoordinateX = "700"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "123";
            startBeat = "124";
            endBeat = "125";
            fadeBeat = "126";
            startCoordinateX = "700"; // only fill this out if movement is wave
            startCoordinateY = "100"; // only fill this out if movement is wave
            endCoordinateX = "700"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();


            movementType = "wave";
            showBeat = "124";
            startBeat = "125";
            endBeat = "126";
            fadeBeat = "127";
            startCoordinateX = "700"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "100"; // only fill this out if movement is wave
            endCoordinateY = "500"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "shake";
            showBeat = "126";
            startBeat = "126";
            endBeat = "128";
            fadeBeat = "128";
            writeMovement();

            movementType = "noop";
            showBeat = "128";
            startBeat = "128";
            endBeat = "131";
            fadeBeat = "131";
            writeMovement();

            movementType = "shake";
            showBeat = "131";
            startBeat = "131";
            endBeat = "133";
            fadeBeat = "133";
            writeMovement();

            movementType = "noop";
            showBeat = "133";
            startBeat = "133";
            endBeat = "134";
            fadeBeat = "134";
            writeMovement();

            movementType = "wave";
            showBeat = "134";
            startBeat = "135";
            endBeat = "137";
            fadeBeat = "138";
            startCoordinateX = "700"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "100"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "70"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "136";
            startBeat = "137";
            endBeat = "138";
            fadeBeat = "139";
            startCoordinateX = "100"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "700"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "-70"; // only fill this out if movement is wave
            writeMovement();

            movementType = "wave";
            showBeat = "137";
            startBeat = "138";
            endBeat = "139";
            fadeBeat = "140";
            startCoordinateX = "400"; // only fill this out if movement is wave
            startCoordinateY = "500"; // only fill this out if movement is wave
            endCoordinateX = "400"; // only fill this out if movement is wave
            endCoordinateY = "100"; // only fill this out if movement is wave
            amplitude = "0"; // only fill this out if movement is wave
            writeMovement();

            movementType = "noop";
            showBeat = "140";
            startBeat = "140";
            endBeat = "141";
            fadeBeat = "141";
            writeMovement();

            //level ends



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

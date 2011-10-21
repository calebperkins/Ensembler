using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace EnsemPro
{
    public class Function
    {

        public enum Types
        {
            Line,
            Curve
        }

        const float INTERVAL_TIME = 1.0f / 60; // Time of each frame in seconds
        Movement movement;

        /// <summary>
        /// The number of intervals
        /// </summary>
        public int Size
        {
            get;
            private set;
        }

        public Vector2[] Positions
        {
            get;
            private set;
        }

        public float[] Slopes
        {
            get;
            private set;
        }

        public Function()
        {
        }

        public Types Form
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor for Line
        /// Line defined by startCoordinate and endCoordinate of movement
        /// </summary>
        /// <param name="type"></param>
        /// <param name="movement"></param>
        public void InitializeLine(Types type, Movement movement, int bpm)
        {
            if (type != Types.Line) Debug.WriteLine("Line constructor called with type that is not line");

            // Same code as constructor for parabola
            Form = type;
            this.movement = movement;
            Size = (int)((movement.endBeat - movement.startBeat + 1) / (float)bpm * 60 / INTERVAL_TIME);
            //Console.WriteLine("INTERVAL_TIME " + INTERVAL_TIME);
            //Console.WriteLine("Size " + Size);

            Positions = new Vector2[Size];
            Slopes = new float[Size];

            float incre = (movement.endCoordinate.X - movement.startCoordinate.X) / (float)Size;
            float curX = movement.startCoordinate.X;
            // Same code as constructor for parabola

            float curY = movement.startCoordinate.Y;
            float slope = (movement.endCoordinate.Y - movement.startCoordinate.Y) /
                (movement.endCoordinate.X - movement.startCoordinate.X);

            for (int i = 0; i < Size; i++)
            {
                Positions[i].X = curX;
                Positions[i].Y = curY;
                Slopes[i] = slope;
                curX += incre;
                curY += incre * slope;

                Console.WriteLine(Positions[i].X + " " + Positions[i].Y + " " + Slopes[i]);
            }
        }

        /// <summary>
        /// Draws a curve between startCoordinate and endCoordinate of movement according to the max offset,
        /// specified by amp. Drawn using a sine function with rotation
        /// Note that both Positions and slope have length of number of intervals + 1. This is for drawing purpose only.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="movement"></param>
        /// <param name="bpm"></param>
        /// <param name="amp"></param>
        public void InitializeCurve(Types type, Movement movement, int bpm, float amp)
        {
            if (type != Types.Curve) Debug.WriteLine("Curve constructor called with type that is not Curve");

            Form = type;
            this.movement = movement;
            Size = (int)((movement.endBeat - movement.startBeat + 1) / (float)bpm * 60 / INTERVAL_TIME);
            Positions = new Vector2[Size + 1];
            Slopes = new float[Size + 1];

            // Change coordinates so that (0,0) is bottom left
            float oStartX = movement.startCoordinate.X;
            float oStartY = movement.startCoordinate.Y;
            float oEndX = movement.endCoordinate.X;
            float oEndY = movement.endCoordinate.Y;

            float curX = 0;

            float length = Vector2.Distance(new Vector2(oStartX, oStartY), new Vector2(oEndX, oEndY));
            float incre = length / Size;
            float k = (float)(Math.PI / length);

            for (int i = 0; i < (Size + 1); i++)
            {
                // Unrotated coordinate and slope
                float pX = curX + oStartX;
                float pY = (float)(amp * Math.Sin(k * curX) + oStartY);
                float pSlope = (float)(amp * k * Math.Cos(k * curX));

                float yInt = pY - pSlope * pX;

                // Rotation angle
                double theta = Math.Atan((oEndY - oStartY) / (oEndX - oStartX));
                if (oEndX < oStartX) theta += Math.PI;

                // Perform rotation
                float rX = (float)(Math.Cos(theta) * (pX - oStartX) - Math.Sin(theta) * (pY - oStartY) + oStartX);
                float rY = (float)(Math.Sin(theta) * (pX - oStartX) + Math.Cos(theta) * (pY - oStartY) + oStartY);
                float oX = (float)(Math.Cos(theta) * (0 - oStartX) - Math.Sin(theta) * (yInt - oStartY) + oStartX);
                float oY = (float)(Math.Sin(theta) * (0 - oStartX) + Math.Cos(theta) * (yInt - oStartY) + oStartY);

                Positions[i].X = rX;
                Positions[i].Y = GameEngine.HEIGHT - rY;
                Slopes[i] = (rY - oY) / (rX - oX);

                curX += incre;
            }
        }
    }
}

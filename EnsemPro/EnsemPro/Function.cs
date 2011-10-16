using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace EnsemPro
{
    public class Function
    {

        public enum Type
        {
            Line,
            Parabola,
            Curve
        }
        
        const float INTERVAL_TIME = 1.0f / 10; // Time of each frame in seconds
        int numIntervals;
        Type type;
        Movement movement;

        Vector2[] pos;
        float[] slopes;

        public Function()
        {
        }


        /// <summary>
        /// Constructor for Line
        /// Line defined by startCoordinate and endCoordinate of movement
        /// </summary>
        /// <param name="type"></param>
        /// <param name="movement"></param>
        public void initializeLine(Type type, Movement movement, int bpm)
        {
            if (type != Type.Line) Console.WriteLine("Line constructor called with type that is not line");
            
            // Same code as constructor for parabola
            this.type = type;
            this.movement = movement;
            numIntervals = (int) ((movement.endBeat - movement.startBeat + 1) / (float) bpm * 60 / INTERVAL_TIME);
            //Console.WriteLine("INTERVAL_TIME " + INTERVAL_TIME);
            //Console.WriteLine("numIntervals " + numIntervals);

            pos = new Vector2[numIntervals];
            slopes = new float[numIntervals];

            float incre = (movement.endCoordinate.X - movement.startCoordinate.X) / (float)numIntervals;
            float curX = movement.startCoordinate.X;
            // Same code as constructor for parabola

            float curY = movement.startCoordinate.Y;
            float slope = (movement.endCoordinate.Y - movement.startCoordinate.Y) / 
                (movement.endCoordinate.X - movement.startCoordinate.X);
            
            for (int i = 0; i < numIntervals; i++)
            {
                pos[i].X = curX;
                pos[i].Y = curY;
                slopes[i] = slope;
                curX += incre;
                curY += incre * slope;

                Console.WriteLine(pos[i].X + " " + pos[i].Y + " " + slopes[i]);
            }
        }

        /// <summary>
        /// Constructor for parabola
        /// Parabola defined through vertex form y = a (x - h)^2 + k, where a is vertical stretch factor and 
        /// (h,k) is vertex
        /// </summary>
        /// <param name="type"></param>
        /// <param name="movement"></param>
        /// <param name="a"></param>
        /// <param name="vertex"></param>
        public void InitializeParabola(Type type, Movement movement,int bpm, Point vertex)
        {
            if (type != Type.Parabola) Console.WriteLine("Parabola constructor called with type that is not Parabola");

            // Same code as constructor for line
            this.type = type;
            this.movement = movement;
            numIntervals = (int)((movement.endBeat - movement.startBeat + 1) / (float)bpm * 60 / INTERVAL_TIME);
            //Console.WriteLine("INTERVAL_TIME " + INTERVAL_TIME);
            //Console.WriteLine("numIntervals " + numIntervals);

            pos = new Vector2[numIntervals];
            slopes = new float[numIntervals];

            float incre = (movement.endCoordinate.X - movement.startCoordinate.X) / (float)numIntervals;
            float curX = movement.startCoordinate.X;
            // Same code as constructor for line

            float a = (movement.endCoordinate.X-movement.startCoordinate.X)/(float) Math.Pow (movement.endCoordinate.X- movement.startCoordinate.X, 2);

            for (int i = 0; i < numIntervals; i++)
            {
                pos[i].X = curX;
                pos[i].Y = a * (float) (Math.Pow(curX - vertex.X, 2)) + vertex.Y;
                slopes[i] = 2 * a * (curX - vertex.X);
                curX += incre;

                //Console.WriteLine(pos[i].X + " " + pos[i].Y + " " + slopes[i]);
            }
        }
        
        /// <summary>
        /// Draws a curve between startCoordinate and endCoordinate of movement according to the max offset,
        /// specified by amp. Drawn using a sine function with rotation
        /// Note that both pos and slope have length of number of intervals + 1. This is for drawing purpose only.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="movement"></param>
        /// <param name="bpm"></param>
        /// <param name="amp"></param>
        public void InitializeCurve(Type type, Movement movement, int bpm, float amp)
        {
            if (type != Type.Curve) Console.WriteLine("Curve constructor called with type that is not Curve");

            this.type = type;
            this.movement = movement;
            numIntervals = (int)((movement.endBeat - movement.startBeat + 1) / (float)bpm * 60 / INTERVAL_TIME);
            pos = new Vector2[numIntervals + 1];
            slopes = new float[numIntervals + 1];

            // Change coordinates so that (0,0) is bottom left
            float oStartX = movement.startCoordinate.X;
            float oStartY = movement.startCoordinate.Y;
            float oEndX = movement.endCoordinate.X;
            float oEndY = movement.endCoordinate.Y;

            float curX = 0;

            float length = Vector2.Distance(new Vector2(oStartX, oStartY), new Vector2(oEndX, oEndY));
            float incre = length / numIntervals;        
            float k = (float) (Math.PI / length);

            for (int i = 0; i < (numIntervals + 1); i++)
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
                
                pos[i].X = rX;
                pos[i].Y = GameEngine.HEIGHT - rY;
                slopes[i] = (rY - oY) / (rX - oX);
                
                //Console.WriteLine(slopes[i]);

                curX += incre;
            }
        }

        /// <summary>
        /// Returns an array of Vector2 representing positions
        /// </summary>
        /// <returns></returns>
        public Vector2[] getPos()
        {
            return pos;
        }

        /// <summary>
        /// Returns an array of slopes
        /// </summary>
        /// <returns></returns>
        public float[] getSlopes()
        {
            return slopes;
        }

        public int getSize() { return numIntervals; }
    }
}

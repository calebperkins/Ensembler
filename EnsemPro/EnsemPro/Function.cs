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
            Parabola
        }
        
        const float INTERVAL_TIME = 1.0f; // Time of each frame in seconds
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
        public void initialize(Type type, Movement movement, int bpm)
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
            //Same code as constructor for parabola

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
        public void Parabola(Type type, Movement movement,int bpm, float a, Point vertex)
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
            //Same code as constructor for line

            for (int i = 0; i < numIntervals; i++)
            {
                pos[i].X = curX;
                pos[i].Y = a * (float) (Math.Pow(curX - vertex.X, 2)) + vertex.Y;
                slopes[i] = 2 * a * (curX - vertex.X);
                curX += incre;

                Console.WriteLine(pos[i].X + " " + pos[i].Y + " " + slopes[i]);
            }
        }

        /// <summary>
        /// Returns an array of Vector2 representing positions of line from movement's startCoordinate
        /// to endCoordinate over the interval between endBeat and startBeat. 
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
    }
}

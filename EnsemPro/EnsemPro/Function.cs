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
    //making this class on behalf of Caleb the lazy lead :P -- shuyan
    public class Function
    {
        /// <summary>
        /// Returns a 2D array of coordinates and slopes of line from start to end with the
        /// specified number of intervals. 
        /// Note that the length of the longer dimension of returned array is number of intervals + 1.
        /// For the second dimension, index 0 and 1 are x and y coordinates, and index 2 is the slope.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="intervals"></param>
        /// <returns></returns>
        public static double[,] getLinePosAndSlopes(Vector2 start, Vector2 end, int intervals)
        {
            double [,] posAndSlopes = new double [intervals + 1, 3];
            double slope = (end.Y - start.Y) / (end.X - start.X);
            double incre = (end.X - start.X) / (double) intervals;
            double curX = start.X;
            for (int i = 0; i < (intervals + 1); i++)
            {
                posAndSlopes[i, 0] = curX;
                posAndSlopes[i, 1] = start.Y + i * slope;
                posAndSlopes[i, 2] = slope;
                curX += incre;
            }
            return posAndSlopes;
        }

        /// <summary>
        /// Returns a 2D array of coordinates and slopes of parabola specified in vertex form
        /// (y = a * (x - h) + k, where (h,k) is vertex) over the given intervals.
        /// Note that the length of the longer dimension of returned array is number of intervals + 1.
        /// For the second dimension, index 0 and 1 are x and y coordinates, and index 2 is the slope.
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="a"></param>
        /// <param name="startX"></param>
        /// <param name="endX"></param>
        /// <param name="intervals"></param>
        /// <returns></returns>
        public static double[,] getParabolaPosAndSlopes(Vector2  vertex, int a, int startX, int endX, int intervals)
        {
            double[,] posAndSlopes = new double[intervals + 1, 3];
            double incre = (endX - startX) / (double) intervals;
            double curX = startX;
            for (int i = 0; i < (intervals + 1); i++)
            {
                posAndSlopes[i, 0] = curX;
                posAndSlopes[i, 1] = a * Math.Pow(curX - vertex.X, 2) + vertex.Y;
                posAndSlopes[i, 2] = 2 * a * (curX - vertex.X);
                curX += incre;
            }
            return posAndSlopes;
        }

        /// <summary>
        /// not done
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="endX"></param>
        /// <param name="intervals"></param>
        /// <returns></returns>
        public static double[] getSinPosAndSlopes(int startX, int endX, int intervals)
        {
            double[] slopes = new double[intervals + 1];
            double incre = (double)(endX - startX) / intervals;
            double curX = startX;
            for (int i = 0; i < slopes.Length; i++)
            {

            }
            return slopes;
        }
    }
}

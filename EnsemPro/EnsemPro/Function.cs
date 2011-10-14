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
        /// Returns the slope of line starting from start and ending at end
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public double getLineSlope(Vector2 start, Vector2 end)
        {
            return ((end.Y - start.Y) / (end.X - start.X));
        }

        /// <summary>
        /// Returns slopes of parabola from the specified number of intervals.
        /// Based on vertex form y = a * (x-h)^2 + k, and its derivative is 
        /// 2 * a * (x-h), where h is verX.
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="a"></param>
        /// <param name="startX"></param>
        /// <param name="endX"></param>
        /// <param name="intervals"></param>
        /// <returns></returns>
        public double[] getParabolaSlopes(double verX, int a, int startX, int endX, int intervals)
        {
            double[] slopes = new double[intervals + 1];
            double incre = (double) (endX - startX) / intervals;
            double curX = startX;
            for (int i = 0; i < slopes.Length; i++)
            {
                slopes[i] = 2 * a * (curX - verX);
                curX += incre;
            }
            return slopes;
        }

        public double[] getSinSlopes(int startX, int endX, int intervals)
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

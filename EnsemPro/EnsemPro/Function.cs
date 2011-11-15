using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace EnsemPro
{
    public class Function
    {

        const float INTERVAL_TIME = 1.0f / 60; // Time of each frame in seconds
        Movement movement;

        Vector2 startPos = new Vector2(0,0);
        Vector2 midPos = new Vector2(0,0);
        Vector2 endPos = new Vector2(0,0);
        Vector2 shiftPos = new Vector2(0, 0);
        bool isStraightLine = false;

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

        public Vector2[] Slopes
        {
            get;
            private set;
        }
        public Vector2[] drawPositions
        {
            get;
            private set;
        }

        public void drawPosition()
        {
            List<Vector2> draw = new List<Vector2>();
            float lastPx = -1;
            float lastPy = -1;
            foreach (Vector2 p in Positions)
            {
                if ((lastPx < 0 || Math.Sqrt((lastPx - p.X) * (lastPx - p.X) + (lastPy - p.Y) * (lastPy - p.Y)) > Movement.circleR))
                {
                    draw.Add(p);
                    lastPx = p.X;
                    lastPy = p.Y;
                }
            }
            drawPositions = draw.ToArray();
        }

        /// <summary>
        /// Draws a curve between startCoordinate and endCoordinate of movement according to the max offset,
        /// specified by amp. Drawn using a sine function with rotation
        /// Note that both Positions and slope have length of number of intervals + 1. This is for drawing purpose only.
        /// </summary>
        /// <param name="movement"></param>
        /// <param name="bpm"></param>
        /// <param name="amp"></param>
        public Function(Movement movement, int bpm, float amp)
        {
            this.movement = movement;
            Size = (int)((movement.endBeat - movement.startBeat + 1) / (float)bpm * 60 / INTERVAL_TIME);
            Positions = new Vector2[Size + 1];

            // Change coordinates so that (0,0) is bottom left
            // Original start and ending positions specified by XML
            Vector2 oStartPos = new Vector2(movement.startCoordinate.X, movement.startCoordinate.Y);
            Vector2 oEndPos = new Vector2(movement.endCoordinate.X, movement.endCoordinate.Y);
            shiftPos = oStartPos;
            endPos = oEndPos - shiftPos;

            if (amp == 0)
            {
                isStraightLine = true;
                Position();
            }
            else
            {
                Vector2 endPosPrime = new Vector2(Vector2.Distance(oStartPos, oEndPos), 0);
                Vector2 midPosPrime = new Vector2(endPosPrime.X / 2, 2 * amp);

                // Rotation angle
                double theta = Math.Atan((oEndPos.Y - oStartPos.Y) / (oEndPos.X - oStartPos.X));
                if (oEndPos.X < oStartPos.X) theta += Math.PI;

                double midPosX = Math.Cos(theta) * midPosPrime.X - Math.Sin(theta) * midPosPrime.Y;
                double midPosY = Math.Sin(theta) * midPosPrime.X + Math.Cos(theta) * midPosPrime.Y;
                midPos = new Vector2((float)midPosX, (float)midPosY);

                Position();
                
            }
        }
        /// <summary>
        /// Set each value in the position array to the appropriate value
        /// </summary>
        /// <param name="startPos">P0 of the Bezier curve</param>
        /// <param name="midPos">P1 of the Bezier curve</param>
        /// <param name="endPos">P2 of the Bezier curve</param>
        /// <param name="size">Number of intervals over which to draw the curve</param>
        /// <param name="isStraightline">True if the curve is a straight line</param>
        public void Position()
        {
            float t = 0;
            float incre = 1 / (float)Size;
            if (isStraightLine) // straight line
            {
                for (int i = 0; i < Size + 1; i++)
                {
                    Positions[i] = new Vector2((1 - t) * startPos.X + t * endPos.X + shiftPos.X,
                                   GameEngine.HEIGHT - ((1 - t) * startPos.Y + t * endPos.Y + shiftPos.Y));
                    t += incre;
                }
            }
            else // curve
            {
                for (int i = 0; i < Size + 1; i++)
                {
                    Positions[i] = new Vector2((1 - t * t) * startPos.X + 2 * (1 - t) * t * midPos.X + t * t * endPos.X + shiftPos.X,
                                               GameEngine.HEIGHT - ((1 - t * t) * startPos.Y + 2 * (1 - t) * t * midPos.Y + t * t * endPos.Y + shiftPos.Y));
                    t += incre;
                }
            }
            drawPosition();
        }

        /// <summary>
        /// Compute the an array of slope 
        /// </summary>
        /// <param name="size">Number of intervals over which to compute the slopes</param>
        /// <param name="isStraightline">True if the curve is a straight line</param>
        public Vector2[] Slope(int size)
        {
            Slopes = new Vector2[size+1];
            if (isStraightLine) // straight line
            {
                
                Vector2 slope = Vector2.Normalize(new Vector2(endPos.X - startPos.X, startPos.Y - endPos.Y));
                for (int i = 0; i < size + 1; i++)
                {
                    Slopes[i] = slope;
                }
            }
            else // curve
            {
                float t = 0;
                float incre = 1 / (float)size;
                Vector2 lastPos = new Vector2((1 - t * t) * startPos.X + 2 * (1 - t) * t * midPos.X + t * t * endPos.X,
                                               (1 - t * t) * startPos.Y + 2 * (1 - t) * t * midPos.Y + t * t * endPos.Y);
                t += incre;
                Slopes[0] = new Vector2(0, 0); // this value is never used in movement evaluator
                for (int i = 1; i < size + 1; i++)
                {
                    Vector2 newPos = new Vector2((1 - t * t) * startPos.X + 2 * (1 - t) * t * midPos.X + t * t * endPos.X,
                                               (1 - t * t) * startPos.Y + 2 * (1 - t) * t * midPos.Y + t * t * endPos.Y);
                    Vector2 posDiff = new Vector2(newPos.X - lastPos.X, newPos.Y - lastPos.Y);
                    Slopes[i] = Vector2.Normalize(new Vector2(posDiff.X / incre, -posDiff.Y / incre));
                    t += incre;
                }
            }
            return Slopes;
        }
    }
}

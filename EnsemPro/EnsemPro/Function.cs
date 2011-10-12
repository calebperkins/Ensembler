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
        private Vector2 pos;
        private FunShape shape;

        // For line
        private int slope = 0;

        public enum FunShape {
            Line,
            Parabola,
            Sin,
        }

        public Function(FunShape shape, Vector2 ori)
        {
            if (shape != FunShape.Line) Console.WriteLine ("THIS IS Line, you called " + shape);
            this.shape = shape;
            pos = ori;
        }

        public Vector2 getPos ()
        {
            return pos;
        }

        public FunShape getShape()
        {
            return shape;
        }
    }
}

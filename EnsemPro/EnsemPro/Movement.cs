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
    class Movement
    {
        public enum Type {
            Shake,
            Noop,
            Wave
        }

        Type my_type;

        public int start_beat
        {
            get;
            set;
        }

        public int end_beat
        {
            get;
            set;
        }

        // null if not a wave
        public Point start_coordinate
        {
            get;
            set;
        }
        public Point end_coordinate
        {
            get;
            set;
        }
        public Function f
        {
            get;
            set;
        }

        public Movement(Movement.Type type, int sb, int eb)
        {
            my_type = type;
            start_beat = sb;
            end_beat = eb;
        }

        public Movement(Movement.Type type, int sb, int eb, Point sc, Point ec, Function f)
        {
            my_type = type;
            start_beat = sb;
            end_beat = eb;
            start_coordinate = sc;
            end_coordinate = ec;
        }

        // returns the type of this movement
        public Type getType()
        {
            return my_type;
        }
         
    }
}

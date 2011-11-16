using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EnsemPro.Models
{
    public class City
    {
        public City Up
        {
            get;
            set;
        }

        public City Down
        {
            get;
            set;
        }

        public City Left
        {
            get;
            set;
        }

        public City Right
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get;
            set;
        }



        public City(DataTypes.WorldData.City c)
        {
        }
    }
}

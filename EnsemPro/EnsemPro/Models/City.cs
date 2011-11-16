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

        public Vector2 AbsolutePosition;
        public DialogController DialogControl
        {
            get;
            set;
        }

        public DataTypes.WorldData.CityState State
        {
            get;
            set;
        }

        public City(DataTypes.WorldData.City c)
        {
            AbsolutePosition = c.Position;
            RelativePosition = c.Position;
            State = c.State;
            clearedDialogue = new DialogModel(c.ClearedDialogAsset);
            unlockedDialogue = new DialogModel(c.UnlockedDialogAsset);
            newlyUnlockedDialogue = new DialogModel(c.NewlyUnlockedDialogAsset);
            Name = c.Name;
        }

        public DialogModel clearedDialogue;
        public DialogModel unlockedDialogue;
        public DialogModel newlyUnlockedDialogue;
        public string Name;

        public Vector2 RelativePosition;
    }
}

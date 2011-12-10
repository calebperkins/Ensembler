using Microsoft.Xna.Framework;

namespace Ensembler.Models
{
    public class City
    {
        public DataTypes.WorldData.City Data;

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

        public City[] Unlocked
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

        public City() { }

        public City(DataTypes.WorldData.City c)
        {
            AbsolutePosition = c.Position;
            RelativePosition = c.Position;
            State = c.State;
            clearedDialogue = new DialogModel(c.ClearedDialogAsset);
            unlockedDialogue = new DialogModel(c.UnlockedDialogAsset);
            newlyUnlockedDialogue = new DialogModel(c.NewlyUnlockedDialogAsset);
            successDialogue = new DialogModel(c.SuccessAsset);
            Name = c.Name;

            Unlocked = new Models.City[] { null, null, null };

            Data = c;
        }

        public DialogModel clearedDialogue;
        public DialogModel unlockedDialogue;
        public DialogModel newlyUnlockedDialogue;
        public DialogModel successDialogue;
        public string Name;

        public Vector2 RelativePosition;

        public bool Locked
        {
            get { return State == DataTypes.WorldData.CityState.Locked; }
        }

        public bool NotLocked
        {
            get { return !Locked; }
        }

    }
}

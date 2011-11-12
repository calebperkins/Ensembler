using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace EnsemPro
{
    public class DialogModel
    {
        private string DialogFileName;

        public DataTypes.Locations Location
        {
            get;
            set;
        }

        public DataTypes.NewlyUnlockedSummary[] NewlyUnlocked
        {
            get;
            set;
        }

        public DataTypes.UnlockedSummary[] Unlocked
        {
            get;
            set;
        }

        public DataTypes.ClearedSummary[] Cleared
        {
            get;
            set;
        }

        public DialogModel(string filename)
        {
            DialogFileName = filename;
        }

        public void LoadContent(ContentManager cm)
        {
            DataTypes.DialogData data = cm.Load<DataTypes.DialogData>("Dialogs//"+DialogFileName);
            Location = data.Location;
            NewlyUnlocked = data.NewlyUnlocked;
            Unlocked = data.Unlocked;
            Cleared = data.Cleared;
        }
    }
}

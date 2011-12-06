using Microsoft.Xna.Framework.Content;

namespace Ensembler
{
    public class DialogModel
    {

        public DataTypes.ContentSummary[] Content
        {
            get;
            set;
        }

        public DataTypes.ColorSummary[] Colors
        {
            get;
            set;
        }

        public DataTypes.FaceTexturePath[] Faces
        {
            get;
            set;
        }

        public DataTypes.CutSceneSummary[] CutScenes
        {
            get;
            set;
        }

        public string DialogFileName
        {
            get;
            private set;
        }

        public DialogModel(string filename)
        {
            DialogFileName = filename;
        }

        public void LoadContent(ContentManager cm)
        {
            Content = cm.Load<DataTypes.DialogData>("Dialogs//" + DialogFileName).Content;
            Colors = cm.Load<DataTypes.DialogData>("Dialogs//" + DialogFileName).Colors;
            Faces = cm.Load<DataTypes.DialogData>("Dialogs//" + DialogFileName).Faces;
            CutScenes = cm.Load<DataTypes.DialogData>("Dialogs//" + DialogFileName).CutScenes;
        }
    }
}

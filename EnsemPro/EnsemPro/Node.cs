using Microsoft.Xna.Framework;

namespace EnsemPro
{
    public class Node
    {
        public DataTypes.WorldData.CityState nodeState;
        public float oriX;
        public Vector2 curPos;
        public DialogModel dialogModel;
        public DialogController dialogController;

        public Node(DataTypes.WorldData.CityState ns, Vector2 p, string dialogFile)
        {
            nodeState = ns;
            oriX = p.X;
            curPos = p;
            dialogModel = new DialogModel(dialogFile);
        }
    }
}

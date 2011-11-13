using Microsoft.Xna.Framework;

namespace EnsemPro
{
    public class Node
    {
        public DataTypes.WorldData.CityState nodeState;
        public string cityName;
        public float oriX;
        public Vector2 curPos;
        public DialogModel clearedDialogue;
        public DialogModel unlockedDialogue;
        public DialogModel newlyUnlockedDialogue;
        public DialogController dialogController;

        public Node(DataTypes.WorldData.CityState ns, Vector2 p, string cd, string ud, string nud, string cn)
        {
            nodeState = ns;
            oriX = p.X;
            curPos = p;
            clearedDialogue = new DialogModel(cd);
            unlockedDialogue = new DialogModel(ud);
            newlyUnlockedDialogue = new DialogModel(nud);
            cityName = cn;
        }
    }
}

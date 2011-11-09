using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EnsemPro
{
    public class Node
    {
        public enum NodeState
        {
            Locked,
            NewlyUnlocked,
            Unlocked,
            Cleared
        }

        public NodeState nodeState;
        public float oriX;
        public Vector2 curPos;
        public DialogController dialogController;
        public string dialogFile="test.txt"; // NEED TO LOAD

        public Node(NodeState ns, Vector2 p)
        {
            nodeState = ns;
            oriX = p.X;
            curPos = p;
        }
    }
}

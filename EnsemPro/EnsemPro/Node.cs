﻿using Microsoft.Xna.Framework;

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
        public DialogModel dialogModel;
        public DialogController dialogController;

        public Node(NodeState ns, Vector2 p, string dialogFile)
        {
            nodeState = ns;
            oriX = p.X;
            curPos = p;
            dialogModel = new DialogModel(dialogFile);
        }
    }
}
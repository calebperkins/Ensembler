using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace EnsemPro
{
    class WorldMapController
    {
        // TODO NEEDS TO CHANGE DYNAMICALLY
        public const int MAP_WIDTH = 2000;
        public const int SHIFT_PER_FRAME = 6;

        GameModel gameState;
        WorldMapView worldView;

        Node[] nodes; //invariant: nodes[i+1].oriX >= nodes[i].oriX
        int selected;
        KeyboardState lastState = Keyboard.GetState();

        public WorldMapController (GameModel gm, SpriteBatch sb)
        {
            gameState = gm;
            worldView = new WorldMapView(sb);
            nodes = CreateNodes();
        }

        // FOR TESTING PURPOSES
        public Node[] CreateNodes()
        {
            Node[] newNodes = new Node[12];
            newNodes[0] = new Node(Node.NodeState.Cleared, new Vector2 (100,300));
            selected = 0;
            newNodes[1] = new Node(Node.NodeState.Unlocked, new Vector2(300, 100));
            newNodes[2] = new Node(Node.NodeState.Unlocked, new Vector2(400, 100));
            newNodes[3] = new Node(Node.NodeState.Unlocked, new Vector2(500, 100));
            newNodes[4] = new Node(Node.NodeState.NewlyUnlocked, new Vector2(700, 300));
            newNodes[5] = new Node(Node.NodeState.Locked, new Vector2(1000, 100));
            newNodes[6] = new Node(Node.NodeState.Locked, new Vector2(1000, 200));
            newNodes[7] = new Node(Node.NodeState.Locked, new Vector2(1300, 300));
            newNodes[8] = new Node(Node.NodeState.Locked, new Vector2(1500, 100));
            newNodes[9] = new Node(Node.NodeState.Locked, new Vector2(1600, 100));
            newNodes[10] = new Node(Node.NodeState.Locked, new Vector2(1700, 100));
            newNodes[11] = new Node(Node.NodeState.Locked, new Vector2(1900, 300));
            return newNodes;
        }

        public void Initialize() 
        { 
        }

        public void LoadContent(ContentManager cm)
        {
            worldView.LoadContent(cm);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Left) && lastState.IsKeyUp(Keys.Left) && selected > 0)
            {
                selected--;
            }
            else if (ks.IsKeyDown(Keys.Right) && lastState.IsKeyUp(Keys.Right) && selected < nodes.Length - 1)
            {
                selected++;
            }
            worldView.WantedBackgroundPosX = MathHelper.Clamp(-1 * (nodes[selected].oriX - GameEngine.WIDTH / 2), -1 * (MAP_WIDTH - GameEngine.WIDTH), 0);
            lastState = ks;

            float diff = worldView.WantedBackgroundPosX - worldView.CurBackgroundPos.X;
            if (diff > 0) // Need to shift origin of background to right
            {
                if (Math.Abs(diff) > SHIFT_PER_FRAME)
                {
                    worldView.CurBackgroundPos = new Vector2(worldView.CurBackgroundPos.X + SHIFT_PER_FRAME, worldView.CurBackgroundPos.Y);
                }
                else
                {
                    worldView.CurBackgroundPos = new Vector2(worldView.WantedBackgroundPosX, worldView.CurBackgroundPos.Y);
                }
            }
            else if (diff < 0)
            {
                if (Math.Abs(diff) > SHIFT_PER_FRAME)
                {
                    worldView.CurBackgroundPos = new Vector2(worldView.CurBackgroundPos.X - SHIFT_PER_FRAME, worldView.CurBackgroundPos.Y);
                }
                else
                {
                    worldView.CurBackgroundPos = new Vector2(worldView.WantedBackgroundPosX, worldView.CurBackgroundPos.Y);
                }
            }
        }

        public void Draw()
        {
            worldView.Draw(nodes, selected);
        }
    }
}

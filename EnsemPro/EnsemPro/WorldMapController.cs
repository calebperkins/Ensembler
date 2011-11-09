using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace EnsemPro
{
    class WorldMapController
    {
        // TODO NEEDS TO CHANGE DYNAMICALLY
        public const int MAP_WIDTH = 4000;
        public const int SHIFT_PER_FRAME = 6;

        Game game;
        SpriteBatch spriteBatch;
        GameModel gameState;
        WorldMapView worldView;


        Node[] nodes; //invariant: nodes[i+1].oriX >= nodes[i].oriX
        int selected;
        KeyboardState lastState = Keyboard.GetState();

        public bool inDialog;
       // DialogController dialogController;

        public WorldMapController (Game g, GameModel gm, SpriteBatch sb)
        {
            game = g;
            gameState = gm;
            worldView = new WorldMapView(sb);
            nodes = CreateNodes();
            inDialog = false;
            spriteBatch = sb;
        }

        // FOR TESTING PURPOSES
        public Node[] CreateNodes()
        {
            int count = 0;
            Node[] newNodes = new Node[18];
            newNodes[count++] = new Node(Node.NodeState.Cleared, new Vector2 (100,300));
            selected = 0;
            newNodes[count++] = new Node(Node.NodeState.Unlocked, new Vector2(300, 100));
            newNodes[count++] = new Node(Node.NodeState.Unlocked, new Vector2(400, 100));
            newNodes[count++] = new Node(Node.NodeState.Unlocked, new Vector2(500, 100));
            newNodes[count++] = new Node(Node.NodeState.NewlyUnlocked, new Vector2(700, 300));
            newNodes[count++] = new Node(Node.NodeState.Locked, new Vector2(1000, 100));
            newNodes[count++] = new Node(Node.NodeState.Locked, new Vector2(1000, 200));
            newNodes[count++] = new Node(Node.NodeState.Locked, new Vector2(1300, 300));
            newNodes[count++] = new Node(Node.NodeState.Locked, new Vector2(1500, 100));
            newNodes[count++] = new Node(Node.NodeState.Locked, new Vector2(1600, 100));
            newNodes[count++] = new Node(Node.NodeState.Locked, new Vector2(1700, 100));
            newNodes[count++] = new Node(Node.NodeState.Locked, new Vector2(1900, 300));
            newNodes[count++] = new Node(Node.NodeState.Locked, new Vector2(2200, 300));
            newNodes[count++] = new Node(Node.NodeState.Locked, new Vector2(2500, 300));
            newNodes[count++] = new Node(Node.NodeState.Locked, new Vector2(2800, 300));
            newNodes[count++] = new Node(Node.NodeState.Locked, new Vector2(3100, 300));
            newNodes[count++] = new Node(Node.NodeState.Locked, new Vector2(3400, 300));
            newNodes[count++] = new Node(Node.NodeState.Locked, new Vector2(3700, 300));
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
            if (inDialog)
            {
                nodes[selected].dialogController.Update(gameTime);
            }
            else
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
                if (ks.IsKeyDown(Keys.D) && lastState.IsKeyUp(Keys.D))
                {
                    inDialog = true;
                //    gameState.CurrentScreen = DataTypes.Screens.Dialog; // K THIS ONE WORKS
                    nodes[selected].dialogController = new DialogController(gameState, spriteBatch, nodes[selected].dialogFile);
                    nodes[selected].dialogController.Initialize();
                    nodes[selected].dialogController.LoadContent(game.Content); // MOVE TO NODE'S 
                   // Console.WriteLine("herehere");
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
        }

        public void Draw(GameTime gameTime)
        {
            worldView.Draw(nodes, selected);
            if (inDialog) nodes[selected].dialogController.Draw(gameTime);
        }
    }
}

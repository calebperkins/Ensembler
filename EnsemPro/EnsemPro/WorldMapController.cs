using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

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
        SoundEffect MapMove;
        SoundEffect EnterCity;
        SoundEffect LevelUnlock;

        public bool inDialog; // SET THIS TO FALSE AT SOME POINT

        public WorldMapController (Game g, GameModel gm, SpriteBatch sb)
        {
            game = g;
            gameState = gm;
            worldView = new WorldMapView(sb);
            inDialog = false;
            spriteBatch = sb;
        }

        public void Initialize() 
        { 
        }

        public void LoadContent(ContentManager cm)
        {
            worldView.LoadContent(cm);
            DataTypes.WorldData data = cm.Load<DataTypes.WorldData>("World");
            MapMove = cm.Load<SoundEffect>("Sounds//MapMove");
            EnterCity = cm.Load<SoundEffect>("Sounds//EnterCity");
            LevelUnlock = cm.Load<SoundEffect>("Sounds//LevelUnlock");
            nodes = new Node[data.Cities.Length];
            for (int i = 0; i < data.Cities.Length; i++)
            {
                nodes[i] = new Node(data.Cities[i].State, data.Cities[i].Position, data.Cities[i].UnlockedDialogAsset); // TODO: clean up
            }
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            Node node = nodes[selected];
            if (inDialog)
            {
                if (ks.IsKeyDown(Keys.Q) && lastState.IsKeyUp(Keys.Q))
                {
                    inDialog = false;
                }
                else
                {
                    node.dialogController.Update(gameTime);
                }
            }
            else
            {
                
                if (ks.IsKeyDown(Keys.Left) && lastState.IsKeyUp(Keys.Left) && selected > 0)
                {
                    selected--;
                    MapMove.Play();
                }
                else if (ks.IsKeyDown(Keys.Right) && lastState.IsKeyUp(Keys.Right) && selected < nodes.Length - 1)
                {
                    selected++;
                    MapMove.Play();
                }
                if (ks.IsKeyDown(Keys.D) && lastState.IsKeyUp(Keys.D))
                {
                    EnterCity.Play();
                    inDialog = true;
                    node.dialogModel.LoadContent(game.Content);
                    node.dialogController = new DialogController(gameState, spriteBatch, node.dialogModel, node.nodeState);
                    node.dialogController.Initialize();
                    node.dialogController.LoadContent(game.Content); // MOVE TO NODE'S 
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

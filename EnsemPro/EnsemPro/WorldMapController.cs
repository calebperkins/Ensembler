using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

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
        State currentState;
        PlayLevel levelController;
        InputBuffer buffer;

        HashSet<Models.City> Cities = new HashSet<Models.City>();

        public enum State { 
            inDialog,
            inGame,
            inMap,
        }



        public bool inDialog; // SET THIS TO FALSE AT SOME POINT

        public WorldMapController (Game g, GameModel gm, SpriteBatch sb, InputBuffer bf)
        {
            game = g;
            gameState = gm;
            worldView = new WorldMapView(sb);
            inDialog = false;
            spriteBatch = sb;
            buffer = bf;
        }

        public void Initialize() 
        {
            currentState = State.inMap;
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
                nodes[i] = new Node(data.Cities[i].State, data.Cities[i].Position, data.Cities[i].ClearedDialogAsset, data.Cities[i].UnlockedDialogAsset, data.Cities[i].NewlyUnlockedDialogAsset, data.Cities[i].Name);
            }


            // Really nasty but hey
            foreach (DataTypes.WorldData.City city in data.Cities)
            {
                Cities.Add(new Models.City(city));
            }
            foreach (Models.City c1 in Cities)
            {
                foreach (Models.City c2 in Cities)
                {
                    
                }
            }

        }

        public void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            Node node = nodes[selected];

            switch (currentState)
            {
                    /// code for inDialog
                case State.inDialog :
                    if (ks.IsKeyDown(Keys.Q) && lastState.IsKeyUp(Keys.Q))
                    {
                        inDialog = false;
                    }
                    else
                    {
                        if (node.dialogController.Finished()) 
                        {
                            if (node.nodeState == DataTypes.WorldData.CityState.Cleared)
                                currentState = State.inMap;
                            else
                            {
                                gameState.SelectedLevel = node.playlevelName;
                                gameState.CurrentScreen = DataTypes.Screens.PlayLevel;
                                currentState = State.inGame;
                            }
                        }
                        else node.dialogController.Update(gameTime);
                    }
                    break;

                    /// code for inGame
                case State.inGame :
                    
                    // TODO
                    break;

                default : //inMap
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
                    DialogModel toLoad = null;
                    switch (node.nodeState)
                    {
                        case DataTypes.WorldData.CityState.Cleared:
                            toLoad = node.clearedDialogue;
                            break;
                        case DataTypes.WorldData.CityState.NewlyUnlocked:
                            toLoad = node.newlyUnlockedDialogue;
                            break;
                        case DataTypes.WorldData.CityState.Unlocked:
                            toLoad = node.unlockedDialogue;
                            break;
                        default: //if the node is locked, though this shouldn't be reachable
                            break;
                    }
                    if (toLoad != null)
                    {
                        EnterCity.Play();
                        inDialog = true;
                        toLoad.LoadContent(game.Content);
                        node.dialogController = new DialogController(gameState, spriteBatch, toLoad, node.cityName);
                        node.dialogController.Initialize();
                        node.dialogController.LoadContent(game.Content); // MOVE TO NODE'S 
                    }
                    currentState = State.inDialog;
                }

                worldView.WantedBackgroundPosX = MathHelper.Clamp(-1 * (nodes[selected].oriX - GameEngine.WIDTH / 2), -1 * (MAP_WIDTH - GameEngine.WIDTH), 0);
                lastState = ks;

                float diff = worldView.WantedBackgroundPosX - worldView.CurBackgroundPos.X;
                if (diff > 0) // Want to shift origin of background to right
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
                break;
            }
        }

        public void Draw(GameTime gameTime)
        {
            worldView.Draw(nodes, selected);
            if (inDialog) nodes[selected].dialogController.Draw(gameTime);
        }
    }
}

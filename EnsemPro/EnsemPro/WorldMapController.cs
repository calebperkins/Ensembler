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

        GameEngine game;
        SpriteBatch spriteBatch;
        GameModel gameState;
        WorldMapView worldView;

        KeyboardState lastState = Keyboard.GetState();
        SoundEffect MapMove;
        SoundEffect EnterCity;
        SoundEffect LevelUnlock;
        State currentState;
        InputBuffer buffer;

        HashSet<Models.City> Cities = new HashSet<Models.City>();

        public enum State { 
            inDialog,
            inGame,
            inMap,
            begin,
            end
        }



        Models.City SelectedCity;


        public WorldMapController (GameEngine g, GameModel gm, SpriteBatch sb, InputBuffer bf)
        {
            game = g;
            gameState = gm;
            worldView = new WorldMapView(sb);
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

            Dictionary<uint, Models.City> map = new Dictionary<uint, Models.City>();

            foreach (DataTypes.WorldData.City city in data.Cities)
            {
                Models.City c = new Models.City(city);
                map.Add(city.ID, c); 
            }

            foreach (DataTypes.WorldData.City city_data in data.Cities)
            {
                Models.City c = map[city_data.ID];
                if (city_data.Left > 0)
                    c.Left = map[city_data.Left];
                if (city_data.Right > 0)
                    c.Right = map[city_data.Right];
                if (city_data.Up > 0)
                    c.Up = map[city_data.Up];
                if (city_data.Down > 0)
                    c.Down = map[city_data.Down];
                if (city_data.Unlock1 > 0)
                    c.Unlocked[0] = map[city_data.Unlock1];
                if (city_data.Unlock2 > 0)
                    c.Unlocked[1] = map[city_data.Unlock1];
                if (city_data.Unlock3 > 0)
                    c.Unlocked[2] = map[city_data.Unlock1];
            }

            Cities = new HashSet<Models.City>(map.Values);
            SelectedCity = map[1];

        }

        public void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            switch (currentState)
            {
                    /// code for inDialog
                case State.inDialog :
                    if (SelectedCity.DialogControl.Finished()) 
                        {
                            if (SelectedCity.State == DataTypes.WorldData.CityState.Cleared)
                                currentState = State.inMap;
                            else
                            {
                                gameState.SelectedLevel = SelectedCity.Data.PlayLevel;
                                Console.WriteLine(gameState.SelectedLevel);
                                gameState.CurrentScreen = DataTypes.Screens.PlayLevel;;
                                currentState = State.inGame;
                            }
                        }
                     else SelectedCity.DialogControl.Update(gameTime);
                    break;

                    /// code for inGame
                case State.inGame :
                    if (gameState.Score > SelectedCity.Data.ScoreReq && gameState.Combo > SelectedCity.Data.ComboReq)
                    {
                        SelectedCity.State = DataTypes.WorldData.CityState.Cleared;
                    }
                    else
                    {
                        SelectedCity.State = DataTypes.WorldData.CityState.Unlocked;
                    }
                    // TODO
                    currentState = State.inMap;
                    break;

                default : //inMap
                  if (ks.IsKeyDown(Keys.Left) && lastState.IsKeyUp(Keys.Left) && SelectedCity.Left != null)
                {
                    SelectedCity = SelectedCity.Left;
                    MapMove.Play();
                }
                else if (ks.IsKeyDown(Keys.Right) && lastState.IsKeyUp(Keys.Right) && SelectedCity.Right != null)
                {
                    SelectedCity = SelectedCity.Right;
                    MapMove.Play();
                }
                else if (ks.IsKeyDown(Keys.Up) && lastState.IsKeyUp(Keys.Up) && SelectedCity.Up != null)
                {
                    SelectedCity = SelectedCity.Up;
                    MapMove.Play();
                }
                else if (ks.IsKeyDown(Keys.Down) && lastState.IsKeyUp(Keys.Down) && SelectedCity.Down != null)
                {
                    SelectedCity = SelectedCity.Down;
                    MapMove.Play();
                }
                if (ks.IsKeyDown(Keys.D) && lastState.IsKeyUp(Keys.D))
                {
                    DialogModel toLoad = null;
                    switch (SelectedCity.State)
                    {
                        case DataTypes.WorldData.CityState.Cleared:
                            toLoad = SelectedCity.clearedDialogue;
                            break;
                        case DataTypes.WorldData.CityState.NewlyUnlocked:
                            toLoad = SelectedCity.newlyUnlockedDialogue;
                            break;
                        case DataTypes.WorldData.CityState.Unlocked:
                            toLoad = SelectedCity.unlockedDialogue;
                            break;
                        default: //if the node is locked, though this shouldn't be reachable
                            break;
                    }
                    if (toLoad != null)
                    {
                        EnterCity.Play();
                        toLoad.LoadContent(game.Content);
                        SelectedCity.DialogControl = new DialogController(gameState, spriteBatch, toLoad, SelectedCity.Name);
                        SelectedCity.DialogControl.Initialize();
                        SelectedCity.DialogControl.LoadContent(game.Content); // MOVE TO NODE'S 
                    }
                    currentState = State.inDialog;
                }  
                
                
                worldView.WantedBackgroundPosX = MathHelper.Clamp(-1 * (SelectedCity.RelativePosition.X - GameEngine.WIDTH / 2), -1 * (MAP_WIDTH - GameEngine.WIDTH), 0);
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
            worldView.Draw(Cities, SelectedCity);
            if (currentState == State.inDialog) SelectedCity.DialogControl.Draw(gameTime);
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Ensembler
{
    class WorldMapController
    {
        GameEngine game;
        SpriteBatch spriteBatch;
        GameState gameState;
        WorldMapView worldView;
        
        SoundEffect MapMove;
        SoundEffect EnterCity;
        SoundEffect LevelUnlock;
        State currentState;
        InputBuffer buffer;

        HashSet<Models.City> Cities = new HashSet<Models.City>();

        bool start = false;
        int big_Six = 0;

        public enum State { 
            inDialog,
            inGame,
            inMap,
            begin,
            end
        }

        Models.City SelectedCity;
        Models.City LastCity;

        public WorldMapController (GameEngine g, GameState gm, SpriteBatch sb, InputBuffer bf)
        {
            game = g;
            gameState = gm;
            worldView = new WorldMapView(sb);
            spriteBatch = sb;
            buffer = bf;
            MediaPlayer.IsRepeating = true;
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
                    c.Unlocked[1] = map[city_data.Unlock2];
                if (city_data.Unlock3 > 0)
                    c.Unlocked[2] = map[city_data.Unlock3];
            }

            Cities = new HashSet<Models.City>(map.Values);
            SelectedCity = map[1];
            LastCity = SelectedCity;

        }

        public void Update(GameTime gameTime, bool stayInDialogue)
        {
            KeyboardState ks = Keyboard.GetState();

            if (!start)
            {
                start = true;
                DialogModel dm = new DialogModel("Introduction");
                dm.LoadContent(game.Content);
                SelectedCity.DialogControl = new DialogController(gameState, spriteBatch, dm, "Tutorial", game.Content);
                SelectedCity.DialogControl.Initialize();
                SelectedCity.DialogControl.LoadContent(game.Content); 
                currentState = State.inDialog;
            }
            else if (!stayInDialogue) 
            {
                currentState = State.inMap;
            }
            switch (currentState)
            {
                /// code for inDialog
                case State.inDialog:
                    if (SelectedCity.DialogControl.Finished)
                    {
                        if (SelectedCity.State == DataTypes.WorldData.CityState.Cleared || SelectedCity.Data.PlayLevel == "NoLevel")
                        {
                            currentState = State.inMap;
                            if (SelectedCity.Data.PlayLevel == "NoLevel")
                            {
                                SelectedCity.State = DataTypes.WorldData.CityState.Success; // For Milan
                            }
                        }
                        else if (SelectedCity.DialogControl.Dialog.DialogFileName == "Introduction" || SelectedCity.DialogControl.Dialog == SelectedCity.successDialogue)
                        {
                            currentState = State.inMap;
                        }
                        else
                        {
                            MediaPlayer.Stop();
                            gameState.SelectedLevel = SelectedCity.Data.PlayLevel;
                            Console.WriteLine(gameState.SelectedLevel);
                            gameState.CurrentScreen = DataTypes.Screens.PlayLevel;
                            currentState = State.inGame;
                        }
                    }
                    else
                    {
                        if (!gameState.Input.Cancel)
                        {
                            SelectedCity.DialogControl.Update(gameTime);
                        }
                        else
                        {
                            currentState = State.inMap;
                        }
                    }
                    break;

                /// code for inGame
                case State.inGame:
                    if (gameState.Score > SelectedCity.Data.ScoreReq && gameState.Combo > SelectedCity.Data.ComboReq)
                    {
                        SelectedCity.State = DataTypes.WorldData.CityState.Cleared;
                        LevelUnlock.Play();
                        foreach (Models.City c in SelectedCity.Unlocked)
                        {
                            if (c != null)
                            {
                                Console.WriteLine("here at city" + c.Data.Name);
                                c.State = DataTypes.WorldData.CityState.NewlyUnlocked;
                            }
                        }
                        if (SelectedCity.Data.BigSix)
                        {
                            this.big_Six++;
                            if (big_Six >= 4)
                            {
                                foreach (Models.City c in Cities)
                                {
                                    if (c.Data.ID == 15) c.State = DataTypes.WorldData.CityState.NewlyUnlocked;
                                }
                            }
                            else if (big_Six == 6)
                            {
                                foreach (Models.City c in Cities)
                                {
                                    if (c.Data.ID == 14) c.State = DataTypes.WorldData.CityState.NewlyUnlocked;
                                }
                            }

                        }
                        DialogModel dm = SelectedCity.successDialogue;
                        dm.LoadContent(game.Content);
                        SelectedCity.DialogControl = new DialogController(gameState, spriteBatch, dm, "(Success!)", game.Content);
                        SelectedCity.DialogControl.Initialize();
                        SelectedCity.DialogControl.LoadContent(game.Content);
                        currentState = State.inDialog;
                    }
                    else
                    {
                        SelectedCity.State = DataTypes.WorldData.CityState.Unlocked;
                        currentState = State.inMap;
                    }
                    // TODO
                    break;

                default: //inMap
                    foreach (Models.City city in Cities)
                    {
                        // TODO: remove magic constant!
                        if (Vector2.Distance(city.AbsolutePosition, gameState.Input.Position) < 20.0f)
                        {
                            SelectedCity = city;
                            break;
                        }
                    }
                    if (SelectedCity != LastCity)
                        MapMove.Play();
                    LastCity = SelectedCity;

                    if (gameState.Input.Confirm && SelectedCity.NotLocked)
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
                            SelectedCity.DialogControl = new DialogController(gameState, spriteBatch, toLoad, SelectedCity.Name,game.Content);
                            SelectedCity.DialogControl.Initialize();
                            SelectedCity.DialogControl.LoadContent(game.Content); // MOVE TO NODE'S 
                        }
                        currentState = State.inDialog;
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

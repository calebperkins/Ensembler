using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace Ensembler
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameEngine : Game
    {
        // Deprecated
        public static int HEIGHT;
        public static int WIDTH;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameState gameState;
        DataTypes.Screens lastState = DataTypes.Screens.Initial;

        MenuController menuController;
        PlayLevel playlevel;
        LevelSelectController levelController;

        WorldMapController worldController;
        PauseScreenController pauseController; // a misnomer

        InputBuffer buffer;

        InputController input;

        public GameEngine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }



        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            

            spriteBatch = new SpriteBatch(GraphicsDevice);

            buffer = new InputBuffer();
            gameState = new GameState();
            gameState.LoadContent(Content); // hack

            try
            {
                input = new WiiController(this, gameState, buffer);
            }
            catch (WiimoteLib.WiimoteNotFoundException)
            {
                input = new MouseController(this, gameState, buffer);
            }
            

            Services.AddService(typeof(GameState), gameState);

            menuController = new MenuController(this, gameState, spriteBatch);
            menuController.Initialize();

            levelController = new LevelSelectController(gameState, spriteBatch);
            levelController.Initialize();

            playlevel = new PlayLevel(this, gameState, spriteBatch, buffer);
            playlevel.Initialize();

            worldController = new WorldMapController(this, gameState, spriteBatch, buffer);
            worldController.Initialize();
            pauseController = new PauseScreenController(this, spriteBatch);
            pauseController.Initialize();

            Components.Add(new GamerServicesComponent(this));
            Components.Add(new Components.SaveManager(this));

            Components.Add(new BatonView(this, spriteBatch));

            base.Initialize();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Deprecated
            WIDTH = gameState.ViewPort.Width;
            HEIGHT = gameState.ViewPort.Height;

            graphics.PreferredBackBufferWidth = gameState.ViewPort.Width;
            graphics.PreferredBackBufferHeight = gameState.ViewPort.Height;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();


            menuController.LoadContent(Content);
            levelController.LoadContent(Content);

            worldController.LoadContent(Content);

            Movement.LoadContent(Content);
            
            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            input.Update(gameTime);

            // transitioning to new state
            if (lastState != gameState.CurrentScreen)
            {
                switch (lastState)
                {
                    case DataTypes.Screens.PlayLevel:
                        if (gameState.CurrentScreen == DataTypes.Screens.Pause)
                        {
                            playlevel.Pause();
                        }
                        break;
                    case DataTypes.Screens.SelectLevel:
                        if (gameState.CurrentScreen == DataTypes.Screens.PlayLevel)
                        {
                            buffer.Clear();
                            playlevel = new PlayLevel(this, gameState, spriteBatch, buffer);
                            playlevel.Initialize();
                        }
                        break;
                    case DataTypes.Screens.WorldMap:
                        if (gameState.CurrentScreen == DataTypes.Screens.PlayLevel)
                        {
                            buffer.Clear();
                            playlevel = new PlayLevel(this, gameState, spriteBatch, buffer);
                            playlevel.Initialize();
                        }
                        break;
                    case DataTypes.Screens.Title:
                        if (gameState.CurrentScreen == DataTypes.Screens.WorldMap)
                        {
                            buffer.Clear();
                            worldController.Update(gameTime, false);
                        }
                        break;
                }

                switch (gameState.CurrentScreen)
                {
                    case DataTypes.Screens.Pause:
                        playlevel.Pause();
                        break;
                    case DataTypes.Screens.PlayLevel:
                        playlevel.Start();
                        break;
                }
                
            }

            lastState = gameState.CurrentScreen;
            
            switch (gameState.CurrentScreen)
            {
                case DataTypes.Screens.Title:
                    menuController.Update(gameTime);
                    break;
                case DataTypes.Screens.SelectLevel:
                    levelController.Update(gameTime);
                    break;
                case DataTypes.Screens.PlayLevel:
                    playlevel.Update(gameTime);
                    break;
                case DataTypes.Screens.WorldMap:
                    worldController.Update(gameTime,true);
                    break;
                case DataTypes.Screens.Pause:
                    pauseController.Update(gameTime);
                    break;
            }
            
            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            
            switch (gameState.CurrentScreen)
            {
                case DataTypes.Screens.Title:
                    menuController.Draw(gameTime);
                    break;
                case DataTypes.Screens.SelectLevel:
                    levelController.Draw(gameTime);
                    break;
                case DataTypes.Screens.PlayLevel:
                    playlevel.Draw(gameTime);
                    break;
                case DataTypes.Screens.WorldMap:
                    worldController.Draw(gameTime);
                    break;
                case DataTypes.Screens.Pause:
                    pauseController.Draw(gameTime);
                    break;
            }
             
            base.Draw(gameTime);
            spriteBatch.End();
        }

        protected void Restart()
        {
            UnloadContent();
            Initialize();
            LoadContent();
        }

    }
}

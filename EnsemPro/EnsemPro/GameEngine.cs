using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnsemPro
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameEngine : Game
    {
        public const int WIDTH = 800;
        public const int HEIGHT = 600;

        // for later
        IUpdateable ActiveController;
        IDrawable ActiveView;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameModel gameState;
        DataTypes.Screens lastState = DataTypes.Screens.Initial;

        MenuController menuController;
        PlayLevel rhythmController;
        LevelSelectController levelController;

        WorldMapController worldController;
        PauseScreen pauseController; // a misnomer

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
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;
            graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(GraphicsDevice);

            buffer = new InputBuffer();
            gameState = new GameModel();
            gameState.SelectedLevel = "Levels/B5/b5-edited-2";

            input = new MouseController(this, gameState, buffer);

            Services.AddService(typeof(GameModel), gameState);

            menuController = new MenuController(this, gameState, spriteBatch);
            menuController.Initialize();

            levelController = new LevelSelectController(gameState, spriteBatch);
            levelController.Initialize();

            rhythmController = new PlayLevel(this, gameState, spriteBatch, buffer);
            rhythmController.Initialize();

            worldController = new WorldMapController(this, gameState, spriteBatch);
            worldController.Initialize();
            pauseController = new PauseScreen(this, spriteBatch);
            pauseController.Initialize();

            base.Initialize();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            gameState.LoadContent(Content);
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

           // dialogController.Update(gameTime);
            
            // transitioning to new state
            if (lastState != gameState.CurrentScreen)
            {
                switch (lastState)
                {
                    case DataTypes.Screens.PlayLevel:
                        if (gameState.CurrentScreen == DataTypes.Screens.Pause)
                        {
                            rhythmController.Pause();
                        }
                        break;
                    case DataTypes.Screens.SelectLevel:
                        if (gameState.CurrentScreen == DataTypes.Screens.PlayLevel)
                        {
                            buffer.Clear();
                            rhythmController = new PlayLevel(this, gameState, spriteBatch, buffer);
                            rhythmController.Initialize();
                            rhythmController.Start();
                        }
                        break;
                }
                switch (gameState.CurrentScreen)
                {
                    case DataTypes.Screens.Pause:
                        rhythmController.Pause();
                        break;
                    case DataTypes.Screens.PlayLevel:
                        rhythmController.Start();
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
                    rhythmController.Update(gameTime);
                    break;
                case DataTypes.Screens.WorldMap:
                    worldController.Update(gameTime);
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
           // 5Controller.Draw(gameTime);
            
            switch (gameState.CurrentScreen)
            {
                case DataTypes.Screens.Title:
                    menuController.Draw(gameTime);
                    break;
                case DataTypes.Screens.SelectLevel:
                    levelController.Draw(gameTime);
                    break;
                case DataTypes.Screens.PlayLevel:
                    rhythmController.Draw(gameTime);
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

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

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameModel gameState;        
        PlayLevel rhythmController;
        LevelSelectController levelController;

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

            

            

            gameState = new GameModel();

            levelController = new LevelSelectController(gameState, spriteBatch);
            levelController.Initialize();
            
            rhythmController = new PlayLevel(this, gameState, spriteBatch);
            rhythmController.Initialize();
            base.Initialize();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            gameState.LoadContent(Content);
            levelController.LoadContent(Content);
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
            switch (gameState.CurrentScreen)
            {
                case DataTypes.Screens.LevelSelect:
                    levelController.Update(gameTime);
                    break;
                case DataTypes.Screens.PlayLevel:
                    rhythmController.Update(gameTime);
                    break;
                case DataTypes.Screens.Pause:
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
                case DataTypes.Screens.LevelSelect:
                    levelController.Draw(gameTime);
                    break;
                case DataTypes.Screens.PlayLevel:
                    rhythmController.Draw(gameTime);
                    break;
                case DataTypes.Screens.Pause:
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

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

        BatonView baton;
        SatisfactionQueue satisfaction;
        PlayLevel level;
        PauseScreen pauseScreen;
        KeyboardState lastState = Keyboard.GetState();

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

            InputBuffer buffer = new InputBuffer();

            try
            {
                Components.Add(new WiiController(this, buffer));
            }
            catch (WiimoteLib.WiimoteNotFoundException)
            {
                Components.Add(new MouseController(this, buffer));
            }

            baton = new BatonView(this, spriteBatch, buffer);
            satisfaction = new SatisfactionQueue(buffer);
            Components.Add(baton);
            level = new PlayLevel(this, baton, buffer, spriteBatch);
            Components.Add(level);

            pauseScreen = new PauseScreen(this, spriteBatch);
            Components.Add(pauseScreen);
            base.Initialize();
            level.Start();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
            satisfaction.LoadContent(Content);
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
            KeyboardState currentState = Keyboard.GetState();
            if (lastState.IsKeyUp(Keys.P) && currentState.IsKeyDown(Keys.P))
                PauseOrResume();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            lastState = Keyboard.GetState();

            satisfaction.Update(gameTime);
            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            base.Draw(gameTime);
            if (level.Visible)
                satisfaction.Draw(spriteBatch);
            spriteBatch.End();
        }

        protected void Restart()
        {
            UnloadContent();
            Initialize();
            LoadContent();
        }

        public void PauseOrResume()
        {
            baton.Enabled = !level.Enabled;
            level.Enabled = !level.Enabled;
            baton.Visible = !baton.Visible;
            level.Visible = !level.Visible;
            pauseScreen.Enabled = !pauseScreen.Enabled;
            pauseScreen.Visible = !pauseScreen.Visible;
        }
    }
}

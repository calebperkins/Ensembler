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

        

        Baton baton;
        SatisfactionQueue satisfaction;
        PlayLevel level;

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

            satisfaction = new SatisfactionQueue();
            baton = new Baton(this, spriteBatch);
            Components.Add(baton);
            level = new PlayLevel(baton);
            level.Initialize();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            
            satisfaction.LoadContent(Content);
            level.LoadContent(Content);
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
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.R))
                Restart();

            level.Update(gameTime);
            satisfaction.Update(gameTime);

            // Not sure if it's the best way to add stars, but here it is for now
            if (gameTime.TotalGameTime.Ticks % 2 == 0)
            {
                satisfaction.Add(baton.Position);
            }
            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();            
            level.Draw(spriteBatch);
            satisfaction.Draw(spriteBatch);


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

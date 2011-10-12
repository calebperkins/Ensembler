using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace EnsemPro
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameEngine : Microsoft.Xna.Framework.Game
    {
        public const int WIDTH = 800;
        public const int HEIGHT = 600;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private static Texture2D background;
        ContentManager content;

        Song song;

        Baton baton;
        NoteQueue notes;
        SatisfactionQueue satisfaction;

        //DEL
        public static int counter = 400;
        public static int scounter = 0;
        public static int numYays = 0;
        //DELETE

        public GameEngine()
        {
            graphics = new GraphicsDeviceManager(this);
            content = new ContentManager(Services);
            content.RootDirectory = "Content";
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

            baton = new Baton();
            notes = new NoteQueue();
            satisfaction = new SatisfactionQueue();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            background = content.Load<Texture2D>("images\\background");
            baton.LoadContent(content);
            notes.LoadContent(content);
            satisfaction.LoadContent(content);

            song = content.Load<Song>("images\\journey");
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Play(song);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            content.Unload();

            //DEL
            counter = 400;
            scounter = 0;
            //DELETE
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.R))
                restart();

            base.Update(gameTime);
        }

        //DELETE

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            spriteBatch.End();

            baton.Draw(spriteBatch);
            notes.Draw(spriteBatch);
            satisfaction.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        protected void restart()
        {
            UnloadContent();
            Initialize();
            LoadContent();
        }
    }
}

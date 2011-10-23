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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class PauseScreen : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont menuFont;

        public PauseScreen(Game game, SpriteBatch sb)
            : base(game)
        {
            spriteBatch = sb;
            // TODO: Construct any child components here
        }

        protected override void LoadContent()
        {
            menuFont = Game.Content.Load<SpriteFont>("images/Lucidia");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            Visible = false;
            Enabled = false;

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            string text = "Paused";
            Vector2 textSize = menuFont.MeasureString(text);
            Vector2 textCenter = new Vector2(Game.GraphicsDevice.Viewport.Width/2, Game.GraphicsDevice.Viewport.Height/2);
            spriteBatch.DrawString(menuFont, text, textCenter - textSize/2, Color.White);
            base.Draw(gameTime);
        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


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
           // graphics = new GraphicsDevice(game);
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
            string pauseText = "Paused";
            string backText = "Back";
            Vector2 pauseSize = menuFont.MeasureString(pauseText);
            Vector2 backSize = menuFont.MeasureString(backText);
            Vector2 textCenter = new Vector2(Game.GraphicsDevice.Viewport.Width/2, Game.GraphicsDevice.Viewport.Height/2);
            spriteBatch.DrawString(menuFont, pauseText, textCenter - pauseSize/2 - new Vector2(0,50), Color.White);
            spriteBatch.DrawString(menuFont, backText, textCenter - backSize / 2 + new Vector2(0, 50), Color.White);
            base.Draw(gameTime);
        }

    }
}

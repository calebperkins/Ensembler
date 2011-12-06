using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace EnsemPro
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class PauseScreenController : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont menuFont;
        GameState state;
        Texture2D background;

        Rectangle titleBox;
        Vector2 textCenter;

        public PauseScreenController(Game game, SpriteBatch sb)
            : base(game)
        {
            spriteBatch = sb;
           // graphics = new GraphicsDevice(game);
            // TODO: Construct any child components here
        }

        protected override void LoadContent()
        {
            menuFont = Game.Content.Load<SpriteFont>("Images//SelectionScreen//GermanUnderground");
            background = Game.Content.Load<Texture2D>("Images//PauseScreen//PauseScreen_temp");
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
            state = Game.Services.GetService(typeof(GameState)) as GameState;

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            textCenter = new Vector2(Game.GraphicsDevice.Viewport.Width/2, Game.GraphicsDevice.Viewport.Height/2);
            titleBox = new Rectangle(400 - 134 / 2, 300 - 40 / 2 + 100, 200, 40);
            if (state.Input.Confirm && state.Input.Inside(titleBox))
            {
                state.CurrentScreen = DataTypes.Screens.Title;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            string pauseText = "~ Paused ~";
            string titleText = "Exit level";
            spriteBatch.Draw(background, new Vector2(), Color.White);
            Vector2 pauseSize = menuFont.MeasureString(pauseText);
            Vector2 titleSize = menuFont.MeasureString(titleText);
            spriteBatch.DrawString(menuFont, pauseText, textCenter - pauseSize/2 - new Vector2(0,50), Color.White);
            spriteBatch.DrawString(menuFont, titleText, textCenter - titleSize / 2 + new Vector2(0, 100), state.Input.Inside(titleBox) ? Color.Yellow : Color.White);
            base.Draw(gameTime);
        }

    }
}

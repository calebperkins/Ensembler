using Microsoft.Xna.Framework;

namespace EnsemPro
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class InputController : Microsoft.Xna.Framework.GameComponent
    {
        protected InputBuffer buffer;
        protected InputState input;
        protected InputState lastState = new InputState();
        protected const float POS_DIFF_THRESHOLD = 5.0f;
        protected GameModel gameState;

        public InputController(Game game, GameModel gm, InputBuffer b)
            : base(game)
        {
            buffer = b;
            gameState = gm;
        }

        public override void Update(GameTime gameTime)
        {
            if (input.Pause && !lastState.Pause)
            {
                if (gameState.CurrentScreen == DataTypes.Screens.Pause)
                {
                    gameState.CurrentScreen = gameState.LastScreen;
                }
                else
                {
                    gameState.CurrentScreen = DataTypes.Screens.Pause;
                }
            }

            gameState.ConfirmChanged = input.Confirm && !lastState.Confirm;

            lastState = input;
            base.Update(gameTime);
        }

    }
}

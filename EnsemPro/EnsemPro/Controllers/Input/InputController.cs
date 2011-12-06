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
        protected GameState gameState;

        public InputController(Game game, GameState gm, InputBuffer b)
            : base(game)
        {
            buffer = b;
            gameState = gm;
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: move this to better location
            if (input.Pause && !lastState.Pause)
            {
                switch (gameState.CurrentScreen)
                {
                    case DataTypes.Screens.Title:
                        break;
                    case DataTypes.Screens.SelectLevel:
                        gameState.CurrentScreen = DataTypes.Screens.Title;
                        break;
                    case DataTypes.Screens.WorldMap:
                        gameState.CurrentScreen = DataTypes.Screens.Title;
                        break;
                    case DataTypes.Screens.Pause:
                        gameState.CurrentScreen = gameState.PreviousScreen;
                        break;
                    default:
                        gameState.CurrentScreen = DataTypes.Screens.Pause;
                        break;
                }
            }

            lastState = input;
            gameState.Input = input;
            base.Update(gameTime);
        }

    }
}

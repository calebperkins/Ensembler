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
        protected const float POS_DIFF_THRESHOLD = 5.0f;

        public InputController(Game game, InputBuffer b)
            : base(game)
        {
            buffer = b;
        }

    }
}

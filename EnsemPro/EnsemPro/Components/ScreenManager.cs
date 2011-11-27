using Microsoft.Xna.Framework;

namespace EnsemPro.Components
{
    // TODO: add state management here
    class ScreenManager : GameComponent
    {
        DataTypes.Screens lastState;
        GameState gameState;

        public ScreenManager(Game g) 
            : base(g)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}

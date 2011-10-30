using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EnsemPro
{
    public class BatonView : DrawableGameComponent
    {
        Color shadow;
        Texture2D batonTexture;
        SpriteBatch spriteBatch;
        InputBuffer buffer;

        public const float POS_DIFF_THRESHOLD = 5.0f;

        public BatonView(Game g, SpriteBatch sb, InputBuffer buf) : base(g)
        {
            shadow = new Color(0, 0, 0, 128);
            spriteBatch = sb;
            buffer = buf;
            DrawOrder = 1;
        }

        protected override void LoadContent()
        {
            batonTexture = Game.Content.Load<Texture2D>("Images\\baton");
            base.LoadContent();
        }

        public override void Draw(GameTime t)
        {
             spriteBatch.Draw(batonTexture, buffer.CurrentPosition, null, Color.White, 0.0f, new Vector2(0, 0), 0.35f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(batonTexture, buffer.CurrentPosition + new Vector2(3.0f), null, shadow, 0.0f, new Vector2(0, 0), 0.35f, SpriteEffects.None, 0.0f);
        }

    }
}

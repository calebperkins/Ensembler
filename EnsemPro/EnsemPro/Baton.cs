using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace EnsemPro
{
    public class Baton
    {
        Color shadow;
        Vector2 pos;
        bool A;

        BatonController controller;
        Texture2D batonTexture;

        public Baton(BatonController bg)
        {
            shadow = new Color(0, 0, 0, 128);
            controller = bg;
        }

        public void LoadContent(ContentManager content)
        {
            batonTexture = content.Load<Texture2D>("images\\baton");
        }

        public void Update(GameTime gameTime)
        {
            pos = controller.Position();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(batonTexture, pos, null, !A ? Color.White : Color.Black, 0.0f, new Vector2(0, 0), 0.35f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(batonTexture, new Vector2(pos.X + 3.0f, pos.Y + 3.0f), null, shadow, 0.0f, new Vector2(0, 0), 0.35f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }

        public Vector2 getPos()
        {
            return pos;
        }
    }
}

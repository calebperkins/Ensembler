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
    // The notes with changed texture upon collision with baton in gameplay prototype
    public class HitQueue : ObQueue
    {
        public HitQueue(int maxAge) : base()
        {
            this.maxAge = maxAge;
        }

        public void LoadContent(ContentManager content)
        {
            base.LoadContent(content, "images\\heart");
        }

        /*
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Add(Vector2 pos)
        {
            base.Add(pos);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        */
    }
}

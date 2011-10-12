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
    public class SatisfactionQueue : ObQueue
    {
        const int MAX_AGE = 30;

        public SatisfactionQueue() : base()
        {
            maxAge = MAX_AGE;
        }


        public void LoadContent(ContentManager content)
        {
            base.LoadContent(content, "images\\flower");
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

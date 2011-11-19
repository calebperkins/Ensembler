using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace EnsemPro
{
    // The stars following the baton
    public class SatisfactionQueue : ObQueue
    {
        public const int MAX_AGE = 30;
        InputBuffer buf;

        public SatisfactionQueue(InputBuffer b) : base()
        {
            maxAge = MAX_AGE;
            buf = b;
        }


        public void LoadContent(ContentManager content, String path)
        {
            base.LoadContent(content, path);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Not sure if it's the best way to add stars, but here it is for now
            if (gameTime.TotalGameTime.Ticks % 2 == 0)
                Add(buf.CurrentPosition);
        }
    }
}

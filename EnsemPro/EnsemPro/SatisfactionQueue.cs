using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace EnsemPro
{
    // The stars following the baton
    public class SatisfactionQueue : ObQueue
    {
        const int MAX_AGE = 30;
        Baton baton;

        public SatisfactionQueue(Baton b) : base()
        {
            maxAge = MAX_AGE;
            baton = b;
        }


        public void LoadContent(ContentManager content)
        {
            base.LoadContent(content, "images\\flower");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Not sure if it's the best way to add stars, but here it is for now
            if (gameTime.TotalGameTime.Ticks % 2 == 0)
                Add(baton.Position);
        }
    }
}

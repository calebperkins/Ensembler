using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Ensembler
{
    /// <summary>
    /// The stars following the baton
    /// </summary>
    public class SatisfactionQueue
    {
        const int MAX_IN_QUEUE = 1024;
        const int OB_SIZE = 40;

        Texture2D texture;
        public int maxAge;

        Satisfaction[] queue;
        int head;
        int tail;
        int queueSize;

        Vector2 origin;

        struct Satisfaction
        {
            public Vector2 pos;
            public int age;
        }

        public const int MAX_AGE = 30;
        InputBuffer buf;

        public SatisfactionQueue(InputBuffer b)
        {
            queue = new Satisfaction[MAX_IN_QUEUE];
            head = 0;
            tail = -1;
            queueSize = 0;

            for (int ii = 0; ii < MAX_IN_QUEUE; ii++)
            {
                queue[ii] = new Satisfaction();
            }

            origin = new Vector2();

            maxAge = MAX_AGE;
            buf = b;
        }

        public int MaxAge()
        {
            return MAX_AGE;
        }

        public void LoadContent(ContentManager content, String path)
        {
            texture = content.Load<Texture2D>(path);
        }

        public void Update(GameTime gameTime)
        {
            while (queueSize > 0 && queue[head].age > maxAge)
            {
                head = ((head + 1) % MAX_IN_QUEUE);
                queueSize--;
            }

            for (int ii = 0; ii < queueSize; ii++)
            {
                int idx = ((head + ii) % MAX_IN_QUEUE);
                queue[idx].age++;
            }

            // Not sure if it's the best way to add stars, but here it is for now
            if (gameTime.TotalGameTime.Ticks % 2 == 0)
                Add(buf.CurrentPosition);
        }

        void Add(Vector2 pos)
        {
            if (queueSize == MAX_IN_QUEUE)
            {
                head = ((head + 1) % MAX_IN_QUEUE);
                queueSize--;
            }

            tail = ((tail + 1) % MAX_IN_QUEUE);
            queue[tail].pos = pos;
            queueSize++;
            queue[tail].age = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;

            for (int ii = 0; ii < queueSize; ii++)
            {
                int idx = ((head + ii) % MAX_IN_QUEUE);

                // How big to make the ob. Increases with age
                float scale = 0.25f + (float)queue[idx].age * 0.5f / (float)maxAge;

                spriteBatch.Draw(texture, queue[idx].pos, null, Color.White, 0.0f, origin, scale, SpriteEffects.None, 0);
            }
        }

    }
}

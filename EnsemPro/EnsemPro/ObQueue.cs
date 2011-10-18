using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EnsemPro
{
    // Superclass of all the queues
    public abstract class ObQueue
    {
        public const int MAX_IN_QUEUE = 1024;
        public const int OB_SIZE = 40;
        
        protected Texture2D texture;
        protected int maxAge;

        protected Ob[] queue;
        protected int head;
        protected int tail;
        protected int queueSize;

        private Vector2 position;
        private Vector2 origin;

        public struct Ob
        {
            public Vector2 pos;
            public int age;
            public float scale;
        }

        public ObQueue()
        {
            queue = new Ob[MAX_IN_QUEUE];
            head = 0;
            tail = -1;
            queueSize = 0;

            for (int ii = 0; ii < MAX_IN_QUEUE; ii++)
            {
                queue[ii] = new Ob();
            }

            position = new Vector2(0.0f, 0.0f);
            origin = new Vector2(0.0f, 0.0f);
        }
        
        public void LoadContent(ContentManager content, String dir)
        {
            texture = content.Load<Texture2D>(dir);
        }

        public virtual void Update(GameTime gameTime)
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
        }

        public virtual void Add(Vector2 pos)
        {
            Add(pos, 0);
        }

        public virtual void Add(Vector2 pos, int curAge)
        {
            if (queueSize == MAX_IN_QUEUE)
            {
                head = ((head + 1) % MAX_IN_QUEUE);
                queueSize--;
            }

            tail = ((tail + 1) % MAX_IN_QUEUE);
            queue[tail].pos = pos;
            queueSize++;
            queue[tail].age = curAge;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //origin = new Vector2(0.5f * OB_SIZE);

            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;

            for (int ii = 0; ii < queueSize; ii++)
            {
                int idx = ((head + ii) % MAX_IN_QUEUE);

                // Tint color.
                // Color tint = Color.White;

                // How big to make the ob. Increases with age
                float scale = 0.25f + (float)queue[idx].age * 0.5f / (float)maxAge;


                // How much to rotate the image
                //queue[idx].rotate -= 0.5f;

                spriteBatch.Draw(texture, queue[idx].pos, null, Color.White, 0.0f, origin, scale, SpriteEffects.None, 0);
            }
        }

        public void decQueueSize()
        {
            queueSize--;
        }
    }
}

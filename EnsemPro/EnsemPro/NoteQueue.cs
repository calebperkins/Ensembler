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
    // The notes that the baton followed in the gameplay prototype
    public class NoteQueue : ObQueue
    {
        const int MAX_AGE = 40;

        HitQueue hits;

        public NoteQueue() : base()
        {
            maxAge = MAX_AGE;
            hits = new HitQueue(MAX_AGE);
        }

        public void LoadContent(ContentManager content)
        {
            base.LoadContent(content, "images\\note_yellow");
            hits.LoadContent(content);
        }

        public void Update(GameTime gameTime, Vector2 batonPos)
        {
            base.Update(gameTime);
            HandleCollision(batonPos);
            hits.Update(gameTime);
        }

        /*
        public override void Add(Vector2 pos)
        {
            base.Add(pos);
        }
         * */

        public void HandleCollision (Vector2 batonPos)
        {
            Vector2 disp = batonPos - queue[head].pos;
            //Determine distance between wand and star
            float dist = disp.Length();
            disp.Normalize();

            float collide = 50;//DOT_SIZE / 2;
            //Console.WriteLine(dist + " " + collide);
            // Are we too close?
            if (dist < collide) //dist < collide
            {
                hits.Add(queue[head].pos, queue[head].age);
                head = ((head + 1) % MAX_IN_QUEUE);
                decQueueSize();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            hits.Draw(spriteBatch);
        }
    }
}

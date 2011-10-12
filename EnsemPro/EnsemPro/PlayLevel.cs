using System;
using System.Diagnostics;
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
    public class PlayLevel
    {
        Stopwatch watch = new Stopwatch();
        int start_time;
        int current_beat;
        int last_beat;
        // hardcoded for now
        int beatTime = 60000 / 100; 
        Queue<Movement> actionQueue = new Queue<Movement>();

        SpriteFont font;

        public PlayLevel()
        {
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("images//Lucidia");

        }
        public void start()
        {
            watch.Start();
        }
        
        public void Update(GameTime gameTime)
        {
            current_beat = (int)Math.Round((float)watch.ElapsedMilliseconds / (float)beatTime);
            //TODO calculates the current beat here
            if (current_beat > last_beat)
            {
                last_beat = current_beat;
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {       spriteBatch.Begin();

                // Draw Hello World
                string output = "beat " + current_beat;
                // Find the center of the string
                Vector2 FontOrigin = font.MeasureString(output) / 2;
                // Draw the string
                spriteBatch.DrawString(font, output, new Vector2(300, 300), Color.Black);

                spriteBatch.End();
          
        }

    }
}

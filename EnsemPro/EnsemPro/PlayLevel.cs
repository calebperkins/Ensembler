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
        int current_beat;
        int last_beat;
        // hardcoded for now
        int beatTime = 60000 / 100;

        int current_score;
        SpriteFont font;

        LinkedList<Movement> actionList;
        HashSet<Movement> drawSet;

        Movement current_act;

        public PlayLevel()
        {
            actionList = new LinkedList<Movement>();
            drawSet = new HashSet<Movement>();
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("images//Lucidia");

        }
        public void start()
        {
            actionList.AddLast(new Movement(Movement.Type.Shake, 1, 6, 1, 6));
            actionList.AddLast(new Movement(Movement.Type.Noop, 7, 8, 7, 8));
            actionList.AddLast(new Movement(Movement.Type.Shake, 9, 14, 9, 14));
            actionList.AddLast(new Movement(Movement.Type.Noop, 15, 16, 15, 16));
            actionList.AddLast(new Movement(Movement.Type.Wave, 17, 32, 17, 32, new Point(50, 50), new Point(180, 180), null));
            watch.Start();

            LevelParser.lol();
        }
        
        public void Update(GameTime gameTime)
        {
            current_beat = (int)Math.Round((float)watch.ElapsedMilliseconds / (float)beatTime);

            if (current_beat > last_beat)
            {
                last_beat = current_beat;
                
                LinkedListNode<Movement> checkMove = actionList.First;

                drawSet.RemoveWhere(expired);

                while (checkMove != null)
                {
                    
                    if (checkMove.Value.showBeat == current_beat)
                    {
                        drawSet.Add(checkMove.Value);
                        checkMove = checkMove.Next;
                    }
                    else break;
                }
                
                // check and remove the head of the list
                if (actionList.First !=  null && actionList.First.Value.startBeat == current_beat)
                {
                    current_act = actionList.First.Value;
                    actionList.RemoveFirst();
                }
            }

        }

        private bool expired(Movement m)
        {
            return m.fadeBeat < current_beat;
        }

        public void Draw(SpriteBatch spriteBatch)
        {      

                // Draw Hello World
                string output = "beat " + current_beat;
                // Find the center of the string
                Vector2 FontOrigin = font.MeasureString(output) / 2;
                // Draw the string
                spriteBatch.Begin();    
                spriteBatch.DrawString(font, output, new Vector2(0, 0), Color.Black);
                spriteBatch.End();
                
                foreach (Movement m in drawSet)
                {
                    m.Draw(spriteBatch);
                }
          
        }

    }
}

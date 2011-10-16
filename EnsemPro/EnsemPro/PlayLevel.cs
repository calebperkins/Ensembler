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

        public const float INTERVAL_TIME = 1.0f;
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
        Baton baton;
        MovementEvaluator moveEval;

        public PlayLevel(Baton b)
        {
            actionList = new LinkedList<Movement>();
            drawSet = new HashSet<Movement>();
            baton = b;
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("images//Lucidia");

        }
        public void Initialize()
        {
            actionList.AddLast(new Movement(Movement.Type.Shake, 1, 6, 1, 6));
            actionList.AddLast(new Movement(Movement.Type.Noop, 7, 9, 7, 9));
            //actionList.AddLast(new Movement(Movement.Type.Wave, 17, 32, 17, 32, new Point(50, 50), new Point(180, 180), null));
            Function f1 = new Function();
            //Movement move1 = new Movement(Movement.Type.Wave, 17, 32, 17, 32, new Point(100, 100), new Point(400, 400), f1);
            Movement move1 = new Movement(Movement.Type.Wave, 10, 15, 9, 16, new Point(200, 400), new Point(600, 400), f1);
            moveEval = new MovementEvaluator(move1);
            f1.InitializeCurve(Function.Type.Curve, move1, 60, 100);
            actionList.AddLast(move1);
            moveEval = new MovementEvaluator(move1);
            watch.Start();

            LevelWriter.writeLevel();
        }
        
        public void Update(GameTime gameTime)
        {
            current_beat = (int)Math.Round((float)watch.ElapsedMilliseconds / (float)beatTime);

            if (current_beat > last_beat) // new beat
            {
                last_beat = current_beat;

                
                baton.Flush();
                
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

                float score = moveEval.Score(current_act, baton.Buffer(), gameTime);
                current_score += (int)(score * 10);
                moveEval.Update(current_act, gameTime);
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
                spriteBatch.DrawString(font, "score " + current_score, new Vector2(300, 0), Color.Black);
                spriteBatch.End();
                
                foreach (Movement m in drawSet)
                {
                    // the following code is for controlling the alpha value, please do not change
                    float alpha = 0f;
                    if (m.startBeat > current_beat)
                    {
                        int total = (m.startBeat - m.showBeat) * beatTime;
                        int elapsed = Math.Max(0,(int)watch.ElapsedMilliseconds - m.showBeat * beatTime);
                        alpha = 1f - elapsed / (float)total;
                    }
                    else if (m.endBeat < current_beat)
                    {
                        int total = (m.fadeBeat - m.endBeat) * beatTime;
                        int elapsed = Math.Max(0,(int)watch.ElapsedMilliseconds - m.endBeat * beatTime);
                        alpha = elapsed / (float)total;
                    }
                    m.Draw(spriteBatch, alpha);
                }
          
        }

    }
}

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

        //public const float INTERVAL_TIME = 1.0f;
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
        public void Initialize(LinkedList<Movement> moves)
        {
            /*
            actionList.AddLast(new Movement(Movement.Type.Shake, 9, 14, 9, 14));
            actionList.AddLast(new Movement(Movement.Type.Noop, 15, 16, 15, 16));*/
            //actionList.AddLast(new Movement(Movement.Type.Wave, 17, 32, 17, 32, new Point(50, 50), new Point(180, 180), null));
            // type, start beat, end beat, show beat, fade beat, start coordinate, end coordinate, 
            /*
            actionList.AddLast(new Movement(Movement.Type.Shake, 1, 1, 7, 7));

            actionList.AddLast(new Movement(Movement.Type.Noop, 8, 8, 9, 9));

            actionList.AddLast(new Movement(Movement.Type.Shake, 9, 9, 15, 15));

            actionList.AddLast(new Movement(Movement.Type.Noop, 16, 16, 17, 18));

            Function f1 = new Function();
            Movement move1 = new Movement(Movement.Type.Wave, 17, 18, 19, 20, new Point(400, 400), new Point(400, 200), f1);
            f1.InitializeCurve(Function.Type.Curve, move1, 100, 0);
            actionList.AddLast(move1);

            Function f2 = new Function();
            Movement move2 = new Movement(Movement.Type.Wave, 18, 19, 20, 21, new Point(400, 200), new Point(600, 200), f2);
            f2.InitializeCurve(Function.Type.Curve, move2, 100, 0);
            actionList.AddLast(move2);

            Function f3 = new Function();
            Movement move3 = new Movement(Movement.Type.Wave, 19, 20, 21, 22, new Point(600, 200), new Point(200, 200), f3);
            f3.InitializeCurve(Function.Type.Curve, move3, 100, 0);
            actionList.AddLast(move3);

            Function f4 = new Function();
            Movement move4 = new Movement(Movement.Type.Wave, 20, 21, 22, 23, new Point(200, 200), new Point(400, 400), f4);
            f4.InitializeCurve(Function.Type.Curve, move4, 100, 0);
            actionList.AddLast(move4);
            */
            actionList = moves;
            //moveEval = new MovementEvaluator(move1);
            watch.Start();
        }
        
        public void Update(GameTime gameTime)
        {
            current_beat = (int)Math.Round((float)watch.ElapsedMilliseconds / (float)beatTime);

            if (current_beat > last_beat) // new beat
            {
                last_beat = current_beat;
          
                LinkedListNode<Movement> checkMove = actionList.First;

                drawSet.RemoveWhere(Expired);

                while (checkMove != null)
                {
                    
                    if (checkMove.Value.showBeat == current_beat)
                    {
                        drawSet.Add(checkMove.Value);
                        checkMove = checkMove.Next;
                    }
                    else if (checkMove.Value.showBeat > current_beat) break;
                    else checkMove = checkMove.Next;
                }
                
                // check and remove the head of the list
                if (actionList.First !=  null && actionList.First.Value.startBeat == current_beat)
                {
                    current_act = actionList.First.Value;
                    actionList.RemoveFirst();
                }

                //float score = moveEval.Score(current_act, baton.Buffer, gameTime);
                //current_score += (int)(score * 10);
                //moveEval.Update(current_act, baton.Buffer, gameTime);
                baton.Flush();
            }

        }

        private bool Expired(Movement m)
        {
            return m.fadeBeat < current_beat;
        }

        public void Draw(SpriteBatch spriteBatch)
        {      

                // Draw beat and score
                string output = "beat " + current_beat;
                // Find the center of the string
                Vector2 FontOrigin = font.MeasureString(output) / 2;
                // Draw the string
                spriteBatch.Begin();    
                spriteBatch.DrawString(font, output, new Vector2(0, 0), Color.Black);
                spriteBatch.DrawString(font, "score " + current_score, new Vector2(300, 0), Color.Black);
                spriteBatch.End();

            // sort it in ascending way
                var drawing =
                from m in drawSet
                orderby m.showBeat ascending
                select m;  
                foreach (Movement m in drawing)
                {
                    // the following code is for controlling the alpha value, please do not change
                    float alpha = 0f;
                    if (m.startBeat > current_beat)
                    {
                        int total = (m.startBeat - m.showBeat) * beatTime;
                        int elapsed = Math.Max(0,(int)watch.ElapsedMilliseconds - m.showBeat * beatTime);
                        alpha = 1f - elapsed / (float)total;
                        m.Draw(spriteBatch, alpha, true);
                    }
                    else if (m.endBeat < current_beat)
                    {
                        int total = (m.fadeBeat - m.endBeat) * beatTime;
                        int elapsed = Math.Max(0,(int)watch.ElapsedMilliseconds - m.endBeat * beatTime);
                        alpha = elapsed / (float)total;
                        m.Draw(spriteBatch, alpha, false);
                    }
                    else m.Draw(spriteBatch, alpha, false);
                    
                }
   
        }

    }
}

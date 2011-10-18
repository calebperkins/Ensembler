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
            actionList = moves;
            moveEval = new MovementEvaluator(moves.First.Value);
            watch.Start();
        }
        
        public void Update(GameTime gameTime)
        {
            current_beat = (int)Math.Round((float)watch.ElapsedMilliseconds / (float)beatTime);
            bool newMovement=false;

            if (current_beat > last_beat) // new beat
            {
                last_beat = current_beat;
                newMovement = true;
                if (current_act != null && current_act.endBeat < current_beat) current_act = null;

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
                if (current_act != null)
                {
                    float score = moveEval.Score(current_act, baton.Buffer, gameTime);
                    float gainedScore = score * 10 - (float)score;
                    if (actionList.First != null) // prevents score from endlessly increasing
                        current_score += (int)(score * 10);
                    baton.Flush();
                    moveEval.Update(current_act, gainedScore, newMovement, gameTime);
                }
                
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
                spriteBatch.DrawString(font, output, new Vector2(0, 0), Color.White);
                spriteBatch.DrawString(font, "score " + current_score, new Vector2(300, 0), Color.White);

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

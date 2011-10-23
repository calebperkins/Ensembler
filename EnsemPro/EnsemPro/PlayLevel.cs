using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace EnsemPro
{
    public class PlayLevel : DrawableGameComponent
    {

        //public const float INTERVAL_TIME = 1.0f;
        Stopwatch watch = new Stopwatch();
        int current_beat;
        int last_beat;
        int beatTime;
        int c = 0;
        int current_score;

        SpriteFont font;
        SpriteBatch spriteBatch;
        Texture2D background;

        LinkedList<Movement> actionList;
        HashSet<Movement> drawSet;

        Movement current_act;
        Baton baton;
        MovementEvaluator moveEval;

        List<Musician> musicians = new List<Musician>();

        public PlayLevel(Game g, Baton b, SpriteBatch sb) : base(g)
        {
            actionList = new LinkedList<Movement>();
            drawSet = new HashSet<Movement>();
            baton = b;
            spriteBatch = sb;
            DrawOrder = 0;
        }

        protected override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("images//Lucidia");

            DataTypes.LevelData data = Game.Content.Load<DataTypes.LevelData>("Levels/B5");
            Song song = Game.Content.Load<Song>(data.SongAssetName);
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Play(song);
            background = Game.Content.Load<Texture2D>(data.Background);

            foreach (DataTypes.MovementData md in data.Movements)
            {
                md.AssertValid();
                actionList.AddLast(new Movement(md));
            }

            // TODO: put this in XML
            musicians.Add(new Musician(Game.Content, spriteBatch, "Characters/alice_sprite", "Characters/alice_map", new Vector2(400), 10));
            musicians.Add(new Musician(Game.Content, spriteBatch, "Characters/Johannes_sprite", "Characters/Johannes_map", new Vector2(200, 400), 20));
            musicians.Add(new Musician(Game.Content, spriteBatch, "Characters/Lance_sprite", "Characters/Lance_map", new Vector2(100, 400), 20));

            beatTime = data.BPM;
            moveEval = new MovementEvaluator(actionList.First.Value);
            base.LoadContent();
        }

        public override void Initialize()
        {    

            base.Initialize();
        }

        public void Start()
        {

            current_beat = 0;
            last_beat = -1;
            watch.Start();
        }

        public override void Update(GameTime gameTime)
        {
            current_beat = (int)Math.Round((float)watch.ElapsedMilliseconds / (float)beatTime);
            bool newMovement = false;
            
            if (current_beat > last_beat) // new beat
            {
                
                last_beat = current_beat;
                if (current_act != null && current_act.endBeat < current_beat)
                {
                    current_act = null; 
                    
                }
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
                if (actionList.First != null && actionList.First.Value.startBeat == current_beat)
                {
                    current_act = actionList.First.Value;
                    actionList.RemoveFirst();c++;newMovement = true;
                }
               // Console.WriteLine("this is movement " + c);
                
                if (current_act != null)
                {
                    
                    float score = moveEval.Accuracy(current_act, baton.Buffer, gameTime);
                    float gainedScore = score * 10 - (float)score;
                    if (actionList.First != null) // prevents score from endlessly increasing
                        current_score += (int)(score * 10);
                  //  Console.WriteLine("number of inputs " + baton.Buffer.Count);
                    if (newMovement) {baton.Flush();
                 ///  Console.WriteLine("new movement! number of inputs" + baton.Buffer.Count);
                    } 
                 //   Console.WriteLine("score passed to moveEval's update is " + score);
                    moveEval.Update(current_act, score, newMovement, gameTime);
                }

            }

        }

        private bool Expired(Movement m)
        {
            return m.fadeBeat < current_beat;
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            if (Enabled)
            {
                MediaPlayer.Resume();
            }
            else
            {
                MediaPlayer.Pause();
            }
            
            base.OnEnabledChanged(sender, args);
        }

        public override void Draw(GameTime t)
        {
            // Draw background
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            // Draw beat and score
            string output = "beat " + current_beat;
            // Find the center of the string
            Vector2 FontOrigin = font.MeasureString(output) / 2;
            // Draw the string
            spriteBatch.DrawString(font, output, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font, "score " + current_score, new Vector2(300, 0), Color.White);

            foreach (Musician m in musicians)
            {
                m.Draw(t);
            }

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
                    int elapsed = Math.Max(0, (int)watch.ElapsedMilliseconds - m.showBeat * beatTime);
                    alpha = 1f - elapsed / (float)total;
                    m.Draw(spriteBatch, alpha, 0);
                }
                else if (m.endBeat > current_beat)
                {
                    int total = (m.endBeat - m.startBeat) * beatTime;
                    int elapsed = Math.Max(0, (int)watch.ElapsedMilliseconds - m.startBeat * beatTime);
                    alpha = 1f - elapsed / (float)total;
                    m.Draw(spriteBatch, alpha, 1);
                }
                else
                {
                    int total = (m.fadeBeat - m.endBeat) * beatTime;
                    int elapsed = Math.Max(0, (int)watch.ElapsedMilliseconds - m.endBeat * beatTime);
                    alpha = elapsed / (float)total;
                    m.Draw(spriteBatch, alpha, 2);
                }

            }

        }

    }
}

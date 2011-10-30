using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace EnsemPro
{
    public class PlayLevel : DrawableGameComponent
    {

        //public const float INTERVAL_TIME = 1.0f;
        TimeSpan watch = new TimeSpan();
        int current_beat;
        int last_beat;
        // beat_sum is for the purpose of beat time change
        int beat_sum = 0;
        int beatTime;
        int c = 0;
        int current_score;
        int gainedScore;
        bool comboOn;
        int comboCount;
        int maxCombo;

        SpriteFont font;
        SpriteBatch spriteBatch;
        Texture2D background;

        LinkedList<Movement> actionList;
        HashSet<Movement> drawSet;

        Movement current_act;
        BatonView baton;
        MovementEvaluator moveEval;
        InputBuffer buffer;
        InputController input;
        SatisfactionQueue satisfaction;

        List<Musician> musicians = new List<Musician>();

        public PlayLevel(Game g, GameModel gm, SpriteBatch sb) : base(g)
        {
            actionList = new LinkedList<Movement>();
            drawSet = new HashSet<Movement>();

            spriteBatch = sb;
            DrawOrder = 0;
            comboOn = false;
            comboCount = -1;
        }

        protected override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("images//ScoreFont");

            DataTypes.LevelData data = Game.Content.Load<DataTypes.LevelData>("Levels/b5-edited");
            Song song = Game.Content.Load<Song>(data.SongAssetName);
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Play(song);
            background = Game.Content.Load<Texture2D>(data.Background);

            beatTime = 60000 / data.BPM;

            int lastBeatTime = beatTime;
            foreach (DataTypes.MovementData md in data.Movements)
            {
                md.AssertValid();
                if (md.NewBPM > 0)
                {
                    lastBeatTime = md.NewBPM;
                }
                actionList.AddLast(new Movement(md, lastBeatTime));
            }

            // TODO: put this in XML
            musicians.Add(new Musician(Game.Content, spriteBatch, "Characters/alice_sprite", "Characters/alice_map", new Vector2(400), 10));
            musicians.Add(new Musician(Game.Content, spriteBatch, "Characters/Johannes_sprite", "Characters/Johannes_map", new Vector2(200, 400), 20));
            musicians.Add(new Musician(Game.Content, spriteBatch, "Characters/Lance_sprite", "Characters/Lance_map", new Vector2(100, 400), 20));
            
            moveEval = new MovementEvaluator(actionList.First.Value);
            base.LoadContent();
        }

        public override void Initialize()
        {
            buffer = new InputBuffer();
            try
            {
                input = new WiiController(Game, buffer);
            }
            catch (WiimoteLib.WiimoteNotFoundException)
            {
                input = new MouseController(Game, buffer);
            }

            baton = new BatonView(Game, spriteBatch, buffer);
            baton.Initialize();
            satisfaction = new SatisfactionQueue(buffer);
            satisfaction.LoadContent(Game.Content);
            base.Initialize();
            Start();
        }

        public void Start()
        {

            current_beat = 0;
            last_beat = -1;
            //watch.Start();
        }

        public override void Update(GameTime gameTime)
        {
            input.Update(gameTime);
            satisfaction.Update(gameTime);

            watch = watch.Add(gameTime.ElapsedGameTime);

            current_beat = beat_sum + (int)Math.Round((float)watch.TotalMilliseconds / (float)beatTime);
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

                do
                {
                    // check and remove the head of the list
                    if (actionList.First != null && actionList.First.Value.startBeat == current_beat)
                    {
                        current_act = actionList.First.Value;
                        if (current_act.myType != Movement.Types.Control)
                        {
                            actionList.RemoveFirst();
                            c++;
                            newMovement = true;
                        }
                        else
                        {
                            beat_sum = current_beat;
                            beatTime = 60000 /  current_act.BPM;
                            actionList.RemoveFirst();
                            watch = new TimeSpan();
                        }
                    }
                    else break;
                    // Console.WriteLine("this is movement " + c);
                } while (true);

                if (current_act != null)
                {
                    Movement.Types type = current_act.myType;
                    float score = moveEval.Accuracy(current_act, buffer, gameTime);
                    gainedScore = (int)(score * 10);

                    /* Keep the combo on if it is now Wave and the most recent gainedScore is greater than FAIL_THRESHOLD (i.e. success continues),
                     * or if combo is on before a Shake phase is entered,
                     * otherwise break the combo. */
                    comboOn = (gainedScore >= 4 && type == Movement.Types.Wave || comboOn && type==Movement.Types.Shake) ? true : false;

                    /** Add to combo count if it is now Wave and combo is on,
                     * else if it is now Wave but combo is broken, reset count to 0,
                     * else if combo is on but a Shape or Noop phase is entered, keep the count until next Wave. */
                    comboCount = comboOn && type == Movement.Types.Wave ? comboCount += 1 : (type == Movement.Types.Wave ? 0 : comboCount);
                    if (comboCount > maxCombo) maxCombo = comboCount;
                  //  if (actionList.First != null) // prevents score from endlessly increasing

                    /** Add gainedScore to current_score.
                     * If there is a combo and the most recent score is non-negative (e.g. Shaking is succuessful), also add the current combo count to the score */
                    current_score += (gainedScore + (comboCount > 1 && gainedScore > 0 ? comboCount : 0));
                    current_score = Math.Max(0, current_score);
                    if (newMovement) buffer.Clear();
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
            spriteBatch.Draw(background, new Vector2(), Color.White);

            // Draw beat and score
            string output = "beat " + current_beat;
            // Find the center of the string
            Vector2 FontOrigin = font.MeasureString(output) / 2;
            // Draw the string
            spriteBatch.DrawString(font, output, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font, "score " + current_score, new Vector2(300, 0), Color.White);
            spriteBatch.DrawString(font, (gainedScore >= 0 ? "+" : "") + gainedScore, new Vector2(460, 0), 
                (gainedScore > 0 ? Color.YellowGreen : (gainedScore < 0 ? Color.Red : Color.White)));
            if (comboCount >= 2) 
            {
                if (gainedScore > 0) // only add combo bonus when the last movement is successful
                {
                    spriteBatch.DrawString(font, "+ " + comboCount + " combo bonus", new Vector2(460, 30), Color.Violet, 0.0f, new Vector2(0, 0), 0.75f, SpriteEffects.None, 0.0f);
                }
                spriteBatch.DrawString(font, comboCount + " Combo", new Vector2(650, 0), Color.White); 
            }
            if (comboCount >= 10) spriteBatch.DrawString(font, "Great!", new Vector2(670, 30), Color.Tomato);
            
            // hard coded for now; should be "if song ends..."
            if (current_beat > 141) spriteBatch.DrawString(font, "Max Combo is " + maxCombo, new Vector2(200, 250), Color.Black);

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
                    int elapsed = Math.Max(0, (int)watch.Milliseconds - m.showBeat * beatTime);
                    alpha = 1f - elapsed / (float)total;
                    m.Draw(spriteBatch, alpha, 0);
                }
                else if (m.endBeat > current_beat)
                {
                    int total = (m.endBeat - m.startBeat) * beatTime;
                    int elapsed = Math.Max(0, (int)watch.Milliseconds - m.startBeat * beatTime);
                    alpha = 1f - elapsed / (float)total;
                    m.Draw(spriteBatch, alpha, 1);
                }
                else
                {
                    int total = (m.fadeBeat - m.endBeat) * beatTime;
                    int elapsed = Math.Max(0, (int)watch.Milliseconds - m.endBeat * beatTime);
                    alpha = elapsed / (float)total;
                    m.Draw(spriteBatch, alpha, 2);
                }

            }

            baton.Draw(t);
            satisfaction.Draw(spriteBatch);
        }

    }
}

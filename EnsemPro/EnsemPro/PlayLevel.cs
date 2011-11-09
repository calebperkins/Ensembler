using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace EnsemPro
{
    public class PlayLevel : DrawableGameComponent
    {
        GameModel gameState;
        //public const float INTERVAL_TIME = 1.0f;
        Stopwatch watch = new Stopwatch();
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
        int backToMenu;

        SpriteFont font;
        SpriteBatch spriteBatch;
        Texture2D background;

        LinkedList<Movement> actionList;
        HashSet<Movement> drawSet;

        Movement current_act;
        BatonView baton;
        MovementEvaluator moveEval;
        InputBuffer buffer;
        SatisfactionQueue satisfaction;
        Song song;
        float volume = 0.5f;

        List<Musician> musicians = new List<Musician>();

        public PlayLevel(Game g, GameModel gm, SpriteBatch sb, InputBuffer buf) : base(g)
        {
            gameState = gm;
            actionList = new LinkedList<Movement>();
            drawSet = new HashSet<Movement>();
            buffer = buf;

            spriteBatch = sb;
            DrawOrder = 0;
            comboOn = false;
            comboCount = -1;
        }

        protected override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("images//ScoreFont");

            // todo: dynamic loading
            DataTypes.LevelData data = Game.Content.Load<DataTypes.LevelData>(gameState.SelectedLevel);
            song = Game.Content.Load<Song>(data.SongAssetName);
            MediaPlayer.IsRepeating = false;
            
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
            musicians.Add(new Musician(Game.Content, spriteBatch, "Characters/alice_sprite", "Characters/alice_map", new Vector2(350,400), 10));
            musicians.Add(new Musician(Game.Content, spriteBatch, "Characters/Johannes_sprite", "Characters/Johannes_map", new Vector2(200, 400), 20));
            musicians.Add(new Musician(Game.Content, spriteBatch, "Characters/Lance_sprite", "Characters/Lance_map", new Vector2(100, 400), 20));
            musicians.Add(new Musician(Game.Content, spriteBatch, "Characters/thorsten_sprite", "Characters/thorsten_map", new Vector2(450, 400), 20));
            musicians.Add(new Musician(Game.Content, spriteBatch, "Characters/bella_sprite", "Characters/bella_map", new Vector2(600, 400), 20));
            
            moveEval = new MovementEvaluator(actionList.First.Value);
            base.LoadContent();
        }

        public override void Initialize()
        {
            //buffer = new InputBuffer();
            /*try
            {
                input = new WiiController(Game, buffer);
            }
            catch (WiimoteLib.WiimoteNotFoundException)
            {
                input = new MouseController(Game, buffer);
            }*/
            //input = new MouseController(Game, buffer);

            baton = new BatonView(Game, spriteBatch, buffer);
            baton.Initialize();
            satisfaction = new SatisfactionQueue(buffer);
            satisfaction.LoadContent(Game.Content);

            current_beat = 0;
            last_beat = -1;

            base.Initialize();
        }

        public void Start()
        {
            if (MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();
            else
                MediaPlayer.Play(song);
            watch.Start();
        }

        public void Pause()
        {
            watch.Stop();
            MediaPlayer.Pause();
        }

        public override void Update(GameTime gameTime)
        {
            
            satisfaction.Update(gameTime);

            // Adjusts volume
            Keys key = buffer.VolumeChange;
            if (key != Keys.None)
            {
            //Console.WriteLine(key);
                if (key == Keys.A)
                {
                    volume = MathHelper.Clamp(volume + 0.01f, 0.1f, 1);
                }
                else if (key == Keys.Z)
                {
                    volume = MathHelper.Clamp(volume - 0.01f, 0.1f, 1);
                }
                MediaPlayer.Volume = volume;
            }

            //watch = watch.Add(gameTime.ElapsedGameTime);

            current_beat = beat_sum + (int)Math.Round((float) watch.ElapsedMilliseconds / beatTime);
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
                            watch.Restart();
                        }
                    }
                    else break;
                    // Console.WriteLine("this is movement " + c);5
                } while (true);

                if (current_act != null)
                {
                    Movement.Types type = current_act.myType;
                    float score = moveEval.Accuracy(current_act, buffer, gameTime);
                    gainedScore = (int)(score * 10);
                    /* Keep the combo on if it is now Wave and the most recent gainedScore is greater than 0 (i.e. success continues),
                     * or if combo is on before a Shake phase is entered,
                     * otherwise break the combo. */
                    comboOn = !(gainedScore < 0 && type == Movement.Types.Wave /* and last is also wave */);

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
                    moveEval.Update(current_act, score, (newMovement || actionList.Count == 0), gameTime);
                }
            }
            if (actionList.Count == 0) backToMenu++;
            if (backToMenu >= 300) gameState.CurrentScreen = DataTypes.Screens.SelectLevel;
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
            string beat = "beat " + current_beat;
            // Find the center of the string
            // Vector2 FontOrigin = font.MeasureString(output) / 2;
            string vol = "volume: " + (int) (volume * 10);
            // Draw the string
            spriteBatch.DrawString(font, beat, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font, vol, new Vector2(125, 0), Color.White);
            spriteBatch.DrawString(font, "score " + current_score, new Vector2(300, 0), Color.White);
            spriteBatch.DrawString(font, actionList.Count!=0 ? (gainedScore >= 0 ? "+"+gainedScore : ""+gainedScore) : "", new Vector2(460, 0), 
                (gainedScore > 0 ? Color.YellowGreen : (gainedScore < 0 ? Color.Red : Color.White)));
            if (comboCount >= 2) 
            {
                if (gainedScore > 0) // only add combo bonus when the last movement is successful
                {
                    spriteBatch.DrawString(font, "+ " + comboCount + " combo bonus", new Vector2(460, 30), Color.Violet, 0.0f, new Vector2(0, 0), 0.75f, SpriteEffects.None, 0.0f);
                }
                spriteBatch.DrawString(font, comboCount + " Combo", new Vector2(650, 0), Color.White); 
            }
            if (comboCount >= 10) {
                if (comboCount >= 20) spriteBatch.DrawString(font, "Excellent!", new Vector2(640, 30), Color.Tomato); 
                else spriteBatch.DrawString(font, "Great!", new Vector2(670, 30), Color.Tomato); 
            }
            
            if (actionList.Count == 0)
            {
                spriteBatch.DrawString(font, "Score is " + current_score, new Vector2(200, 150), Color.Black, 0.0f, new Vector2(0, 0),
                    1.4f, SpriteEffects.None, 0.0f);
                spriteBatch.DrawString(font, "Max Combo is " + (maxCombo > 1 ? maxCombo : 0), new Vector2(200, 200), Color.Black, 0.0f, new Vector2(0, 0),
                    1.4f, SpriteEffects.None, 0.0f);
            }

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
                //Debug.WriteLine(alpha);
                satisfaction.Draw(spriteBatch);
                baton.Draw(t);
            }

            baton.Draw(t);
            satisfaction.Draw(spriteBatch);

        }

    }
}

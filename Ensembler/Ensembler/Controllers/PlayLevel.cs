using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Ensembler
{
    public class PlayLevel : DrawableGameComponent
    {
        // For adjusting curMaxAge of satisfaction queue
        public const int AGE_DECR = 0;
        public const int AGE_INCR = 1;
        bool failed;

        Stopwatch countdownWatch = new Stopwatch();
        public const int COUNTDOWN = 4;

        GameState gameState;
        //public const float INTERVAL_TIME = 1.0f;
        Stopwatch watch = new Stopwatch();
        
        int current_beat;
        int last_beat;
        // beat_sum is for the purpose of beat time change
        int beat_sum = 0;
        int accumulated_ms = 0;
        int countDownBeatSum = 0;
        int beatTime;
        int c = 0;
        int current_score;
        int gainedScore;
        bool comboOn;
        int comboCount;
        int maxCombo;
        int backToMenu;
        String satisfactionImagePath="Images\\aquarium"; // stubbed, but doesnt matter

        SpriteFont font;
        SpriteBatch spriteBatch;
        Texture2D background;
        Texture2D failTexture;
        float failScale = 2;

        LinkedList<Movement> actionList;
        HashSet<Movement> drawSet;

        Movement current_act;

        bool startTiming;
        bool endTiming;
        
        MovementEvaluator moveEval;
        InputBuffer buffer;
        SatisfactionQueue satisfaction;
        Song song;
        SoundEffectInstance distorted;
        float volume;
        float scaledVol;
        Texture2D volumeTexture;

        SoundEffect SmallApplause;
        SoundEffect LargeApplause;
        SoundEffect CountDown;
        Song LevelFail;

        List<Musician> musicians = new List<Musician>();

        public PlayLevel(Game g, GameState gm, SpriteBatch sb, InputBuffer buf) : base(g)
        {
            gameState = gm;
            actionList = new LinkedList<Movement>();
            drawSet = new HashSet<Movement>();
            buffer = buf;

            spriteBatch = sb;
            DrawOrder = 0;
            comboOn = false;
            comboCount = -1;
            failed = false;

            volume = 0.5f;
            scaledVol = (float)(0.524 * Math.Pow(Math.E, volume) - 0.425);
        }


        protected override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("images//ScoreFont");
            failTexture = Game.Content.Load<Texture2D>("images//fail");

            // todo: dynamic loading
            DataTypes.LevelData data = Game.Content.Load<DataTypes.LevelData>(gameState.SelectedLevel);
            song = Game.Content.Load<Song>(data.SongAssetName);
            distorted = Game.Content.Load<SoundEffect>(data.SongAssetName + "_distorted").CreateInstance();
            distorted.Volume = 0.0f;
            satisfactionImagePath = data.SatisfactionAssetName;
            satisfaction.LoadContent(Game.Content, satisfactionImagePath);

            MediaPlayer.IsRepeating = false;
            Console.WriteLine(gameState.SelectedLevel);

            CountDown = Game.Content.Load<SoundEffect>("Sounds//CountDown");
            SmallApplause = Game.Content.Load<SoundEffect>("Sounds//SmallApplause");
            LargeApplause = Game.Content.Load<SoundEffect>("Sounds//LargeApplause");
            LevelFail = Game.Content.Load<Song>("Sounds//LevelFail");

            background = Game.Content.Load<Texture2D>(data.Background);
            volumeTexture = Game.Content.Load<Texture2D>("images\\vol");

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

            if (data.Musicians != null)
            {
                foreach (DataTypes.MusicianData md in data.Musicians)
                {
                    musicians.Add(new Musician(md, Game.Content, spriteBatch));
                }
            }
                   
            moveEval = new MovementEvaluator(null);
            base.LoadContent();
        }

        public override void Initialize()
        {
            satisfaction = new SatisfactionQueue(buffer);
            current_beat = 0;
            last_beat = -1;
            base.Initialize();
        }

        public void Start()
        {
            if (CountDownDone())
            {
                countdownWatch.Stop();
                if (MediaPlayer.State == MediaState.Paused)
                {
                    MediaPlayer.Resume();
                    distorted.Resume();
                }
                else
                {
                    MediaPlayer.Play(song);
                    distorted.Play();
                }
                watch.Start();
            }
            else
            {
                countdownWatch.Start();
            }
        }

        public void Pause()
        {
            if (CountDownDone())
            {
                watch.Stop();
                MediaPlayer.Pause();
                distorted.Pause();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (CountDownDone() && !watch.IsRunning)
            {
                Start();
            }

            if (satisfaction.maxAge == 0 )
            {
                if (!failed)
                {
                    MediaPlayer.Stop();
                    distorted.Stop();
                    MediaPlayer.Play(LevelFail);
                }
                failed = true;
            }
            else
            {
                satisfaction.Update(gameTime);

                // Adjusts volume
                Keys key = gameState.Input.Key;
                if (key != Keys.None)
                {
                    if (key == Keys.A)
                    {
                        volume = MathHelper.Clamp(volume + 0.1f, 0.1f, 1);
                    }
                    else if (key == Keys.Z)
                    {
                        volume = MathHelper.Clamp(volume - 0.1f, 0.1f, 1);
                    }

                    scaledVol = (float)(0.524 * Math.Pow(Math.E, volume) - 0.425); // exponential scale
                    if (MediaPlayer.IsMuted) distorted.Volume = scaledVol;
                    else MediaPlayer.Volume = scaledVol;
                }

                //watch = watch.Add(gameTime.ElapsedGameTime);

                current_beat = beat_sum + (int)Math.Round((float)watch.ElapsedMilliseconds / beatTime);
                bool newMovement = false;
                startTiming = true;
                endTiming = true;
                if (current_beat > last_beat) // new beat
                {
                    if (current_act != null)
                    {
                        if (current_act.myType == Movement.Types.Wave)
                        {
                            if (Alpha(current_act.endBeat, current_act.startBeat) > 0.0f)
                            {
                                startTiming = moveEval.Timing(buffer, current_act.startCoordinate, true);
                            }
                            if (current_beat == current_act.endBeat)
                            {
                                endTiming = moveEval.Timing(buffer, current_act.endCoordinate, false);
                            }
                        }
                    }

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
                         //   buffer.Clear();
                            current_act = actionList.First.Value;
                         //   moveEval.currentMovement = current_act;
                           // evalBeat = current_beat + 1;
                            if (current_act.myType != Movement.Types.Control)
                            {
                                actionList.RemoveFirst();
                                c++;
                                newMovement = true;
                            }
                            else
                            {
                                beat_sum = current_beat;
                                accumulated_ms = accumulated_ms + (int)watch.ElapsedMilliseconds;
                                beatTime = 60000 / current_act.BPM;
                                actionList.RemoveFirst();
                                watch.Restart();
                            }
                        }
                        else break;
                    } while (true);

                    if (current_act != null)
                    {
                        Movement.Types type = current_act.myType;
                        float score = moveEval.Accuracy(current_act, buffer, (current_beat ==0 || current_beat ==1 ? true : startTiming&&endTiming), gameTime);

                        gainedScore = (int)(score * 10);
                        
                        // Adjusts age of satisfaction queue
                        if (gainedScore < 0)
                        {
                            if (satisfaction.maxAge < 5) satisfaction.maxAge = 0;
                            else satisfaction.maxAge = Math.Max(4, satisfaction.maxAge - AGE_DECR);
                            MediaPlayer.IsMuted = true;
                            distorted.Volume = scaledVol;
                        }
                        else
                        {
                            satisfaction.maxAge = Math.Min(SatisfactionQueue.MAX_AGE, satisfaction.maxAge + AGE_INCR);
                            MediaPlayer.IsMuted = false;
                            distorted.Volume = 0.0f;
                        }

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
                        
                        moveEval.Update(current_act, score, (newMovement || actionList.Count == 0), gameTime);
                        if (newMovement) { buffer.Clear();  }
                    }
                }
            }

            if (actionList.Count == 0 || failed) 
            {
                if (!failed && backToMenu == 0)
                {
                    if (current_score < 1000)
                    {
                        SmallApplause.Play();
                    }
                    else
                    {
                        LargeApplause.Play();
                    }
                }
                backToMenu++; 
            }
            if (backToMenu >= 420)
            {                 
                //UnloadContent(); // doesn't seem to work, i.e. memory usage does not decrease
                if (!failed)
                {
                    gameState.Score = current_score;
                    gameState.Combo = (maxCombo > 1 ? maxCombo : 0);

                    gameState.UpdateStats();
                }
                else
                {
                    gameState.Score = -1;
                    gameState.Combo = -1;
                }
                gameState.CurrentScreen = gameState.PreviousScreen;
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
                distorted.Resume();
            }
            else
            {
                MediaPlayer.Pause();
                distorted.Pause();
            }
            
            base.OnEnabledChanged(sender, args);
        }

        public override void Draw(GameTime t)
        {
            // Draw background
            int colorChange = satisfaction.MaxAge()-satisfaction.maxAge;
            int newRGB = 255-(int)255/satisfaction.MaxAge()*colorChange;
            Color tint = new Color(newRGB, newRGB, newRGB);
            spriteBatch.Draw(background, new Vector2(), tint);
            
            
#if DEBUG
            // Draw beat and score
            string beat = "beat " + current_beat;
            spriteBatch.DrawString(font, beat, new Vector2(350, 30), Color.White);
#endif


            // Find the center of the string
            // Vector2 FontOrigin = font.MeasureString(output) / 2;
            //string vol = "volume: " + (int) (volume * 10);
            spriteBatch.DrawString(font, "volume", new Vector2(35, 0), Color.White);
            for (int i = 0; i < (int)(volume * 10); i++)
            {
                spriteBatch.Draw(volumeTexture, new Vector2(120 + i * 20, 6), null, Color.White, 0.0f, new Vector2(), 0.8f, SpriteEffects.None, 0.0f);
            }

            spriteBatch.DrawString(font, "score " + current_score, new Vector2(350, 0), Color.White);
            spriteBatch.DrawString(font, actionList.Count!=0 ? (gainedScore >= 0 ? "+"+gainedScore : ""+gainedScore) : "", new Vector2(500, 0), 
                (gainedScore > 0 ? Color.YellowGreen : (gainedScore < 0 ? Color.Red : Color.White)));
            if (comboCount >= 2) 
            {
                if (gainedScore > 0) // only add combo bonus when the last movement is successful
                {
                    spriteBatch.DrawString(font, "+ " + comboCount + " combo bonus", new Vector2(500, 30), Color.Violet, 0.0f, new Vector2(0, 0), 0.75f, SpriteEffects.None, 0.0f);
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
                m.Draw(t, !failed, tint, CountDownDone());
            }
            // sort it in ascending way
            var drawing =
            from m in drawSet
            orderby m.showBeat ascending
            select m;
            foreach (Movement m in drawing)
            {
                if (m.startBeat > current_beat)
                {
                    float alpha = Alpha(m.startBeat, m.showBeat);
                    m.Draw(spriteBatch, alpha, Movement.Stages.Show);
                }
                else if (m.endBeat > current_beat)
                {
                    float alpha = Alpha(m.endBeat, m.startBeat);
                    m.Draw(spriteBatch, alpha, Movement.Stages.Ready);
                }
                else
                {
                    float alpha = Alpha(m.fadeBeat, m.endBeat);
                    m.Draw(spriteBatch, alpha, Movement.Stages.Fade);
                }
                satisfaction.Draw(spriteBatch);
            }
            satisfaction.Draw(spriteBatch);

            // Draw beginning countdown
            if (!CountDownDone())
            {
                int increment = (int)Math.Round((float)countdownWatch.ElapsedMilliseconds / beatTime);
                if (increment > 0)
                {
                    if (increment > countDownBeatSum) 
                    {
                        if (increment <= COUNTDOWN) CountDown.Play();
                        countDownBeatSum++; 
                    }
                    string counter = countDownBeatSum < COUNTDOWN ? (COUNTDOWN - countDownBeatSum).ToString() : "Go!";
                    Vector2 textCenter = new Vector2(Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
                    Vector2 counterSize = font.MeasureString(counter);
                    spriteBatch.DrawString(font, counter, (textCenter - counterSize) / 2, Color.White);
                }
            }

            // Draw to screen if level failed
            if (failed)
            {
                Vector2 center = gameState.ScreenCenter();
                Vector2 origin = gameState.ScreenCenter();
                failScale = Math.Max(1, failScale - 0.01f);
                spriteBatch.Draw(failTexture, center, null, Color.White, 0.0f, origin, failScale, SpriteEffects.None, 0);
            }
        }

        /// <summary>
        /// Calculates alpha between two beat points. a > b!
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private float Alpha(int a, int b)
        {
            float total = (a-b) * beatTime / 2;
            //Console.WriteLine(watch.ElapsedMilliseconds + " " + " " + (b - beat_sum) * beatTime + " "+ total + " " + b);
            return Math.Max(0, (int)watch.ElapsedMilliseconds - (b - beat_sum) * beatTime) / total;

        }

        private bool CountDownDone()
        {
            return countDownBeatSum > COUNTDOWN;
        }

    }
}

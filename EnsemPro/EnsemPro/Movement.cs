using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EnsemPro
{
    public class Movement
    {
        public enum Types
        {
            Shake,
            Noop,
            Wave,
            Control,

        }

        public enum States
        {
            Succeed,
            Fail,
            None
        }

        Texture2D current_trace;
        Texture2D current_circle;
        Texture2D current_shake;

        static Texture2D circleTexture;
        static Texture2D circleReadyTexture;
        static Texture2D ringTexture;
        static Texture2D shakeTexture;
        static Texture2D traceTexture;
        static Texture2D traceTexture_s;
        static Texture2D traceTexture_f;
        static float circleR;
        static Vector2 CircleOrigin;
        static Vector2 RingOrigin;
        static Vector2 shakePos = new Vector2(130, 300);

        public static void LoadContent(ContentManager content)
        {
            circleTexture = content.Load<Texture2D>("images\\circle");
            circleReadyTexture = content.Load<Texture2D>("images\\circle_ready");
            shakeTexture = content.Load<Texture2D>("images\\shake");
            traceTexture = content.Load<Texture2D>("images\\line");
            traceTexture_s = content.Load<Texture2D>("images\\dot_win");
            traceTexture_f = content.Load<Texture2D>("images\\dot_fail");
            ringTexture = content.Load<Texture2D>("images\\ring");
            CircleOrigin = new Vector2(circleTexture.Width / 2, circleTexture.Height / 2);
            RingOrigin = new Vector2(ringTexture.Width / 2, ringTexture.Height / 2);
            circleR = circleTexture.Width / 2;
        }

        public Types myType
        {
            get;
            set;
        }

        public States myState
        {
            get;
            set;
        }

        public int startBeat
        {
            get;
            set;
        }

        public int endBeat
        {
            get;
            set;
        }

        public int showBeat
        {
            get;
            set;
        }

        public int fadeBeat
        {
            get;
            set;
        }

        public int BPM

        {
            get;
            set;
        }

        // null if not a wave
        public Point startCoordinate
        {
            get;
            set;
        }
        public Point endCoordinate
        {
            get;
            set;
        }
        public Function f
        {
            get;
            set;
        }

        public Movement(DataTypes.MovementData md, int lastBPM)
        {
            startBeat = md.StartBeat - 1;
            endBeat = md.EndBeat - 1;
            showBeat = md.ShowBeat - 1;
            fadeBeat = md.FadeBeat -1;
            myState = States.None;
            switch (md.Kind)
            {
                case "Wave":
                    myType = Types.Wave;
                    startCoordinate = new Point(md.StartX, md.StartY);
                    endCoordinate = new Point(md.EndX, md.EndY);
                    f = new Function(this, lastBPM, md.A);
                    break;
                case "Shake":
                    myType = Types.Shake;
                    break;
                case "Control":
                    myType = Types.Control;
                    BPM = md.NewBPM;
                    break;
                default:
                    myType = Types.Noop;
                    break;
            }
        }

        public void setState(States s){
            myState = s;
            switch (myState)
            {
                case States.Fail:
                    current_trace = traceTexture_f;
                    current_circle = circleTexture;
                    current_shake = shakeTexture;
                    break;
                case States.Succeed:
                    current_trace = traceTexture_s;
                    current_circle = circleTexture;
                    current_shake = shakeTexture;
                    break;
                default:
                    current_trace = traceTexture;
                    current_circle = circleTexture;
                    current_shake = shakeTexture;
                    break;
            }

        }

        // action : 0 -- show stage; 1 -- start-end stage; 2 -- fade stage
        public void Draw(SpriteBatch spriteBatch, float progress, int stage)
        {
            if (current_shake == null || current_circle == null || current_trace == null) setState(myState);

            if (myType == Movement.Types.Shake)
            {
                spriteBatch.Draw(current_shake, shakePos, Color.White);
            }

            else if (myType == Movement.Types.Wave)
            {
                
                switch (stage)
                {
                    case 0:
                        // draw the start circle
                        spriteBatch.Draw(circleTexture, new Vector2(startCoordinate.X, GameEngine.HEIGHT - startCoordinate.Y), null, Color.White, 0.0f, CircleOrigin, 1.0f, SpriteEffects.None, 0.0f);
                        spriteBatch.Draw(ringTexture, new Vector2(startCoordinate.X, GameEngine.HEIGHT - startCoordinate.Y), null, Color.Lerp(Color.White, Color.Transparent, 1f - progress), 0.0f, RingOrigin, progress, SpriteEffects.None, 0.0f);
                        break;
                    case 1:
                        spriteBatch.Draw(circleReadyTexture, new Vector2(startCoordinate.X, GameEngine.HEIGHT - startCoordinate.Y), null, Color.White, 0.0f, CircleOrigin, 1.0f, SpriteEffects.None, 0.0f);
                        if (f != null)
                        {
                            float lastPx = -1;
                            float lastPy = -1;
                            int count = 1;
                            foreach (Vector2 p in f.Positions)
                            {
                                float index = count / (float)f.Size;
                                if (index < (1f - progress))
                                {
                                    if (lastPx < 0 || Math.Sqrt((lastPx - p.X) * (lastPx - p.X) + (lastPy - p.Y) * (lastPy - p.Y)) > circleR)
                                    {
                                        count++;
                                        Vector2 ori = new Vector2(p.X - traceTexture.Width / 2, p.Y - traceTexture.Height / 2);
                                        spriteBatch.Draw(current_trace, ori, Color.White);
                                        lastPx = p.X;
                                        lastPy = p.Y;
                                    }
                                }
                                else { break; }
                            }
                        } 
                        break;
                    case 2:
                        Color alpha = Color.Lerp(Color.White, Color.Transparent, progress);
                        spriteBatch.Draw(circleTexture, new Vector2(startCoordinate.X, GameEngine.HEIGHT - startCoordinate.Y), null, alpha, 0.0f, CircleOrigin, 1.0f, SpriteEffects.None, 0.0f);
                        float lPx = -1;
                        float lPy = -1;
                        foreach (Vector2 p in f.Positions)
                        {   
                            if (lPx < 0 || Math.Sqrt((lPx - p.X) * (lPx - p.X) + (lPy - p.Y) * (lPy - p.Y)) > circleR)
                            {
                                Vector2 ori = new Vector2(p.X - traceTexture.Width / 2, p.Y - traceTexture.Height / 2);
                                spriteBatch.Draw(current_trace, ori, alpha);
                                lPx = p.X;
                                lPy = p.Y;
                            }
                        }
                        break;
                    default:
                        break;
                
                }
            }

        }

    }
}

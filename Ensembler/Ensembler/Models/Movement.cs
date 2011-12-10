using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Ensembler
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

        public enum Stages
        {
            Show,
            Ready,
            Fade,
        }

        Texture2D current_trace;
        Texture2D current_circle;
        Texture2D current_shake;
        static Texture2D endTexture;
        static Texture2D circleTexture;
        static Texture2D circleReadyTexture;
        static Texture2D ringTexture;
        static Texture2D shakeTexture;
        static Texture2D traceTexture;
        static Texture2D traceTexture_s;
        static Texture2D traceTexture_f;
        static Texture2D traceTexture_r;
        public static float circleR;
        static Vector2 CircleOrigin;
        static Vector2 RingOrigin;
        static Vector2 shakePos = new Vector2(200, 150);

        public static void LoadContent(ContentManager content)
        {
            circleTexture = content.Load<Texture2D>("images\\circle");
            endTexture = content.Load<Texture2D>("images\\stop");
            circleReadyTexture = content.Load<Texture2D>("images\\circle_ready");
            shakeTexture = content.Load<Texture2D>("images\\shake");
            traceTexture = content.Load<Texture2D>("images\\dot");
            traceTexture_s = content.Load<Texture2D>("images\\dot_win");
            traceTexture_f = content.Load<Texture2D>("images\\dot_fail");
            traceTexture_r = content.Load<Texture2D>("images\\dot_ready");
            ringTexture = content.Load<Texture2D>("images\\ring");
            CircleOrigin = new Vector2(circleTexture.Width / 2, circleTexture.Height / 2);
            RingOrigin = new Vector2(ringTexture.Width / 2, ringTexture.Height / 2);
            circleR = traceTexture.Width / 2;
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
            myType = (Types) Enum.Parse(typeof(Types), md.Kind);
            switch (myType)
            {
                case Types.Wave:
                    startCoordinate = new Point(md.StartX, md.StartY);
                    endCoordinate = new Point(md.EndX, md.EndY);
                    f = new Function(this, lastBPM, md.A);
                    break;
                case Types.Control:
                    BPM = md.NewBPM;
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
                    current_trace = traceTexture_r;
                    current_circle = circleTexture;
                    current_shake = shakeTexture;
                    break;
            }

        }

        public void Draw(SpriteBatch spriteBatch, float progress, Stages stage)
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
                    case Stages.Show:
                        // draw the start circle
                        spriteBatch.Draw(circleTexture, new Vector2(startCoordinate.X, GameEngine.HEIGHT - startCoordinate.Y), null, Color.White, 0.0f, CircleOrigin, 1.0f, SpriteEffects.None, 0.0f);
                        spriteBatch.Draw(ringTexture, new Vector2(startCoordinate.X, GameEngine.HEIGHT - startCoordinate.Y), null, Color.Lerp(Color.White, Color.Transparent, progress), 0.0f, RingOrigin, 1 -　progress, SpriteEffects.None, 0.0f);
                        float lastPx = -1;
                        float lastPy = -1;
                        int count = 1;
                            foreach (Vector2 p in f.drawPositions){
                                float index = count / (float)(f.drawPositions.Length);
                                    if (index < progress)
                                    {
                                        Vector2 ori = new Vector2(p.X - traceTexture.Width / 2, p.Y - traceTexture.Height / 2);
                                        spriteBatch.Draw(traceTexture, ori, Color.White);
                                        lastPx = p.X;
                                        lastPy = p.Y;
                                    }
                                    count++;
                            }
                            
                        break;
                    case Stages.Ready:
                        spriteBatch.Draw(circleTexture, new Vector2(startCoordinate.X, GameEngine.HEIGHT - startCoordinate.Y), null, Color.Lerp(Color.White, Color.Transparent, progress), 0.0f, CircleOrigin, 1.0f, SpriteEffects.None, 0.0f);
                        float lPx = -1;
                        float lPy = -1;
                        int cnt = 1;
                        foreach (Vector2 p in f.drawPositions)
                            {
                                float index = cnt / (float)(f.drawPositions.Length);   
                                Texture2D tempT = traceTexture;
                                if (index < progress) tempT = current_trace;
                                Vector2 ori = new Vector2(p.X - tempT.Width / 2, p.Y - tempT.Height / 2);
                                spriteBatch.Draw(tempT, ori, Color.White);
                                lPx = p.X;
                                lPy = p.Y;
                                cnt++;
                                    
                            }
                        spriteBatch.Draw(endTexture, new Vector2(endCoordinate.X, GameEngine.HEIGHT - endCoordinate.Y), null, Color.Lerp(Color.White, Color.Transparent, progress), 0.0f, CircleOrigin, 1.0f, SpriteEffects.None, 0.0f);
                            break;
                    case Stages.Fade:
                        Color alpha = Color.Lerp(Color.White, Color.Transparent, progress);
                       // spriteBatch.Draw(circleTexture, new Vector2(startCoordinate.X, GameEngine.HEIGHT - startCoordinate.Y), null, alpha, 0.0f, CircleOrigin, 1.0f, SpriteEffects.None, 0.0f);
                        float lPx2 = -1;
                        float lPy2 = -1;
                        foreach (Vector2 p in f.drawPositions)
                        {   
                                Vector2 ori = new Vector2(p.X - traceTexture.Width / 2, p.Y - traceTexture.Height / 2);
                                spriteBatch.Draw(current_trace, ori, alpha);
                                lPx2 = p.X;
                                lPy2 = p.Y;
                           
                        }
                        spriteBatch.Draw(endTexture, new Vector2(endCoordinate.X, GameEngine.HEIGHT - endCoordinate.Y), null, alpha, 0.0f, CircleOrigin, 1.0f, SpriteEffects.None, 0.0f);
                       
                        break;
                    default:
                        break;
                
                }
            }

        }

    }
}

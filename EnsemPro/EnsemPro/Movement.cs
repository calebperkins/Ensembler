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
            Nonsense // for parseryy
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
        static Texture2D shakeTexture;
        static Texture2D traceTexture;
        static Texture2D traceTexture_s;
        static Texture2D traceTexture_f;
        static Vector2 shakePos = new Vector2(130, 300);

        public static void LoadContent(ContentManager content)
        {
            circleTexture = content.Load<Texture2D>("images\\circle");
            shakeTexture = content.Load<Texture2D>("images\\shake");
            traceTexture = content.Load<Texture2D>("images\\dot_normal");
            traceTexture_s = content.Load<Texture2D>("images\\dot_win");
            traceTexture_f = content.Load<Texture2D>("images\\dot_fail");
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

        public Movement(DataTypes.MovementData md)
        {
            startBeat = md.StartBeat;
            endBeat = md.EndBeat;
            showBeat = md.ShowBeat;
            fadeBeat = md.FadeBeat;
            myState = States.None;
            switch (md.Kind)
            {
                case "Wave":
                    myType = Types.Wave;
                    f = new Function();
                    startCoordinate = new Point(md.StartX, md.StartY);
                    endCoordinate = new Point(md.EndX, md.EndY);
                    f.InitializeCurve(Function.Types.Curve, this, 100, md.A); // TODO: replace hard coded BPM
                    break;
                case "Shake":
                    myType = Types.Shake;
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

        public void Draw(SpriteBatch spriteBatch, float alpha, bool walk)
        {
            if (current_shake == null || current_circle == null || current_trace == null) setState(myState);
            
            // setting the transparency
            Color transparency = Color.Lerp(Color.White, Color.Transparent, alpha);

            if (myType == Movement.Types.Shake)
            {
                spriteBatch.Draw(current_shake, shakePos, transparency);
            }

            else if (myType == Movement.Types.Wave)
            {
                int count = 1;
                if (f != null)
                {
                    if (walk)
                    {
                        foreach (Vector2 p in f.Positions)
                        {
                            float index = count / (float)f.Size;
                            if (index < (1f - alpha))
                            {
                                count++;
                                Vector2 ori = new Vector2(p.X - traceTexture.Width / 2, p.Y - traceTexture.Height / 2);
                                spriteBatch.Draw(current_trace, ori,  transparency);
                            }
                            else { break; }
                        }
                    }
                    else // do not draw in "walking"  manner
                    {
                        foreach (Vector2 p in f.Positions)
                        {
                            Vector2 ori = new Vector2(p.X - traceTexture.Width / 2, p.Y - traceTexture.Height / 2);
                            spriteBatch.Draw(current_trace, ori, transparency);
                        }
                    }
                }
                Vector2 origin = new Vector2(circleTexture.Width / 2, circleTexture.Height / 2);
                spriteBatch.Draw(current_circle, new Vector2(startCoordinate.X, GameEngine.HEIGHT - startCoordinate.Y), null, transparency, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
                if (!walk || count == f.Size)
                spriteBatch.Draw(current_circle, new Vector2(endCoordinate.X, GameEngine.HEIGHT - endCoordinate.Y), null, transparency, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
            }

        }

    }
}

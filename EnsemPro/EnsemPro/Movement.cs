using System;
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
    public class Movement
    {
        public enum Type
        {
            Shake,
            Noop,
            Wave
        }

        public enum State
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

        Type myType;
        State myState;

        public static void LoadContent(ContentManager content)
        {
            circleTexture = content.Load<Texture2D>("images\\circle");
            shakeTexture = content.Load<Texture2D>("images\\shake");
            traceTexture = content.Load<Texture2D>("images\\dot_normal");
            traceTexture_s = content.Load<Texture2D>("images\\dot_win");
            traceTexture_f = content.Load<Texture2D>("images\\dot_fail");
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

        public Movement(Movement.Type type, int show_b, int sb, int eb, int fade_b)
        {
            myType = type;
            startBeat = sb;
            endBeat = eb;
            showBeat = show_b;
            fadeBeat = fade_b;
            myState = State.None;
        }

        public Movement(Movement.Type type, int show_b, int sb, int eb, int fade_b, Point sc, Point ec, Function f)
            : this(type, show_b, sb, eb, fade_b)
        {
            startCoordinate = sc; // Note that the coordinate assumes (0,0) is bottom left
            endCoordinate = ec;   // Note that the coordinate assumes (0,0) is bottom left
            this.f = f;
        }

        // returns the type of this movement
        public Type getType()
        {
            return myType;
        }

        public void setState(State s){
            myState = s;
            // setting the texture
            if (myState == State.Fail)
            {
                current_trace = traceTexture_f;
                current_circle = circleTexture;
                current_shake = shakeTexture;
            }
            else if (myState == State.Succeed)
            {
                current_trace = traceTexture_s;
                current_circle = circleTexture;
                current_shake = shakeTexture;
            }
            else
            {
                current_trace = traceTexture;
                current_circle = circleTexture;
                current_shake = shakeTexture;
            }

        }

        public State getState(){
            return myState;
        }

        public void Draw(SpriteBatch spriteBatch, float alpha, bool walk)
        {
            if (current_shake == null || current_circle == null || current_trace == null) setState(myState);
            
            // setting the transparency
            Color transparency = Color.Lerp(Color.White, Color.Transparent, alpha);

            if (getType() == Movement.Type.Shake)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(current_shake, shakePos, transparency);
                spriteBatch.End();
            }

            else if (getType() == Movement.Type.Wave)
            {
                int count = 1;
                if (f != null)
                {
                    if (walk)
                    {
                        foreach (Vector2 p in f.getPos())
                        {
                            float index = count / (float)f.getSize();
                            if (index < (1f - alpha))
                            {
                                count++;
                                Vector2 ori = new Vector2(p.X - traceTexture.Width / 2, p.Y - traceTexture.Height / 2);
                                spriteBatch.Begin();
                                spriteBatch.Draw(current_trace, ori,  transparency);
                                spriteBatch.End();
                            }
                            else { break; }
                        }
                    }
                    else // do not draw in "walking"  manner
                    {
                        foreach (Vector2 p in f.getPos())
                        {
                            Vector2 ori = new Vector2(p.X - traceTexture.Width / 2, p.Y - traceTexture.Height / 2);
                            spriteBatch.Begin();
                            spriteBatch.Draw(current_trace, ori, transparency);
                            spriteBatch.End();
                        }
                    }
                }
                Vector2 origin = new Vector2(circleTexture.Width / 2, circleTexture.Height / 2);
                spriteBatch.Begin();
                spriteBatch.Draw(current_circle, new Vector2(startCoordinate.X, GameEngine.HEIGHT - startCoordinate.Y), null, transparency, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
                if (!walk || count == f.getSize())
                spriteBatch.Draw(current_circle, new Vector2(endCoordinate.X, GameEngine.HEIGHT - endCoordinate.Y), null, transparency, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
                spriteBatch.End();
            }

        }

    }
}

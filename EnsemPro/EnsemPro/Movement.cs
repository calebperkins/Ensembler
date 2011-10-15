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
        public enum Type {
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

        static Texture2D circleTexture;
        static Texture2D shakeTexture;
        static Texture2D traceTexture;
        static Texture2D traceTexture_s;
        static Texture2D traceTexture_f;
        static Vector2 shakePos = new Vector2(200, 200);

        Type myType;

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

        public Movement(Movement.Type type, int sb, int eb, int show_b, int fade_b)
        {
            myType = type;
            startBeat = sb;
            endBeat = eb;
            showBeat = show_b;
            fadeBeat = fade_b;
        }

        public Movement(Movement.Type type, int sb, int eb, int show_b, int fade_b, Point sc, Point ec, Function f)
        {
            myType = type;
            startBeat = sb;
            endBeat = eb;
            startCoordinate = sc;
            endCoordinate = ec;
            showBeat = show_b;
            fadeBeat = fade_b;
            this.f = f;
        }

        // returns the type of this movement
        public Type getType()
        {
            return myType;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            if (getType() == Movement.Type.Shake)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(shakeTexture, shakePos, Color.White);
                spriteBatch.End();
            }
            else if (getType() == Movement.Type.Wave)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(circleTexture, new Vector2(startCoordinate.X, startCoordinate.Y), null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(circleTexture, new Vector2(endCoordinate.X, endCoordinate.Y), null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.0f);
                spriteBatch.End();
                if (f != null)
                {
                    foreach (Vector2 p in f.getPos())
                    {
                        Console.WriteLine("<");
                        spriteBatch.Begin();
                        spriteBatch.Draw(traceTexture, p, Color.White);
                        spriteBatch.End();
                    }
                }
            }
            
        }
         
    }
}

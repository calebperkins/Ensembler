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

        Texture2D shakeTexture;
        Texture2D circleTexture;

        Vector2 shakePos;

        public enum Type {
            Shake,
            Noop,
            Wave
        }

        static Texture2D circleTexture;
        static Texture2D shakeTexture;
        static Vector2 shakePos = new Vector2(200, 200);

        Type my_type;

        public static void LoadContent(ContentManager content)
        {
            circleTexture = content.Load<Texture2D>("images\\circle");
            shakeTexture = content.Load<Texture2D>("images\\shake");

        }

        public int start_beat
        {
            get;
            set;
        }

        public int end_beat
        {
            get;
            set;
        }

        public int show_beat
        {
            get;
            set;
        }

        public int fade_beat

        {
            get;
            set;
        }

        // null if not a wave
        public Point start_coordinate
        {
            get;
            set;
        }
        public Point end_coordinate
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
            my_type = type;
            start_beat = sb;
            end_beat = eb;
        }

        public Movement(Movement.Type type, int sb, int eb, int db, Point sc, Point ec, Function f)
		{
            show_beat = show_b;
            fade_beat = fade_b;
        }

        public Movement(Movement.Type type, int sb, int eb, int show_b, int fade_b, Point sc, Point ec, Function f)
        {
            my_type = type;
            start_beat = sb;
            end_beat = eb;
            draw_beat = db;
            start_coordinate = sc;
            end_coordinate = ec;
            show_beat = show_b;
            fade_beat = fade_b;
        }

        // returns the type of this movement
        public Type getType()
        {
            return my_type;
        }

        public void LoadContent(ContentManager content)
        {
            circleTexture = content.Load<Texture2D>("images\\circle");
            shakeTexture = content.Load<Texture2D>("images\\shake");
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
                spriteBatch.Draw(circleTexture, new Vector2(start_coordinate.X, start_coordinate.Y), null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(circleTexture, new Vector2(end_coordinate.X, end_coordinate.Y), null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.0f);
                spriteBatch.End();
                // draw dotted lines?
            }

        }
         
    }
}

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
    class Movement
    {

        Texture2D shakeTexture;
        Texture2D circleTexture;

        Vector2 shakePos;

        public enum Type {
            Shake,
            Noop,
            Wave
        }

        Type my_type;

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

        // The beat to draw this movement onto the screen prior to start_beat so player can get ready
        public int draw_beat
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

        public Movement(Movement.Type type, int sb, int eb, int db)
        {
            my_type = type;
            start_beat = sb;
            end_beat = eb;
            draw_beat = db;
        }

        public Movement(Movement.Type type, int sb, int eb, int db, Point sc, Point ec, Function f)
        {
            my_type = type;
            start_beat = sb;
            end_beat = eb;
            draw_beat = db;
            start_coordinate = sc;
            end_coordinate = ec;
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
            spriteBatch.Begin();
            if (my_type == Movement.Type.Shake)
            {
                spriteBatch.Draw(shakeTexture, shakePos, null, Color.White, 0.0f, new Vector2(0, 0), 0.0f, SpriteEffects.None, 0.0f);
            }
            else if (my_type == Movement.Type.Wave)
            {
                spriteBatch.Draw(circleTexture, new Vector2(start_coordinate.X, start_coordinate.Y), null, Color.White, 0.0f, new Vector2(0, 0), 0.0f, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(circleTexture, new Vector2(end_coordinate.X, end_coordinate.Y), null, Color.White, 0.0f, new Vector2(0, 0), 0.0f, SpriteEffects.None, 0.0f);
                // draw dotted lines according to function
            }
            spriteBatch.End();
        }
         
    }
}

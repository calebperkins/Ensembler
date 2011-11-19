using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EnsemPro
{
    public class DialogView
    {
        SpriteBatch spriteBatch;
        Texture2D background;
        Texture2D dialogBox;
        SpriteFont font;

        Vector2 facePos;
        Vector2 dialogBoxPos;
        Vector2 namePos;
        Vector2 linePos;

        public DialogView(SpriteBatch sb)
        {
            spriteBatch = sb;
            // TODO: remove the hardcoding
            dialogBoxPos = new Vector2(50, 350);
            facePos = new Vector2(40, 170);
            namePos = new Vector2(60, 365);
            linePos = new Vector2(90, 440);
        }

        public void LoadContent(ContentManager cm)
        {
            background = cm.Load<Texture2D>("Images//SelectionScreen//background");
            dialogBox = cm.Load<Texture2D>("Images//WorldMap//fbg_edited");
            font = cm.Load<SpriteFont>("images/Lucidia");
        }

        public void Draw(GameTime t,String n, String s, Color c, Texture2D f)
        {
            spriteBatch.Draw(dialogBox, dialogBoxPos, Color.White);
            spriteBatch.DrawString(font, n, namePos, c, 0.0f, new Vector2(),0.85f,SpriteEffects.None,0.0f);
            spriteBatch.DrawString(font, s, linePos, Color.Black, 0.0f, new Vector2(), 0.75f, SpriteEffects.None, 0.0f);
            if (f != null) 
            {
                spriteBatch.Draw(f, facePos, Color.White);
            }
        }
    }
}
using System;
//using System.Drawing;
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
        Texture2D personATexture;
        Texture2D personBTexture;
        SpriteFont font;

        Vector2 personAPos;
        Vector2 personBPos;
        Vector2 dialogBoxPos;
        Vector2 namePos;
        Vector2 linePos;

        public DialogView(SpriteBatch sb)
        {
            spriteBatch = sb;
            personAPos = new Vector2(200, 400);
            personBPos = new Vector2(600, 400);
            dialogBoxPos = new Vector2(50, 350);
            namePos = new Vector2(60, 365);
            linePos = new Vector2(90, 440);
        }

        public void LoadContent(ContentManager cm)
        {
            background = cm.Load<Texture2D>("Images//SelectionScreen//background");
            dialogBox = cm.Load<Texture2D>("Images//WorldMap//fbg_edited");
            font = cm.Load<SpriteFont>("images/Lucidia");
        }

        public void Draw(GameTime t,String n, String s)
        {
          //  spriteBatch.Draw(background, new Vector2(), Color.White);
            spriteBatch.Draw(dialogBox, dialogBoxPos, Color.White);
            spriteBatch.DrawString(font, n, namePos, Color.Black, 0.0f, new Vector2(),0.85f,SpriteEffects.None,0.0f);
            spriteBatch.DrawString(font, s, linePos, Color.Black, 0.0f, new Vector2(), 0.75f, SpriteEffects.None, 0.0f);
        }
    }
}
using System;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaColor = Microsoft.Xna.Framework.Color;

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

        public void Draw(GameTime t,String n, String s, String c)
        {
            System.Drawing.Color color = System.Drawing.Color.FromName(c);
            XnaColor xnaColor = new XnaColor(color.R, color.G, color.B, color.A);
            spriteBatch.Draw(dialogBox, dialogBoxPos, XnaColor.White);
            spriteBatch.DrawString(font, n, namePos, xnaColor, 0.0f, new Vector2(),0.85f,SpriteEffects.None,0.0f);
            spriteBatch.DrawString(font, s, linePos, XnaColor.Black, 0.0f, new Vector2(), 0.75f, SpriteEffects.None, 0.0f);
        }
    }
}
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
        Texture2D personATexture;
        Texture2D personBTexture;
        SpriteFont font;

        Vector2 personAPos;
        Vector2 personBPos;

        public DialogView(SpriteBatch sb)
        {
            spriteBatch = sb;
            personAPos = new Vector2(200, 400);
            personBPos = new Vector2(600, 400);
        }

        public void LoadContent(ContentManager cm)
        {
            background = cm.Load<Texture2D>("Images//SelectionScreen//background");
            dialogBox = cm.Load<Texture2D>("images/dialogBoxTest");
            font = cm.Load<SpriteFont>("images/Lucidia");
        }

        public void Draw(GameTime t,String n, String s)
        {
          //  spriteBatch.Draw(background, new Vector2(), Color.White);
            spriteBatch.Draw(dialogBox, new Vector2(50, 350), Color.White);
            spriteBatch.DrawString(font, n, new Vector2(100, 370), Color.Black);
            spriteBatch.DrawString(font, s, new Vector2(100, 420), Color.Black);
        }
    }
}
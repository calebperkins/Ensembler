using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Ensembler
{
    public class DialogView
    {
        SpriteBatch spriteBatch;
        Texture2D background;
        Texture2D dialogBox;
        SpriteFont nameFont;
        SpriteFont lineFont;
        float nameScale;

        Vector2 cutscenePos;
        Vector2 facePos;
        Vector2 dialogBoxPos;
        Vector2 namePos;
        Vector2 linePos;
        Vector2 linePosForLong;

        public DialogView(SpriteBatch sb)
        {
            spriteBatch = sb;
            // TODO: add dynamic coords
            cutscenePos = new Vector2(0,0);
            dialogBoxPos = new Vector2(50, 350);
            facePos = new Vector2(40, 170);
            namePos = new Vector2(142, 365);
            linePos = new Vector2(90, 440);
            linePosForLong = new Vector2(90, 430);
            nameScale = 0.9f;
        }

        public void LoadContent(ContentManager cm)
        {
            background = cm.Load<Texture2D>("Images//SelectionScreen//background");
            dialogBox = cm.Load<Texture2D>("Images//WorldMap//fbg_edited2");
            nameFont = cm.Load<SpriteFont>("Images//WorldMap//BlackChancery");
            lineFont = cm.Load<SpriteFont>("Images//WorldMap//Nosferatu");
        }

        public void Draw(GameTime t,String n, String s, Color c, Texture2D f, Texture2D cs)
        {
            if (cs != null) spriteBatch.Draw(cs, cutscenePos, null, Color.White);
            spriteBatch.Draw(dialogBox, dialogBoxPos, Color.White);
            Vector2 nameOrigin = nameFont.MeasureString(n)/2;
            Color outlineColor = new Color(255, 255, 255, (byte)MathHelper.Clamp(255, 255, 255));
            spriteBatch.DrawString(nameFont, n, new Vector2(namePos.X - nameOrigin.X + 1, namePos.Y + 1), outlineColor, 0.0f, new Vector2(), nameScale, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(nameFont, n, new Vector2(namePos.X - nameOrigin.X - 1, namePos.Y - 1), outlineColor, 0.0f, new Vector2(), nameScale, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(nameFont, n, new Vector2(namePos.X - nameOrigin.X + 1, namePos.Y - 1), outlineColor, 0.0f, new Vector2(), nameScale, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(nameFont, n, new Vector2(namePos.X - nameOrigin.X - 1, namePos.Y + 1), outlineColor, 0.0f, new Vector2(), nameScale, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(nameFont, n, new Vector2(namePos.X - nameOrigin.X, namePos.Y), c, 0.0f, new Vector2(), 0.90f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(lineFont, s, s.Length>=230 ? linePosForLong : linePos, Color.Black, 0.0f, new Vector2(), 0.75f, SpriteEffects.None, 0.0f);
            if (f != null) spriteBatch.Draw(f, facePos, Color.White);
        }
    }
}
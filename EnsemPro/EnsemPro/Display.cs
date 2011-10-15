using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace EnsemPro
{
    public class Display
    {
        Movement lastMovement;

        Texture2D circleTexture;
        Texture2D shakeTexture;

        Vector2 shakePos;

        public Display(Movement m)
        {
            lastMovement = m;
            shakePos = new Vector2(400, 300); // stubbed
        }

        public void LoadContent(ContentManager content)
        {
            circleTexture = content.Load<Texture2D>("images\\circle");
            shakeTexture = content.Load<Texture2D>("images\\shake");

        }

        public void Update(Movement m)
        {
            Movement newMovement = m;
            if (newMovement != lastMovement)
            {
                lastMovement = newMovement; // update movement
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (lastMovement.getType() == Movement.Type.Shake)
            {
                spriteBatch.Draw(shakeTexture, shakePos, null, Color.White, 0.0f, new Vector2(0, 0), 0.0f, SpriteEffects.None, 0.0f);
            }
            else if (lastMovement.getType() == Movement.Type.Wave)
            {
                Point startingCoords = lastMovement.startCoordinate;
                Point endingCoords = lastMovement.endCoordinate;
                spriteBatch.Draw(circleTexture, new Vector2(startingCoords.X, startingCoords.Y), null, Color.White, 0.0f, new Vector2(0, 0), 0.0f, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(circleTexture, new Vector2(endingCoords.X, endingCoords.Y), null, Color.White, 0.0f, new Vector2(0, 0), 0.0f, SpriteEffects.None, 0.0f);
                // draw dotted lines?
            }
            spriteBatch.End();
        }
    }
}
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
        Movement nextMovement;

        Texture2D circleTexture;
        Texture2D shakeTexture;

        Vector2 shakePos;

        int currBeat; // ?

        public Display(Movement m, int cb)
        {
            lastMovement = m;
            currBeat = cb;
            shakePos = new Vector2(400, 300); // stubbed
        }

        public void LoadContent(ContentManager content)
        {
            circleTexture = content.Load<Texture2D>("images\\circle");
            shakeTexture = content.Load<Texture2D>("images\\shake");

        }

        public void Update(Movement m, int cb)
        {
            currBeat = cb;
            Movement newMovement = m;
            if (newMovement != lastMovement)
            {
                if (currBeat > lastMovement.end_beat)
                {
                    lastMovement = newMovement; // update movement to the next
                    // get next movement
                }
                else
                {

                }
                // NEEDS CHANGE
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            lastMovement.Draw(spriteBatch);
        }
    }
}
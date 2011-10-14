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
    public class Baton
    {
        Color shadow;
        Vector2 pos;
        WiimoteLib.Wiimote wm;
        bool wii_activated;
        //Vector2 position;
        bool A;

        Texture2D batonTexture;

        public Baton()
        {
            shadow = new Color(0, 0, 0, 128);
            wii_activated = false;
        }

        public void LoadContent(ContentManager content)
        {
            batonTexture = content.Load<Texture2D>("images\\baton");
        }

        public void Initialize()
        {
            try
            {
                wm = new WiimoteLib.Wiimote();
                wm.Connect();
                wm.SetReportType(WiimoteLib.InputReport.IRAccel, WiimoteLib.IRSensitivity.Maximum, true);
                wii_activated = true;
            }
            catch (WiimoteLib.WiimoteNotFoundException)
            {
                wii_activated = false;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (wii_activated)
            {
                WiimoteLib.PointF ws = wm.WiimoteState.IRState.Midpoint;
                pos.X = 800 * (1 - ws.X);
                pos.Y = 600 * ws.Y;
            }
            else
            {
                MouseState ms = Mouse.GetState();
                pos.X = Math.Max(Math.Min(GameEngine.WIDTH, ms.X), 0);
                pos.Y = Math.Max(Math.Min(GameEngine.HEIGHT, ms.Y), 0);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(batonTexture, pos, null, !A ? Color.White : Color.Black, 0.0f, new Vector2(0, 0), 0.35f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(batonTexture, new Vector2(pos.X + 3.0f, pos.Y + 3.0f), null, shadow, 0.0f, new Vector2(0, 0), 0.35f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }

        public Vector2 getPos()
        {
            return pos;
        }
    }
}

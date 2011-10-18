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
    // Fill this out. Placeholder for now.
    public struct InputState
    {
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 acceleration;
    }

    public class Baton
    {
        Color shadow;
        Vector2 pos;
        WiimoteLib.Wiimote wm;
        bool wii_activated;
        List<InputState> buffer = new List<InputState>();

        Vector2 lastPos; // used for shake evaluation
        Vector2 lastVel;

        Texture2D batonTexture;

        public Baton()
        {
            shadow = new Color(0, 0, 0, 128);
            wii_activated = false;
            lastPos = new Vector2(0, 0);
            lastVel = new Vector2(0, 0);
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
            InputState i = new InputState();
            if (wii_activated)
            {
                WiimoteLib.PointF ws = wm.WiimoteState.IRState.Midpoint;
                pos.X = GameEngine.WIDTH * (1 - ws.X);
                pos.Y = GameEngine.HEIGHT * ws.Y;
                WiimoteLib.Point3F acc = wm.WiimoteState.AccelState.Values;
                i.acceleration = new Vector2(acc.X, acc.Y);
            }
            else
            {
                MouseState ms = Mouse.GetState();
                pos.X = Math.Max(Math.Min(GameEngine.WIDTH, ms.X), 0);
                pos.Y = Math.Max(Math.Min(GameEngine.HEIGHT, ms.Y), 0);

                float time = gameTime.ElapsedGameTime.Milliseconds; // time elapsed since last update
                Vector2 posDiff = new Vector2(pos.X - lastPos.X, pos.Y - lastPos.Y); // change in displacement
                Vector2 newVel = new Vector2(posDiff.X / time, posDiff.Y / time); // new velocity
                Vector2 velDiff = new Vector2(newVel.X-lastVel.X,newVel.Y-lastVel.Y); // change in velocity
                Vector2 newAcc = new Vector2(velDiff.X / time, velDiff.Y / time); // new acceleration

                // add to inputstate
                i.velocity = newVel;
                i.acceleration = newAcc; 

                lastPos = pos; // update
                lastVel = newVel;
            }
            i.position = pos;
            buffer.Add(i);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(batonTexture, pos, null, Color.White, 0.0f, new Vector2(0, 0), 0.35f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(batonTexture, new Vector2(pos.X + 3.0f, pos.Y + 3.0f), null, shadow, 0.0f, new Vector2(0, 0), 0.35f, SpriteEffects.None, 0.0f);
        }

        public Vector2 Position
        {
            get { return pos; }
        }

        public void Flush()
        {
            buffer.Clear();
        }

        public IEnumerable<InputState> Buffer
        {
            get { return buffer; }
        }
    }
}

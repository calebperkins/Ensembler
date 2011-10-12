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
    public class BatonController
    {
        WiimoteLib.Wiimote wm;
        bool A;
        bool wii_activated;

        public BatonController()
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

            A = false;
        }

        public Vector2 getPos()
        {
            if (wii_activated)
            {
                WiimoteLib.PointF ws = wm.WiimoteState.IRState.Midpoint;
                A = wm.WiimoteState.ButtonState.A;
                return new Vector2(800 * (1 - ws.X), 600 * ws.Y);
            }
            else
            {
                MouseState ms = Mouse.GetState();
                KeyboardState ks = Keyboard.GetState();
                A = ks.IsKeyDown(Keys.A);
                return new Vector2(Math.Max(Math.Min(GameEngine.WIDTH, ms.X), 0),
                    Math.Max(Math.Min(GameEngine.HEIGHT, ms.Y), 0));
            }
        }
    }
}

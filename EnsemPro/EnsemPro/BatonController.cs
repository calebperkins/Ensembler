using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EnsemPro
{
    public class BatonController : GameComponent
    {
        WiimoteLib.Wiimote wm;
        bool wii_activated;
        Vector2 position;

        public BatonController(Game g) : base(g)
        {
            position = new Vector2();
            wii_activated = false;
        }

        public override void Initialize()
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
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (wii_activated)
            {
                WiimoteLib.PointF ws = wm.WiimoteState.IRState.Midpoint;
                position.X = 800 * (1 - ws.X);
                position.Y = 600 * ws.Y;
            }
            else
            {
                MouseState ms = Mouse.GetState();
                position.X = Math.Max(Math.Min(GameEngine.WIDTH, ms.X), 0);
                position.Y = Math.Max(Math.Min(GameEngine.HEIGHT, ms.Y), 0);
            }
            base.Update(gameTime);
        }

        public Vector2 Position()
        {
            return position;
        }

    }
}

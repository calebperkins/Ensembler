using System;
using Microsoft.Xna.Framework;

namespace EnsemPro
{
    public class WiiController : InputController
    {
        WiimoteLib.Wiimote wm;
        Vector2 lastPosition = new Vector2();

        public WiiController(Game game, GameModel gm, InputBuffer b)
            : base(game, gm, b)
        {
            wm = new WiimoteLib.Wiimote();
            wm.Connect();
            wm.SetReportType(WiimoteLib.InputReport.IRAccel, WiimoteLib.IRSensitivity.Maximum, true);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            input = new InputState();
            WiimoteLib.PointF ws = wm.WiimoteState.IRState.Midpoint;
            input.Position.X = GameEngine.WIDTH * (1 - ws.X);
            input.Position.Y = GameEngine.HEIGHT * ws.Y;
            float time = gameTime.ElapsedGameTime.Milliseconds; // time elapsed since last update
            Vector2 posDiff = input.Position - buffer.CurrentPosition;
            input.Velocity = posDiff / time;
            WiimoteLib.Point3F acc = wm.WiimoteState.AccelState.Values;
            input.Acceleration = new Vector2(acc.X, acc.Y);

            if (ws.X == 0.0f && ws.Y == 0.0f) // sensor bar out of range
            {
                input.Position = lastPosition;
                posDiff = new Vector2();
            }
            else
            {
                lastPosition = input.Position;
            }

            if (Math.Abs(posDiff.X) > POS_DIFF_THRESHOLD || Math.Abs(posDiff.Y) > POS_DIFF_THRESHOLD) // add only only if the baton has moved at least a decent amount of distance 
            {
                buffer.Add(input);
            }
            base.Update(gameTime);
        }
    }
}

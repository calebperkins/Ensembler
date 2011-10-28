using System;
using Microsoft.Xna.Framework;

namespace EnsemPro
{
    public class WiiController : InputController
    {
        WiimoteLib.Wiimote wm;

        public WiiController(Game game, InputBuffer b)
            : base(game, b)
        {
            wm = new WiimoteLib.Wiimote();
            wm.Connect();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            input = new InputState();
            WiimoteLib.PointF ws = wm.WiimoteState.IRState.Midpoint;
            input.position.X = GameEngine.WIDTH * (1 - ws.X);
            input.position.Y = GameEngine.HEIGHT * ws.Y;
            float time = gameTime.ElapsedGameTime.Milliseconds; // time elapsed since last update
            Vector2 posDiff = new Vector2(input.position.X - buffer.CurrentPosition.X, input.position.Y - buffer.CurrentPosition.Y); // change in displacement
            input.velocity = posDiff / time;
            WiimoteLib.Point3F acc = wm.WiimoteState.AccelState.Values;
            input.acceleration = new Vector2(acc.X, acc.Y);
            if (Math.Abs(posDiff.X) > POS_DIFF_THRESHOLD || Math.Abs(posDiff.Y) > POS_DIFF_THRESHOLD) // add only only if the baton has moved at least a decent amount of distance 
            {
                buffer.Add(input);
            }
            base.Update(gameTime);
        }
    }
}

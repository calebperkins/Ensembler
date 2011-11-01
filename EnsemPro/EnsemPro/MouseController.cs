using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace EnsemPro
{
    public class MouseController : InputController
    {
        Vector2 lastP;
        Vector2 lastV;

        public MouseController(Game game, GameModel gm, InputBuffer b)
            : base(game, gm, b)
        {
            lastP = new Vector2();
            lastV = new Vector2();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            input = new InputState();
            MouseState ms = Mouse.GetState();
            input.position.X = MathHelper.Clamp(ms.X, 0, GameEngine.WIDTH);
            input.position.Y = MathHelper.Clamp(ms.Y, 0, GameEngine.HEIGHT);
            float time = gameTime.ElapsedGameTime.Milliseconds; // time elapsed since last update
            Vector2 posDiff = input.position - lastP; // change in displacement
            Vector2 newVel = posDiff/time; // new velocity
            Vector2 velDiff = newVel - lastV; // change in velocity
            Vector2 newAcc = velDiff / time; // new acceleration

            // add to inputstate
            input.velocity = newVel;
            input.acceleration = newAcc;

            lastV = input.velocity;
            lastP = input.position;

            input.Pause = ms.RightButton == ButtonState.Pressed;


            if (Math.Abs(posDiff.X) > POS_DIFF_THRESHOLD || Math.Abs(posDiff.Y) > POS_DIFF_THRESHOLD && !input.Pause) // add only only if the baton has moved at least a decent amount of distance 
            {
                buffer.Add(input);
            }

            base.Update(gameTime);
        }
    }
}

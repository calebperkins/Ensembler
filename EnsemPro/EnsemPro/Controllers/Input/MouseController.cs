using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace EnsemPro
{
    public class MouseController : InputController
    {

        public MouseController(Game game, GameModel gm, InputBuffer b)
            : base(game, gm, b)
        {
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            input = new InputState();
            MouseState ms = Mouse.GetState();
            KeyboardState ks = Keyboard.GetState();

            input.Position.X = MathHelper.Clamp(ms.X, 0, GameEngine.WIDTH);
            input.Position.Y = MathHelper.Clamp(ms.Y, 0, GameEngine.HEIGHT);
            float time = gameTime.ElapsedGameTime.Milliseconds; // time elapsed since last update
            Vector2 posDiff = input.Position - lastState.Position; // change in displacement
            Vector2 newVel = posDiff/time; // new velocity
            Vector2 velDiff = newVel - lastState.Velocity; // change in velocity
            Vector2 newAcc = velDiff / time; // new acceleration

            // add to inputstate
            input.Velocity = newVel;
            input.Acceleration = newAcc;

            input.Confirm = (ks.IsKeyDown(Keys.Enter) || ks.IsKeyDown(Keys.Space)) && !lastState.Confirm;
            
            input.Pause = ks.IsKeyDown(Keys.Escape);
            if (ks.IsKeyDown(Keys.A) && ks.IsKeyDown(Keys.Z)) { }
            else if (ks.IsKeyDown(Keys.A))
            {
                input.Key = Keys.A;
            }
            else if (ks.IsKeyDown(Keys.Z))
            {
                input.Key = Keys.Z;
            }


            if (Math.Abs(posDiff.X) > POS_DIFF_THRESHOLD || Math.Abs(posDiff.Y) > POS_DIFF_THRESHOLD && !input.Pause) // add only only if the baton has moved at least a decent amount of distance 
            {
                buffer.Add(input);
            }

            base.Update(gameTime);
        }
    }
}

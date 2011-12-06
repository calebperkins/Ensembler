using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Ensembler
{
    public class MouseController : InputController
    {
        KeyboardState lastKs = Keyboard.GetState();
        MouseState lastMouse = Mouse.GetState();

        public MouseController(Game game, GameState gm, InputBuffer b)
            : base(game, gm, b)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (!Game.IsActive)
                return;

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

            input.Confirm = lastMouse.LeftButton == ButtonState.Released && ms.LeftButton == ButtonState.Pressed;
            input.Cancel = lastMouse.RightButton == ButtonState.Released && ms.RightButton == ButtonState.Pressed;
 
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

            lastKs = ks;
            lastMouse = ms;

            base.Update(gameTime);
        }
    }
}

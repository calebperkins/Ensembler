using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;


namespace EnsemPro
{
    public class MovementEvaluator
    {
        public const float PERFECT = 1.0f;
        public const float FAIL_THRESHOLD = 0.3f;
        public const float ACC_THRESHOLD = 0.05f;

        Movement currentMovement;
        float localScore; // score for this movement

        public MovementEvaluator(Movement m)
        {
            currentMovement = m;
        }

        /*Returns a floating number 0 to 1 which indicates how well the input is matching the movement */
        public float Score(Movement m, IEnumerable<InputState> input, GameTime t)
        {
            if (m.getType() == Movement.Type.Shake)
            {
                int scoreCounter = 0;
                int totalCounter = 0;
                foreach (InputState state in input)
                {
                    if (Math.Abs(state.acceleration.X) > ACC_THRESHOLD || Math.Abs(state.acceleration.Y) > ACC_THRESHOLD)
                    {
                        Console.WriteLine("acceleration is "+(new Vector2(state.acceleration.X,state.acceleration.Y)));
                        scoreCounter++;
                    }
                    totalCounter++;
                }
                Console.WriteLine("score counter is "+scoreCounter+"\n total counter is "+totalCounter);

                // if fewer than 20 moves, NOT SHAKING! => fail
                return (totalCounter<20 ? 0.00005f : ((float)scoreCounter/(float)totalCounter));
            }
            else if (m.getType() == Movement.Type.Wave)
            {
                return 0;
                Function f = m.f;
                float product = 0.0f;
                int count = 0;
                
                foreach (InputState _input in input)
                {
                    if (count >= f.Slopes.Length)
                        break;
                    product += 1 - Math.Abs(_input.acceleration.Y / _input.acceleration.X - f.Slopes[count]) / Math.Abs(_input.acceleration.Y / _input.acceleration.X + f.Slopes[count]);
                    count++;
                }
                return product / (float) count;
            }
            else
            {
                // movement type == noop
                return 0.0f;
            }
        }

        public void Update(Movement m, float score, GameTime t)
        {
            if (m != currentMovement) // current movement is over, set state accordingly
            {
                Console.WriteLine("NEW MOVEMENT!");
                // send score back to Movement
                if (localScore <= FAIL_THRESHOLD && score != 0.0f)
                {
                    Console.WriteLine("state is FAIL");
                    currentMovement.setState(Movement.State.Fail);
                }
                else if (localScore > FAIL_THRESHOLD)
                {
                    Console.WriteLine("state is SUCCEED");
                    currentMovement.setState(Movement.State.Succeed);
                }
                else
                { // noop, localScore = 0.0f
                    Console.WriteLine("state is NONE");
                    currentMovement.setState(Movement.State.None);
                }

                currentMovement = m; // update movement
                localScore = 0.0f; // reset score
            }
            else
            { // movement not done yet, keep adding the score
                localScore += score;
            }
        }
    }
}
using System;
using Microsoft.Xna.Framework;

namespace EnsemPro
{
    public class MovementEvaluator
    {
        public const float FAIL_THRESHOLD = 0.4f;
        public const float ACC_THRESHOLD = 0.05f;
        public const float MAGIC_WAVE_THRESHOLD = 0.6f / 1f;

        Movement currentMovement;

        public MovementEvaluator(Movement m)
        {
            currentMovement = m;
        }

        /*Returns a floating number 0 to 1 which indicates how well the input is matching the movement */
        public float Accuracy(Movement m, InputBuffer inputs, GameTime t)
        {
            int totalInput = inputs.Count;
            int correct = 0;
            switch (currentMovement.myType)
            {
                case Movement.Types.Noop:
                    return (totalInput > 20 ? -0.5f : 0.0f);
                case Movement.Types.Shake:
                    if (totalInput < 20)
                    {
                        return 0.0f;
                    }
                    else
                    {
                        foreach (InputState state in inputs)
                        {
                            if (Math.Abs(state.acceleration.X) > ACC_THRESHOLD || Math.Abs(state.acceleration.Y) > ACC_THRESHOLD)
                            {
                                correct++;
                            }
                        }
                        return (float)correct / totalInput / 2;
                    }
                case Movement.Types.Wave:
                    if (totalInput < 5)
                    {
                        return -0.3f;
                    }
                    else
                    {
                        Vector2[] slopes = currentMovement.f.Slope(totalInput - 1);
                        Console.WriteLine("TOTALINPUT " + (totalInput - 1));
                        float errorSum = 0.0f;
                        for (int i = 1; i < totalInput; i++)
                        {
                            Vector2 normVel = Vector2.Normalize(inputs[i].velocity);
                            Vector2 slope = slopes[i];
                            errorSum += (normVel.X - slope.X) * (normVel.X - slope.X) + (normVel.Y - slope.Y) * (normVel.Y - slope.Y);
                        }
                        float rmsError = (float)Math.Sqrt((double)errorSum / (double)(totalInput - 1));
                        float accuracy = (1 - rmsError * MAGIC_WAVE_THRESHOLD);
                        return (accuracy > FAIL_THRESHOLD ? accuracy : -0.3f);
                    }
                default:
                    return 0.0f;
            }
        }

        public void Update(Movement m, float score, bool newMovement, GameTime t)
        {
            if (newMovement) // current movement is over, set state accordingly
            {
                //Debug.WriteLine("NEW MOVEMENT!");
                // send score back to Movement
                if (score <= FAIL_THRESHOLD)
                {
                    currentMovement.setState(Movement.States.Fail);
                }
                else if (score > FAIL_THRESHOLD)
                {
                    currentMovement.setState(Movement.States.Succeed);
                }
                else // no op
                {
                    currentMovement.setState(Movement.States.None);
                }

                currentMovement = m; // update movement
            }
        }
    }
}
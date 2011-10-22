using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace EnsemPro
{
    public class MovementEvaluator
    {
        public const float FAIL_THRESHOLD = 0.4f;
        public const float ACC_THRESHOLD = 0.05f;

        Movement currentMovement;

        public MovementEvaluator(Movement m)
        {
            currentMovement = m;
        }

        /*Returns a floating number 0 to 1 which indicates how well the input is matching the movement */
        public float Accuracy(Movement m, List<InputState> inputs, GameTime t)
        {
            int totalInput = inputs.Count;
            int correct = 0;
            if (totalInput < 20) // if not many inputs, player hasn't moved that much => FAIL
            {
                Console.WriteLine("YAYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY");
                return 0.00005f;
            }
            else
            {
                switch (currentMovement.myType)
                {
                    case Movement.Types.Shake:
                        foreach (InputState state in inputs)
                        {
                            if (Math.Abs(state.acceleration.X) > ACC_THRESHOLD || Math.Abs(state.acceleration.Y) > ACC_THRESHOLD)
                            {
                                correct++;
                            }
                        }
                        return (float)correct / totalInput;
                    case Movement.Types.Wave:
                        Vector2[] slopes = currentMovement.f.Slopes;
                        float errorSum = 0.0f;
                        for (int i = 1; i < totalInput; i++)
                        {
                            inputs[i].velocity.Normalize();
                            Vector2 normVel = inputs[i].velocity;
                            Vector2 slope = slopes[i];
                            errorSum += (normVel.X - slope.X) * (normVel.X - slope.X) + (normVel.Y - slope.Y) * (normVel.Y - slope.Y);
                        }
                        float rmsError = (float)Math.Sqrt((double)errorSum / (double)(totalInput - 1));

                        Console.WriteLine("RMS ERROR is " + rmsError);

                        /*
                        foreach (InputState input in inputs)
                        {
                            if (totalInput >= f.Slopes.Length)
                                break;
                            float dist = Vector2.Distance(f.Positions[totalInput], input.position);
                            if (dist < 150)
                                correct++;
                        }
                         * */
                        return correct / (float)totalInput;

                    default: // no movement
                        return 0.0f;
                }
            }
        }

        public void Update(Movement m, float score, bool newMovement, GameTime t)
        {
            if (newMovement) // current movement is over, set state accordingly
            {
                Debug.WriteLine("NEW MOVEMENT!");
                // send score back to Movement
                if (score <= FAIL_THRESHOLD && score >= 0.0f)
                {
                    Debug.WriteLine("state is FAIL");
                    currentMovement.setState(Movement.States.Fail);
                }
                else if (score > FAIL_THRESHOLD)
                {
                    Debug.WriteLine("state is SUCCEED");
                    currentMovement.setState(Movement.States.Succeed);
                }
                else
                { // noop, localScore is negative
                    Debug.WriteLine("state is NONE");
                    currentMovement.setState(Movement.States.None);
                }

                currentMovement = m; // update movement
            }
        }
    }
}
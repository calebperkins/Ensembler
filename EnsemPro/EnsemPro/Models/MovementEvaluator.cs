using System;
using Microsoft.Xna.Framework;

namespace EnsemPro
{
    public class MovementEvaluator
    {
        public const float FAIL_THRESHOLD = 0.4f;
        public const float ACC_THRESHOLD = 0.05f;
        public const float MAGIC_WAVE_THRESHOLD = 0.6f / 1f;

        public Movement CurrentMovement
        {
            get;
            private set;
        }

        public MovementEvaluator(Movement m)
        {
            CurrentMovement = m;
        }

        public bool Timing(InputBuffer inputs, Point p, bool start)
        {
            if (inputs.Count != 0)
            {
                Vector2 coords = new Vector2(p.X, 600 - p.Y);
                Vector2 pos = (start ? inputs[0].Position : inputs[inputs.Count - 1].Position);
                //  Console.WriteLine("coords is " + coords);
                //  Console.WriteLine("pos is " + pos);
                //  Console.WriteLine("difference in pos is " + Vector2.Distance(coords, pos)+"\n");
                return Vector2.Distance(coords, pos) <= 70;
            }
            else
            {
                return false;
            }
        }

        /*Returns a floating number 0 to 1 which indicates how well the input is matching the movement */
        public float Accuracy(Movement m, InputBuffer inputs, bool timing, GameTime t)
        {
            int totalInput = inputs.Count;
            int correct = 0;

            if (CurrentMovement != null)
            {
                switch (CurrentMovement.myType)
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
                                if (Math.Abs(state.Acceleration.X) > ACC_THRESHOLD || Math.Abs(state.Acceleration.Y) > ACC_THRESHOLD)
                                {
                                    correct++;
                                }
                            }
                            return (float)correct / totalInput / 2;
                        }
                    case Movement.Types.Wave:
                        if (totalInput < 5 || !timing)
                        {
                            return -0.3f;
                        }
                        else
                        {
                            Vector2 startPos = new Vector2(CurrentMovement.startCoordinate.X, CurrentMovement.startCoordinate.Y);
                            Vector2 endPos = new Vector2(CurrentMovement.endCoordinate.X, CurrentMovement.endCoordinate.Y);
                            float DIST_THRESHOLD = 0.55f * Vector2.Distance(startPos, endPos);

                            Vector2[] slopes = CurrentMovement.f.Slope(totalInput - 1);
                            float errorSum = 0.0f;
                            float dist = Vector2.Distance(inputs[totalInput - 1].Position, inputs[0].Position);

                            if (dist >= DIST_THRESHOLD)
                            {
                                for (int i = 1; i < totalInput; i++)
                                {
                                    Vector2 normVel = Vector2.Normalize(inputs[i].Velocity);
                                    Vector2 slope = slopes[i];
                                    errorSum += (normVel.X - slope.X) * (normVel.X - slope.X) + (normVel.Y - slope.Y) * (normVel.Y - slope.Y);
                                }
                                float rmsError = (float)Math.Sqrt((double)errorSum / (double)(totalInput - 1));
                                float accuracy = (1 - rmsError * MAGIC_WAVE_THRESHOLD);
                                return (accuracy > FAIL_THRESHOLD ? accuracy : -0.3f);
                            }
                            else
                            {
                                return -0.3f;
                            }
                        }
                    default:
                        return 0.0f;
                }
            }
            else
            {
                return 0.0f;
            }
        }

        public void Update(Movement m, float score, bool newMovement, GameTime t)
        {
            if (newMovement) // current movement is over, set state accordingly
            {
                //Debug.WriteLine("NEW MOVEMENT!");
                // send score back to Movement
                if (CurrentMovement != null)
                {
                    if (score <= FAIL_THRESHOLD)
                    {
                        CurrentMovement.setState(Movement.States.Fail);
                    }
                    else if (score > FAIL_THRESHOLD)
                    {
                        CurrentMovement.setState(Movement.States.Succeed);
                    }
                    else // no op
                    {
                        CurrentMovement.setState(Movement.States.None);
                    }
                }
                CurrentMovement = m; // update movement
            }
        }
    }
}
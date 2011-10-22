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

        //fdfdf/dfdf/df//dfdf

        Movement currentMovement;
        float localScore; // score for this movement

        public MovementEvaluator(Movement m)
        {
            currentMovement = m;
        }

        /*Returns a floating number 0 to 1 which indicates how well the input is matching the movement */
        public float Accuracy(Movement m, ICollection<InputState> inputs, GameTime t)
        {
            switch (currentMovement.myType)
            {
                case Movement.Types.Shake:
                    int scoreCounter = 0;
                    int totalCounter = 0;
                    foreach (InputState state in inputs)
                    {
                        if (Math.Abs(state.acceleration.X) > ACC_THRESHOLD || Math.Abs(state.acceleration.Y) > ACC_THRESHOLD)
                        {
                            scoreCounter++;
                        }
                        totalCounter++;
                    }
                    Debug.WriteLine("score counter is " + scoreCounter + "\n total counter is " + totalCounter);

                    // if fewer than 20 moves, NOT SHAKING! => fail
                    return (totalCounter < 20 ? 0.00005f : ((float)scoreCounter / (float)totalCounter));
                case Movement.Types.Wave:
                    Function f = currentMovement.f;
                    int count = 0;
                    int correct = 0;

                    //Debug.Assert(input.Count == f.Slopes.Length);

                    foreach (InputState input in inputs)
                    {
                        if (count >= f.Slopes.Length)
                            break;
                        float dist = Vector2.Distance(f.Positions[count], input.position);
                        if (dist < 150)
                            correct++;
                        count++;
                    }
                    return correct / (float)count;
                default:
                    return 0.0f;
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
                localScore = 0.0f; // reset score
            }
        }
    }
}
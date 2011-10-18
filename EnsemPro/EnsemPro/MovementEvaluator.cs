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
        float localScore; // score for this movement

        public MovementEvaluator(Movement m)
        {
            currentMovement = m;
        }

        /*Returns a floating number 0 to 1 which indicates how well the input is matching the movement */
        public float Score(Movement m, IEnumerable<InputState> input, GameTime t)
        {
            if (currentMovement.getType() == Movement.Type.Shake)
            {
                int scoreCounter = 0;
                int totalCounter = 0;
                foreach (InputState state in input)
                {
                    if (Math.Abs(state.acceleration.X) > ACC_THRESHOLD || Math.Abs(state.acceleration.Y) > ACC_THRESHOLD)
                    {
                        scoreCounter++;
                    }
                    totalCounter++;
                }
                Debug.WriteLine("score counter is "+scoreCounter+"\n total counter is "+totalCounter);

                // if fewer than 20 moves, NOT SHAKING! => fail
                return (totalCounter<20 ? 0.00005f : ((float)scoreCounter/(float)totalCounter));
            }
            else if (currentMovement.getType() == Movement.Type.Wave)
            {
                Function f = currentMovement.f;
                int count = 0;
                int correct = 0;
                
                foreach (InputState _input in input)
                {
                    if (f == null) Debug.WriteLine(currentMovement.getType() + " " + currentMovement.endBeat + " " + currentMovement.startBeat);
                    if (count >= f.Slopes.Length)
                        break;
                    float dist = Vector2.Distance(f.Positions[count], _input.position);
                    if (dist < 150)
                        correct++;
                    count++;
                }
                return correct / (float) count;
            }
            else
            {
                // movement type == noop
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
                    currentMovement.setState(Movement.State.Fail);
                }
                else if (score > FAIL_THRESHOLD)
                {
                    Debug.WriteLine("state is SUCCEED");
                    currentMovement.setState(Movement.State.Succeed);
                }
                else
                { // noop, localScore is negative
                    Debug.WriteLine("state is NONE");
                    currentMovement.setState(Movement.State.None);
                }

                currentMovement = m; // update movement
                localScore = 0.0f; // reset score
            }
        }
    }
}
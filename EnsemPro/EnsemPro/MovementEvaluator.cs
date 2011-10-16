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
        public const float ACC_THRESHOLD = 0.0f;
        /* A collection of input detections, which together form a movement */
        IEnumerable<InputState> states;
      //  IEnumerator statesIEnum;

        Movement currentMovement;


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
                    if (Math.Abs(state.acceleration.X) > ACC_THRESHOLD && Math.Abs(state.acceleration.Y) > ACC_THRESHOLD)
                    {
                        Console.WriteLine("acceleration is "+(new Vector2(state.acceleration.X,state.acceleration.Y)));
                        scoreCounter++;
                    }
                    totalCounter++;
                }
                Console.WriteLine(scoreCounter);
                return (float)(1);
            }
            else if (m.getType() == Movement.Type.Wave)
            {
                Function f = m.f;
                return 0.0f;
            }
            else
            {
                // movement type == noop
                return 0.0f;
            }
        }

        public void Update(Movement m,IEnumerable<InputState> input,GameTime t)
        {
            if (m != currentMovement) // new movement, compute score
            {
                float score = Score(currentMovement, input, t);

                // send score back to Movement
                if (score <= FAIL_THRESHOLD && score != 0.0f){
                    currentMovement.setState(Movement.State.Fail);
                }
                else if (score > FAIL_THRESHOLD)
                {
                    currentMovement.setState(Movement.State.Succeed);
                }
                else
                { // noop, score = 0.0f
                    currentMovement.setState(Movement.State.None);
                }
                //  reset IEnumerable<Input>states
                currentMovement = m; // update movement
            }
            else
            {
                // add input to states
            }
        }
    }
}
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
        /* A collection of input detections, which together form a movement */
        IEnumerable<InputState> states;
      //  IEnumerator statesIEnum;

        Movement currentMovement;


        public MovementEvaluator(Movement m)
        {
          //  states = ?.GetEnumerator();
            currentMovement = m;
        }

        /*Returns a floating number 0 to 1 which indicates how well the input is matching the movement */
        public float Score(Movement m, IEnumerable<InputState> input, GameTime t)
        {
            if (m.getType() == Movement.Type.Shake)
            {
                return 0.0f;
            }
            else if (m.getType() == Movement.Type.Wave)
            {
                return 0.0f;
            }
            else
            {
                // movement type == noop
                return 0.0f;
            }
        }

        public void Update(Movement m,GameTime t)
        {
            if (m != currentMovement) // new movement, compute score
            {
                Score(currentMovement, states, t);
                // send score to (playlevel) ?
                //  reset states
                currentMovement = m; // update movement
            }
            else
            {
                // add input to states
            }
        }
    }
}
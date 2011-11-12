using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace EnsemPro
{
    public class DialogController
    {
        GameModel gameState;
        SpriteBatch spriteBatch;

        Queue<String> names;
        Queue<String> lines;

        String speaker;
        String speech;

        DialogModel dialogModel;
        DialogView screen;
        DataTypes.WorldData.CityState nodeState;
        KeyboardState lastState;

	    public DialogController(GameModel gm, SpriteBatch sb, DialogModel dm, DataTypes.WorldData.CityState ns)
	    {
            gameState = gm;
            spriteBatch = sb;
            dialogModel = dm;
            nodeState = ns;
            
            names = new Queue<String>();
            lines = new Queue<String>();
            Parse();
            speaker = "";
            speech = dialogModel.Location.ToString();
            screen = new DialogView(sb);
            
	    }

        public void Initialize()
        {
            
        }

        public void LoadContent(ContentManager cm)
        {
            screen.LoadContent(cm);
        }

        private void Parse()
        {
            switch (nodeState)
            {
                case DataTypes.WorldData.CityState.NewlyUnlocked:
                    for (int i = 0; i < dialogModel.NewlyUnlocked.Length; i++)
                    {
                        names.Enqueue(dialogModel.NewlyUnlocked[i].Character);
                        lines.Enqueue(dialogModel.NewlyUnlocked[i].Line);
                    }
                    break;
                case DataTypes.WorldData.CityState.Unlocked:
                    for (int i = 0; i < dialogModel.Unlocked.Length; i++)
                    {
                        names.Enqueue(dialogModel.Unlocked[i].Character);
                        lines.Enqueue(dialogModel.Unlocked[i].Line);
                    }
                    break;
                case DataTypes.WorldData.CityState.Cleared:
                    for (int i = 0; i < dialogModel.Cleared.Length; i++)
                    {
                        names.Enqueue(dialogModel.Cleared[i].Character);
                        lines.Enqueue(dialogModel.Cleared[i].Line);
                    }
                    break;
                default:
                    break;
            }

        }

        public void Update(GameTime t)
        {
            KeyboardState ks = Keyboard.GetState();
            if (names.Count == 0 && ks.IsKeyDown(Keys.Right) && lastState.IsKeyUp(Keys.Right)) // end of this dialog mode, move on to playlevel or map
            {
                gameState.CurrentScreen = DataTypes.Screens.SelectLevel;
            }
            else
            {
                if (ks.IsKeyDown(Keys.Right) && lastState.IsKeyUp(Keys.Right))
                {
                    speaker = names.Dequeue();
                    speech = lines.Dequeue();
                }
                lastState = ks;
            }
        }

        /// <summary>
        /// Command all related views to draw.
        /// </summary>
        /// <param name="t"></param>
        public void Draw(GameTime t)
        {
            screen.Draw(t,speaker,speech);
        }
    }
}

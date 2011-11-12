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
        StorageContainer storageContainer;

        Queue<String> names;
        Queue<String> dialogs;

        String speaker = "";
        String speech = "";

        DialogView screen;
        StreamReader reader;
        KeyboardState lastState;

	    public DialogController(GameModel gm, SpriteBatch sb, String dn)
	    {
            gameState = gm;
            spriteBatch = sb;
            // parse file then send background etc to view
            // "dialogs/" + dn
          //  storageContainer = device.OpenContainer("WindowsGame1"); 
           // string path = Path.Combine(storageContainer.StorageDevice=, "dialogs/"+dn);
           // reader = new StreamReader(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),dn));
          //  reader = new StreamReader(TitleContainer.OpenStream(dn));
         //   string fullpath = StorageContainer.TitalLocation + dn;
            reader = new StreamReader(dn);
            names = new Queue<String>();
            dialogs = new Queue<String>();
            Parse();
            speaker = names.Dequeue();
            speech = dialogs.Dequeue();
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
            string line = reader.ReadLine();
            while (line != null) {
                int colonIndex = line.IndexOf(":");
                names.Enqueue(line.Substring(0,colonIndex));
                dialogs.Enqueue(line.Substring(colonIndex + 1));
                line = reader.ReadLine();
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
                    speech = dialogs.Dequeue();
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

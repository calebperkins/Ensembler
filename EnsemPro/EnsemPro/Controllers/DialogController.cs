using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using XnaColor = Microsoft.Xna.Framework.Color;


namespace EnsemPro
{
    public class DialogController
    {
        GameState gameState;
        SpriteBatch spriteBatch;
        ContentManager contentManager;

        Queue<String> names;
        Queue<String> lines;
        Dictionary<String,XnaColor> colors;
        Dictionary<String,Texture2D> faces;

        String speaker;
        String speech;
        XnaColor color;
        Texture2D face;

        DialogModel dialogModel;
        DialogView screen;
        KeyboardState lastState;
        SoundEffect NextDialog;
        SoundEffect ReceiveItem;

        bool IsFinished;

        public DialogController(GameState gm, SpriteBatch sb, DialogModel dm, string cityName, ContentManager cm)
        {
            gameState = gm;
            spriteBatch = sb;
            dialogModel = dm;
            contentManager = cm;

            names = new Queue<String>();
            lines = new Queue<String>();
            colors = new Dictionary<String,XnaColor>();
            faces = new Dictionary<String,Texture2D>();
            Parse();
            speaker = "";
            speech = cityName;
            color = XnaColor.Black;
            screen = new DialogView(sb);
            
        }

        public void Initialize()
        {
            IsFinished = false;

        }

        public void LoadContent(ContentManager cm)
        {
            screen.LoadContent(cm);
            NextDialog = cm.Load<SoundEffect>("Sounds//NextDialog");
            ReceiveItem = cm.Load<SoundEffect>("Sounds//ReceiveItem");
        }

        public void UnloadContent() 
        {
            contentManager.Unload();
        }

        private void Parse()
        {
            for (int i = 0; i < dialogModel.Colors.Length; i++)
            {
                string characterName = dialogModel.Colors[i].Character;
                string colorName = dialogModel.Colors[i].Color;
                System.Drawing.Color color = System.Drawing.Color.FromName(colorName);
                XnaColor xnaColor = new XnaColor(color.R, color.G, color.B, color.A);
                colors.Add(characterName, xnaColor);
            }
            for (int i = 0; i < dialogModel.Faces.Length; i++)
            {
                string face = dialogModel.Faces[i].FaceAssetName;
                if (face != null)
                {
                    string characterName = face.Substring(face.LastIndexOf("\\") + 1, face.Length - (face.LastIndexOf("\\") + 1));
                    faces.Add(characterName, contentManager.Load<Texture2D>(face));
                }
            }
            for (int i = 0; i< dialogModel.Content.Length; i++)
            {
                names.Enqueue(dialogModel.Content[i].Character);
                lines.Enqueue(dialogModel.Content[i].Line);
            }
        }

        public bool Finished() { return IsFinished; }

        public void Update(GameTime t)
        {
            KeyboardState ks = Keyboard.GetState();
            if (names.Count == 0 && ks.IsKeyDown(Keys.Right) && lastState.IsKeyUp(Keys.Right)) // end of this dialog mode, move on to playlevel or map
            {
               
                IsFinished = true;
            }
            else
            {
                if (ks.IsKeyDown(Keys.Right) && lastState.IsKeyUp(Keys.Right))
                {
                    NextDialog.Play();
                    speaker = names.Dequeue();
                    speech = lines.Dequeue();
                    color = colors[speaker];
                    if (faces.ContainsKey(speaker))
                    {
                        face = faces[speaker];
                    }
                    else
                    {
                        face = null;
                    }
                    if (speech[0] == '*') ReceiveItem.Play();
                    string firstPart = "";
                    string secondPart = speech;
                    while (secondPart.Length > 55)
                    {
                        string subSpeech = secondPart.Substring(0,55);
                        int lastSpace = subSpeech.LastIndexOf(" ");
                        firstPart = firstPart + secondPart.Substring(0, lastSpace) + "\n";
                        secondPart = secondPart.Substring(lastSpace, secondPart.Length - lastSpace);
                    }
                    speech = firstPart + secondPart;
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
            screen.Draw(t,speaker,speech,color,face);
        }
    }
}

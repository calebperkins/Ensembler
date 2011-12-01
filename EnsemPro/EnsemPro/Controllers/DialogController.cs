using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        Dictionary<String, Texture2D> cutscenes;
        Dictionary<String, String> startStopCues;

        String speaker;
        String speech;
        XnaColor color;
        Texture2D face;
        Texture2D cutscene;
        String stopCue;

        DialogModel dialogModel;
        DialogView screen;
        KeyboardState lastState;
        SoundEffect NextDialog;
        SoundEffect ReceiveItem;

        public DialogController(GameState gm, SpriteBatch sb, DialogModel dm, string cityName, ContentManager cm)
        {
            gameState = gm;
            spriteBatch = sb;
            Dialog = dm;
            contentManager = cm;

            names = new Queue<String>();
            lines = new Queue<String>();
            colors = new Dictionary<String,XnaColor>();
            faces = new Dictionary<String,Texture2D>();
            cutscenes = new Dictionary<String, Texture2D>();
            startStopCues = new Dictionary<String, String>();
            Parse();
            speaker = "";
            speech = cityName;
            color = XnaColor.Black;
            screen = new DialogView(sb);
            stopCue = "";
        }

        public void Initialize()
        {
            Finished = false;
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
            for (int i = 0; i < dialogModel.Content.Length; i++)
            {
                names.Enqueue(dialogModel.Content[i].Character);
                lines.Enqueue(dialogModel.Content[i].Line);
            }
            for (int i = 0; i < dialogModel.CutScenes.Length; i++)
            {
                cutscenes.Add(dialogModel.CutScenes[i].StartCue, contentManager.Load<Texture2D>(dialogModel.CutScenes[i].Picture));
                startStopCues.Add(dialogModel.CutScenes[i].StartCue, dialogModel.CutScenes[i].StopCue);
            }
        }

        public bool Finished
        {
            get;
            private set;
        }

        public DialogModel Dialog
        {
            get {return dialogModel;}
            private set {dialogModel = value;}
        }

        public void Update(GameTime t)
        {
            KeyboardState ks = Keyboard.GetState();
            if (names.Count == 0 && gameState.Input.Confirm || ks.IsKeyDown(Keys.Q) && lastState.IsKeyUp(Keys.Q)) // end of this dialog mode, move on to playlevel or map
            {
               
                Finished = true;
            }
            else
            {
                if (gameState.Input.Confirm)
                {
                    NextDialog.Play();
                    speaker = names.Dequeue();
                    speech = lines.Dequeue();
                    color = colors[speaker];
                    face = (faces.ContainsKey(speaker) ? faces[speaker] : null);
                    if (speech == stopCue)
                    { 
                        cutscene = null;
                        stopCue = "";
                    }
                    if (cutscenes.ContainsKey(speech)) // start of a cutscene
                    {
                        cutscene = cutscenes[speech];
                        stopCue = startStopCues[speech];
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
            screen.Draw(t,speaker,speech,color,face,cutscene);
        }
    }
}

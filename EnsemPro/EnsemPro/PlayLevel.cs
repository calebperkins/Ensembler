﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace EnsemPro
{
    public class PlayLevel
    {
        Texture2D shake;
       // Queue<> actionQueue = new Queue<>();

        public PlayLevel()
        {
        }

        public void start()
        {
        }

        public void LoadContent(ContentManager content)
        {
            shake = content.Load<Texture2D>("images\\shake");
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
        public void Update(GameTime gameTime)
        {

        }

    }
}

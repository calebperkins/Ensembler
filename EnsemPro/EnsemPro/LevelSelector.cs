using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace EnsemPro
{
    public class LevelSelector : DrawableGameComponent
    {
        Texture2D selectionBackground;
        Texture2D normalBox;
        Texture2D selectedBox;
        Texture2D completed;

        List<String> songs;
        
        SpriteBatch spriteBatch;
        
        public LevelSelector(Game g, SpriteBatch sb) : base (g)
        {
            spriteBatch = sb;
        }
        
        protected override void LoadContent()
        {
            selectionBackground = Game.Content.Load<Texture2D>("Images\\LevelSelection\\background");
            normalBox = Game.Content.Load<Texture2D>("Images\\LevelSelection\\normalbox");
            selectedBox = Game.Content.Load<Texture2D>("Images\\LevelSelection\\selectedbox");
            completed = Game.Content.Load<Texture2D>("Images\\LevelSelection\\completed");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(selectionBackground, new Vector2(0, 0), Color.White);
            base.Draw(gameTime);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace EnsemPro
{
    class MenuView
    {
        SpriteBatch spriteBatch;
        Texture2D background;
        Texture2D storyHover;
        Texture2D freeHover;
        Texture2D exitHover;

        public MenuView(SpriteBatch sb)
        {
            spriteBatch = sb;
        }

        public void LoadContent(ContentManager cm)
        {
            background = cm.Load<Texture2D>("Images\\MainMenu\\cover");
            storyHover = cm.Load<Texture2D>("Images\\MainMenu\\story_hover");
            freeHover = cm.Load<Texture2D>("Images\\MainMenu\\free_hover");
            exitHover = cm.Load<Texture2D>("Images\\MainMenu\\exit_hover");
        }

        public void Draw(MenuController.Hover hover)
        {
            spriteBatch.Draw(background, new Vector2(), Color.White);
            switch (hover)
            {
                case MenuController.Hover.Story:
                    spriteBatch.Draw(storyHover, new Vector2(), Color.White);
                    break;
                case MenuController.Hover.Free:
                    spriteBatch.Draw(freeHover, new Vector2(), Color.White);
                    break;
                case MenuController.Hover.Exit:
                    spriteBatch.Draw(exitHover, new Vector2(), Color.White);
                    break;
                default:
                    break;
            }
        }
    }
}

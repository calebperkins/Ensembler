using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

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
            background = cm.Load<Texture2D>("Images\\MainMenu\\newtitle");
            storyHover = cm.Load<Texture2D>("Images\\MainMenu\\storymode_hover");
            freeHover = cm.Load<Texture2D>("Images\\MainMenu\\freemode_hover");
            exitHover = cm.Load<Texture2D>("Images\\MainMenu\\exitmode_hover");
            Song s = cm.Load<Song>("journey");
            MediaPlayer.Play(s);
            MediaPlayer.Volume = 0.3f;
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

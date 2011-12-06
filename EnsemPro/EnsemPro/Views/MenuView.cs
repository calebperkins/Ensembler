using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Ensembler
{
    class MenuView
    {
        SpriteBatch spriteBatch;
        Texture2D background;
        Texture2D storyHover;
        Texture2D freeHover;
        Texture2D exitHover;
        Song song;
        Game game;
        GameState state;
        MenuController menu;

        public MenuView(Game g, SpriteBatch sb, MenuController c)
        {
            spriteBatch = sb;
            game = g;
            menu = c;
        }

        public void LoadContent(ContentManager cm)
        {
            state = game.Services.GetService(typeof(GameState)) as GameState;

            background = cm.Load<Texture2D>("Images\\MainMenu\\newtitle");
            storyHover = cm.Load<Texture2D>("Images\\MainMenu\\storymode_hover");
            freeHover = cm.Load<Texture2D>("Images\\MainMenu\\freemode_hover");
            exitHover = cm.Load<Texture2D>("Images\\MainMenu\\exitmode_hover");
            song = cm.Load<Song>("journey");
            MediaPlayer.Play(song);
            MediaPlayer.Volume = 0.3f;
        }

        public void Draw(GameTime t)
        {
            if (MediaPlayer.State != MediaState.Playing)
                MediaPlayer.Play(song);

            spriteBatch.Draw(background, new Vector2(), Color.White);
            if (menu.hover == MenuController.Hover.Story)
                spriteBatch.Draw(storyHover, new Vector2(), Color.White);
            else if (menu.hover == MenuController.Hover.Free)
                spriteBatch.Draw(freeHover, new Vector2(), Color.White);
            else if (menu.hover == MenuController.Hover.Exit)
                spriteBatch.Draw(exitHover, new Vector2(), Color.White);
        }

    }
}

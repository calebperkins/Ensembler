using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;

namespace Ensembler
{
    class LevelSelectController
    {
        GameState gameState;
        SpriteBatch spriteBatch;

        LevelSelectView levelSelectScreen;
        int selected = 0;
        int lastSelected = 0;

        SoundEffect MenuMove;
        SoundEffect MenuSelect;

        Song bgSong;

        int page = 0;
        Rectangle nextBox = new Rectangle(300, 550, 40, 40);
        Rectangle prevBox = new Rectangle(300, 500, 40, 40);
        int PAGES;

        public LevelSelectController(GameState gm, SpriteBatch sb)
        {
            gameState = gm;
            PAGES = (gm.Levels.Length + LevelSelectView.PER_PAGE - 1) / LevelSelectView.PER_PAGE;
            spriteBatch = sb;
            levelSelectScreen = new LevelSelectView(sb, gameState);
        }

        public void Initialize()
        {

        }

        public void LoadContent(ContentManager cm)
        {
            levelSelectScreen.LoadContent(cm);
            MenuMove = cm.Load<SoundEffect>("Sounds//MenuMove");
            MenuSelect = cm.Load<SoundEffect>("Sounds//MenuSelect");
            bgSong = cm.Load<Song>("journey");
            MediaPlayer.IsRepeating = true;
        }

        void SetSelected()
        {
            // TODO: remove these magic constants :)
            Vector2 p = gameState.Input.Position;
            if (p.X >= GameEngine.WIDTH / 2)
                selected = Math.Max(0, (int)(p.Y - 120) / 105 ) + page* LevelSelectView.PER_PAGE;
            else
                selected = -1;
        }

        public void Update(GameTime t)
        {
            if (MediaPlayer.State != MediaState.Playing)
                MediaPlayer.Play(bgSong);

            SetSelected();

            if (lastSelected != selected)
                MenuMove.Play();

            lastSelected = selected;

            if (gameState.Input.Confirm && gameState.Input.Inside(nextBox))
            {
                page = (page + 1) % PAGES;
                selected = -1;
                MenuSelect.Play();
            }
            else if (gameState.Input.Confirm && gameState.Input.Inside(prevBox))
            {
                page -= 1;
                if (page < 0)
                    page = PAGES - 1;
                selected = -1;
                MenuSelect.Play();
            }
            else if (gameState.Input.Confirm && selected >= 0 && selected < gameState.Levels.Length)
            {
                MenuSelect.Play();
                MediaPlayer.Stop();
                gameState.CurrentScreen = DataTypes.Screens.PlayLevel;
                gameState.SelectedLevel = gameState.Levels[selected].AssetName;
            }
        }

        /// <summary>
        /// Command all related views to draw.
        /// </summary>
        /// <param name="t"></param>
        public void Draw(GameTime t)
        {
            levelSelectScreen.Draw(t, page, selected);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;

namespace EnsemPro
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

        public LevelSelectController(GameState gm, SpriteBatch sb)
        {
            gameState = gm;
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
        }

        void SetSelected()
        {
            // TODO: remove these magic constants :)
            Vector2 p = gameState.Input.Position;
            if (p.X < GameEngine.WIDTH/2)
                return;
            selected = (int) p.Y / 105;
        }

        public void Update(GameTime t)
        {
            if (MediaPlayer.State != MediaState.Playing)
                MediaPlayer.Play(bgSong);

            SetSelected();

            if (lastSelected != selected)
                MenuMove.Play();

            lastSelected = selected;

            if (gameState.Input.Confirm)
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
            levelSelectScreen.Draw(t, gameState.Levels, selected);
        }
    }
}

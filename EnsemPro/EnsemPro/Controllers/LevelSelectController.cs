using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace EnsemPro
{
    class LevelSelectController
    {
        GameState gameState;
        SpriteBatch spriteBatch;

        LevelSelectView levelSelectScreen;
        int selected = 0;

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

        public void Update(GameTime t)
        {
            if (MediaPlayer.State != MediaState.Playing)
                MediaPlayer.Play(bgSong);

            if (gameState.Input.Down)
            {
                selected = (selected + 1) % gameState.Levels.Length;
                MenuMove.Play();
            }
            else if (gameState.Input.Up)
            {
                selected = (selected == 0) ? (gameState.Levels.Length - 1) : (selected - 1);
                MenuMove.Play();
            }
            else if (gameState.Input.Confirm)
            {
                MenuSelect.Play();
                MediaPlayer.Stop();
                gameState.CurrentScreen = DataTypes.Screens.PlayLevel;
                gameState.SelectedLevel = gameState.Levels[selected].AssetName;
                // gameState.Input.Confirm = false;
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

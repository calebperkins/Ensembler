using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace EnsemPro
{
    class ChooseLevelScreen
    {
        Texture2D background;
        SpriteBatch spriteBatch;
        Texture2D normalTexture;
        Texture2D selectedTexture;
        SpriteFont font;
        Song bgSong;

        public ChooseLevelScreen(SpriteBatch sb)
        {
            spriteBatch = sb;
        }

        public void LoadContent(ContentManager cm)
        {
            background = cm.Load<Texture2D>("Images//SelectionScreen//background");
            normalTexture = cm.Load<Texture2D>("Images//SelectionScreen//normalbox");
            selectedTexture = cm.Load<Texture2D>("Images//SelectionScreen//selectedbox");
            font = cm.Load<SpriteFont>("images//ScoreFont");
            bgSong = cm.Load<Song>("journey");
        }

        public void Draw(GameTime t, DataTypes.LevelSummary[] levels, int selected)
        {
            if (MediaPlayer.State != MediaState.Playing)
                MediaPlayer.Play(bgSong);

            spriteBatch.Draw(background, new Vector2(), Color.White);
            for (int i = 0; i < levels.Length; i++)
            {
                spriteBatch.Draw(i == selected ? selectedTexture : normalTexture, new Rectangle(400, i * 100, 400, 100), Color.White);
                spriteBatch.DrawString(font, levels[i].Title, new Vector2(450, 30+i*100), Color.Black);
            }
            
        }
    }
}

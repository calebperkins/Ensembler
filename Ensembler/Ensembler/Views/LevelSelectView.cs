using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Ensembler
{
    class LevelSelectView
    {
        Texture2D background;
        SpriteBatch spriteBatch;
        Texture2D normalTexture;
        Texture2D selectedTexture;
        SpriteFont scoreFont;
        SpriteFont songFont;
        Color scoreColor;
        Color songColor;
        Song bgSong;
        GameState state;
        float offsetBottom;

        public const int PER_PAGE = 5;

        public LevelSelectView(SpriteBatch sb, GameState s)
        {
            spriteBatch = sb;
            state = s;
            offsetBottom = state.ViewPort.Height - 170;
            scoreColor = new Color(87, 50, 19);
            songColor = new Color(128, 93, 25);
        }

        public void LoadContent(ContentManager cm)
        {
            background = cm.Load<Texture2D>("Images//SelectionScreen//background");
            normalTexture = cm.Load<Texture2D>("Images//SelectionScreen//normalbox");
            selectedTexture = cm.Load<Texture2D>("Images//SelectionScreen//selectedbox");
            scoreFont = cm.Load<SpriteFont>("Images//SelectionScreen//VTKAnimal2");
            songFont = cm.Load<SpriteFont>("Images//SelectionScreen//GermanUnderground");
            bgSong = cm.Load<Song>("journey");
        }

        public void Draw(GameTime t, int page, int selected)
        {
            DataTypes.LevelSummary[] levels = state.Levels;
            spriteBatch.Draw(background, new Vector2(), Color.White);
            int limit = System.Math.Min(page * PER_PAGE + PER_PAGE, state.Levels.Length);
            for (int i = page * PER_PAGE; i < limit; i++)
            {
                int j = i % PER_PAGE; // geometric position on page
                spriteBatch.Draw(j == selected ? selectedTexture : normalTexture, new Rectangle(400, 120+j * 100, 400, 100), Color.White);
                spriteBatch.DrawString(songFont, levels[i].Title, new Vector2(450, 140+j*100), songColor);
            }
            
            // Draws data about the selected level
            spriteBatch.DrawString(scoreFont, "High Score: " + levels[selected].HighScore, new Vector2(10, offsetBottom - 150), scoreColor, 0.0f, new Vector2(), 0.8f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(scoreFont, "Developer High Score: " + levels[selected].DeveloperHighScore, new Vector2(10, offsetBottom - 100), scoreColor, 0.0f, new Vector2(), 0.8f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(scoreFont, "High Combo: " + levels[selected].HighCombo, new Vector2(10, offsetBottom - 50), scoreColor, 0.0f, new Vector2(), 0.8f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(scoreFont, "Developer High Combo: " + levels[selected].DeveloperHighCombo, new Vector2(10, offsetBottom), scoreColor, 0.0f, new Vector2(), 0.8f, SpriteEffects.None, 0.0f);
        }
    }
}

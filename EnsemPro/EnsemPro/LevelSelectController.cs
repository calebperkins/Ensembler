using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnsemPro
{
    class LevelSelectController
    {
        GameModel gameState;
        SpriteBatch spriteBatch;

        ChooseLevelScreen levelSelectScreen;
        int selected = 0;

        public LevelSelectController(GameModel gm, SpriteBatch sb)
        {
            gameState = gm;
            spriteBatch = sb;
            levelSelectScreen = new ChooseLevelScreen(sb);
        }

        public void Initialize()
        {

        }

        public void LoadContent(ContentManager cm)
        {
            levelSelectScreen.LoadContent(cm);
        }

        public void Update(GameTime t)
        {
            if (gameState.Input.Down)
                selected = (selected + 1) % gameState.Levels.Length;
            else if (gameState.Input.Up)
                selected = (selected == 0) ? (gameState.Levels.Length - 1) : (selected - 1);
            else if (gameState.Input.Confirm)
            {
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

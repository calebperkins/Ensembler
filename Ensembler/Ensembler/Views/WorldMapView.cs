using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Ensembler
{
    class WorldMapView
    {
        public static int nodeXShift = 0;

        SpriteFont font;
        SpriteFont instructionFont;
        SpriteBatch spriteBatch;
        Texture2D background;
        Texture2D lockedTexture;
        Texture2D newlyUnlockedTexture;
        Texture2D unlockedTexture;
        Texture2D clearedTexture;
        Texture2D selectedTexture;

        Vector2 instructionPos;
        Vector2 instructionOffset;
        string tutorial;

        public WorldMapView(SpriteBatch sb)
        {
            spriteBatch = sb;
            instructionPos = new Vector2(20, 21);
            instructionOffset = new Vector2(0, 40);
            tutorial = "Press I to open tutorial";
        }

        public void LoadContent(ContentManager cm)
        {
            // TODO CHANGE TEXTURES OF THESE
            font = cm.Load<SpriteFont>("images\\WorldMap\\text");
            instructionFont = cm.Load<SpriteFont>("Images//WorldMap//Nosferatu");
            background = cm.Load<Texture2D>("images\\WorldMap\\map");
            lockedTexture = cm.Load<Texture2D>("images\\WorldMap\\locked");
            newlyUnlockedTexture = cm.Load<Texture2D>("images\\WorldMap\\unlocked");
            unlockedTexture = cm.Load<Texture2D>("images\\WorldMap\\unlocked");
            clearedTexture = cm.Load<Texture2D>("images\\WorldMap\\cleared");
            selectedTexture = cm.Load<Texture2D>("images\\ring");
        }

        public void Draw(HashSet<Models.City> cities, Models.City selected)
        {
            spriteBatch.Draw(background, new Vector2(), Color.White);
            foreach (Models.City node in cities)
            {
                Texture2D current = null;
                switch (node.State)
                {
                    case DataTypes.WorldData.CityState.Locked:
                        current = lockedTexture;
                        break;
                    case DataTypes.WorldData.CityState.NewlyUnlocked:
                        current = newlyUnlockedTexture;
                        break;
                    case DataTypes.WorldData.CityState.Unlocked:
                        current = unlockedTexture;
                        break;
                    case DataTypes.WorldData.CityState.Cleared:
                        current = clearedTexture;
                        break;
                }

                Vector2 origin = new Vector2(current.Width / 2, current.Height / 2);
                float scale = 1.0f;

                spriteBatch.Draw(current, node.AbsolutePosition, null, Color.White, 0.0f, origin, scale, SpriteEffects.None, 0);

                if (node == selected)
                {
                    spriteBatch.DrawString(font, node.Data.Name, new Vector2(node.AbsolutePosition.X + 8, node.AbsolutePosition.Y - 7), Color.Black);
                }
            }

            // Draw instructions
            int counter = 0;
            spriteBatch.DrawString(instructionFont, tutorial, instructionPos + (counter++) * instructionOffset, Color.Black, 0.0f, new Vector2(), 0.55f, SpriteEffects.None, 0.0f);
        }
    }
}

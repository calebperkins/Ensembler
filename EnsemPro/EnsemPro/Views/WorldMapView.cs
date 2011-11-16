using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace EnsemPro
{
    class WorldMapView
    {
        public static int nodeXShift = 0;

        SpriteBatch spriteBatch;
        Texture2D background1;
        Texture2D background2;
        Texture2D lockedTexture;
        Texture2D newlyUnlockedTexture;
        Texture2D unlockedTexture;
        Texture2D clearedTexture;

        Texture2D selectedTexture;

        public Vector2 CurBackgroundPos
        {
            get;
            set;
        }

        public float WantedBackgroundPosX
        {
            get;
            set;
        }
        
        public WorldMapView(SpriteBatch sb)
        {
            spriteBatch = sb;
            CurBackgroundPos = new Vector2();
        }

        public void LoadContent(ContentManager cm)
        {
            // TODO CHANGE TEXTURES OF THESE
            background1 = cm.Load<Texture2D>("images\\WorldMap\\map1");
            background2 = cm.Load<Texture2D>("images\\WorldMap\\map2");
            lockedTexture = cm.Load<Texture2D>("images\\WorldMap\\locked");
            newlyUnlockedTexture = cm.Load<Texture2D>("images\\WorldMap\\newly_unlocked");
            unlockedTexture = cm.Load<Texture2D>("images\\WorldMap\\unlocked");
            clearedTexture = cm.Load<Texture2D>("images\\WorldMap\\cleared");
            selectedTexture = cm.Load<Texture2D>("images\\ring");
        }

        public void Draw(HashSet<Models.City> cities, Models.City selected)
        {
            spriteBatch.Draw(background1, CurBackgroundPos, Color.White);
            spriteBatch.Draw(background2, new Vector2(CurBackgroundPos.X + background1.Width, CurBackgroundPos.Y), Color.White);
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

                node.AbsolutePosition.X = CurBackgroundPos.X + node.RelativePosition.X;

                Vector2 origin = new Vector2(current.Width / 2, current.Height / 2);
                float scale = 1.0f;

                spriteBatch.Draw(current, node.AbsolutePosition, null, Color.White, 0.0f, origin, scale, SpriteEffects.None, 0);

                if (node == selected)
                {
                    origin = new Vector2(selectedTexture.Width / 2, selectedTexture.Height / 2);
                    spriteBatch.Draw(selectedTexture, node.AbsolutePosition, null, Color.White, 0.0f, origin, scale, SpriteEffects.None, 0);
                }
            }
        }
    }
}

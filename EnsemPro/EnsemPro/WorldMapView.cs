using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
            lockedTexture = cm.Load<Texture2D>("images\\dot_fail");
            newlyUnlockedTexture = cm.Load<Texture2D>("images\\dot");
            unlockedTexture = cm.Load<Texture2D>("images\\dot_win");
            clearedTexture = cm.Load<Texture2D>("images\\dot_normal");
            selectedTexture = cm.Load<Texture2D>("images\\ring");
        }

        public void Draw(Node[] nodes, int selected)
        {
            spriteBatch.Draw(background1, CurBackgroundPos, Color.White);
            spriteBatch.Draw(background2, new Vector2 (CurBackgroundPos.X + background1.Width, CurBackgroundPos.Y), Color.White);
            for (int i = 0; i< nodes.Length; i++)
            {
                Texture2D current = null;
                switch (nodes[i].nodeState)
                {
                    case Node.NodeState.Locked:
                        current = lockedTexture;
                        break;
                    case Node.NodeState.NewlyUnlocked:
                        current = newlyUnlockedTexture;
                        break;
                    case Node.NodeState.Unlocked:
                        current = unlockedTexture;
                        break;
                    case Node.NodeState.Cleared:
                        current = clearedTexture;
                        break;
                }

                nodes[i].curPos.X = CurBackgroundPos.X + nodes[i].oriX;

                Vector2 origin = new Vector2(current.Width / 2, current.Height / 2);
                float scale = 1.0f;

                if (current == newlyUnlockedTexture)
                {
                    scale = 0.2f;
                }

                spriteBatch.Draw(current, nodes[i].curPos, null, Color.White, 0.0f, origin, scale, SpriteEffects.None, 0);

                if (i == selected)
                {
                    origin = new Vector2(selectedTexture.Width / 2, selectedTexture.Height / 2);
                    spriteBatch.Draw(selectedTexture, nodes[i].curPos, null, Color.White, 0.0f, origin, scale, SpriteEffects.None, 0);
                }
            }
        }
    }
}

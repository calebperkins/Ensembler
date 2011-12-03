using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EnsemPro
{
    class Musician
    {
        Texture2D texture;
        Rectangle[] map;
        Rectangle src;
        Vector2 position;
        SpriteBatch spriteBatch;
        int frameRate;

        public Musician(DataTypes.MusicianData md, ContentManager cm, SpriteBatch sb)
        {
            texture = cm.Load<Texture2D>(md.Texture);
            map = cm.Load<Dictionary<string, Rectangle>>(md.SpriteMap).Values.ToArray();
            position = md.Position;
            frameRate = md.FrameRate;
            spriteBatch = sb;
        }

        public void Draw(GameTime t, bool update, Color tint, bool countDownDone)
        {
            if (!countDownDone) src = map[0];
            else if (update) src = map[(int)(t.TotalGameTime.TotalSeconds * frameRate) % map.Length];
            spriteBatch.Draw(texture, position, src, tint);
        }

        public bool Visible
        {
            get { return true; }
        }

    }
}

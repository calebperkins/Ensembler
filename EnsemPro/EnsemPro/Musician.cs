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

        public Musician(ContentManager cm, SpriteBatch sb, string texture_name, string texture_map, Vector2 pos, int frameRate)
        {
            texture = cm.Load<Texture2D>(texture_name);
            map = cm.Load<Dictionary<string, Rectangle>>(texture_map).Values.ToArray();
            position = pos;
            this.frameRate = frameRate;
            spriteBatch = sb;
        }

        public void Draw(GameTime t, bool update)
        {
            if (update) src = map[(int)(t.TotalGameTime.TotalSeconds * frameRate) % map.Length];
            spriteBatch.Draw(texture, position, src, Color.White);
        }

        public bool Visible
        {
            get { return true; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace RogueLikePlatfromer
{
    public class Sprite
    {
        public Texture2D texture;
        public Vector2 origin = Vector2.Zero;
        public Rectangle rect;
        public Rectangle sourceRect;
        public Color color = Color.White;
        public float rotation = 0f;
        public float layer = 0f;
        public int scale = 1;
        public SpriteEffects flip = SpriteEffects.None;
        public Vector2 position;

        public Sprite()
        {

        } 

        public void Load(string path)
        {
            texture = Setup.ContentDevice.Load<Texture2D>(path);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, rect, color, 0f, Vector2.Zero, scale, flip, 0);
        }

    }
}

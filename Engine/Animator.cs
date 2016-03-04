using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLikePlatfromer
{
    class Animator
    {
        float speed;
        float rectFrame;
        bool _loop;
        public float time;

        Rectangle rect;

        public List<Rectangle> clip = new List<Rectangle>();

        public void Set(float animationSpeed, List<Rectangle> clips, bool loop)
        {
            speed = animationSpeed;
            clip = clips;
            _loop = loop;
        }

        public void Update(GameTime gameTime)
        {
          //  Frames(gameTime);
        }

        public void Frames(GameTime gameTime)
        {
            time += speed * gameTime.DeltaTime();
            rectFrame += speed * gameTime.DeltaTime();

           // time = MathHelper.Clamp(time, 0, clip.Count);

            if (_loop)
            {
                if (time >= clip.Count)
                {
                    rectFrame = 1f;
                    time = 0F;
                }
            }

            if (time < clip.Count)
            {
                for (int i = 0; i < clip.Count; i++)
                {
                    if (rectFrame >= (int)time)
                        rect = clip[(int)time];
                }
            }

            //Console.WriteLine(" / " + rectFrame + " / " + time);

        }

        public void Dispose()
        {
            for (int i = clip.Count - 1; i >= 0; i--)
            {
                clip.Remove(clip[i]);
            }
        }

        public Rectangle UpdateRect
        {
            get { return rect; }
        }

    }
}

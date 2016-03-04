using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace RogueLikePlatfromer
{
    public static class MonoGameExtension
    {

        public static float DeltaTime(this GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.DebugView;

namespace RogueLikePlatfromer
{
    public static class Setup
    {
        private static ContentManager c;
        private static GraphicsDeviceManager g;

        public static void Init(ContentManager _content, GraphicsDeviceManager _graphics)
        {
            g = _graphics;
            c = _content;
        }

        public static ContentManager ContentDevice
        {
            get
            {
                return c;
            }
        }

        public static GraphicsDeviceManager graphics
        {
            get
            {
                return g;
            }
        }
    }
}

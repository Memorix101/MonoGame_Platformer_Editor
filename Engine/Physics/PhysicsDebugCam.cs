using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace RogueLikePlatfromer
{
    public class PhysicsDebugCam
    {
        public Matrix view;
        public Matrix projection;
        public Matrix debugScale;
      // float PosX = 8;
      // float PosY = 8;

        public PhysicsDebugCam()
        {
            /// CURRENT FAKE CAMERA
            projection = Matrix.CreateOrthographicOffCenter(0, Setup.graphics.GraphicsDevice.Viewport.Width / 2, Setup.graphics.GraphicsDevice.Viewport.Height / 2, 0, 0, 1);
            view = Matrix.Identity;
            view = Matrix.CreateTranslation(8, 8, 0);
        }

        public Matrix View
        {
            get { return debugScale; }
        }

        public Matrix Proj
        {
            get { return projection; }
        }

        public void UpdatePos()
        {
           // PosX -= 5;
        }

        public void Update()
        {
            view = Matrix.CreateTranslation(Game1.camera.Position.X/2, Game1.camera.Position.Y/2, 0); // correction the view after Farseer pixels to meter convertation
        }
    }
}

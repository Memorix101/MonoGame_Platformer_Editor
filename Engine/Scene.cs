using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLikePlatfromer
{
    static class Scene
    {
        public static List<Tile> levelObjects = new List<Tile>();
        public static List<Enemy> ActorObjects = new List<Enemy>();
        public static List<Tile> EntityObjects = new List<Tile>();
        public static List<Player> PlayerList = new List<Player>();
        //static List<Sprite> addObj = new List<Sprite>();
        //static List<Sprite> delObj = new List<Sprite>();

        static Texture2D background;

        static Scene()
        {
           background = Setup.ContentDevice.Load<Texture2D>("Sprites/atlas");
        }

        public static void Dispose()
        {            
            for(int i = levelObjects.Count - 1; i >= 0; i--)
            {
                levelObjects[i].rigidbody.Dispose();
                levelObjects.Remove(levelObjects[i]);
            }

            for (int i = ActorObjects.Count - 1; i >= 0; i--)
            {
                ActorObjects[i].rigidbody.Dispose();
                ActorObjects.Remove(ActorObjects[i]);
            }

            for (int i = PlayerList.Count - 1; i >= 0; i--)
            {
                PlayerList[i].rigidbody.Dispose();
                PlayerList.Remove(PlayerList[i]);
            }
        }

        public static void Update(GameTime gameTime)
        {
            foreach (var s in Scene.PlayerList)
                s.Update(gameTime);

            foreach (var s in Scene.ActorObjects)
                s.Update(gameTime);
        }

        public static void Sky(SpriteBatch spriteBatch)
        {
            Rectangle screenRectangle = new Rectangle(0, 0, Screen.width, Screen.height);
            Rectangle skyRect = new Rectangle(0, 0, 10 * 32, 9 * 32);
            spriteBatch.Draw(background, screenRectangle, skyRect, Color.White);
        }
        
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var s in Scene.PlayerList.OrderBy(t => t.layer))
                s.Draw(spriteBatch);

            foreach (var s in Scene.ActorObjects.OrderBy(t => t.layer))
                s.Draw(spriteBatch);

            foreach (var s in Scene.levelObjects.OrderBy(t => t.layer))
                s.Draw(spriteBatch);
        }
    }
}

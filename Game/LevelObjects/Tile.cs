using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.DebugView;

namespace RogueLikePlatfromer
{

    enum TileType
    {
        None,
        DirtTopLeft,
        DirtTopMid,
        DirtTopRight,

        BlueBox,
        DirtMidLeft,
        DirtMid,
        DirtMidRight,

        Crate,
        DirtBotLeft,
        PlayerSpawn,
        DirtBotRight,

        RedBox,
        EnemyBlu,
        SpikeInv,
        Spike,

        /*
                DirtPilSmall,
        DirtSmall,
        DirtSmallLeft,
        DirtSmallRight,

        DirtPil,
        Dirt,
        DirtLeft,
        DirtRight,

        DirtPilMid,
        DirtMid,
        DirtMidLeft,
        DirtMidRight,

        DirtPilBot,
        DirtBot,
        DirtBotLeft,
        DirtBotRight,

        EnemyBlu,
        SpikesDown,
        Crate,
        Spikes,
        */

    };

    class Tile : Sprite
    {
        public TileType tileType;
        public Body rigidbody;

        public Tile(TileType type, Vector2 pos)
        {
            position = pos;
            Load("Sprites/atlas");
            tileType = type;
            SetType((TileType)(int)type);


            //Set rigidbody behaivior here
            rigidbody = BodyFactory.CreateRectangle(Game1.world, ConvertUnits.ToSimUnits(rect.Width), ConvertUnits.ToSimUnits(rect.Height), 1.0f, ConvertUnits.ToSimUnits(position)); //default 1:64 ratio 1 meter = 64 pixel
            rigidbody.BodyType = BodyType.Kinematic;
            rigidbody.UserData = (string)"Tile";
            rigidbody.Restitution = 0f; // No bounciness
            rigidbody.Friction = 1f;
            rigidbody.CollisionCategories = Category.Cat4; // <- cat4 is floor cat
        }

        public Rectangle TileBoundingBox
        {
            get
            {
                return new Rectangle(
                 (int)(position.X),
                 (int)(position.Y),
                    rect.Width,
                    rect.Height);
            }
        }

        private void SetType(TileType type)
        {
            switch(type)
            {
                case (TileType)0:
                    rect = new Rectangle(448, 32, 32, 32);
                    layer = 1;
                    break;
                case (TileType)1:
                    rect = new Rectangle(480, 32, 32, 32);
                    layer = 1;
                    break;
                case (TileType)2:
                    rect = new Rectangle(512, 32, 32, 32);
                    layer = 1;
                    break;
                case (TileType)3:
                    rect = new Rectangle(544, 32, 32, 32);
                    layer = 1;
                    break;
                case (TileType)4:
                    rect = new Rectangle(448, 64, 32, 32);
                    layer = 1;
                    break;
                case (TileType)5:
                    rect = new Rectangle(480, 64, 32, 32);
                    layer = 1;
                    break;
                case (TileType)6:
                    rect = new Rectangle(512, 64, 32, 32);
                    layer = 1;
                    break;
                case (TileType)7:
                    rect = new Rectangle(544, 64, 32, 32);
                    layer = 1;
                    break;
                case (TileType)8:
                    rect = new Rectangle(448, 96, 32, 32);
                    layer = 1;
                    break;
                case (TileType)9:
                    rect = new Rectangle(480, 96, 32, 32);
                    layer = 1;
                    break;
                case (TileType)10:
                    rect = new Rectangle(512, 96, 32, 32);
                    layer = 1;
                    break;
                case (TileType)11:
                    rect = new Rectangle(544, 96, 32, 32);
                    layer = 1;
                    break;
                case (TileType)12:
                    rect = new Rectangle(448, 128, 32, 32);
                    layer = 1;
                    break;
                case (TileType)13:
                    rect = new Rectangle(480, 160, 32, 32); //y 128 blu //y 160 red
                    layer = 1;
                    break;
                case (TileType)14:
                    rect = new Rectangle(512, 128, 32, 32);
                    layer = 1;
                    break;
                case (TileType)15:
                    rect = new Rectangle(544, 128, 32, 32);
                    layer = 1;
                    break;
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(rigidbody.Position), rect, 
                Color.White, rigidbody.Rotation, new Vector2(rect.Width / 2.0f, rect.Height / 2.0f), scale, SpriteEffects.None, 1f);
            //base.Draw(spriteBatch);
        }

    }
}

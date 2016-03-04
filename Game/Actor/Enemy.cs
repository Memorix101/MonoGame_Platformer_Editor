using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;


namespace RogueLikePlatfromer
{
    class Enemy : Sprite
    {
        enum FaceDir
        {
            left,
            right,
        }
        //Vector2 p_pos;
        public Body rigidbody;

        //    SoundEffect sn_kill_enemy;

        FaceDir faceDir;

        bool dirChange;
        bool killed;
        bool bite;

        float walkSpeed = 1f;

        float distance = 0.5f;
        float distanceUp = 0.35f;

        float speed = 5f;

        float time;
        float rectFrame = 1;

        public Enemy(Vector2 p)
        {
            Load("Sprites/actor");
            position = p;
            rect = new Rectangle(224, 128, 32, 32);

            // sn_kill_enemy = Setup.ContentDevice.Load<SoundEffect>("Sounds/Randomize3");

            killed = false;
            bite = false;
            faceDir = FaceDir.right;

            //Set rigidbody behaivior here
            // rigidbody = BodyFactory.CreateRectangle(Game1.world, ConvertUnits.ToSimUnits(rect.Width), ConvertUnits.ToSimUnits(rect.Height), 1.0f, ConvertUnits.ToSimUnits(this.Position));
            rigidbody = BodyFactory.CreateCircle(Game1.world, ConvertUnits.ToSimUnits(rect.Width / 2), 1.0f, ConvertUnits.ToSimUnits(position));
            rigidbody.BodyType = BodyType.Dynamic;
            rigidbody.FixedRotation = true;
            rigidbody.Restitution = 0f; // No bounciness
            rigidbody.Friction = 0f;
            rigidbody.CollisionCategories = Category.Cat3; // cat3 is coins

            rigidbody.OnCollision += Rigidbody_OnCollision;
        }

        public Body GetRigidbody
        {
            get { return rigidbody; }
        }

        public bool IsKilled
        {
            private set { killed = value; }
            get { return killed; }
        }

        public bool Bite
        {
            private set { bite = value; }
            get { return bite; }
        }

        public Rectangle TileBoundingBox
        {
            get
            {
                return new Rectangle(
                 (int)ConvertUnits.ToDisplayUnits(rigidbody.Position.X),
                 (int)ConvertUnits.ToDisplayUnits(rigidbody.Position.Y),
                    rect.Width,
                    rect.Height);
            }
        }

        private bool Rigidbody_OnCollision(Fixture me, Fixture other, Contact contact)
        {
            //  throw new NotImplementedException();

            if (other.CollisionCategories == Category.Cat2)
            {
                // me.IgnoreCollisionWith(other);
                bite = true;
            }

            return true;
        }

        void RayUp()
        {
            IsKilled = false;
            Func<Fixture, Vector2, Vector2, float, float> get_first_callback = delegate (Fixture fixture, Vector2 point, Vector2 normal, float fraction)
            {

                if (fixture.CollisionCategories == Category.Cat2)
                {
                    IsKilled = true;
                        // sn_kill_enemy.Play();
                    }

                return 0;
            };

            Game1.world.RayCast(get_first_callback, rigidbody.Position, rigidbody.Position + new Vector2(-distanceUp / 2, -distanceUp));
            Game1.world.RayCast(get_first_callback, rigidbody.Position, rigidbody.Position + new Vector2(0, -distanceUp));
            Game1.world.RayCast(get_first_callback, rigidbody.Position, rigidbody.Position + new Vector2(distanceUp / 2, -distanceUp));
        }

        void RaySide()
        {
            dirChange = false;
            Func<Fixture, Vector2, Vector2, float, float> get_first_callback = delegate (Fixture fixture, Vector2 point, Vector2 normal, float fraction)
            {
                if (fixture.CollisionCategories == Category.Cat4)
                {
                    dirChange = true;
                }

                return 0;
            };

            if (faceDir == FaceDir.right)
                Game1.world.RayCast(get_first_callback, rigidbody.Position, rigidbody.Position + new Vector2(distance, 0));
            if (faceDir == FaceDir.left)
                Game1.world.RayCast(get_first_callback, rigidbody.Position, rigidbody.Position + new Vector2(-distance, 0));
        }


        public void Update(GameTime gameTime)
        {
            if (!IsKilled)
            {
                rigidbody.LinearVelocity = new Vector2(walkSpeed, rigidbody.LinearVelocity.Y);

                RaySide();
                RayUp();

                if (dirChange)
                {
                    walkSpeed *= -1f;
                    dirChange = false;

                    if (faceDir == FaceDir.left)
                    {
                        faceDir = FaceDir.right;
                    }
                    else if (faceDir == FaceDir.right)
                    {
                        faceDir = FaceDir.left;
                    }

                }
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Draw(texture, Position, coinRect, Color.White);
            if (faceDir == FaceDir.left)
                spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(rigidbody.Position), rect, Color.White, rigidbody.Rotation, new Vector2(rect.Width / 2.0f, rect.Height / 2.0f), 1f, SpriteEffects.None, 1f);
            else if (faceDir == FaceDir.right)
                spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(rigidbody.Position), rect, Color.White, rigidbody.Rotation, new Vector2(rect.Width / 2.0f, rect.Height / 2.0f), 1f, SpriteEffects.FlipHorizontally, 1f);
            //base.Draw(spriteBatch);
            /*

#if DEBUG
            SpriteBatchEx.GraphicsDevice = Game1.graphics.GraphicsDevice;

            spriteBatch.DrawLine(ConvertUnits.ToDisplayUnits(rigidbody.Position), ConvertUnits.ToDisplayUnits(rigidbody.Position + new Vector2(distanceUp / 2, -distanceUp)), Color.Yellow);
            spriteBatch.DrawLine(ConvertUnits.ToDisplayUnits(rigidbody.Position), ConvertUnits.ToDisplayUnits(rigidbody.Position + new Vector2(0, -distanceUp)), Color.Yellow);
            spriteBatch.DrawLine(ConvertUnits.ToDisplayUnits(rigidbody.Position), ConvertUnits.ToDisplayUnits(rigidbody.Position + new Vector2(-distanceUp / 2, -distanceUp)), Color.Yellow);

            if (faceDir == FaceDir.right)
                spriteBatch.DrawLine(ConvertUnits.ToDisplayUnits(rigidbody.Position), ConvertUnits.ToDisplayUnits(rigidbody.Position + new Vector2(distance, 0)), Color.BlueViolet);
            else if (faceDir == FaceDir.left)
                spriteBatch.DrawLine(ConvertUnits.ToDisplayUnits(rigidbody.Position), ConvertUnits.ToDisplayUnits(rigidbody.Position + new Vector2(-distance, 0)), Color.BlueViolet);
#endif
            */
        }
    }
}

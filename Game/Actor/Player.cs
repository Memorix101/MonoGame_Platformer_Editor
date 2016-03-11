using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;

namespace RogueLikePlatfromer
{
  
    enum AnimationState
    {
        Walk,
        Jump,
        Idle,
        Fall,
    }

    class Player : Sprite
    {
        const float jumpForce = -50f;
        const float moveSpeed = 2f;
        bool grounded;

        bool buttonPressed;

        AnimationState aniState;
        AnimationState lastAniState;

        float distance = 0.3f;

        Vector2 initPos;
        
        Animator animator;
       public  Body rigidbody;

        List<Rectangle> walkClips = new List<Rectangle>();
        List<Rectangle> idleClips = new List<Rectangle>();
        List<Rectangle> jumpClips = new List<Rectangle>();
        List<Rectangle> fallClips = new List<Rectangle>();

        public Player(Vector2 pos)
        {
            initPos = pos;
            position = pos;
            rect = new Rectangle(32, 32, 32, 32);
            Load("Sprites/actor");

            // Setup physics
            rigidbody = BodyFactory.CreateCircle(Game1.world, ConvertUnits.ToSimUnits(rect.Width / 2), 1f, ConvertUnits.ToSimUnits(this.position));
            //Set rigidbody behaivior here
            rigidbody.SleepingAllowed = false;
            rigidbody.BodyType = BodyType.Dynamic;
            rigidbody.UserData = (string)"Player";
            rigidbody.FixedRotation = true;
            rigidbody.Restitution = 0f; // No bounciness
            rigidbody.Friction = 1f;
            rigidbody.CollisionCategories = Category.Cat2; // cat2 player
            rigidbody.CollidesWith = Category.All;

            rigidbody.OnCollision += Rigidbody_OnCollision; // Tack collision

            animator = new Animator();

            idleClips.Add(rect = new Rectangle(32, 224, 32, 32));
            idleClips.Add(rect = new Rectangle(64, 224, 32, 32));
            idleClips.Add(rect = new Rectangle(96, 224, 32, 32));
           // idleClips.Add(rect = new Rectangle(128,224, 32, 32));

            walkClips.Add(rect = new Rectangle(32, 288, 32, 32));
            walkClips.Add(rect = new Rectangle(64, 288, 32, 32));
            walkClips.Add(rect = new Rectangle(96, 288, 32, 32));

            jumpClips.Add(rect = new Rectangle(32, 384, 32, 32));
          //  jumpClips.Add(rect = new Rectangle(64, 384, 32, 32));

            fallClips.Add(rect = new Rectangle(64, 384, 32, 32));
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

        private bool Rigidbody_OnCollision(Fixture me, Fixture other, Contact contact)
        {
            //  throw new NotImplementedException();
            return true;
        }

        void Raycast()
        {
            grounded = false;

            Func<Fixture, Vector2, Vector2, float, float> get_first_callback = delegate (Fixture fixture, Vector2 point, Vector2 normal, float fraction)
            {
                grounded = true;
                return 0;
            };

            Game1.world.RayCast(get_first_callback, rigidbody.Position, rigidbody.Position + new Vector2(0, distance)); //* (Math.Abs(rigidbody.LinearVelocity.Y) + 1)
        }
                
        public void Update(GameTime gameTime)
        {
            var pos =  MathHelper.Clamp(rigidbody.Position.X, 0, ConvertUnits.ToSimUnits(Screen.width - rect.Width));
            rigidbody.Position = new Vector2(pos, rigidbody.Position.Y);

            buttonPressed = false;

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                rigidbody.Position = new Vector2(rigidbody.Position.X + moveSpeed * gameTime.DeltaTime(), rigidbody.Position.Y);

                buttonPressed = true;

                flip = SpriteEffects.None;

                if (grounded && aniState != AnimationState.Jump)
                {
                    aniState = AnimationState.Walk;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                rigidbody.Position = new Vector2(rigidbody.Position.X - moveSpeed * gameTime.DeltaTime(), rigidbody.Position.Y);

                buttonPressed = true;

                flip = SpriteEffects.FlipHorizontally;

                if (grounded && aniState != AnimationState.Jump)
                {
                    aniState = AnimationState.Walk;
                }
            }

            if (ControlInput.IsKeyPressed(Keys.Space) && grounded && aniState != AnimationState.Jump)
            {
                rigidbody.ApplyForce(new Vector2(0f, jumpForce));
                    aniState = AnimationState.Jump;
            }

            if (ControlInput.IsKeyPressed(Keys.R))
            {
               rigidbody.Position = ConvertUnits.ToSimUnits(initPos);
                rigidbody.ResetDynamics();
            }

            Raycast();

            if (!grounded && aniState != AnimationState.Jump)
            {
                aniState = AnimationState.Fall;
            }
            else if (!grounded && rigidbody.LinearVelocity.Y > 0)
            {
                aniState = AnimationState.Fall;
            }
            else if (grounded && !buttonPressed && aniState != AnimationState.Jump)
            {
                aniState = AnimationState.Idle;
            }
            else if (grounded && aniState == AnimationState.Jump && !ControlInput.IsKeyPressed(Keys.Space))
            {
                aniState = AnimationState.Idle;
            }

            // Console.WriteLine(grounded + " / " + aniState);

            //Console.WriteLine(rigidbody.LinearVelocity.Y);

            setClip();        
            animator.Frames(gameTime);
            animator.Update(gameTime);
            rect = animator.UpdateRect;
        }

        void setClip()
        {
            if (aniState == AnimationState.Walk)
            {
                animator.Set(8, walkClips, true);
            }
            else if (aniState == AnimationState.Idle)
            {
                animator.Set(6, idleClips, true);
            }
            else if (aniState == AnimationState.Jump)
            {
                animator.Set(4f, jumpClips, false);
            }
            else if (aniState == AnimationState.Fall)
            {
                animator.Set(2, fallClips, false);
            }

            if (lastAniState != aniState)
                animator.time = 0;

            lastAniState = aniState;
        }

        void CameraBounds()
        {
            float _pos = rigidbody.Position.X;
            float _posMax = ConvertUnits.ToSimUnits(Game1.camera.offsetRight - rect.Width);
            float _posDown = ConvertUnits.ToSimUnits(Screen.height - rect.Width);

            _pos = MathHelper.Clamp(rigidbody.Position.X, 0, rigidbody.Position.X);
            rigidbody.Position = new Vector2(_pos, rigidbody.Position.Y);

            if (rigidbody.Position.X >= _posMax)
            {
                Game1.camera.UpdatePos(ConvertUnits.ToDisplayUnits(rigidbody.Position.X));
                Game1.DebugCam.UpdatePos();
            }

            if (rigidbody.Position.Y >= _posDown)
            {
              //  ReceiveDamage();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(rigidbody.Position), rect, Color.White, 0f,
                       new Vector2(rect.Width / 2.0f, rect.Height / 2.0f), 1f, flip, layer);
            //base.Draw(spriteBatch);
        }


    }
}

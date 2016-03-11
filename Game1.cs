using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FarseerPhysics.Dynamics;
using FarseerPhysics;

using System;

namespace RogueLikePlatfromer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        public static World world { get; private set; }
        //public static DebugViewXNA DebugView { get; private set; }
        public static PhysicsDebugCam DebugCam { get; private set; }
        public static Camera2D camera { get; private set; }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Editor editor;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Setup.Init(Content, graphics);
            Screen.resolution(800, 600);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ControlInput.keyState = Keyboard.GetState();
            camera = new Camera2D();
            DebugCam = new PhysicsDebugCam();
            PhysicLoad();

            editor = new Editor();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            //  if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            ControlInput.Update(Keyboard.GetState());

            Menu.Update();
            editor.Update();

            if (Menu.gameState == GameState.Play)
            {
                world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
                camera.Update();

                Scene.Update(gameTime);
            }


            base.Update(gameTime);
        }

        private void PhysicLoad()
        {
            if (world == null)
            { 
            
                // Farseer expects objects to be scaled to MKS (meters, kilos, seconds)
                ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);  // 1 meters equals 64 pixels here

                //Create a world with gravity.
                world = new World(new Vector2(0, 9.82f));
            }
            else
            {
                world.Clear();
            }
        }

        //void DebugDraw()
        //{
        //    Matrix view2 = Matrix.CreateScale(32); //default 32
        //    view2 *= Game1.DebugCam.view;

        //   // debugActive = true;

        //    DebugView = new DebugViewXNA(Game1.world);
        // //   DebugView.AppendFlags(FarseerPhysics.DebugViewFlags.DebugPanel);
        //    DebugView.DefaultShapeColor = Color.Red;
        //    DebugView.SleepingShapeColor = Color.Green;
        //    DebugView.StaticShapeColor = Color.Violet;
        //    DebugView.LoadContent(Setup.graphics.GraphicsDevice, Setup.ContentDevice);
        //    DebugView.RenderDebugData(ref Game1.DebugCam.projection, ref view2);
        //}


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.TransparentBlack);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            //Scene.Sky(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.view);
            Scene.Draw(spriteBatch);
            editor.Draw(spriteBatch);

            spriteBatch.End();


//#if DEBUG
//            DebugDraw();
//#endif

            //UI & HUD stuff here
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            Menu.Draw(spriteBatch);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}

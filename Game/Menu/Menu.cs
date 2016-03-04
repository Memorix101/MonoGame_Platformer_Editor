using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RogueLikePlatfromer
{
    enum MenuState
    {
        Play,
        Editor,
        Quit
    }

    enum GameState
    {
        Play,
        Pause,
    }

    enum MenuLayer
    {
        Menu,
        Game,
        Editor,
        Tiles,
    }

    static class Menu
    {

        public static bool enabled = true;

        public static GameState gameState;

        static bool pressed = false;

        static Texture2D editor_title;
        static Texture2D title;
        static Texture2D background;

        static GUI copyright;
        static GUI pauseText;

        static List<GUI> MenuItems;
        static int currentItem = 0;

        static MenuLayer lastMenuLayer;
        static MenuState menuState;
        public static MenuLayer menuLayer;

        static Menu()
        {
            menuLayer = MenuLayer.Menu;
            title = Setup.ContentDevice.Load<Texture2D>("Sprites/title");
            background = Setup.ContentDevice.Load<Texture2D>("Sprites/area04_bkg0");
            editor_title = Setup.ContentDevice.Load<Texture2D>("Sprites/editor");


            copyright = new GUI("A Game by Memorix101");
            copyright.Load("Fonts\\Pixel_12");
            copyright.Position = new Vector2(Screen.width / 2 - copyright.Size.X / 2, Screen.height - copyright.Size.Y-5);
            copyright.Color = Color.White;

            pauseText = new GUI("PAUSE");
            pauseText.Load("Fonts\\Pixel_20");
            pauseText.Position = new Vector2(Screen.width / 2 - copyright.Size.X/2 + 50 , Screen.height/2 - copyright.Size.Y);
            pauseText.Color = Color.OrangeRed;


            MenuItems = new List<GUI>();
            Layer();
        }

        public static void Update()
        {

            if (ControlInput.IsKeyPressed(Keys.Enter) && menuLayer == MenuLayer.Tiles && menuLayer == lastMenuLayer)
            {
                menuLayer = MenuLayer.Editor;
            }

            if (enabled)
            {
                Input();
                Layer();
                Selection();
            }

            if (ControlInput.IsKeyPressed(Keys.Escape) && menuLayer == MenuLayer.Game)
            {
                enabled = !enabled;

                if (enabled)
                    gameState = GameState.Pause;
                else if(!enabled)
                    gameState = GameState.Play;
            }

            if (ControlInput.IsKeyPressed(Keys.Escape) && menuLayer == MenuLayer.Editor || ControlInput.IsKeyPressed(Keys.Escape) && menuLayer == MenuLayer.Tiles)
            {
                menuLayer = MenuLayer.Editor;
                enabled = !enabled;
            }
            
        }

        private static void Selection()
        {
            foreach (GUI m in MenuItems)
            {
                m.Color = Color.White;
            }

            MenuItems[currentItem].Color = Color.Orange;
        }

        static void ItemSelect()
        {
            if (menuLayer == MenuLayer.Menu)
            {
                switch (currentItem)
                {
                    case 0:
                        LevelIO.Load();
                        menuLayer = MenuLayer.Game;
                        Menu.gameState = GameState.Play;
                        enabled = false;
                        break;
                    case 1:
                        menuLayer = MenuLayer.Editor;
                        Menu.gameState = GameState.Pause;
                        Scene.Dispose();
                        break;
                    case 2:
                        System.Environment.Exit(1);
                        break;
                }
            }
            else if (menuLayer == MenuLayer.Editor)
            {
                switch (currentItem)
                {
                    case 0:
                        menuLayer = MenuLayer.Tiles;
                        enabled = false;
                        break;
                    case 1:
                        LevelIO.Load();
                        break;

                    case 2:
                        LevelIO.Save();
                        break;

                    case 3:
                        Scene.Dispose();
                        break;

                    case 4:
                        menuLayer = MenuLayer.Menu;
                        Scene.Dispose();
                        break;
                }
            }
           else if (menuLayer == MenuLayer.Game)
            {
                switch (currentItem)
                {
                    case 0:
                        enabled = false;
                        gameState = GameState.Play;
                        break;
                    case 1:
                        Scene.Dispose();
                        menuLayer = MenuLayer.Menu;
                        gameState = GameState.Play;
                        break;
                }
            }
        }

        private static void Layer()
        {
            MenuItems.Clear();

            if (menuLayer == MenuLayer.Menu)
            {
                MenuItems.Add(new GUI("Play"));
                MenuItems.Add(new GUI("Editor"));
                MenuItems.Add(new GUI("Quit"));
            }
            else if (menuLayer == MenuLayer.Editor)
            {
                MenuItems.Add(new GUI("Tiles"));
                MenuItems.Add(new GUI("Load"));
                MenuItems.Add(new GUI("Save"));
                MenuItems.Add(new GUI("Clear Map"));
                MenuItems.Add(new GUI("Back"));
            }
            else if (menuLayer == MenuLayer.Game)
            {
                MenuItems.Add(new GUI("Resume"));
                MenuItems.Add(new GUI("Back To Menu"));
            }
            else if (menuLayer == MenuLayer.Tiles)
            {
                MenuItems.Add(new GUI("..."));
            }

            currentItem = MathHelper.Clamp(currentItem, (int)0, (int)MenuItems.Count - 1);

            for (int i = 0; i < MenuItems.Count; i++)
            {
                MenuItems[i].Load("Fonts\\70sPixel_20");
                MenuItems[i].Position = new Vector2(Screen.width / 2 - MenuItems[i].Size.X / 2, 50 * i + Screen.height / 2 + 50);
            }

        }

        private static void Input()
        {
  
            if (ControlInput.IsKeyPressed(Keys.Up))
            {
                currentItem--;
            }

            else if (ControlInput.IsKeyPressed(Keys.Down))
            {
                currentItem++;
            }

            else if (ControlInput.IsKeyPressed(Keys.Enter) && menuLayer == lastMenuLayer)
            {
                ItemSelect();
            }

            if(lastMenuLayer != menuLayer)
                currentItem = 0;

            lastMenuLayer = menuLayer;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {

            Rectangle screenRectangle = new Rectangle(0, 0, Screen.width, Screen.height);

            if (enabled)
            {
                spriteBatch.Draw(background, screenRectangle, Color.White * 0.75f);

                if (menuLayer == MenuLayer.Menu)
                {
                    //     spriteBatch.Draw(background, screenRectangle, Color.White);
                    spriteBatch.Draw(title, new Vector2(Screen.width / 2 - title.Width / 2, Screen.height / 2 - title.Height), Color.White);
                    copyright.Draw(spriteBatch);
                }

                if (menuLayer == MenuLayer.Game)
                {
                    pauseText.Draw(spriteBatch);
                }

                if (menuLayer == MenuLayer.Editor)
                    spriteBatch.Draw(editor_title, new Vector2(Screen.width / 2 - title.Width / 2, Screen.height / 2 - title.Height), Color.White);
            

                foreach (GUI m in MenuItems)
                {
                    m.Draw(spriteBatch);
                }
            }
        }

    }
}

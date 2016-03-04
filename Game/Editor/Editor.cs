using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;

namespace RogueLikePlatfromer
{

    class Editor : Sprite
    {

        List<Tile> browserTiles = new List<Tile>();

        Texture2D prevHud;

        Texture2D browserCursor;
        Vector2 bCursorPos;
        Vector2 normalbCursorPos;

        private int currentTile;
        private int cursorOffseth = 80;
        private int move = 32;

        bool buttonUpLocked;
        bool buttonDownLocked;
        bool buttonLeftLocked;
        bool buttonRightLocked;

        bool canPlant;

        static Texture2D background;

        static GUI browserTitle;
        static GUI browserInstr;

        public Editor()
        {         
            currentTile = 0;
            position = new Vector2(0, 32*12);
            rect = new Rectangle(0, 0, 32, 32);
            Load("Sprites/editor_cursor");
            // color = Color.Red;

            background = Setup.ContentDevice.Load<Texture2D>("Sprites/area04_bkg0");

            ///
            TileBrowser();
            prevHud = Setup.ContentDevice.Load<Texture2D>("Sprites/preview_hud");

            browserCursor = Setup.ContentDevice.Load<Texture2D>("Sprites/tile_cursor");
            bCursorPos = new Vector2(Screen.width / 2 - browserCursor.Width - 125, Screen.height / 2 - browserCursor.Height / 2 - 144);
            normalbCursorPos = bCursorPos;

            ///
            browserTitle = new GUI("Tile Browser");
            browserTitle.Load("Fonts\\70sPixel_20");
            browserTitle.Position = new Vector2(Screen.width / 2 - browserTitle.Size.X / 2, Screen.height/2 - browserTitle.Size.Y - 200);
            browserTitle.Color = Color.White;

            browserInstr = new GUI("ENTER TO SELECT   ESCAPE TO GO BACK");
            browserInstr.Load("Fonts\\Pixel_20");
            browserInstr.Position = new Vector2(Screen.width / 2 - browserInstr.Size.X/2, Screen.height  - browserInstr.Size.Y - 35);
            browserInstr.Color = Color.White;
        }

        public void Update()
        {
            Input();
        }

        private void Input()
        {
            if (Menu.menuLayer == MenuLayer.Tiles && !Menu.enabled)
            {
         
                    if (ControlInput.IsKeyPressed(Keys.Up) && !buttonUpLocked)
                    {
                        currentTile -= 4;
                        bCursorPos = new Vector2(bCursorPos.X, bCursorPos.Y - cursorOffseth);
                    }
                    else if (ControlInput.IsKeyPressed(Keys.Down) && !buttonDownLocked)
                    {
                        currentTile += 4;
                        bCursorPos = new Vector2(bCursorPos.X, bCursorPos.Y + cursorOffseth);
                    }
                    else if (ControlInput.IsKeyPressed(Keys.Right) && !buttonRightLocked)
                    {
                        currentTile += 1;
                        bCursorPos = new Vector2(bCursorPos.X + cursorOffseth, bCursorPos.Y);
                    }
                    else if (ControlInput.IsKeyPressed(Keys.Left) && !buttonLeftLocked)
                    {
                        currentTile -= 1;
                        bCursorPos = new Vector2(bCursorPos.X - cursorOffseth, bCursorPos.Y);
                    }

                currentTile = MathHelper.Clamp(currentTile, 0, 16);

                //Console.WriteLine(currentTile);

                // guide cursor row
                bCursorPos.X = MathHelper.Clamp(bCursorPos.X, normalbCursorPos.X, 483); //483
                bCursorPos.Y = MathHelper.Clamp(bCursorPos.Y, normalbCursorPos.Y, 380); //460

                // ugly code in coming !!!!! AAAAAAAAAAAAHHHH 

                Console.WriteLine(bCursorPos);

                if (bCursorPos.X == normalbCursorPos.X)
                {
                    buttonLeftLocked = true;
                }
                else if(bCursorPos.X == 483) //483
                {
                    buttonRightLocked = true;
                }
                else 
                    buttonRightLocked = buttonLeftLocked = false;

                if (bCursorPos.Y == normalbCursorPos.Y)
                {
                    buttonUpLocked = true;
                }
                else if( bCursorPos.Y == 380) //460
                {
                    buttonDownLocked = true;
                }
                else
                    buttonUpLocked = buttonDownLocked = false;


            }

            ///

            if (Menu.menuLayer == MenuLayer.Editor && !Menu.enabled)
            {
                if (ControlInput.IsKeyPressed(Keys.Left))
                {
                    position.X -= move;
                }

                else if (ControlInput.IsKeyPressed(Keys.Right))
                {
                    position.X += move;
                }
                else if (ControlInput.IsKeyPressed(Keys.Up))
                {
                    position.Y -= move;
                }
                else if (ControlInput.IsKeyPressed(Keys.Down))
                {
                    position.Y += move;
                }
                else if (ControlInput.IsKeyPressed(Keys.Space) && canPlant)
                {
                    if ((TileType)currentTile == TileType.EnemyBlu)
                        Scene.ActorObjects.Add(new Enemy(position));
                    else if ((TileType)currentTile == TileType.PlayerSpawn)
                        Scene.PlayerList.Add(new Player(position));
                    else
                        Scene.levelObjects.Add(new Tile((TileType)currentTile, position));
                }
                /*
                else if (ControlInput.IsKeyPressed(Keys.F1))
                {
                    LevelIO.Save();
                }
                else if (ControlInput.IsKeyPressed(Keys.F2))
                {
                    LevelIO.Load();
                }
                */
                else if (ControlInput.IsKeyPressed(Keys.Delete))
                {
                    for (int i = 0; i < Scene.levelObjects.Count; i++)
                    {
                        if (Scene.levelObjects[i].position.Equals(this.position))
                        {
                            Scene.levelObjects[i].rigidbody.Dispose();
                            Scene.levelObjects.Remove(Scene.levelObjects[i]);
                        }
                    }

                    for (int i = 0; i < Scene.ActorObjects.Count; i++)
                    {
                        if (Scene.ActorObjects[i].position.Equals(this.position))
                        {
                            Scene.ActorObjects[i].rigidbody.Dispose();
                            Scene.ActorObjects.Remove(Scene.ActorObjects[i]);
                        }
                    }

                    for (int i = 0; i < Scene.PlayerList.Count; i++)
                    {
                        if (Scene.PlayerList[i].position.Equals(this.position))
                        {
                            Scene.PlayerList[i].rigidbody.Dispose();
                            Scene.PlayerList.Remove(Scene.PlayerList[i]);
                        }
                    }
                }

                CheckPosition();

                position.X = MathHelper.Clamp(position.X, 0, position.X);
                position.Y = MathHelper.Clamp(position.Y, 0, position.Y);
            }
        }


        private void CheckPosition()
        {
            if (Scene.levelObjects.Count > 0)
            {
                for (int i = 0; i < Scene.levelObjects.Count; i++)
                {
                    if (Scene.levelObjects[i].position.Equals(this.position))
                    {
                        color = Color.Red;
                        canPlant = false;
                    }
                    else
                    {
                        color = Color.Green;
                        canPlant = true;
                    }

                }
            }
            else
            {
                color = Color.Green;
                canPlant = true;
            }
        }

        private void TileBrowser()
        {
            int c = 0;

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    Tile temp = new Tile((TileType)c, new Vector2(Screen.width/2 - 125 + i * rect.Width + i * rect.Width*1.5f, Screen.height/2 - 128 + j * rect.Height + j * rect.Height * 1.5f));
                    temp.rigidbody.Dispose();
                    temp.scale = 2;

                    c++; // brother's watching ... 

                    browserTiles.Add(temp);
                }
            }            

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Menu.menuLayer == MenuLayer.Editor && !Menu.enabled)
                spriteBatch.Draw(texture, position, rect, color, 0f, new Vector2(rect.Width / 2.0f, rect.Height / 2.0f), scale, flip, 0);

            if (Menu.menuLayer == MenuLayer.Tiles && !Menu.enabled)
            {
                Rectangle screenRectangle = new Rectangle(0, 0, Screen.width, Screen.height);
                spriteBatch.Draw(background, screenRectangle, null, Color.White * 0.75f, 0f, new Vector2(13, 13), SpriteEffects.None, 1f);

                browserTitle.Draw(spriteBatch);
                browserInstr.Draw(spriteBatch);

                foreach (var t in browserTiles)
                    t.Draw(spriteBatch);

                spriteBatch.Draw(browserCursor, bCursorPos, null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            }


            if (Menu.menuLayer == MenuLayer.Editor && !Menu.enabled || Menu.menuLayer == MenuLayer.Tiles && !Menu.enabled)
            {
                Vector2 prevHudPos = new Vector2(Screen.width - prevHud.Width - 79, 15);

                Tile temp = new Tile((TileType)currentTile, new Vector2(Screen.width - texture.Width - 47, 47));
                temp.rigidbody.Dispose();
                temp.scale = 2;
                temp.Draw(spriteBatch);

                spriteBatch.Draw(prevHud, prevHudPos, null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
            }
            
            //base.Draw(spriteBatch);
        }

    }
}

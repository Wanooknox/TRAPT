using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using xTile;
using xTile.Dimensions;
using xTile.Display;
using xTile.Layers;
using xTile.Tiles;
using System.Diagnostics;

namespace TestTile_Collisions
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        Texture2D player;
        Vector2 playerPosition;
        Microsoft.Xna.Framework.Rectangle playerDestination;

        int screenWidth, screenHeight;

        int mapWidth, mapHeight;
        int layerWidth, layerHeight;
        
        //xTile stuff
        Map map;
        IDisplayDevice mapDisplayDevice;
        xTile.Dimensions.Rectangle viewport;
        Layer collisionLayer;
        Location location;
        xTile.Tiles.TileArray tileArray;
        Tile tile;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            mapDisplayDevice = new XnaDisplayDevice(Content, GraphicsDevice);
            map.LoadTileSheets(mapDisplayDevice);
            viewport = new xTile.Dimensions.Rectangle(new Size(800, 600));

            //get the map width and height
            mapWidth = map.DisplayWidth;
            mapHeight = map.DisplayHeight;


            Console.WriteLine("Map width: " + mapWidth + ".. Map height: " + mapHeight);
            //6400 by 1600

            //load the layer
            collisionLayer = map.Layers[0];
            //load the tiles into an array
            tileArray = collisionLayer.Tiles;

            //check if we go over a tile with passable = false property
           

           // simpleCollisionDetection();


            int layerWidth = collisionLayer.LayerWidth;//(collisionLayer.LayerWidth / collisionLayer.TileWidth) + 1;
            int layerHeight = collisionLayer.LayerHeight;//(collisionLayer.LayerHeight / collisionLayer.TileHeight) + 1;

            Console.WriteLine("Layer width: " + layerWidth + ".. Layer height: " + layerHeight);


            for (int i = 0; i < layerWidth; i++)
            {
                for (int j = 0; j < layerHeight; j++)
                {
                    if (collisionLayer.Tiles[i, j] != null)
                    {
                        try
                        {
                            bool passable = collisionLayer.Tiles[i, j].Properties["passable"];
                            Console.WriteLine("Is [" + i * 64 + ", " + j * 64 + "] passable:" + passable);
                        }
                        catch (KeyNotFoundException e)
                        {
                            //do nothing
                        }
                    }
                }
            }

           // tile = collisionLayer.GetTileLocation(new Location(0, 0));

            //bool passable = collisionLayer.Tiles[0, 0].Properties["passable"];
            //Console.WriteLine("Passable property for tile at location [0,0] should = false and is " + passable);


            

            //collision = map.Layers[0];
            //foreach (Layer collision in map.Layers)
            //{
                //collision = map.GetLayer("maps").;
                //TileArray tiles = collision.Tiles;
                //Tile t = tiles[0, 0];
                //int i = 0;
                //Console.WriteLine("Trying...");
                //foreach (String key in t.Properties.Keys)
                //{
                //    Console.Write(i);
                //    Console.WriteLine(key);
                //    i++;
                //}
            //}            
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;

            player = Content.Load<Texture2D>("blueGuard_walk_middle");

            playerPosition = new Vector2(screenWidth / 2, screenHeight / 2);

            playerDestination = new Microsoft.Xna.Framework.Rectangle(
                (int)playerPosition.X - player.Width / 2,
                (int)playerPosition.Y - player.Height / 2, 64, 64);
                   
            map = Content.Load<Map>("Maps\\TestMap_passable");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Escape)) this.Exit();

            Vector2 newPos = playerPosition;
            Console.WriteLine("Player newPos.X= " + newPos.X + "    Player newPos.Y=" + newPos.Y);

            //Move the player
            if (ks.IsKeyDown(Keys.Up))
            {
                playerPosition.Y -= 4.0f;
            }
            if (ks.IsKeyDown(Keys.Down))
            {
                playerPosition.Y += 4.0f;
            }
            if (ks.IsKeyDown(Keys.Left))
            {
                playerPosition.X -= 4.0f;
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                playerPosition.X += 4.0f;
            }



            //if ((playerPosition.X - screenWidth / 2) < 0)
            //{
            //    Console.WriteLine("Hitting wall");
            //    playerDestination.X = (int)playerPosition.X - (int)(playerDestination.Width / 2);
            //}
            
            //else if(mapWidth < (playerPosition.X + screenWidth/2))
            //    playerDestination.X = (int)playerPosition.X - (int)(playerDestination.Width / 2);

            //if (playerPosition.Y < 0)
            //    playerCollisionBox.Y = (int)playerPosition.Y - (int)(playerCollisionBox.Height / 2);
            //else if (playerPosition.Y > mapHeight)
            //    playerCollisionBox.Y = (int)playerPosition.Y - (int)(playerCollisionBox.Height / 2);


            
            
            map.Update(gameTime.ElapsedGameTime.Milliseconds);
            viewport.X = (int)playerPosition.X;
            viewport.Y = (int)playerPosition.Y;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            //playerCollisionBox.X = (int)playerPosition.X - (int)(playerCollisionBox.Width  / 2);
            //playerCollisionBox.Y = (int)playerPosition.Y - (int)(playerCollisionBox.Height / 2);                       

            spriteBatch.Draw(player, playerDestination, Color.White);

            //render xTile map
            map.Draw(mapDisplayDevice, viewport);


            spriteBatch.End();

            base.Draw(gameTime);
        }

        public bool simpleCollisionDetection()
        {
            bool result = false;

            //check if we go over a tile with passable = false property
            for (int i = 0; i < layerWidth; i++)
            {
                for (int j = 0; j < layerHeight; j++)
                {
                    if (collisionLayer.Tiles[i, j] != null)
                    {
                        try
                        {
                            bool passable = collisionLayer.Tiles[i, j].Properties["passable"];
                            Console.WriteLine("Is [" + i * 64 + ", " + j * 64 + "] passable:" + passable);
                        }
                        catch (KeyNotFoundException e)
                        {
                            //do nothing
                        }
                    }
                }
            }
           


            return result;
        }

        private bool calculateCollision(Vector2 newPos)
        {
            Tile tile;
            Location tileLocation;

            Debug.WriteLine("cx: " + playerDestination.X + " cy: " + playerDestination.Y + "\n");

            tileLocation = new Location((int)(newPos.X - playerDestination.Width / 2) / 64, ((int)newPos.Y - playerDestination.Height / 2) / 64);
            Console.WriteLine("TileLocation1: "+ tileLocation.ToString());
            tile = collisionLayer.Tiles[tileLocation];
            //if (tile.Properties["passable"] == false)
            if( tile.TileIndex == 1 )
                return true;

            tileLocation = new Location((int)(newPos.X + playerDestination.Width / 2) / 64, ((int)newPos.Y - playerDestination.Height / 2) / 64);
            Console.WriteLine("TileLocation2: "+ tileLocation.ToString());
            tile = collisionLayer.Tiles[tileLocation];
            //if (tile.Properties["passable"] == false)
            if (tile.TileIndex == 1)
                return true;

            tileLocation = new Location((int)(newPos.X + playerDestination.Width / 2) / 64, ((int)newPos.Y + playerDestination.Height / 2) / 64);
            Console.WriteLine("TileLocation3: " + tileLocation.ToString());
            tile = collisionLayer.Tiles[tileLocation];
            //if (tile.Properties["passable"] == false)
            if (tile.TileIndex == 1)
                 return true;

            tileLocation = new Location((int)(newPos.X - playerDestination.Width / 2) / 64, ((int)newPos.Y + playerDestination.Height / 2) / 64);
            Console.WriteLine("TileLocation4: " + tileLocation.ToString());
            tile = collisionLayer.Tiles[tileLocation];
            //if (tile.Properties["passable"] == false )
            if (tile.TileIndex == 1)
                return true;

            return false;
        }
    }
}

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

//xTile engine namespace
using xTile;
using xTile.Dimensions;
using xTile.Display;

namespace Project
{   
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameState
        {
            MainMenu,
            Instructions,
            Playing,
        }
        //Set gamestate
        GameState currentGameState = GameState.MainMenu;

        Button btnPlayTutorial;
        Button btnInstructions;
        int screenWidth = 800, screenHeight = 600;

        //Alien character and cursor
        Alien player;
        Cursor cursor;
        Camera camera;

        //xTile map, display device reference, and rednering viewport
        Map map;
        IDisplayDevice mapDisplayDevice;
        xTile.Dimensions.Rectangle viewport;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            player = new Alien(this);
            cursor = new Cursor(this);
        }

      
        protected override void Initialize()
        {
            base.Initialize();

            //Used for the player object
            Vector2 centerView = new Vector2((graphics.GraphicsDevice.Viewport.Width / 2 - 100), graphics.GraphicsDevice.Viewport.Height / 2);
            
            player.Initalize(centerView, 1f);
            cursor.Initialize();            

            //initialize xTile map display device
            mapDisplayDevice = new XnaDisplayDevice(this.Content, this.GraphicsDevice);
            //initialize xTile map resources
            map.LoadTileSheets(mapDisplayDevice);
            //initialize xTile rendering viewport (hardcoded for now)
            viewport = new xTile.Dimensions.Rectangle(new Size(1024, 720));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            camera = new Camera(GraphicsDevice.Viewport);
                        
            //Screen stuff
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            btnPlayTutorial = new Button(Content.Load<Texture2D>("playTutorial_New"), graphics.GraphicsDevice);
            btnPlayTutorial.setPosition(new Vector2(270, 240));
            btnInstructions = new Button(Content.Load<Texture2D>("instructions_New"), graphics.GraphicsDevice);
            btnInstructions.setPosition(new Vector2(270, 280));

            //load xTile map from content pipeline
            map = Content.Load<Map>("Maps\\testing");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
              
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) this.Exit();

            KeyboardState ks = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (ks.IsKeyDown(Keys.Escape)) this.Exit();

            switch (currentGameState)
            {
                case GameState.MainMenu:
                    if (btnPlayTutorial.isClicked)
                        currentGameState = GameState.Playing;
                    if (btnInstructions.isClicked)
                        currentGameState = GameState.Instructions;

                    btnPlayTutorial.Update(mouse);
                    btnInstructions.Update(mouse);
                    cursor.Update(gameTime);
                    base.Update(gameTime);
                    break;

                case GameState.Instructions:
                    if (mouse.LeftButton == ButtonState.Pressed)
                        currentGameState = GameState.Playing;
                    cursor.Update(gameTime);
                    base.Update(gameTime);
                    break;

                case GameState.Playing:
                    //work out zoom and rotation of camera
                    if (ks.IsKeyDown(Keys.Up))
                        camera.Zoom += 0.01f;
                    if (ks.IsKeyDown(Keys.Down))
                        camera.Zoom -= 0.01f;
                    if (ks.IsKeyDown(Keys.Left))
                        camera.Rotation -= 0.01f;
                    if (ks.IsKeyDown(Keys.Right))
                        camera.Rotation += 0.01f;                    

                    //update xTile map for animations etc. and update viewport for camera movement
                    map.Update(gameTime.ElapsedGameTime.Milliseconds);
                    viewport.X = (int)player.screenPosition.X;
                    viewport.Y = (int)player.screenPosition.Y;
                    player.Update(gameTime);
                    camera.Update(player.alienPosition);
                    cursor.Update(gameTime);
                    base.Update(gameTime);
                    break;
            }

            //base.Update(gameTime);
        }

       
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            //used for the camera zooming
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);            
            spriteBatch.End();
            
            
            spriteBatch.Begin();

            switch (currentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(Content.Load<Texture2D>("MainMenu"), new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight),
                        Color.White);
                    btnPlayTutorial.Draw(this.spriteBatch);  //draw the start button
                    btnInstructions.Draw(this.spriteBatch);  //draw the instructions button
                    cursor.Draw(this.spriteBatch);           //draw the cursor
                    break;

                case GameState.Instructions:
                    spriteBatch.Draw(Content.Load<Texture2D>("GameInstructions"), new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight),
                        Color.White);
                    cursor.Draw(this.spriteBatch);
                    break;

                case GameState.Playing:
                    //render xTile map
                    map.Draw(mapDisplayDevice, viewport);
                    player.Draw(this.spriteBatch);
                    cursor.Draw(this.spriteBatch);
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

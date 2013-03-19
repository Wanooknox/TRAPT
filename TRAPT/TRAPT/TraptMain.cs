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

using Graph;
using TRAPT.Levels;


namespace TRAPT
{
    public enum GameState
    {
        MainMenu,
        Instructions,
        Playing,
        Paused,
        Loading,
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TraptMain : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region GameState
        
        //Set gamestate
        public static GameState currentGameState = GameState.MainMenu;
        public static GameState nextGameState = GameState.MainMenu;

        Button btnPlay, btnInstructions, btnQuit;
        Level lvl;
        public static string nextlvl;
        TimeSpan switchDelay = TimeSpan.Zero;
        private bool loaddrawn = false;

        //Pause stuff
        bool paused = false;
        Texture2D pausedTexture;
        Microsoft.Xna.Framework.Rectangle pausedRectangle;
        Button btnReturn, btnMainMenu;

        #endregion 

        int screenWidth = 800, screenHeight = 600;

        public const int GRID_CELL_SIZE = 128;
        public static TileLayer tileLayer;
        public static XMLObjectReader xmlReader;
        public static UGraphList<Cell> locationTracker;
        public static GameComponentCollection[] layers;
        public static Cursor cursor;
        public static Camera camera;
        public static Player player;
        public static HUD hud;
        public static SpriteFont font;
        public KeyboardState ks, ksold;
        public MouseState ms, msold;
        //public static KeyboardState ks, ksold;


        #region XNA Built In

        public TraptMain()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //create layers
            layers = new GameComponentCollection[3];
            layers[0] = new GameComponentCollection();
            layers[1] = new GameComponentCollection();
            layers[2] = new GameComponentCollection();

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Screen stuff
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            //load the font
            font = Content.Load<SpriteFont>("SpriteFont1");

            //Init camera
            camera = new Camera(this);
            camera.Initialize(GraphicsDevice.Viewport);
            //camera.Limits = new Rectangle(0, 0, tileLayer.mapWidth * GRID_CELL_SIZE, tileLayer.mapHeight * GRID_CELL_SIZE);

            //Init cursor
            cursor = new Cursor(this);
            cursor.Initialize();
            //this.IsMouseVisible = true;

            //map and location tracking
            tileLayer = new TileLayer(this);
            //locationTracker = new UGraphList<Cell>(); // being done in PopulateGraph now 
            xmlReader = new XMLObjectReader(this);

            //this.lvl = new Level1(this);
            hud = new HUD(this);
            hud.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Main Menu Buttons
            btnPlay = new Button(Content.Load<Texture2D>("playTutorial_New"), graphics.GraphicsDevice);
            btnPlay.setPosition(new Vector2(270, 240));
            btnInstructions = new Button(Content.Load<Texture2D>("instructions_New"), graphics.GraphicsDevice);
            btnInstructions.setPosition(new Vector2(270, 280));
            btnQuit = new Button(Content.Load<Texture2D>("quitGame"), graphics.GraphicsDevice);
            btnQuit.setPosition(new Vector2(270, 320));

            //Pause stuff
            pausedTexture = Content.Load<Texture2D>("paused");
            pausedRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, pausedTexture.Width, pausedTexture.Height);

            btnReturn = new Button(Content.Load<Texture2D>("playTutorial_New"), graphics.GraphicsDevice);
            btnReturn.setPosition(new Vector2(300, 225));
            btnMainMenu = new Button(Content.Load<Texture2D>("quitGame"), graphics.GraphicsDevice);
            btnMainMenu.setPosition(new Vector2(300, 275));

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) this.Exit();

            //cursor.Update(gameTime);

            //get newest keyboard state
            ks = Keyboard.GetState();
            ms = Mouse.GetState();

            //if (ks.IsKeyDown(Keys.Escape)) this.Exit();

            switch (currentGameState)
            {
                case GameState.MainMenu:
                    if (btnPlay.isClicked)
                    {
                        cursor.ChangeMouseMode("play");
                        btnPlay.isClicked = false;
                        nextlvl = "level1";
                        nextGameState = GameState.Playing;
                        currentGameState = GameState.Loading;
                    }
                    if (btnInstructions.isClicked)
                    {
                        btnInstructions.isClicked = false;
                        currentGameState = GameState.Instructions;
                    }
                    if (btnQuit.isClicked)
                    {
                        //btnQuit.isClicked = false;
                        //currentGameState = GameState.Instructions;
                        this.Exit();
                    }
                    if (ks.IsKeyDown(Keys.Escape)) this.Exit();

                    btnPlay.Update(ms);
                    btnInstructions.Update(ms);
                    btnQuit.Update(ms);
                    cursor.Update(gameTime);
                    //base.Update(gameTime);
                    break;

                case GameState.Instructions:
                    if (ms.LeftButton == ButtonState.Pressed && msold.LeftButton == ButtonState.Released)
                    {
                        currentGameState = GameState.MainMenu;
                    }
                    if (ks.IsKeyDown(Keys.Escape)) this.Exit();

                    cursor.Update(gameTime);
                    //base.Update(gameTime);
                    break;

                case GameState.Playing:
                    //if no level loaded
                    if (this.lvl == null)
                    {
                        //load next level
                        //this.lvl = new Level1(this);
                        //this.lvl.Initialize();
                        this.ChangeLevel("level1");
                    }
                    else //else run level update
                    {
                        CollisionTest();
                        this.lvl.Update(gameTime);
                    }

                    if (ks.IsKeyDown(Keys.Escape) && !ksold.IsKeyDown(Keys.Escape))
                    {
                        EnableAllObjects(false);
                        //cursor.cameraMode = false;
                        cursor.ChangeMouseMode("menu");
                        currentGameState = GameState.Paused;
                        //btnPlay.isClicked = false;
                    }

                    if (!ks.IsKeyDown(Keys.F1) && ksold.IsKeyDown(Keys.F1))
                    {
                        currentGameState = GameState.MainMenu;
                    }

                    break;
                case GameState.Paused:

                    if (btnReturn.isClicked)
                    {
                        EnableAllObjects(true);
                        //cursor.cameraMode = true;
                        cursor.ChangeMouseMode("play");
                        btnReturn.isClicked = false;
                        currentGameState = GameState.Playing;
                    }
                    if (btnMainMenu.isClicked)
                    {
                        btnMainMenu.isClicked = false;
                        nextlvl = "mainmenu";
                        nextGameState = GameState.MainMenu;
                        currentGameState = GameState.Loading;
                    }
                    if (ks.IsKeyDown(Keys.Escape) && !ksold.IsKeyDown(Keys.Escape)) this.Exit();

                    btnReturn.Update(ms);
                    btnMainMenu.Update(ms);
                    cursor.Update(gameTime);

                    break;
                case GameState.Loading:
                    if (loaddrawn)
                    {
                        this.ChangeLevel(nextlvl);
                        loaddrawn = false;
                        currentGameState = nextGameState;//GameState.Playing;
                    }
                    break;
            }

            if (ks.IsKeyDown(Keys.Down) && !ksold.IsKeyDown(Keys.Down))
            {
                Console.WriteLine("KEY PRESSED: the player just pressed down");
            }
            else if (ks.IsKeyDown(Keys.Down) && ksold.IsKeyDown(Keys.Down))
            {
                Console.WriteLine("KEY: the player is holding the key down");
            }
            else if (!ks.IsKeyDown(Keys.Down) && ksold.IsKeyDown(Keys.Down))
            {
                Console.WriteLine("KEY RELEASED: the player was holding the key down, but has just let it go");
            }
            
            //save current keyboard state as the old state
            ksold = ks;
            msold = ms;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (currentGameState)
            {
                case GameState.MainMenu:
                    //start draw
                    this.spriteBatch.Begin();

                    spriteBatch.Draw(Content.Load<Texture2D>("MainMenu"), new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight),
                        Color.White);
                    btnPlay.Draw(this.spriteBatch);  //draw the start button
                    btnInstructions.Draw(this.spriteBatch);  //draw the instructions button
                    btnQuit.Draw(this.spriteBatch);  //draw the quit button
                    cursor.Draw(this.spriteBatch);
                    //end draw
                    this.spriteBatch.End();
                    break;

                case GameState.Instructions:
                    //start draw
                    this.spriteBatch.Begin();

                    spriteBatch.Draw(Content.Load<Texture2D>("GameInstructions"), new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight),
                        Color.White);
                    cursor.Draw(this.spriteBatch);
                    //end draw
                    this.spriteBatch.End();
                    break;

                case GameState.Playing:

                    //for each layer
                    foreach (GameComponentCollection layer in layers)
                    {
                        //start a batch
                        this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.GetViewMatrix());
                        //draw each component in the player
                        foreach (DrawableGameComponent i in layer)
                        {
                            ((EnvironmentObj)i).Draw(this.spriteBatch);
                        }
                        //end the batch
                        this.spriteBatch.End();
                    }

                    this.spriteBatch.Begin();
                    hud.Draw(this.spriteBatch);
                    this.spriteBatch.End();

                    break;
                case GameState.Paused:
                    //start draw
                    this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                    Color pauseBack = Color.Green;
                    pauseBack.A = 128;
                    spriteBatch.Draw(Content.Load<Texture2D>("paused"), new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight),
                        pauseBack);
                    btnReturn.Draw(this.spriteBatch);
                    btnMainMenu.Draw(this.spriteBatch);
                    cursor.Draw(this.spriteBatch);
                    //end draw
                    this.spriteBatch.End();
                    break;
                case GameState.Loading:
                    this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                    spriteBatch.Draw(Content.Load<Texture2D>("loading"), new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight),
                        Color.White);
                    loaddrawn = true;
                    //spriteBatch.DrawString(font, "Loading", new Vector2(GraphicsDevice.Viewport.Width - 
                    this.spriteBatch.End();
                    break;
            }

            ////for each layer
            //foreach (GameComponentCollection layer in layers)
            //{
            //    //start a batch
            //    this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.GetViewMatrix());
            //    //draw each component in the player
            //    foreach (DrawableGameComponent i in layer)
            //    {
            //        ((EnvironmentObj)i).Draw(this.spriteBatch);
            //    }
            //    //end the batch
            //    this.spriteBatch.End();
            //}

            //this.spriteBatch.Begin();
            //cursor.Draw(this.spriteBatch);
            //this.spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion

        #region Our Methods
        /// <summary>
        /// check if a point is inside the game world.
        /// </summary>
        /// <param name="point"></param>
        public bool IsInWorld(Vector2 point)
        {
            //default to false
            bool result = false;
            //if inside the drawing area
            if (point.X >= 0 && point.Y >= 0
                && point.X <= (tileLayer.mapWidth * GRID_CELL_SIZE) + 1 && point.Y <= (tileLayer.mapHeight * GRID_CELL_SIZE) + 1)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Call to populate the locationTracker graph
        /// </summary>
        public static void PopulateGraph()
        {
            //width and height for graph are width of room / 128
            int width = tileLayer.mapWidth + 1;
            int height = tileLayer.mapHeight + 1;

            // create a new location graph
            locationTracker = new UGraphList<Cell>();

            // add vertecies
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //add a vertex to the graph for every cell
                    locationTracker.AddVertex(new Cell(i, j));
                }
            }

            //add edges
            foreach (IVertex<Cell> v in locationTracker.EnumerateVertices())
            {

                //if cell below
                if (v.Data.Y < height - 1)
                {
                    //make an edge between this and the one lower
                    locationTracker.AddEdge(v.Data, new Cell(v.Data.X, v.Data.Y + 1));

                    //if cell below and right or left
                    if (v.Data.X >= 1)
                    {
                        //make an edge between this and the one to the left
                        locationTracker.AddEdge(v.Data, new Cell(v.Data.X - 1, v.Data.Y + 1));
                    }
                    if (v.Data.X < width - 1)
                    {
                        //make an edge between this and the one to the left
                        locationTracker.AddEdge(v.Data, new Cell(v.Data.X + 1, v.Data.Y + 1));
                    }
                }

                //if cell right
                if (v.Data.X < width - 1)
                {
                    //make an edge between this and the one to the left
                    locationTracker.AddEdge(v.Data, new Cell(v.Data.X + 1, v.Data.Y));
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void CollisionTest()
        {
            foreach (GameComponent me in this.Components)
            {
                //only agents need to manage colliding with things.
                if (me is Agent)
                {
                    Cell meCell = ((Agent)me).checkin;

                    //create a list for collision checking
                    List<GameComponentRef> toCollide = new List<GameComponentRef>();
                    //and copy this cell's items into it
                    //toCollide = (List<GameComponentRef>)toCollide.Concat((List<GameComponentRef>)meCell);
                    toCollide.AddRange(meCell);
                    //get a list of neighbouring cells
                    foreach (IVertex<Cell> neighbour in locationTracker.EnumerateNeighbours(meCell))
                    {
                        //add each cells items to the collision list
                        //toCollide = (List<GameComponentRef>)toCollide.Concat(neighbour.Data);
                        toCollide.AddRange(neighbour.Data);
                    }

                    //for all found nearby items, check for collision
                    foreach (GameComponentRef component in toCollide)
                    {
                        //do not collide with the current component 
                        if (!me.Equals(component.item))
                        {
                            //if this is colliding with that
                            if (((EnvironmentObj)me).IsColliding(component.item))
                            {
                                //throw new ApplicationException("hit!");
                                //((EnvironmentObj)me).Collide(component.item);
                                ((EnvironmentObj)me).imHitting.Add(component.item);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// call to enable or disable all objects in Game.Components
        /// </summary>
        /// <param name="on"></param>
        public void EnableAllObjects(bool on)
        {
            if (on) //if we want to enable all
            {
                foreach (GameComponent i in this.Components)
                {
                    i.Enabled = true;
                }
            }
            else //else disable all
            {
                foreach (GameComponent i in this.Components)
                {
                    i.Enabled = false;
                }
            }
        }

        public void ChangeLevel(string level)
        {
            //RefreshLayers();
            //layers[0] = new GameComponentCollection();
            switch (level)
            {
                case "mainmenu":
                    this.lvl.Destory();
                    player.Destory();
                    break;
                case "level1":
                    this.lvl = new Level1(this);
                    this.lvl.Initialize();
                    break;
                case "level2":
                    this.lvl.Destory();
                    this.lvl = new Level2(this);
                    this.lvl.Initialize();
                    break;
            }
            
        }

        public void ConstructLayers()
        {
            //foreach (GameComponentCollection layer in layers)
            //{
            //    //foreach (GameComponent i in layer)
            //    for (int i = 0; i < layer.Count(); i++)
            //    {
            //        //if not the player
            //        if (!(layer[i] is Player || layer[i] is Cursor))
            //        {
            //            ((GameComponent)layer[i]).Dispose();
            //        }
            //    }
            //}

            //TODO: pull the splitting code i wrote in the old oe out and use it to make a layer building method.
            layers[0] = new GameComponentCollection();
            layers[1] = new GameComponentCollection();
            layers[2] = new GameComponentCollection();
            //if (player != null)
            //{
            //    layers[1].Add(player);
            //}
            //if (cursor != null)
            //{
            //    layers[2].Add(cursor);
            //}

            //GameComponentCollection[] layers = new GameComponentCollection[3];
            //layers[0] = new GameComponentCollection();
            //layers[1] = new GameComponentCollection();
            //layers[2] = new GameComponentCollection();

            foreach (GameComponent i in this.Components)
            {
                if (i is DrawableGameComponent && ((DrawableGameComponent)i).Visible)
                {
                    if (i is Tile)
                    {
                        layers[0].Add(i);
                    }
                    else if (i is Agent || i is Weapon || i is Projectile)
                    {
                        layers[1].Add(i);
                    }
                    else if (i is Cursor)
                    {
                        layers[2].Add(i);
                    }
                    
                }

            }
        }
        #endregion
    }
}

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

namespace TRAPT
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TraptMain : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        protected const int GRID_CELL_SIZE = 128;

        //List<List<Tile>> map;
        Tile[,] map;
        TileLayer tileLayer;
        public UGraphList<Cell> locationTracker;

        Player player;
        Weapon testGun;
        Vector2 actorStart;
        public Cursor cursor;
        Tile testTile;
        public Camera camera;

        public TraptMain()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //graphics.PreferredBackBufferHeight = 600;
            //graphics.IsFullScreen = true;
            //graphics.ApplyChanges();
            this.camera = new Camera(this);
            this.camera.Initialize(GraphicsDevice.Viewport);
            //camera.Position = new Vector2(GraphicsDevice.DisplayMode.Width/2, GraphicsDevice.DisplayMode.Height/2);
            

            //graphics.GraphicsDevice.PresentationParameters.

            //this.IsMouseVisible = true;
            

            //this.Window.AllowUserResizing = false;

            //Viewport temp = new Viewport(0,0,600,600);
            //this.GraphicsDevice.Viewport = temp;

            this.locationTracker = new UGraphList<Cell>();
            PopulateGraph();
            
            
            
            
            //set start point to center screen
            this.actorStart = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            this.player = new Player(this);
            player.Initialize(this.actorStart, 0.0f);

            //LoadTileTest();
            this.tileLayer = new TileLayer(this);
            this.tileLayer.Initialize(Content.Load<Texture2D>("environment_tiles"), Content.RootDirectory);
            this.tileLayer.OpenMap("Map1");
            //this.Components.Add(tileLayer);

            //this.testTile = new WallTile(this);
            //testTile.Initialize(actorStart, 1);

            Vector2 gunStart = new Vector2((GraphicsDevice.Viewport.Width / 4) *3, (GraphicsDevice.Viewport.Height / 4) *3);
            this.testGun = new Weapon(this);
            this.testGun.Initialize(gunStart, 30, "SMG");


            this.cursor = new Cursor(this);
            this.cursor.Initialize();

            this.camera.Limits = new Rectangle(0, 0, this.tileLayer.mapWidth * GRID_CELL_SIZE, this.tileLayer.mapHeight * GRID_CELL_SIZE);

            //graphics.PreferredBackBufferWidth = (this.tileLayer.mapWidth * GRID_CELL_SIZE)+1;
            //graphics.PreferredBackBufferHeight = (this.tileLayer.mapHeight * GRID_CELL_SIZE)+1;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            

            base.Initialize();
        }

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
                && point.X <= (this.tileLayer.mapWidth * GRID_CELL_SIZE)-1 && point.Y <= (this.tileLayer.mapHeight * GRID_CELL_SIZE) + 1)
            {
                result = true;
            }
            return result;
        }

        private void PopulateGraph()
        {
            //width and height for graph are width of room / 128
            //int width = (graphics.PreferredBackBufferWidth / GRID_CELL_SIZE)+1;
            //int height = (graphics.PreferredBackBufferHeight / GRID_CELL_SIZE)+1;
            int width = 18;
            int height = 7;

            // add vertecies
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //add a vertex to the graph for every cell
                    this.locationTracker.AddVertex(new Cell(i, j));
                }
            }

            //add edges
            foreach (IVertex<Cell> v in this.locationTracker.EnumerateVertices())
            {
                ////if cell above (y greater than or equal to 1 mean there must be one above)
                //if (v.Data.Y >= 1)
                //{
                //    if (!this.locationTracker.HasEdge(v.Data, new Cell(v.Data.X, v.Data.Y - 1)))
                //    {
                //        //make an edge between this and the one higher
                //        this.locationTracker.AddEdge(v.Data, new Cell(v.Data.X, v.Data.Y - 1));
                //    }

                //    //if cell up and right or left
                //    if (v.Data.X >= 1 && !this.locationTracker.HasEdge(v.Data, new Cell(v.Data.X - 1, v.Data.Y-1)))
                //    {
                //        //make an edge between this and the one to the left
                //        this.locationTracker.AddEdge(v.Data, new Cell(v.Data.X - 1, v.Data.Y-1));
                //    }
                //    if (v.Data.X < width - 1 && !this.locationTracker.HasEdge(v.Data, new Cell(v.Data.X + 1, v.Data.Y+1)))
                //    {
                //        //make an edge between this and the one to the left
                //        this.locationTracker.AddEdge(v.Data, new Cell(v.Data.X + 1, v.Data.Y-1));
                //    }
                //}

                //if cell below
                if (v.Data.Y < height-1)
                {
                    //make an edge between this and the one lower
                    this.locationTracker.AddEdge(v.Data, new Cell(v.Data.X, v.Data.Y + 1));

                    //if cell below and right or left
                    if (v.Data.X >= 1)
                    {
                        //make an edge between this and the one to the left
                        this.locationTracker.AddEdge(v.Data, new Cell(v.Data.X - 1, v.Data.Y + 1));
                    }
                    if (v.Data.X < width - 1)
                    {
                        //make an edge between this and the one to the left
                        this.locationTracker.AddEdge(v.Data, new Cell(v.Data.X + 1, v.Data.Y + 1));
                    }
                }

                ////if cell left
                //if (v.Data.X >= 1)
                //{
                //    //make an edge between this and the one to the left
                //    this.locationTracker.AddEdge(v.Data, new Cell(v.Data.X - 1, v.Data.Y));
                //}
                //if cell right
                if (v.Data.X < width - 1)
                {
                    //make an edge between this and the one to the left
                    this.locationTracker.AddEdge(v.Data, new Cell(v.Data.X + 1, v.Data.Y));
                }
            }
            
        }

        private void LoadTileTest()
        {
            this.map = new Tile[10,10];

            map[0, 0] = new WallTile(this);
            Vector2 temp = new Vector2(0,0);
            map[0, 0].Initialize(temp, 0);

            map[0, 1] = new WallTile(this);
            temp = new Vector2(0, 1);
            map[0, 1].Initialize(temp, 1);

            map[0, 2] = new WallTile(this);
            temp = new Vector2(0, 2);
            map[0, 2].Initialize(temp, 0);
        }

        private void CollisionTest()
        {
            foreach (Cell me in this.locationTracker.EnumerateVertices())
            {
                //create a list for collision checking
                List<GameComponentRef> toCollide = new List<GameComponentRef>();
                //and copy this cell's items into it
                toCollide = (List<GameComponentRef>)toCollide.Concat(me);

                //get a list of neighbouring cells
                foreach (IVertex<Cell> neighbour in locationTracker.EnumerateNeighbours(me))
                {
                    //add each cells items to the collision list
                    toCollide = (List<GameComponentRef>) toCollide.Concat(neighbour.Data);
                }

                //TODO: Implement collision checks on list items
                foreach (GameComponentRef component in toCollide)
                {
                    //component.item.
                }
            }
        }

        private void CollisionTestv2()
        {
            foreach (GameComponent me in this.Components)
            {
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

                    //TODO: Implement collision checks on list items
                    foreach (GameComponentRef component in toCollide)
                    {
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
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            if (ks.IsKeyDown(Keys.B))
            {
                ((Player)this.Components.ElementAt(this.Components.IndexOf(this.player))).color = Color.Chartreuse;
                
            }
            if (ks.IsKeyDown(Keys.F4))
            {
                graphics.IsFullScreen = true;
                graphics.ApplyChanges();
            }

            
            CollisionTestv2();


            GameComponentCollection currComponentState = new GameComponentCollection();
            currComponentState.Concat(this.Components);
            //Array currComponentState = this.Components.ToArray();
            //Array.Sort(currComponentState);
            foreach (GameComponent i in currComponentState)
            {
                
                i.Update(gameTime);
                
            }

            MouseState ms = Mouse.GetState();

            //Vector2 temp = new Vector2(this.player.Position.X - GraphicsDevice.Viewport.Width / 2, this.player.Position.Y - GraphicsDevice.Viewport.Height / 2);
            //this.camera.Position = new Vector2((this.cursor.Position.X + temp.X) / 2, (this.cursor.Position.Y + temp.Y) / 2);

            //this.camera.Position = new Vector2(this.player.Position.X - GraphicsDevice.Viewport.Width / 2, this.player.Position.Y - GraphicsDevice.Viewport.Height / 2);

            //Vector2 temp = new Vector2(GraphicsDevice.Viewport.X + GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Y + GraphicsDevice.Viewport.Height / 2);
            //this.camera.Position = new Vector2(ms.X - temp.X, ms.Y - temp.Y);

            Vector2 playerCamPos = new Vector2(this.player.Position.X - GraphicsDevice.Viewport.Width / 2, this.player.Position.Y - GraphicsDevice.Viewport.Height / 2);
            Vector2 cursorCamPos = new Vector2(this.cursor.Position.X - GraphicsDevice.Viewport.Width / 2, this.cursor.Position.Y - GraphicsDevice.Viewport.Height / 2);
            this.camera.Position = new Vector2((cursorCamPos.X + playerCamPos.X) / 2, (cursorCamPos.Y + playerCamPos.Y) / 2);

            Console.WriteLine(camera.Position);
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Vector2 parallax = new Vector2(0.5f);
            //spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetViewMatrix(parallax));
            //spriteBatch.Draw(texture, position, Color.White); // This sprite will appear to move at 50% of the normal speed
            //spriteBatch.End();


            //this.spriteBatch.Begin(SpriteSortMode.BackToFront,BlendState.AlphaBlend,null,null,null,null,camera.GetViewMatrix());
            //this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            //GraphicsDevice.Clear(Color.CornflowerBlue);

            
            

            //this.testTile.Draw(this.spriteBatch);

            //for (int i = 0; i < this.map.GetUpperBound(0); i++)
            //{
            //    for (int j = 0; j < this.map.GetUpperBound(1); j++)
            //    {
            //        if (this.map[i, j] != null)
            //        {
            //            this.map[i, j].Draw(this.spriteBatch);
            //        }
            //    }
            //}


            //this.player.Draw(this.spriteBatch);
            //this.testGun.Draw(this.spriteBatch);

            //this.cursor.Draw(this.spriteBatch);

            GameComponentCollection[] layers = new GameComponentCollection[3];
            layers[0] = new GameComponentCollection();
            layers[1] = new GameComponentCollection();
            layers[2] = new GameComponentCollection();

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
                    else if (i is Cursor)//(i is Mover && !(i is Agent))
                    {
                        layers[2].Add(i);
                    }

                    //((EnvironmentObj)i).Draw(this.spriteBatch);
                }

            }

            
            foreach (GameComponentCollection layer in layers)
            {
                this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.GetViewMatrix());
                foreach (DrawableGameComponent i in layer)
                {
                    ((EnvironmentObj)i).Draw(this.spriteBatch);
                }
                this.spriteBatch.End();
            }
            


            //this.spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}

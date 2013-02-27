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
        public UGraphList<Cell> locationTracker;

        Player player;
        Vector2 actorStart;
        Cursor cursor;
        Tile testTile;

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
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            //this.Window.AllowUserResizing = false;

            //Viewport temp = new Viewport(0,0,600,600);
            //this.GraphicsDevice.Viewport = temp;

            this.locationTracker = new UGraphList<Cell>();
            PopulateGraph();
            
            
            
            
            //set start point to center screen
            this.actorStart = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            this.player = new Player(this);
            player.Initialize(this.actorStart, 0.0f);

            LoadTileTest();

            //this.testTile = new WallTile(this);
            //testTile.Initialize(actorStart, 1);

            this.cursor = new Cursor(this);
            this.cursor.Initialize();
            

            base.Initialize();
        }

        private void PopulateGraph()
        {
            //width and height for graph are width of room / 128
            int width = (graphics.PreferredBackBufferWidth / GRID_CELL_SIZE)+1;
            int height = (graphics.PreferredBackBufferHeight / GRID_CELL_SIZE)+1;

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
                if (me is EnvironmentAgent)
                {
                    Cell meCell = ((EnvironmentAgent)me).checkin;

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
                                ((EnvironmentObj)me).Collide(component.item);
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

            

            //this.cursor.Update(gameTime);

            //this.player.Update(gameTime);

            foreach (GameComponent i in this.Components)
            {
                CollisionTestv2();
                i.Update(gameTime);
                
            }

            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            this.spriteBatch.Begin();

            GraphicsDevice.Clear(Color.CornflowerBlue);

            
            

            //this.testTile.Draw(this.spriteBatch);
            
            for (int i = 0; i < this.map.GetUpperBound(0); i++)
            {
                for (int j = 0; j < this.map.GetUpperBound(1); j++)
                {
                    if (this.map[i, j] != null)
                    {
                        this.map[i, j].Draw(this.spriteBatch);
                    }
                }
            }

            this.player.Draw(this.spriteBatch);

            this.cursor.Draw(this.spriteBatch);
            this.spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}

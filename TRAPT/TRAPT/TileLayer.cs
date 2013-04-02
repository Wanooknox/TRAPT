using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework;
using Graph;

namespace TRAPT
{
    public class TileLayer : Microsoft.Xna.Framework.GameComponent
    {
        #region Attributes
        private string texture;
        private string contentPath;

        //int[,] mapData;
        private Tile[,] mapData;
        public int mapWidth;
        public int mapHeight;

        public Tile[,] MapData
        {
            get { return mapData; }
            set { mapData = value; }
        }

        private UGraphList<PathNode> transitionGrid;

        public UGraphList<PathNode> TransitionGrid
        {
            get { return transitionGrid; }
            set { transitionGrid = value; }
        }
        #endregion

        public TileLayer(Game game)
            : base(game)
        {
            //game.Components.Add(this);
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize(string texture, string contentPath)
        {
            this.contentPath = contentPath;
            this.texture = texture;

            base.Initialize();
        }

        //Open the fil
        public void OpenMap(string MapName)
        {
            string path = contentPath + @"\" + MapName + ".map";
            //string path = MapName + ".map";
            //string path = Game.Content.Load<string>(MapName);
            this.transitionGrid = new UGraphList<PathNode>();
            //check if file exists
            using (StreamReader sr = new StreamReader(path))
            {
                int currentY = 0;
                while (sr.Peek() >= 0)  //read it line by line, see if there is a next liine
                {
                    string line = sr.ReadLine();

                    if (currentY == 0)
                    {
                        string[] dimensions = line.Split(',');
                        mapWidth = int.Parse(dimensions[0]);
                        mapHeight = int.Parse(dimensions[1]);
                        //mapData = new int[mapWidth, mapHeight];
                        mapData = new Tile[mapWidth, mapHeight];

                        //clear out the map
                        for (int x = 0; x < mapWidth; x++)
                        {
                            for (int y = 0; y < mapHeight; y++)
                            {
                                //mapData[x, y] = -1;
                                mapData[x, y] = null;
                            }
                        }
                    }
                    else  //reading part
                    {
                        int currentX = 0;

                        foreach(char c in line.ToArray() )
                        {
                            try
                            {
                                if (c.ToString() != " ")
                                {
                                    //mapData[currentX, currentY - 1] = int.Parse(c.ToString());                               
                                    int type = int.Parse(c.ToString());

                                    if (type == 1) //if wall
                                    {
                                        mapData[currentX, currentY - 1] = new WallTile(Game);
                                        mapData[currentX, currentY - 1].Initialize(new Vector2(currentX, currentY - 1), this.texture, 0, 0);
                                    }
                                    else if (type == 0) //if floor
                                    {
                                        mapData[currentX, currentY - 1] = new FloorTile(Game);
                                        mapData[currentX, currentY - 1].Initialize(new Vector2(currentX, currentY - 1), this.texture, 0, 0);
                                        this.transitionGrid.AddVertex(new PathNode(currentX, currentY - 1, 0));
                                    }
                                }
                                
                            }
                            catch(Exception e)
                            {
                                //Do nothing
                            }
                            currentX ++;
                        }
                    }
                    currentY++;
                }
            }

            //do texture transformations
            for (int i = 0; i < this.mapWidth; i++)
            {
                for (int j = 0; j < this.mapHeight; j++)
                {
                    if (this.MapData[i, j] != null)
                    {
                        ((Tile)this.mapData[i, j]).CheckAroundMe();
                    }
                }
            }

            //do transition node grid
            foreach (IVertex<PathNode> v in this.transitionGrid.EnumerateVertices())
            {

                //if cell below
                if (v.Data.position.Y < mapHeight - 1)
                //if (this.transitionGrid.HasVertex(new PathNode((int)v.Data.position.X, (int)v.Data.position.Y + 1, 0)))
                {
                    if (this.transitionGrid.HasVertex(new PathNode((int)v.Data.position.X, (int)v.Data.position.Y + 1, 0)))
                    {
                        //make an edge between this and the one lower
                        this.transitionGrid.AddEdge(v.Data, new PathNode((int)v.Data.position.X, (int)v.Data.position.Y + 1, 0));
                    }

                    //if cell below and right or left
                    if (this.transitionGrid.HasVertex(new PathNode((int)v.Data.position.X - 1, (int)v.Data.position.Y + 1,0)))//(v.Data.position.X >= 1)
                    {
                        //make an edge between this and the one to the left
                        this.transitionGrid.AddEdge(v.Data, new PathNode((int)v.Data.position.X - 1, (int)v.Data.position.Y + 1,0));
                    }
                    if (this.transitionGrid.HasVertex(new PathNode((int)v.Data.position.X + 1, (int)v.Data.position.Y + 1,0)))//(v.Data.position.X < mapWidth - 1)
                    {
                        //make an edge between this and the one to the left
                        this.transitionGrid.AddEdge(v.Data, new PathNode((int)v.Data.position.X + 1, (int)v.Data.position.Y + 1,0));
                    }
                }

                //if cell right
                if (this.transitionGrid.HasVertex(new PathNode((int)v.Data.position.X + 1, (int)v.Data.position.Y,0)))//(v.Data.position.X < mapWidth - 1)
                {
                    //make an edge between this and the one to the left
                    this.transitionGrid.AddEdge(v.Data, new PathNode((int)v.Data.position.X + 1, (int)v.Data.position.Y,0));
                }
            }
        }

        //Open the fil
        public void ReadMapDimensions(string MapName)
        {
            string path = Game.Content.RootDirectory + @"\" + MapName + ".map";
            //string path = MapName + ".map";
            //string path = Game.Content.Load<string>(MapName);

            //check if file exists
            using (StreamReader sr = new StreamReader(path))
            {
                if (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();

                    string[] dimensions = line.Split(',');
                    mapWidth = int.Parse(dimensions[0]);
                    mapHeight = int.Parse(dimensions[1]);
                    //mapData = new int[mapWidth, mapHeight];
                    mapData = new Tile[mapWidth, mapHeight];
                }
                
            }
        }
        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    //go through the spritesheet
        //    for (int x = 0; x < mapWidth; x++)
        //    {
        //        for (int y = 0; y < mapHeight; y++)
        //        {
        //            if (mapData[x, y] != null)
        //            {
        //                //int sourceY = 64 * mapData[x, y]; // for what position we are at on the screen
        //                //Rectangle source = new Rectangle(x, sourceY, 64, 64);
        //                //Rectangle dest = new Rectangle(x * 64, y * 64, 64, 64);
        //                //spriteBatch.Draw(texture, dest, source, Color.White);

        //                mapData[x, y].Draw(spriteBatch);
        //            }
        //        }
        //    }
        //}
    }
}
